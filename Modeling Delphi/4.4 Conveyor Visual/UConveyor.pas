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
    // ��������� �������
    Generator : TDetailGenerator;
    // ������ ��������, �������������� �������������� ����������
    MachRes : array of TResource;
    // ���������� �� ��������� �������������� ���������
    ActStat : TActionStatistics;
    // ���������� �� ������� ���������� ������� � �������
    TimeStat : TStatistics;
    // ���������� �� ��������� ���������
    ConvStat : TActionStatistics;
    // ���������� ������� �� ������ ��������� ���������
    ConvCount : array of Integer;
    // ���������� �������, ������������ ������ �����������
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
  // ������ ������ �� ��������
  par.ConvStat.Start(SimTime);
  i := 0;
  // ���� ��������� ���������� ������
  while par.MachRes[i].Available = 0 do
  begin
    // ��������� � ����������
    Inc(i);
    // ���� ��� ���������� ��������, ���� � �������
    if i = MachineCount then
      i := 0;
    // ������ �� ��������� ������� ���������
    Inc(par.ConvCount[i]);
    // ������ ������� ���������
    if i > 0 then
      Hold(StepTime)
    else
      Hold(BackStepTime);
    // ������� �������
    Dec(par.ConvCount[i]);
  end;
  // ������ ����� � ���������
  par.ConvStat.Finish(SimTime);
  // ������ ����������
  GetResource(par.MachRes[i]);
  StartRunning;
  // ��������� ��������
  par.ActStat.Start(SimTime);
  Hold(rndDetail.Exponential(1));
  par.ActStat.Finish(SimTime);
  // ���������� ����������
  ReleaseResource(par.MachRes[i]);
  // ������������� ���������� �� �������
  par.TimeStat.AddData(SimTime - StartingTime);
  // ��������� ������ ���������
  Inc(par.Completed[i]);
  Finish;
end;

{ TDetailGenerator }

procedure TDetailGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // ������ ���� ������
    TDetail.Create.ActivateDelay(0);
    // ������� �������� ���������
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
  // ������ ����������� �������
  Generator.ActivateDelay(0);
  Hold(PreSimulationTime);
  ClearStat;
  // ����� ����� ��������
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

