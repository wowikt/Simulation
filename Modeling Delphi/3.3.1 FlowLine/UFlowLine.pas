unit UFlowLine;

interface
uses USimulation;

type
  // ����� TPiece - �������
  TPiece = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� Tworker1 - ������ ������� �����
  TWorker1 = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TWorker2 - ������ ������� �����
  TWorker2 = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TFlowLineSimulation - �������� ��������� ������������
  TFlowLineSimulation = class(TSimulation)
  public
    // ���������� �� �������
    Balks : TTimeBetStatistics;
    // ���������� �� ������� ���������� � �������
    TimeInSystem : TStatistics;
    // ����������� �� ������� ���������� � �������
    TimeHist : TUniformHistogram;
    // ������ ������� �����
    Worker1 : TWorker1;
    // ������ ������� �����
    Worker2 : TWorker2;
    // ������� �������, ��������� ������������
    //   �� ������ ������� �����
    Queue1 : TList;
    // ������� �������, ��������� ������������
    //   �� ������ ������� �����
    Queue2 : TList;
    // ���������� �� ������ ��������
    Stat1 : TServiceStatistics;
    // ���������� �� ������ ��������
    Stat2 : TServiceStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndPiece : TRandom;
  rndWorker1 : TRandom;
  rndWorker2 : TRandom;
  Queue1Size : Integer = 4;
  Queue2Size : Integer = 2;
  PieceMeanInterval : Double = 0.4;
  Worker1MeanTime : Double = 0.25;
  Worker2MeanTime : Double = 0.5;
  HistMin : Double = 0;
  HistStep : Double = 0.5;
  HistStepCount : Integer = 20;
  SimulationTime : Double = 300;

implementation

{ TPiece }

procedure TPiece.RunProcess;
var
  par : TFlowLineSimulation;
begin
  par := Parent as TFlowLineSimulation;
  ClearFinished;
  // ������������� �������� ���������� �������
  TPiece.Create.ActivateDelay(rndPiece.Exponential(PieceMeanInterval));
  // ���� ������� � ������� �������� ����� �� ���������
  if par.Queue1.Size < Queue1Size then
  begin
    // ������������ ������������
    par.Worker1.ActivateDelay(0);
    // ������ � ������� ��������
    Wait(par.Queue1);
  end
  else
  begin
    // �������� ���������� �� �������
    par.Balks.AddData(SimTime);
  end;
  Finish;
end;

{ TWorker1 }

procedure TWorker1.RunProcess;
var
  Piece : TPiece;
  par : TFlowLineSimulation;
begin
  par := Parent as TFlowLineSimulation;
  // ������ ������� �����
  while True do
  begin
    // ����� ��������� ������� � �������
    while par.Queue1.Empty do
      Passivate;
    // ������ ��������
    par.Stat1.Start(SimTime);
    // ������� ��������� �������
    Piece := par.Queue1.First as TPiece;
    Piece.StartRunning;
    // ��������� ������������
    Hold(rndWorker1.Exponential(Worker1MeanTime));
    // ��������� ��������
    par.Stat1.Finish(SimTime);
    // ���� ������� �� ������� �������� ����� ���������
    if par.Queue2.Size >= Queue2Size then
    begin
      // ������������� �������
      par.Stat1.StartBlock(SimTime);
      // ������� ������� ����� � ������� � ������� �������� �����
      while par.Queue2.Size >= Queue2Size do
        Passivate;
      // ��������������
      par.Stat1.FinishBlock(SimTime);
    end;
    // ��������� ������� � �������
    Piece.Insert(par.Queue2);
    // ������������ ������ �������
    par.Worker2.ActivateDelay(0);
  end;
end;

{ TWorker2 }

procedure TWorker2.RunProcess;
var
  Piece : TPiece;
  par : TFlowLineSimulation;
begin
  par := Parent as TFlowLineSimulation;
  // ������ ������� �����
  while True do
  begin
    // ������� ��������� ������� � �������
    while par.Queue2.Empty do
      Passivate;
    // ������ ��������
    par.Stat2.Start(SimTime);
    // ������� �������
    Piece := par.Queue2.First as TPiece;
    Piece.StartRunning;
    // ������������ ������� ��������� �� ������, ���� �� ��� ������������
    par.Worker1.ActivateDelay(0);
    // ��������� ������������
    Hold(rndWorker2.Exponential(Worker2MeanTime));
    // ������������� ���������� �� ������� ���������� � �������
    par.TimeInSystem.AddData(SimTime - Piece.StartingTime);
    par.TimeHist.AddData(SimTime - Piece.StartingTime);
    // ��������� ��������
    par.Stat2.Finish(SimTime);
    // ������������ ������� ��� ���������� ������
    Piece.ActivateDelay(0);
  end;
end;

{ TFlowLineSimulation }

destructor TFlowLineSimulation.Destroy;
begin
  Worker1.Free;
  Worker2.Free;
  Stat1.Free;
  Stat2.Free;
  Balks.Free;
  TimeInSystem.Free;
  TimeHist.Free;
  Queue1.Free;
  Queue2.Free;
  inherited;
end;

procedure TFlowLineSimulation.Init;
begin
  inherited;
  Balks := TTimeBetStatistics.Create;
  TimeInSystem := TStatistics.Create;
  TimeHist := TUniformHistogram.Create(HistMin, HistStep, HistStepCount);
  Queue1 := TList.Create;
  Queue2 := TList.Create;
  Worker1 := TWorker1.Create;
  Worker2 := TWorker2.Create;
  Stat1 := TServiceStatistics.Create(1);
  Stat2 := TServiceStatistics.Create(1);
end;

procedure TFlowLineSimulation.RunSimulation;
begin
  // ��������� ������ ������� � ������ 0
  TPiece.Create.ActivateDelay(0);
  // ���������� �������������
  Hold(SimulationTime);
  StopStat;
end;

procedure TFlowLineSimulation.StopStat;
begin
  inherited;
  Queue1.StopStat(SimTime);
  Queue2.StopStat(SimTime);
  Stat1.StopStat(SimTime);
  Stat2.StopStat(SimTime);
end;

end.
