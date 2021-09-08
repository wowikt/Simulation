unit UTVControl;

interface
uses USimulation;

// ������ �������� ����� �� �������� �����������
type
  // ����� TTVSet - ����������� ���������
  TTVSet = class(TProcess)
  public
    IncomeTime : Double;
  protected
    procedure RunProcess; override;
  end;

  // ����� TInspector - ���������
  TInspector = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TAdjuster  - ����������
  TAdjuster = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TTVControl - �������� ����� ��������
  TTVControl = class(TSimulation)
  public
    // ������� ����������� �� ��������
    InspectionQueue : TList;
    // ������� ��������� �����������
    Inspectors : array [0 .. 1] of TProcess;
    // ����������
    Adjuster : TAdjuster;
    // ������� �� ���������
    AdjustmentQueue : TList;
    // ���������� �� ������� ���������� � �������
    TimeInSystemStat : TStatistics;
    // ���������� �� ��������� �����������
    InspectorsStat : TServiceStatistics;
    // ���������� �� ��������� �����������
    AdjustmentStat : TServiceStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  rndTVSet,
  rndInspector,
  rndAdjuster : TRandom;
  MinCreationDelay : Double = 3.5;
  MaxCreationDelay : Double = 7.5;
  MinInspectionTime : Double = 6;
  MaxInspectionTime : Double = 12;
  NoAdjustmentProb : Double = 0.85;
  MinAdjustmentTime : Double = 20;
  MaxAdjustmentTime : Double = 40;
  SimulationTime : Double = 480;

implementation

{ TTVSet }

procedure TTVSet.RunProcess;
var
  par : TTVControl;
begin
  par := Parent as TTVControl;
  // ��������� ���� ������������ ����������
  // ������������� ����� ��������
  IncomeTime := SimTime;
  ClearFinished;
  // ������������� �������� ���������� ����������
  TTVSet.Create.
      ActivateDelay(rndTVSet.Uniform(MinCreationDelay, MaxCreationDelay));
  ActivateDelay(par.Inspectors, 0);
  // ������� ������������
  Wait(par.InspectionQueue);
end;

{ TInspector }

procedure TInspector.RunProcess;
var
  Piece : TTVSet;
  par : TTVControl;
begin
  // ������ ������������
  par := Parent as TTVControl;
  while True do
  begin
    // ���� ��� ����������� ��� ��������
    while par.InspectionQueue.Empty do
      // ������ � ������� � ����� ��������
      Passivate;
    // ������������� ������ ������
    par.InspectorsStat.Start(SimTime);
    // ������� �� ������� ������ ���������
    Piece := par.InspectionQueue.First as TTVSet;
    Piece.StartRunning;
    // ��������
    Hold(rndInspector.Uniform(MinInspectionTime, MaxInspectionTime));
    // ������������� ������ ������
    par.InspectorsStat.Finish(SimTime);
    // � ����������� NoAdjustmentProb ��������� ��������
    if rndInspector.Draw(NoAdjustmentProb) then
    begin
      // ������ ���������� � ������� ���������� � �������
      par.TimeInSystemStat.AddData(SimTime - Piece.IncomeTime);
      // ������� ���������
      Piece.Finish;
    end
    else
    begin
      // ��������� ��������� � ������� �� ���������
      Piece.Insert(par.AdjustmentQueue);
      // ���� ������ �����������
      par.Adjuster.ActivateDelay(0);
    end;
  end;
end;

{ TAdjuster }

procedure TAdjuster.RunProcess;
var
  par : TTVControl;
  Piece : TTVSet;
begin
  // ������ �����������
  par := Parent as TTVControl;
  while True do
  begin
    // ���� ��� ����������� ��� ���������, �������
    while par.AdjustmentQueue.Empty do
      Passivate;
    // ������� ������ ��������� �� �������
    Piece := par.AdjustmentQueue.First as TTVSet;
    Piece.StartRunning;
    // ������������� ������ ������
    par.AdjustmentStat.Start(SimTime);
    // ��������� ������
    Hold(rndAdjuster.Uniform(MinAdjustmentTime, MaxAdjustmentTime));
    // ������������� ����� ������
    par.AdjustmentStat.Finish(SimTime);
    // ��������� ��������� � ������� �� ��������
    Piece.Insert(par.InspectionQueue);
  end;
end;

{ TTVControl }

destructor TTVControl.Destroy;
begin
  TimeInSystemStat.Free;
  InspectorsStat.Free;
  AdjustmentStat.Free;
  Inspectors[0].Free;
  Inspectors[1].Free;
  Adjuster.Free;
  InspectionQueue.Free;
  AdjustmentQueue.Free;
  inherited;
end;

procedure TTVControl.Init;
begin
  inherited;
  InspectionQueue := TList.Create;
  Adjuster := TAdjuster.Create;
  AdjustmentQueue := TList.Create;
  // ��������� ���� ����������� � �������
  Inspectors[0] := TInspector.Create;
  Inspectors[1] := TInspector.Create;
  TimeInSystemStat := TStatistics.Create;
  InspectorsStat := TServiceStatistics.Create(2, 0, 0);
  AdjustmentStat := TServiceStatistics.Create(1, 0, 0);
  MakeVisualizator(0.2);
end;

procedure TTVControl.RunSimulation;
begin
  TTVSet.Create.ActivateDelay(0);
  // ����� �������������
  Hold(SimulationTime);
  // ��������������� ����������
  StopStat;
end;

procedure TTVControl.StopStat;
begin
  inherited;
  InspectionQueue.StopStat(SimTime);
  AdjustmentQueue.StopStat(SimTime);
  InspectorsStat.StopStat(SimTime);
  AdjustmentStat.StopStat(SimTime);
end;

end.
