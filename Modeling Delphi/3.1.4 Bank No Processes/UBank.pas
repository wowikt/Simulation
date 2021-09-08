unit UBank;

interface
uses USimulation;

type
  // ����� TClient - �������, ������������ ������� �����
  TClient = class(TLink)
  public
    StartingTime : Double;
  end;

  // ����� TClientGenerator - �������, ����������� �������� �����
  TClientGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TCashman - �������, ������������ ������ �������
  TCashman = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TBankSimulation - ������������� ������ �����
  TBankSimulation = class(TSimulation)
  public
    Generator : TClientGenerator;
    Cashman : TCashman;
    InBankTime : TStatistics;
    InBankHist : THistogram;
    CashStat : TServiceStatistics;
    Queue : TList;
    NotWaited : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  rndClient : TRandom;
  rndCashman : TRandom;
  MaxClientCount : Integer = 100;
  MeanClientInterval : Double = 5;
  MinCashTime : Double = 2;
  MaxCashTime : Double = 6;
  VisTimeStep : Double = 0.5;

implementation

{ TClientGenerator }

procedure TClientGenerator.RunProcess;
var
  i : Integer;
  par : TBankSimulation;
  cl : TClient;
begin
  par := Parent as TBankSimulation;
  for i := 1 to MaxClientCount do
  begin
    ClearFinished;
    // ������� �������
    cl := TClient.Create;
    // �������� ����� ��������
    cl.StartingTime := SimTime;
    // �������� ������� � �������
    cl.Insert(par.Queue);
    // ������������ �������
    par.Cashman.ActivateDelay(0);
    Hold(rndClient.Exponential(MeanClientInterval));
  end;
end;

{ TCashman }

procedure TCashman.RunProcess;
var
  Client : TClient;
  InTime : Double;
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;
  while True do
  begin
    // ���� ������� �����, ����� �������� �������
    while par.Queue.Empty do
      Passivate;

    // ������� ������� ������� �� �������
    Client := par.Queue.First as TClient;
    Client.Insert(par.RunningObjects);

    // ���� ������ �� ����, ������ ���
    if Client.StartingTime = SimTime then
      Inc(par.NotWaited);

    // ��������� ������������
    par.CashStat.Start(SimTime);
    Hold(rndCashman.Uniform(MinCashTime, MaxCashTime));
    par.CashStat.Finish(SimTime);

    // ������ ������ ����� ���������� � �����
    InTime := SimTime - Client.StartingTime;
    par.InBankTime.AddData(InTime);
    par.InBankHist.AddData(InTime);

    // ��������� ������ �������
    Client.Insert(par.FinishedObjects);

    // ���� ��� ������� ���������, ��������� ������
    if par.CashStat.Finished >= MaxClientCount then
      par.ActivateDelay(0);
  end;
end;

{ TBankSimulation }

destructor TBankSimulation.Destroy;
begin
  Cashman.Free;
  Queue.Free;
  InBankTime.Free;
  InBankHist.Free;
  CashStat.Free;
  inherited;
end;

procedure TBankSimulation.Init;
begin
  inherited;
  Queue := TList.Create;
  Cashman := TCashman.Create;
  Generator := TClientGenerator.Create;
  InBankTime := TStatistics.Create;
  CashStat := TServiceStatistics.Create(1, 0, 0);
  InBankHist := TUniformHistogram.Create(2, 2, 30);
  NotWaited := 0;
  MakeVisualizator(VisTimeStep);
end;

procedure TBankSimulation.RunSimulation;
begin
  // ��������� ������� �������� ��������
  Generator.ActivateDelay(0);

  // ����� ����� ��������
  Passivate;
  StopStat;
end;

procedure TBankSimulation.StopStat;
begin
  inherited;
  Queue.StopStat(SimTime);
  CashStat.StopStat(SimTime);
end;

end.
