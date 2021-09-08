unit UTools;

interface
uses USimulation;

type
  // ������������ TMachineState ���������� ��������� ������
  TMachineState = (msFree, msPreparing, msWorking, msRepaired);

  // ����� TDetail ��������� ������� ����������� ������ ����� ������
  TDetail = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TDetailGenerator - ��������� �������
  TDetailGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TFaults ��������� ��������� ������
  TFaults = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TTools ��������� ������ ������ � ���������
  TTools = class(TSimulation)
  public
    // ��������� �������
    Generator : TDetailGenerator;
    // ���������� ���������
    Faults : TFaults;
    // ������ ��������� � ��������������� �������
    Tool : TResource;
    ExtraTool : TResource;
    // ��������� ��������� � ��������������� �������
    State : TMachineState;
    ExtraState : TMachineState;
    // ���������� �� ������� ���������� ������ � �������
    TimeStat : TStatistics;
    // ���������� �� �������� ������� ��� ��������� � ��������������� �������
    PrepStat : TActionStatistics;
    PrepTimeStat : TStatistics;
    ExtraPrepStat : TActionStatistics;
    ExtraPrepTimeStat : TStatistics;
    // ���������� �� ��������� �������� ��� ��������� � ��������������� �������
    OperStat : TActionStatistics;
    OperTimeStat : TStatistics;
    ExtraOperStat : TActionStatistics;
    ExtraOperTimeStat : TStatistics;
    // ���������� �� ������� ������
    RepairStat : TActionStatistics;
    RepairTimeStat : TStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  // ���������� ��������� �����
  rndDetail,
  rndFaults : TRandom;
  // ���������� �������� ������� ������
  ResQueueCount : Integer = 2;
  // ������� �������� �������
  FaultQueueIndex : Integer = 0;
  DetailQueueIndex : Integer = 1;
  // ������� ������� ����������������� ��������
  MinPrepTime : Double = 0.2;
  MaxPrepTime : Double = 0.5;
  // ��������� ������� ��������� ��������
  MeanMainTime : Double = 0.5;
  DeviationMainTime : Double = 0.1;
  // ������� �������� ����� ������������ �������
  MeanDetailInterval : Double = 1;
  // ��������� ������� �������
  MeanFaultInterval : Double = 20;
  DeviationFaultInterval : Double = 2;
  // ��������� ������� �������
  MeanRepairIntervalTime : Double = 0.75;
  RepairIntervalCount : Integer = 3;
  // ����� ��������
  SimulationTime : Double = 500;
  VisTimeStep : Double = 0.1;

implementation

{ TDetail }

procedure TDetail.RunProcess;
var
  par : TTools;
  ActionStartTime : Double;
  OperTime : Double;
begin
  par := Parent as TTools;
  // �������� ������ � ������
  GetResource(par.Tool, 1, DetailQueueIndex);
  // ��������� ������ - �������
  par.State := msPreparing;
  // ��������� �������
  par.PrepStat.Start(SimTime);
  ActionStartTime := SimTime;
  Hold(rndDetail.Uniform(MinPrepTime, MaxPrepTime));
  // ���� ������� ��������, ������� �� �������������� ������
  if TimeLeft > 0 then
  begin
    // ������ ���������� ������
    TimeLeft := 0;
    // �������� ������ �������������� ������
    GetResource(par.ExtraTool);
    // ������ ������� �� �������������� ������
    par.ExtraState := msPreparing;
    par.ExtraPrepStat.Start(SimTime);
    ActionStartTime := SimTime;
    Hold(rndDetail.Uniform(MinPrepTime, MaxPrepTime) * 2);
    // ��������� �������
    par.ExtraPrepStat.Finish(SimTime);
    par.ExtraPrepTimeStat.AddData(SimTime - ActionStartTime);
    // ������ �������� ��������
    par.ExtraState := msWorking;
    par.ExtraOperStat.Start(SimTime);
    ActionStartTime := SimTime;
    Hold(rndDetail.Normal(MeanMainTime, DeviationMainTime) * 2);
    // ��������� ��������
    par.ExtraOperStat.Finish(SimTime);
    par.ExtraOperTimeStat.AddData(SimTime - ActionStartTime);
    par.ExtraState := msFree;
    // ���������� �������������� ������
    ReleaseResource(par.ExtraTool);
  end
  else
  begin
    // ��������� ������� �� �������� ������
    par.PrepStat.Finish(SimTime);
    par.PrepTimeStat.AddData(SimTime - ActionStartTime);
    // ��������� ������ - �������� ��������
    par.State := msWorking;
    // ��������� �������� ��������
    par.OperStat.Start(SimTime);
    ActionStartTime := SimTime;
    Hold(rndDetail.Normal(MeanMainTime, DeviationMainTime));
    // ���� �������� �������� ���� ��������
    if TimeLeft > 0 then
    begin
      // ��������� ���������� ����� ���������� ��������� ��������
      OperTime := TimeLeft;
      TimeLeft := 0;
      // �������� ������ �������������� ������
      GetResource(par.ExtraTool);
      // ������ �������
      par.ExtraState := msPreparing;
      par.ExtraPrepStat.Start(SimTime);
      ActionStartTime := SimTime;
      Hold(rndDetail.Uniform(MinPrepTime, MaxPrepTime) * 2);
      // ��������� �������
      par.ExtraPrepStat.Finish(SimTime);
      par.ExtraPrepTimeStat.AddData(SimTime - ActionStartTime);
      // ������ �������� ��������
      par.ExtraState := msWorking;
      par.ExtraOperStat.Start(SimTime);
      ActionStartTime := SimTime;
      Hold(OperTime * 2);
      // ��������� �������� ��������
      par.ExtraOperStat.Finish(SimTime);
      par.ExtraOperTimeStat.AddData(SimTime - ActionStartTime);
      par.ExtraState := msFree;
      // ���������� ������
      ReleaseResource(par.ExtraTool);
    end
    else
    begin
      // ��������� �������� �� �������� ������
      par.OperStat.Finish(SimTime);
      par.OperTimeStat.AddData(SimTime - ActionStartTime);
      // ��������� ������ - ��������
      par.State := msFree;
      // ���������� ������
      ReleaseResource(par.Tool);
    end;
  end;
  // ������� ���������� �� ������� ���������� � �������
  par.TimeStat.AddData(SimTime - StartingTime);
  Finish;
end;

{ TDetailGenerator }

procedure TDetailGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // ������� ������� � ��������� �������
    TDetail.Create.ActivateDelay(0);
    // ��������� ����� ��������� ����������
    Hold(rndDetail.Exponential(MeanDetailInterval));
  end;
end;

{ TFaults }

procedure TFaults.RunProcess;
var
  par : TTools;
  RepairStartTime : Double;
begin
  par := Parent as TTools;
  while True do
  begin
    // ������� ��������� �������
    Hold(rndFaults.Normal(MeanFaultInterval, DeviationFaultInterval));
    // �������� ������� ��������
    PreemptResourceNoWait(par.Tool, FaultQueueIndex);
    // ���������� ���� ���������� ��������
    case par.State of
    msPreparing :
      par.PrepStat.Preempt(SimTime);
    msWorking :
      par.OperStat.Preempt(SimTime);
    end;
    // ������ ��������� �������
    par.State := msRepaired;
    // ������ ������
    RepairStartTime := SimTime;
    par.RepairStat.Start(SimTime);
    Hold(rndFaults.Erlang(MeanRepairIntervalTime, RepairIntervalCount));
    par.RepairStat.Finish(SimTime);
    par.RepairTimeStat.AddData(SimTime - RepairStartTime);
    // ������ ��������
    par.State := msFree;
    // ��������� ������
    ReleaseResource(par.Tool);
  end;
end;

{ TTools }

destructor TTools.Destroy;
begin
  Faults.Free;
  Generator.Free;
  PrepStat.Free;
  PrepTimeStat.Free;
  ExtraPrepStat.Free;
  ExtraPrepTimeStat.Free;
  OperStat.Free;
  OperTimeStat.Free;
  ExtraOperStat.Free;
  ExtraOperTimeStat.Free;
  RepairStat.Free;
  RepairTimeStat.Free;
  TimeStat.Free;
  Tool.Free;
  ExtraTool.Free;
  inherited;
end;

procedure TTools.Init;
begin
  inherited;
  Faults := TFaults.Create;
  Generator := TDetailGenerator.Create;
  TimeStat := TStatistics.Create;
  PrepStat := TActionStatistics.Create;
  PrepTimeStat := TStatistics.Create;
  ExtraPrepStat := TActionStatistics.Create;
  ExtraPrepTimeStat := TStatistics.Create;
  OperStat := TActionStatistics.Create;
  OperTimeStat := TStatistics.Create;
  ExtraOperStat := TActionStatistics.Create;
  ExtraOperTimeStat := TStatistics.Create;
  RepairStat := TActionStatistics.Create;
  RepairTimeStat := TStatistics.Create;
  Tool := TResource.Create(1, 0, ResQueueCount);
  ExtraTool := TResource.Create;
  MakeVisualizator(VisTimeStep);
end;

procedure TTools.RunSimulation;
begin
  State := msFree;
  Generator.ActivateDelay(0);
  Faults.ActivateDelay(0);
  // ������� ��������� ��������
  Hold(SimulationTime);
  StopStat;
end;

procedure TTools.StopStat;
begin
  inherited;
  OperStat.StopStat(SimTime);
  ExtraOperStat.StopStat(SimTime);
  PrepStat.StopStat(SimTime);
  ExtraPrepStat.StopStat(SimTime);
  RepairStat.StopStat(SimTime);
  Tool.StopStat(SimTime);
  ExtraTool.StopStat(SimTime);
end;

end.
