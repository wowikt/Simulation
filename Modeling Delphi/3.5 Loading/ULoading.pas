unit ULoading;

interface
uses USimulation;

type
  // Класс THeap - куча земли.
  //   По существу, просто ячейка списка
  THeap = TLink;

  // Класс TBulldozer  - бульдозер
  TBulldozer = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс Tloader - погрузчик
  TLoader = class(TProcess)
  private
    // Среднее время погрузки
    MeanWorkTime : Double;
    // Объект сбора статистики
    Stat : TServiceStatistics;
  public
    constructor Create(AStat : TServiceStatistics; AWork : Double);
  protected
    procedure RunProcess; override;
  end;

  // Класс TTruck - самосвал
  TTruck = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TLoading - моделирование процесса перевозок
  TLoading = class(TSimulation)
  public
    // Очередь куч, готовых к перевозке
    HeapQueue : TList;
    // Очередь самосвалов, ожидающих работы
    TrucksQueue : TList;
    // Очередь погрузчиков, ожидающих работы
    LoadersQueue : TList;
    // Статистика по погрузчикам
    LoadersStat : array of TServiceStatistics;
    // Бульдозер
    Bulldozer : TBulldozer;
    // Признак завершения работы
    Finished : Boolean;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndBulldozer,
  rndLoader,
  rndTruck : TRandom;
  ModelingTime : Double = 480;
  MinHeapQueueSize : Integer = 2;
  HeapParamInterval : Double = 4;
  HeapParamCount : Integer = 2;
  LoaderReturnTime : Double = 5;
  TruckForwardMean : Double = 22;
  TruckForwardDeviation : Double = 3;
  TruckUnloadMin : Double = 2;
  TruckUnloadMax : Double = 8;
  TruckBackMean : Double = 18;
  TruckBackDeviation : Double = 3;
  TrucksCount : Integer = 4;
  LoadersCount : Integer = 2;
  LoaderTimeMean : array [0 .. 1] of Double = (12, 14);

implementation

{ TBulldozer }

procedure TBulldozer.RunProcess;
var
  par : TLoading;
begin
  // Ссылка на объект присутствует в классе моделирования.
  //   Нет необходимости в отдельном его учете
  par := Parent as TLoading;
  while SimTime <= ModelingTime do
  begin
    // Создать кучу земли и поставить ее в очередь
    THeap.Create.Insert(par.HeapQueue);
    // Если куч хотя бы минимальное количество
    if par.HeapQueue.Size >= MinHeapQueueSize then
      // Активировать первого погрузчика (если есть)
      ActivateDelay(par.LoadersQueue, 0);
    Hold(rndBulldozer.Erlang(HeapParamInterval, HeapParamCount));
  end;
  // Работа завершена
  par.Finished := True;
end;

{ TLoader }

constructor TLoader.Create(AStat: TServiceStatistics; AWork : Double);
begin
  Stat := AStat;
  MeanWorkTime := AWork;
  inherited Create;
end;

procedure TLoader.RunProcess;
var
  par : TLoading;
  trk : TTruck;
  i : Integer;
begin
  // Процесс, моделирующий работу погрузчика
  par := Parent as TLoading;
  while True do
  begin
    // Ожидать наличия не менее необходимого количества куч
    //   и свободного самосвала
    while (par.HeapQueue.Size < MinHeapQueueSize) or par.TrucksQueue.Empty do
    begin
      // Если бульдозер закончил работу,
      //   осталось меньше минимального числа куч и все самосвалы свободны
      if par.Finished and (par.HeapQueue.Size < MinHeapQueueSize) and
          (par.TrucksQueue.Size = 4) then
      begin
        // Завершить работу посредством активации
        //   родительского процесса
        par.ActivateDelay(0);
        Passivate;
      end
      else
        // Иначе - ждать
        Passivate;
    end;
    // Начало обслуживания
    // Зафиксировать начало работы
    Stat.Start(SimTime);
    // Извлечь из очереди первый самосвал
    trk := par.TrucksQueue.First as TTruck;
    trk.StartRunning;
    StartRunning;
    // Убрать кучи из начала очереди
    for i := 1 to MinHeapQueueSize do
      par.HeapQueue.First.Free;
    // Выполнить погрузку
    Hold(rndLoader.Exponential(MeanWorkTime));
    // Закончить работу
    Stat.Finish(SimTime);
    // Активировать самосвал
    trk.ActivateDelay(0);
    // Возвращение
    Hold(LoaderReturnTime);
    // Встать в очередь ожидания на последнее место
    Insert(par.LoadersQueue);
  end;
end;

{ TTruck }

procedure TTruck.RunProcess;
var
  par : TLoading;
begin
  // Процесс, моделирующий работу грузовика
  par := Parent as TLoading;
  while True do
  begin
    // Грузовик загружен после ожидания
    // Перевозка
    Hold(rndTruck.Normal(TruckForwardMean, TruckForwardDeviation));
    // Разгрузка
    Hold(rndTruck.Uniform(TruckUnloadMin, TruckUnloadMax));
    // Возвращение
    Hold(rndTruck.Normal(TruckBackMean, TruckBackDeviation));
    // Активировать свободный погрузчик
    ActivateDelay(par.LoadersQueue, 0);
    // Встать в очередь ожидания
    Wait(par.TrucksQueue);
  end;
end;

{ TLoading }

destructor TLoading.Destroy;
var
  i : Integer;
begin
  Bulldozer.Free;
  for i := 0 to LoadersCount - 1 do
    LoadersStat[i].Free;
  TrucksQueue.Free;
  LoadersQueue.Free;
  HeapQueue.Free;
  inherited;
end;

procedure TLoading.Init;
var
  i : Integer;
begin
  inherited;
  // Создать списки самосвалов, погрузчиков и куч
  TrucksQueue := TList.Create;
  LoadersQueue := TList.Create;
  HeapQueue := TList.Create;
  // Массив статисик погрузчиков
  SetLength(LoadersStat, LoadersCount);
  for i := 0 to LoadersCount - 1 do
    LoadersStat[i] := TServiceStatistics.Create(1);
  // Создать погрузчики и поместить из в очередь
  for i := 0 to LoadersCount - 1 do
    TLoader.Create(LoadersStat[i], LoaderTimeMean[i]).Insert(LoadersQueue);
  // Создать самосвалы и поместить их в очередь
  for i := 1 to TrucksCount do
    TTruck.Create.Insert(TrucksQueue);
  // Создать бульдозер
  Bulldozer := TBulldozer.Create;
  // Процесс работы не окончен
  Finished := False;
end;

procedure TLoading.RunSimulation;
begin
  // Запустить бульдозер
  Bulldozer.ActivateDelay(0);
  // Ожидать окончания
  Passivate;
  // Скорректировать статистики
  StopStat;
end;

procedure TLoading.StopStat;
begin
  inherited;
  LoadersStat[0].StopStat(SimTime);
  LoadersStat[1].StopStat(SimTime);
  TrucksQueue.StopStat(SimTime);
  LoadersQueue.StopStat(SimTime);
  HeapQueue.StopStat(SimTime);
end;

end.
