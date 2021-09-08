unit UTraffic;

interface
uses USimulation;

type
  // Направление движения
  TDirection = (dirLeftRight, dirRightLeft);

  // Класс TGenerator - генератор автомобилей
  TGenerator = class(TProcess)
  public
    constructor Create(Dir : TDirection; Mean : Double);
  protected
    // Направление движения автомобилей
    Direction : TDirection;
    // Средний интервал движения
    MeanInterval : Double;
    procedure RunProcess; override;
  end;

  // Класс TCar - автомобиль
  TCar = class(TProcess)
  public
    constructor Create(Dir : TDirection);
  protected
    // Направление движения
    Direction : TDirection;
    procedure RunProcess; override;
  end;

  // Класс TLights - процесс, управляющий светофорами
  TLights = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TTraffic - имитация одностороннего участка дороги
  TTraffic = class(TSimulation)
  public
    // Генераторы автомобилей
    Gen : array [TDirection] of TGenerator;
    // Процесс, управляющий светофорами
    Lights : TLights;
    // Ресурсы, управляющие прохождением автомобилей
    LightRes : array [TDirection] of TResource;
    // Затворы, обозначающие светофоры
    LightGate : array [TDirection] of TGate;
    // Статистика по времени ожидания перед светофорами
    WaitStat : array [TDirection] of TStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndCar : TRandom;
  // Время открытого состояния светофоров каждого направления
  OpenTime : array [TDirection] of Double = (60, 45);
  // Пауза между моментами проезда автомобилей
  CarPauseTime : Double = 2;
  // Длительность запрещающего сигнала
  RedLightTime : Double = 55;
  // Средние интервалы между прибытием автомобилей
  MeanLeftInterval : Double = 9;
  MeanRightInterval : Double = 12;
  // Время имитации
  SimulationTime : Double = 3600;

implementation

{ TGenerator }

constructor TGenerator.Create(Dir: TDirection; Mean: Double);
begin
  Direction := Dir;
  MeanInterval := Mean;
  inherited Create;
end;

procedure TGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // Создать автомобиль
    TCar.Create(Direction).ActivateDelay(0);
    // Дождаться создания следующего
    Hold(rndCar.Exponential(MeanInterval));
  end;
end;

{ TCar }

constructor TCar.Create(Dir: TDirection);
begin
  Direction := Dir;
  inherited Create;
end;

procedure TCar.RunProcess;
var
  par : TTraffic;
begin
  par := Parent as TTraffic;
  // Ожидать возможности проезда
  GetResource(par.LightRes[Direction]);
  // Ожидать разрешающего сигнала светофора
  WaitGate(par.LightGate[Direction]);
  // Зафиксировать статистику по времени ожидания
  par.WaitStat[Direction].AddData(SimTime - StartingTime);
  // Проехать мимо светофора
  Hold(CarPauseTime);
  // Разрешить проезд следующего автомобиля
  ReleaseResource(par.LightRes[Direction]);
  Finish;
end;

{ TLights }

procedure TLights.RunProcess;
var
  par : TTraffic;
begin
  par := Parent as TTraffic;
  while True do
  begin
    // Ожидать завершения проезда автомобилей
    Hold(RedLightTime);
    // Открыть светофор для движения слева направо
    OpenGate(par.LightGate[dirLeftRight]);
    // Выждать время
    Hold(OpenTime[dirLeftRight]);
    // Закрыть светофор
    CloseGate(par.LightGate[dirLeftRight]);
    // Ожидать завершения проезда автомобилей
    Hold(RedLightTime);
    // Открыть светофор для движения справа налево
    OpenGate(par.LightGate[dirRightLeft]);
    // Выждать время
    Hold(OpenTime[dirRightLeft]);
    // Закрыть светофор
    CloseGate(par.LightGate[dirRightLeft]);
  end;
end;

{ TTraffic }

destructor TTraffic.Destroy;
begin
  Gen[dirLeftRight].Free;
  Gen[dirRightLeft].Free;
  WaitStat[dirLeftRight].Free;
  WaitStat[dirRightLeft].Free;
  Lights.Free;
  LightGate[dirLeftRight].Free;
  LightGate[dirRightLeft].Free;
  LightRes[dirLeftRight].Free;
  LightRes[dirRightLeft].Free;
  inherited;
end;

procedure TTraffic.Init;
begin
  inherited;
  Gen[dirLeftRight] := TGenerator.Create(dirLeftRight, MeanLeftInterval);
  Gen[dirRightLeft] := TGenerator.Create(dirRightLeft, MeanRightInterval);
  LightGate[dirLeftRight] := TGate.Create;
  LightGate[dirRightLeft] := TGate.Create;
  LightRes[dirLeftRight] := TResource.Create;
  LightRes[dirRightLeft] := TResource.Create;
  Lights := TLights.Create;
  WaitStat[dirLeftRight] := TStatistics.Create;
  WaitStat[dirRightLeft] := TStatistics.Create;
end;

procedure TTraffic.RunSimulation;
begin
  Gen[dirLeftRight].ActivateDelay(0);
  Gen[dirRightLeft].ActivateDelay(0);
  Lights.ActivateDelay(0);
  // Дождаться окончания имитации
  Hold(SimulationTime);
  StopStat;
end;

procedure TTraffic.StopStat;
begin
  inherited;
  LightGate[dirLeftRight].StopStat(SimTime);
  LightGate[dirRightLeft].StopStat(SimTime);
  LightRes[dirLeftRight].StopStat(SimTime);
  LightRes[dirRightLeft].StopStat(SimTime);
end;

end.
