unit UPort;

interface
uses USimulation;

type
  // Процесс, имитирующий работу танкера
  TTanker = class(TProcess)
  public
    // Тип танкера
    TankerType : Integer;
    constructor Create(TType : Integer);
  protected
    procedure RunProcess; override;
  private
    // Совместное выделение ресурсов причала и буксира
    function GetBerthAndTug : Boolean;
  end;

  // Генератор танкеров 0-2 типов
  TTankerGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Генератора танкеров 3 типа
  TTanker3Generator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Процесс шторма
  TStorm = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Имитация работы порта
  TPort = class(TSimulation)
  public
    // Причалы
    Berth : TResource;
    // Буксир
    Tug : TResource;
    // Генераторы танкеров
    Gen : TTankerGenerator;
    Gen3 : TTanker3Generator;
    // Шторм
    Storm : TStorm;
    // Статистика по времени пребывания танкеров по типам
    TimeStat : array of TStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

const
  TypeCount = 4;

var
  rndTanker : TRandom;
  rndStorm : TRandom;
  // Границы времени погрузки по типам
  MinLoadingTime : array [0 .. TypeCount - 1] of Double = (16, 21, 32, 18);
  MaxLoadingTime : array [0 .. TypeCount - 1] of Double = (20, 27, 40, 24);
  // Границы времени перехода танкеров типа 3
  MinTripTime : Double = 216;
  MaxTripTime : Double = 264;
  // Вероятности прихода танкеров типов 0-2
  TypeProb : array [0 .. TypeCount - 3] of Double = (0.25, 0.8);
  // Границы интервала времени между прибытием танкеров
  MinInterval : Double = 4;
  MaxInterval : Double = 18;
  // Интервал первоначального прибытия танкеров типа 3
  Interval3 : Double = 48;
  // Количетсво танкеров типа 3
  Type3Count : Integer = 5;
  // Интервал межды штормами
  StormInterval : Double = 48;
  // Границы времени шторма
  MinStormTime : Double = 2;
  MaxStormTime : Double = 6;
  // Количество причалов
  BerthCount : Integer = 3;
  // Время имитации (1 год)
  SimulationTime : Double = 8760;

implementation

{ TTanker }

constructor TTanker.Create(TType: Integer);
begin
  TankerType := TType;
  inherited Create;
end;

function TTanker.GetBerthAndTug: Boolean;
var
  par : TPort;
begin
  par := Parent as TPort;
  // Совместное получение ресурсов причала и буксира
  // Если оба ресурса имеются в наличии
  if (par.Berth.Available > 0) and (par.Tug.Available > 0) then
  begin
    // Получить по 1 единице ресурса
    par.Berth.Get;
    par.Tug.Get;
    // Возвратить True
    Result := True;
  end
  else
    // Наличных ресурсов нет в наличии, возвратить False
    Result := False;
end;

procedure TTanker.RunProcess;
var
  par : TPort;
  ArriveTime : Double;
begin
  par := Parent as TPort;
  while True do
  begin
    // Запомнить время прибытия
    ArriveTime := SimTime;
    // Получить свободный причал и буксир
    GetResource(par.Tug, GetBerthAndTug);
    // Выполнить причаливание
    Hold(1);
    // Освободить буксир
    ReleaseResource(par.Tug);
    // Выполнить погрузку. Время выполнения зависит от типа танкера
    Hold(rndTanker.Uniform(MinLoadingTime[TankerType],
        MaxLoadingTime[TankerType]));
    // Получить буксир для отчаливания
    GetResource(par.Tug, 1, 1);
    // Выполнить отчаливание
    Hold(1);
    // Освободить причал
    ReleaseResource(par.Berth);
    // Освободить буксир
    ReleaseResource(par.Tug);
    // Зафиксировать статистику времени пребывания по типу танкера
    par.TimeStat[TankerType].AddData(SimTime - ArriveTime);
    // Если танкер типа 0-2, закончить работу
    if TankerType < TypeCount - 1 then
      Break;
    // Выполнить переход на разгрузку и обратно
    Hold(rndTanker.Uniform(MinTripTime, MaxTripTime));
  end;
  Finish;
end;

{ TTankerGenerator }

procedure TTankerGenerator.RunProcess;
var
  TType : Integer;
begin
  while True do
  begin
    // Очистить список завершенных процессов
    ClearFinished;
    // Случайно выбрать тип танкера
    TType := rndTanker.TableIndex(TypeProb);
    // Создать танкер
    TTanker.Create(TType).ActivateDelay(0);
    // ОЖидать прибытия следующего танкера
    Hold(rndTanker.Uniform(MinInterval, MaxInterval));
  end;
end;

{ TTanker3Generator }

procedure TTanker3Generator.RunProcess;
var
  i : Integer;
begin
  // Всего создается 5 танкеров 3 типа
  for i := 1 to Type3Count do
  begin
    // Создать танкер 3 типа
    TTanker.Create(3).ActivateDelay(0);
    // Ожидать прибытия следующего танкера
    Hold(Interval3);
  end;
end;

{ TStorm }

procedure TStorm.RunProcess;
var
  par : TPort;
begin
  par := Parent as TPort;
  while True do
  begin
    // Время до следующего шторма
    Hold(rndStorm.Exponential(StormInterval));
    // Сделать буксир нерабочим на время шторма
    ChangeResource(par.Tug, -1);
    // Длительность шторма
    Hold(rndStorm.Uniform(MinStormTime, MaxStormTime));
    // Освободить буксир после шторма
    ChangeResource(par.Tug, 1);
  end;
end;

{ TPort }

destructor TPort.Destroy;
var
  i : Integer;
begin
  Gen.Free;
  Gen3.Free;
  Storm.Free;
  Berth.Free;
  Tug.Free;
  for i := 0 to TypeCount - 1 do
    TimeStat[i].Free;
  inherited;
end;

procedure TPort.Init;
var
  i : Integer;
begin
  inherited;
  Berth := TResource.Create(BerthCount);
  Tug := TResource.Create(1, 0, 2);
  Gen := TTankerGenerator.Create;
  Gen3 := TTanker3Generator.Create;
  Storm := TStorm.Create;
  SetLength(TimeStat, TypeCount);
  for i := 0 to TypeCount - 1 do
    TimeStat[i] := TStatistics.Create;
end;

procedure TPort.RunSimulation;
begin
  // Запустить процессы
  Gen.ActivateDelay(0);
  Gen3.ActivateDelay(0);
  Storm.ActivateDelay(0);
  // Время имитации (1 год)
  Hold(SimulationTime);
  StopStat;
end;

procedure TPort.StopStat;
begin
  inherited;
  Berth.StopStat(SimTime);
  Tug.StopStat(SimTime);
end;

end.
