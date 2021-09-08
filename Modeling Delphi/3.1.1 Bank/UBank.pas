unit UBank;

interface
uses USimulation;

type
  // ����� TClient - �������, ������������ ������� �����
  TClient = class(TProcess)
  protected
    procedure RunProcess; override;
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
    // ������
    Cashman : TCashman;
    // ��������� ��������
    Generator : TClientGenerator;
    // ������� ��������
    Queue : TList;
    // ���������� � ����������� �� ������� ����������
    //   �������� � �����
    InBankTime : TStatistics;
    InBankHist : THistogram;
    // ���������� �� ��������� �������
    CashStat : TServiceStatistics;
    // ������� ��������, ����������� ��� ��������
    NotWaited : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  // ������� ��������� �����
  rndClient : TRandom;
  rndCashman : TRandom;
  // ���������� ������������� ��������
  MaxClientCount : Integer = 100;
  // ������� �������� ����� ��������� ��������
  MeanClientInterval : Double = 5;
  // ������� ������� ������������
  MinCashTime : Double = 2;
  MaxCashTime : Double = 6;
  // ��������� �����������
  HistMin : Double = 2;
  HistStep : Double = 2;
  HistStepCount : Integer = 20;

implementation

{ TClient }

procedure TClient.RunProcess;
var
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;
  // ������������ �����
  par.Cashman.ActivateDelay(0);
  // ������ � ������� � ����� ������������
  Wait(par.Queue);
  // ������ � ������� ����������� ���������
  Finish;
end;

{ TClientGenerator }

procedure TClientGenerator.RunProcess;
var
  i : Integer;
begin
  for i := 1 to MaxClientCount do
  begin
    ClearFinished;
    TClient.Create.ActivateDelay(0);
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
    Client.StartRunning;

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

    // ����������� �������, ��� ��� �����������
    //   ��������� ������
    Client.ActivateDelay(0);

    // ���� ��� ������� ���������, ��������� ������
    if par.CashStat.Finished = MaxClientCount then
      par.ActivateDelay(0);
  end;
end;

{ TBankSimulation }

destructor TBankSimulation.Destroy;
begin
  Cashman.Free;
  Generator.Free;
  InBankTime.Free;
  InBankHist.Free;
  CashStat.Free;
  Queue.Free;
  inherited;
end;

procedure TBankSimulation.Init;
begin
  inherited;
  Queue := TList.Create;
  Cashman := TCashman.Create;
  Generator := TClientGenerator.Create;
  InBankTime := TStatistics.Create;
  CashStat := TServiceStatistics.Create;
  InBankHist := TUniformHistogram.Create(HistMin, HistStep, HistStepCount);
  NotWaited := 0;
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