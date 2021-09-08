unit UPert;

interface
uses USimulation;

type
  // ���� ����
  TNode = class(TProcess)
  public
    // ������ ��������� ���
    OutArcs : TList;
    // ������� ����������� �������� ���
    IncomeArcs : Integer;
    // ���������� �������� ���, ������� ������ ���� ���������
    //   ��� ���������� ����
    LimitIncomeArcs : Integer;
    // �������� �� ���� ��������� ��� ��������
    First, Last : Boolean;
    // ������ ����� ���������� ����
    NodeStat : TStatistics;
    // ������ ����� �����������
    NodeHist : THistogram;
    constructor Create(Stat : TStatistics; Hist : THistogram);
    destructor Destroy; override;
  protected
    procedure RunProcess; override;
  end;

  // ���� ����
  TArc = class(TProcess)
  public
    // �������� ���� ����
    ToNode : TNode;
    // ����� ���������� ������
    ActionTime : Double;
    constructor Create(AToNode : TNode; ActTime : Double);
  protected
    procedure RunProcess; override;
  end;

  TPert = class(TSimulation)
  public
    // ������ �����
    Nodes : array of TProcess;
    destructor Destroy; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

// ����� ����� � ���
const
  NodeCount = 6;
  ArcCount = 9;

// ��������� ���������� �� ������ ����
type
  TArcData = record
    FromNode, ToNode : Integer;
    MinTime, ModaTime, MaxTime : Double;
  end;

var
  rndArc : TRandom;
  // �������� ������ � ������������ ����
  ArcData : array [0 .. ArcCount - 1] of TArcData = (
      (FromNode : 0; ToNode : 1; MinTime :  1; ModaTime :  3; MaxTime :  5),
      (FromNode : 0; ToNode : 2; MinTime :  3; ModaTime :  6; MaxTime :  9),
      (FromNode : 0; ToNode : 3; MinTime : 10; ModaTime : 13; MaxTime : 19),
      (FromNode : 1; ToNode : 4; MinTime :  3; ModaTime :  9; MaxTime : 12),
      (FromNode : 1; ToNode : 2; MinTime :  1; ModaTime :  3; MaxTime :  8),
      (FromNode : 2; ToNode : 5; MinTime :  8; ModaTime :  9; MaxTime : 16),
      (FromNode : 2; ToNode : 3; MinTime :  4; ModaTime :  7; MaxTime : 13),
      (FromNode : 4; ToNode : 5; MinTime :  3; ModaTime :  6; MaxTime :  9),
      (FromNode : 3; ToNode : 5; MinTime :  1; ModaTime :  3; MaxTime :  8)
  );
  // ������� ����� ����������
  NodeStat : array [0 .. NodeCount - 1] of TStatistics;
  NodeHist : array [0 .. NodeCount - 1] of THistogram;
  RunCount : Integer = 400;
  HistMin : array [1 .. NodeCount - 1] of Double =
      (1, 3, 10, 4, 11);
  HistStep : array [1 .. NodeCount - 1] of Double =
      (0.2, 0.5, 1, 0.5, 1);
  HistStepCount : array [1 .. NodeCount - 1] of Integer =
      (20, 20, 16, 26, 23);

implementation

{ TNode }

constructor TNode.Create(Stat : TStatistics; Hist : THistogram);
begin
  LimitIncomeArcs := 0;
  IncomeArcs := 0;
  First := False;
  Last := False;
  OutArcs := TList.Create;
  NodeStat := Stat;
  NodeHist := Hist;
  inherited Create;
end;

destructor TNode.Destroy;
begin
  OutArcs.Free;
  inherited;
end;

procedure TNode.RunProcess;
var
  par : TPert;
begin
  par := Parent as TPert;
  // ������� ���������� ���� �������� ���
  while IncomeArcs < LimitIncomeArcs do
    Passivate;
  // ������� ���������� �� ������� ���������� (����� ���������� ����)
  if not First then
  begin
    NodeStat.AddData(SimTime);
    NodeHist.AddData(SimTime);
  end;
  // ��������� ���� ��������� ��������
  if Last then
    par.ActivateDelay(0)
  // ��� ��������� - ��������� ��������� ����
  else
    ActivateAllDelay(OutArcs, 0);
end;

{ TArc }

constructor TArc.Create(AToNode : TNode; ActTime : Double);
begin
  ToNode := AToNode;
  ActionTime := ActTime;
  inherited Create;
end;

procedure TArc.RunProcess;
begin
  // ��������� ������
  Hold(ActionTime);
  // �������� ���� � ���������� ������
  Inc(ToNode.IncomeArcs);
  ToNode.ActivateDelay(0);
end;

{ TPert }

destructor TPert.Destroy;
var
  i : Integer;
begin
  for i := 0 to NodeCount - 1 do
    Nodes[i].Free;
  inherited;
end;

procedure TPert.Init;
var
  i : Integer;
begin
  inherited;
  // ������� ����
  SetLength(Nodes, NodeCount);
  for i := 0 to NodeCount - 1 do
    Nodes[i] := TNode.Create(NodeStat[i], NodeHist[i]);
  // �������� ��������� � ��������
  (Nodes[0] as TNode).First := True;
  (Nodes[NodeCount - 1] as TNode).Last := True;
  // ������� ����
  for i := 0 to ArcCount - 1 do
  begin
    with ArcData[i] do
    begin
      // �������� �������� ����,
      TArc.Create(Nodes[ToNode] as TNode,
          //   ����� ���������� ������
          rndArc.Triangular(MinTime, ModaTime, MaxTime)).
          // �������� � ������ ��������� ��� ���������������� ����
          Insert((Nodes[FromNode] as TNode).OutArcs);
      // ��������� � ��������������� ����
      Inc((Nodes[ToNode] as TNode).LimitIncomeArcs);
    end;
  end;
end;

procedure TPert.RunSimulation;
begin
  // ��������� ��� ����
  ActivateAllDelay(Nodes, 0);
  // ����� ����������
  Passivate;
end;

end.
