unit UConveyor;

interface
uses USimulation;

type
  TDetail = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TDetailGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TConveyor = class(TSimulation)
  public
    // Генератор деталей
    Generator : TDetailGenerator;
    // Массив ресурсов, представляющих обрабатывающие устройства
    MachRes : array of TResource;
    // Статистика по занятости обрабатывающих устройств
    ActStat : TActionStatistics;
    // Статистика по времени пребывания деталей в системе
    TimeStat : TStatistics;
    // Статистика по занятости конвейера
    ConvStat : TActionStatistics;
    // Количество деталей на каждом фрагменте конвейера
    ConvCount : array of Integer;
    // Количество деталей, обработанных каждым устройством
    Completed : array of Integer;
    destructor Destroy; override;
    procedure StopStat; override;
    procedure ClearStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndDetail : TRandom;
  MachineCount : Integer = 5;
  StepTime : Double = 1;
  BackStepTime : Double = 5;
  DetailInterval : Double = 0.25;
  PreSimulationTime : Double = 720;
  SimulationTime : Double = 1440;
  VisTimeStep : Double = 0.5;

implementation

{ TDetail }

procedure TDetail.RunProcess;
var
  par : TConveyor;
  i : Integer;
begin
  par := Parent as TConveyor;
  // Деталь встала на конвейер
  par.ConvStat.Start(SimTime);
  i := 0;
  // Пока очередное устройство занято
  while par.MachRes[i].Available = 0 do
  begin
    // Двигаться к следующему
    Inc(i);
    // Если все устройства пройдены, идти к первому
    if i = MachineCount then
      i := 0;
    // Деталь на следующем участке конвейера
    Inc(par.ConvCount[i]);
    // Пройти участок конвейера
    if i > 0 then
      Hold(StepTime)
    else
      Hold(BackStepTime);
    // Участок пройден
    Dec(par.ConvCount[i]);
  end;
  // Деталь сошла с конвейера
  par.ConvStat.Finish(SimTime);
  // Занять устройство
  GetResource(par.MachRes[i]);
  StartRunning;
  // Выполнить действие
  par.ActStat.Start(SimTime);
  Hold(rndDetail.Exponential(1));
  par.ActStat.Finish(SimTime);
  // Освободить устройство
  ReleaseResource(par.MachRes[i]);
  // Зафиксировать статистику по времени
  par.TimeStat.AddData(SimTime - StartingTime);
  // Обработка детали завершена
  Inc(par.Completed[i]);
  Finish;
end;

{ TDetailGenerator }

procedure TDetailGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // Создат нову деталь
    TDetail.Create.ActivateDelay(0);
    // Ожидать прибытия следующей
    Hold(DetailInterval);
  end;
end;

{ TConveyor }

procedure TConveyor.ClearStat;
var
  i : Integer;
begin
  inherited;
  for i := 0 to MachineCount - 1 do
    MachRes[i].ClearStat(SimTime);
  ActStat.Clear(SimTime);
  ConvStat.Clear(SimTime);
  TimeStat.Clear;
  for i := 0 to MachineCount - 1 do
    Completed[i] := 0;
end;

destructor TConveyor.Destroy;
var
  i : Integer;
begin
  Generator.Free;
  ActStat.Free;
  TimeStat.Free;
  ConvStat.Free;
  for i := 0 to MachineCount - 1 do
    MachRes[i].Free;
  SetLength(MachRes, 0);
  SetLength(ConvCount, 0);
  SetLength(Completed, 0);
  inherited;
end;

procedure TConveyor.Init;
var
  i : Integer;
begin
  inherited;
  ActStat := TActionStatistics.Create;
  TimeStat := TStatistics.Create;
  ConvStat := TActionStatistics.Create;
  Generator := TDetailGenerator.Create;
  SetLength(MachRes, MachineCount);
  for i := 0 to MachineCount - 1 do
    MachRes[i] := TResource.Create;
  SetLength(ConvCount, MachineCount);
  for i := 0 to MachineCount - 1 do
    ConvCount[i] := 0;
  SetLength(Completed, MachineCount);
  for i := 0 to MachineCount - 1 do
    Completed[i] := 0;
  MakeVisualizator(VisTimeStep);
end;

procedure TConveyor.RunSimulation;
begin
  // Начать поступление деталей
  Generator.ActivateDelay(0);
  Hold(PreSimulationTime);
  ClearStat;
  // Ждать конца имитации
  Hold(SimulationTime);
  StopStat;
end;

procedure TConveyor.StopStat;
var
  i : Integer;
begin
  inherited;
  for i := 0 to MachineCount - 1 do
    MachRes[i].StopStat(SimTime);
  ActStat.StopStat(SimTime);
  ConvStat.StopStat(SimTime);
end;

end.

