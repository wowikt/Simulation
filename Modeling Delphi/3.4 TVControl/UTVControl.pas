unit UTVControl;

interface
uses USimulation;

// ������ �������� ����� �� �������� �����������
type
  // ����� TTVSet - ����������� ���������
  TTVSet = class(TLink)
  public
    StartingTime : Double;
  end;

  TTVSetGenerator = class(TProcess)
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
    Inspectors : array of TProcess;
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
    // ��������� ��������
    Generator : TTVSetGenerator;
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
  InspectorCount : Integer = 2;
  SimulationTime : Double = 480;

implementation

{ TTVSetGenerator }

procedure TTVSetGenerator.RunProcess;
var
  par : TTVControl;
  tv : TTVSet;
begin
  par := Parent as TTVControl;
  while True do
  begin
    ClearFinished;
    // ������� ����� ���������
    tv := TTVSet.Create;
    // ������������� ����� ��������
    tv.StartingTime := SimTime;
    // ��������� ��������� � ������� ��������
    tv.Insert(par.InspectionQueue);
    ActivateDelay(par.Inspectors, 0);
    // ��������� �� ����������
    Hold(rndTVSet.Uniform(MinCreationDelay, MaxCreationDelay));
  end;
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
    // ������� �� ������� ������ ���������
    Piece := par.InspectionQueue.First as TTVSet;
    Piece.Insert(par.RunningObjects);
    // ������������� ������ ������
    par.InspectorsStat.Start(SimTime);
    // ��������
    Hold(rndInspector.Uniform(MinInspectionTime, MaxInspectionTime));
    // ������������� ����� ������
    par.InspectorsStat.Finish(SimTime);
    // � ����������� NoAdjustmentProb ��������� ��������
    if rndInspector.Draw(NoAdjustmentProb) then
    begin
      // ������ ���������� � ������� ���������� � �������
      par.TimeInSystemStat.AddData(SimTime - Piece.StartingTime);
      // ������� ���������
      Piece.Insert(par.FinishedObjects);
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
    Piece.Insert(par.RunningObjects);
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
var
  i : Integer;
begin
  Generator.Free;
  TimeInSystemStat.Free;
  InspectorsStat.Free;
  AdjustmentStat.Free;
  for i := 0 to InspectorCount - 1 do
    Inspectors[i].Free;
  Adjuster.Free;
  InspectionQueue.Free;
  AdjustmentQueue.Free;
  inherited;
end;

procedure TTVControl.Init;
var
  i : Integer;
begin
  inherited;
  InspectionQueue := TList.Create;
  Adjuster := TAdjuster.Create;
  AdjustmentQueue := TList.Create;
  Generator := TTVSetGenerator.Create;
  // ������� �����������
  SetLength(Inspectors, InspectorCount);
  for i := 0 to InspectorCount - 1 do
    Inspectors[i] := TInspector.Create;
  TimeInSystemStat := TStatistics.Create;
  InspectorsStat := TServiceStatistics.Create(InspectorCount);
  AdjustmentStat := TServiceStatistics.Create(1);
end;

procedure TTVControl.RunSimulation;
begin
  Generator.ActivateDelay(0);
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
