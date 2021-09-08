unit UBank;

interface
uses USimulation;

type
  // ����� TClient - �������, ������������ ������� �����
  TClient = class(TProcess)
  public
    Inserted : Boolean;
    constructor Create; overload;
    constructor Create(AInserted : Boolean); overload;
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
  public
    Client : TClient;
    constructor Create(AClient : TClient);
  protected
    procedure RunProcess; override;
  end;

  // ����� TBankSimulation - ������������� ������ �����
  TBankSimulation = class(TSimulation)
  public
    Cashmen : array of TProcess;
    Generator : TClientGenerator;
    InBankTime : TStatistics;
    InBankHist : THistogram;
    CashStat : TServiceStatistics;
    Queue : TList;
    NotWaited : Integer;
    NotServiced : Integer;
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
  // ������������ ����� �������
  MaxQueueLength : Integer = 10;
  // ��������� ���������� �������� � �������
  StartClientNum : Integer = 2;
  // ������� �������� �������� ��������
  MeanClientInterval : Double = 5;
  // ������ �������� ������� �������
  FirstClientArrival : Double = 5;
  // ������� ������� ������������ ��������
  MinCashTime : Double = 6;
  MaxCashTime : Double = 12;
  // ���������� ����
  CashCount : Integer = 2;
  // ������ ��������� �����������
  HistInterval : Double = 2;
  // ���������� ���������� �����������
  HistIntervalCount : Integer = 20;

implementation

{ TClient }

constructor TClient.Create(AInserted: Boolean);
begin
  Inserted := AInserted;
  inherited Create;
end;

constructor TClient.Create;
begin
  Inserted := False;
  inherited Create;
end;

procedure TClient.RunProcess;
var
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;

  // ���� ������ ��� ������� � ������� �������������
  if Inserted then
  begin
    // ����� ������� ������������
    Passivate;
  end
  // ������ � �������
  else if par.Queue.Size < MaxQueueLength then
  begin
    Insert(par.Queue);
    // ������������ ��������
    ActivateDelay(par.Cashmen, 0);

    // ����� ������������
    Passivate;
  end
  else
    // ������� ��������� - ��������� ������� �������
    Inc(par.NotServiced);
  // ������ � ������� ����������� ���������
  Finish;
end;

{ TClientGenerator }

procedure TClientGenerator.RunProcess;
begin
  // ��������� ����� ���������� ������� �������
  Hold(FirstClientArrival);
  while True do
  begin
    ClearFinished;
    TClient.Create.ActivateDelay(0);
    Hold(rndClient.Exponential(MeanClientInterval));
  end;
end;

{ TCashman }

constructor TCashman.Create(AClient : TClient);
begin
  Client := AClient;
  inherited Create;
end;

procedure TCashman.RunProcess;
var
  InTime : Double;
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;

  // �������� ���������� �������, ����� �� ��������
  //   ����� �������
  Client.StartRunning;
  Client.ActivateDelay(0);
  Hold(0);
  // ���������� � ������ � ���
  while True do
  begin
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

    // ����������� ������� �������, ��� ��� �����������
    //   ��������� ������
    Client.ActivateDelay(0);
    // ���� ���������� ���������� ����� ��������,
    //   ��������� ������
    if par.CashStat.Finished = MaxClientCount then
    begin
      par.ActivateDelay(0);
      Passivate;
    end;

    // ���� ������� �����, ����� �������� �������
    while par.Queue.Empty do
      Passivate;

    // ������� ������� ������� �� �������
    Client := par.Queue.First as TClient;
    Client.StartRunning;
  end;
end;

{ TBankSimulation }

destructor TBankSimulation.Destroy;
var
  i : Integer;
begin
  for i := 0 to CashCount - 1 do
    Cashmen[i].Free;
  Queue.Free;
  InBankTime.Free;
  InBankHist.Free;
  CashStat.Free;
  inherited;
end;

procedure TBankSimulation.Init;
var
  i : Integer;
begin
  inherited;
  Queue := TList.Create;
  SetLength(Cashmen, CashCount);
  for i := 0 to CashCount - 1 do
    Cashmen[i] := TCashman.Create(TClient.Create(True));
  for i := 1 to StartClientNum do
    TClient.Create(True).Insert(Queue);
  Generator := TClientGenerator.Create;
  InBankTime := TStatistics.Create;
  CashStat := TServiceStatistics.Create(CashCount);
  InBankHist :=
      TUniformHistogram.Create(MinCashTime, HistInterval, HistIntervalCount);
  NotWaited := 0;
  NotServiced := 0;
end;

procedure TBankSimulation.RunSimulation;
begin
  // ��������� ������� �������� ��������
  Generator.ActivateDelay(0);
  // ������������ ��������
  ActivateAllDelay(Cashmen, 0);
  // ������������ ��������, ������������� ������������ � �������,
  //  ����� ��� ��������� ����� ������ ������
  ActivateAllDelay(Queue, 0);
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

