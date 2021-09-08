unit UQuarry;

interface
uses USimulation;

type
  TExcavator = class(TProcess)
  public
    // Очередь самосвалов
    Queue : TList;
    // Статистика
    Stat : TServiceStatistics;
    constructor Create(AQueue : TList; AStat : TServiceStatistics);
  protected
    procedure RunProcess; override;
  end;

  TTruck = class(TProcess)
  public
    // Индекс экскаватора
    Index : Integer;
    // Грузоподъемность
    Tonnage : Double;
    // Время загрузки
    LoadingTime : Double;
    // Время холостого пробега
    TripTime : Double;
    // Время разгрузки
    UnloadingTime : Double;
    constructor Create(Idx : Integer;
        Tng, LdTm, TrTm, UnldTm : Double);
  protected
    procedure RunProcess; override;
  end;

  TMill = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TQuarry = class(TSimulation)
  public
    // Очереди к экскаваторам
    ExcavatorQueue : array of TList;
    // Очереди к измельчителю
    MillQueue : TList;
    // Экскаваторы
    Excavator : array of TProcess;
    // Измельчитель
    Mill : TMill;
    // Статистика по возврату самосвалов
    ReturnStat : TActionStatistics;
    // Статистики по экскаваторам
    ExcavatorStat : array of TServiceStatistics;
    // Статистика по измельчителю
    MillStat : TServiceStatistics;
    // Количество самосвалов, движущихся на разгрузку
    ForwardTrip : array of Integer;
    // Количество разгруженных самосвалов по типам
    HeavyCount : Integer;
    LightCount : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndExcavator,
  rndMill : TRandom;
  ExcavatorCount : Integer = 3;
  HeavyTonnage : Double = 50;
  LightTonnage : Double = 20;
  HeavyLoadingTime : Double = 10;
  LightLoadingTime : Double = 5;
  HeavyTripTime : Double = 2;
  LightTripTime : Double = 1.5;
  HeavyUnloadingTime : Double = 4;
  LightUnloadingTime : Double = 2;
  SimulationTime : Double = 480;
  InitLightTrucks : Integer = 2;
  InitHeavyTrucks : Integer = 1;
  VisTimeStep : Double = 0.2;

implementation

{ TExcavator }

constructor TExcavator.Create(AQueue : TList; AStat : TServiceStatistics);
begin
  Queue := AQueue;
  Stat := AStat;
  inherited Create;
end;

procedure TExcavator.RunProcess;
var
  trk : TTruck;
begin
  // работа экскаватора
  while True do
  begin
    // Ожидать появления самосвалов в очереди
    while Queue.Empty do
      Passivate;
    // Взять первый самосвал
    trk := Queue.First as TTruck;
    trk.StartRunning;
    // Погрузка с записью статистики
    Stat.Start(SimTime);
    Hold(rndExcavator.Exponential(trk.LoadingTime));
    Stat.Finish(SimTime);
    // Отправить самосвал
    trk.ActivateDelay(0);
  end;
end;

{ TTruck }

constructor TTruck.Create(Idx: Integer; Tng, LdTm, TrTm, UnldTm: Double);
begin
  Index := Idx;
  Tonnage := Tng;
  LoadingTime := LdTm;
  TripTime := TrTm;
  UnloadingTime := UnldTm;
  inherited Create;
end;

procedure TTruck.RunProcess;
var
  par : TQuarry;
begin
  par := Parent as TQuarry;
  while True do
  begin
    // Самосвал загружен и отправляется к измельчителю
    Inc(par.ForwardTrip[Index]);
    Hold(TripTime + 1);
    Dec(par.ForwardTrip[Index]);
    // Активировать разгрузку
    par.Mill.ActivateDelay(0);
    // Встать в очередь к измельчителю
    Wait(par.MillQueue);
    // Обратный путь с записью статистики
    par.ReturnStat.Start(SimTime);
    Hold(TripTime);
    par.ReturnStat.Finish(SimTime);
    // Активировать свой экскаватор
    par.Excavator[Index].ActivateDelay(0);
    // Встать в очередь к своему экскаватору
    Wait(par.ExcavatorQueue[Index]);
  end;
end;

{ TMill }

procedure TMill.RunProcess;
var
  par : TQuarry;
  trk : TTruck;
begin
  // Процесс работы измельчителя
  par := Parent as TQuarry;
  while True do
  begin
    // Ожидать поступления самосвалов на рагрузку
    while par.MillQueue.Empty do
      Passivate;
    // Извлчеь первый самосвал из очереди
    trk := par.MillQueue.First as TTruck;
    trk.StartRunning;
    // Разгрузка с записью статистики
    par.MillStat.Start(SimTime);
    Hold(rndMill.Exponential(trk.UnloadingTime));
    if trk.Tonnage > LightTonnage then
      Inc(par.HeavyCount)
    else
      Inc(par.LightCount);
    par.MillStat.Finish(SimTime);
    // Отправить самосвал в обратный путь
    trk.ActivateDelay(0);
  end;
end;

{ TQuarry }

destructor TQuarry.Destroy;
var
  i : Integer;
begin
  for i := 0 to ExcavatorCount - 1 do
    Excavator[i].Free;
  SetLength(Excavator, 0);
  Mill.Free;
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorStat[i].Free;
  SetLength(ExcavatorStat, 0);
  MillStat.Free;
  ReturnStat.Free;              ;
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorQueue[i].Free;
  SetLength(ExcavatorQueue, 0);
  MillQueue.Free;
  inherited;
end;

// Функция сравнения для вставки в очередь ожидания разгрузки
function MillQueueFunc(A, B : TLink) : Boolean;
begin
  // Крупнотоннажные самосвалы имеют преимущество
  Result := (A as TTruck).Tonnage > (B as TTruck).Tonnage;
end;

procedure TQuarry.Init;
var
  i, j : Integer;
begin
  inherited;
  SetLength(ExcavatorQueue, ExcavatorCount);
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorQueue[i] := TList.Create;
  MillQueue := TList.Create(MillQueueFunc);
  SetLength(ExcavatorStat, ExcavatorCount);
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorStat[i] := TServiceStatistics.Create(1);
  MillStat := TServiceStatistics.Create(1);
  ReturnStat := TActionStatistics.Create;
  SetLength(Excavator, ExcavatorCount);
  for i := 0 to ExcavatorCount - 1 do
    Excavator[i] := TExcavator.Create(ExcavatorQueue[i], ExcavatorStat[i]);
  Mill := TMill.Create;
  for i := 0 to ExcavatorCount - 1 do
  begin
    for j := 1 to InitLightTrucks do
      TTruck.Create(i, LightTonnage, LightLoadingTime, LightTripTime,
          LightUnloadingTime).Insert(ExcavatorQueue[i]);
    for j := 1 to InitHeavyTrucks do
      TTruck.Create(i, HeavyTonnage, HeavyLoadingTime, HeavyTripTime,
          HeavyUnloadingTime).Insert(ExcavatorQueue[i]);
  end;
  SetLength(ForwardTrip, ExcavatorCount);
  for i := 0 to ExcavatorCount - 1 do
    ForwardTrip[i] := 0;
  HeavyCount := 0;
  LightCount := 0;
  MakeVisualizator(VisTimeStep);
end;

procedure TQuarry.RunSimulation;
begin
  ActivateAllDelay(Excavator, 0);
  Hold(SimulationTime);
  StopStat;
end;

procedure TQuarry.StopStat;
var
  i : Integer;
begin
  inherited;
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorStat[i].StopStat(SimTime);
  MillStat.StopStat(SimTime);
  ReturnStat.StopStat(SimTime);
end;

end.
