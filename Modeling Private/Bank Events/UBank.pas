unit UBank;

interface
uses USimulation;

type
  // ����� TClient ������������ �������, �������������� � �����
  TClient = class(TLink)
  public
    StartingTime : Double;
    constructor Create(Start : Double);
  end;

  // ����� TArrivalEvent - ���������� �������
  //   ��� ������� �������� �������� � ����
  TArrivalEvent = class(TEventHandler)
  public
    procedure DefaultEventProc; override;
  end;

  // ����� TFinishedEvent - ���������� �������
  //   ��� ������� ��������� ������������
  TFinishedEvent = class(TEventHandler)
  public
    // ����� �������, ������������ ������������
    Index : Integer;
    constructor Create(Idx : Integer);
    procedure DefaultEventProc; override;
  end;

  // ����� TBank - ������ ����� ��� ��������������
  TBank = class(TSimulation)
  public
    // ������� � ��������
    Queue : array of TList;
    // ������������� �������
    Current : array of TClient;
    // ���������� �� ������������� ��������
    CashmenStat : array of TServiceStatistics;
    // ���������� �� ����� �������� � �����
    ClientStat : TActionStatistics;
    // ���������� �� ���������� ������� ����� ��������� ��������
    DepartStat : TTimeBetStatistics;
    // ���������� �� ������� ���������� �������� � �����
    TimeStat : TStatistics;
    // ���������� �� ���������� ������� ����� ��������
    BalksStat : TTimeBetStatistics;
    // ���������� ��������� ����� ���������
    JerkCount : Integer;
    // ���������� ��������, ��������� � ����
    IncomeCount : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndClient : TRandom;
  // ������������ ����� �������
  MaxQueueSize : Integer = 3;
  // ������� �������� �������� ��������
  MeanArrivalInterval : Double = 0.5;
  // ��������� ������� ������������
  MeanClientTime : Double = 1;
  DeviationClientTime : Double = 0.3;
  // ����������� ������� � ������ ��������, ����� ��� �������� �������
  MinQueueDiff : Integer = 2;
  // ���������� ����
  CashCount : Integer = 2;
  // �������� ���������� �������� � ������ �������
  InitClientCount : Integer = 2;
  // ����� �������� ������� �������
  FirstClientArrivalTime : Double = 0.1;
  // ����� �������������
  SimulationTime : Double = 1000;

implementation

{ TClient }

constructor TClient.Create(Start : Double);
begin
  StartingTime := Start;
end;

{ TArrivalEvent }

procedure TArrivalEvent.DefaultEventProc;
var
  par : TBank;
  Client : TClient;
  Index : Integer;
begin
  par := Parent as TBank;
  ClearFinished;
  // ������� ������ �������
  Client := TClient.Create(SimTime);
  Inc(par.IncomeCount);
  // ���� ��� ������� ��������� �� �������
  if (par.Queue[0].Size = MaxQueueSize) and
      (par.Queue[1].Size = MaxQueueSize) then
  begin
    // ������� ������� �� �������
    par.BalksStat.AddData(SimTime);
    Client.Free;
  end
  else
  begin
    // ��������� ����� �������� � �������
    par.ClientStat.Start(SimTime);
    // ������� ����� �������� �������
    if par.Queue[0].Size <= par.Queue[1].Size then
      Index := 0
    else
      Index := 1;
    // ��������� ������� � ���
    Client.Insert(par.Queue[Index]);
    // ���� ��������� ������ ��������
    if par.Current[Index] = nil then
    begin
      // ������� ������� �� ������� � ��������� ��� �� ������������
      Client.Remove;
      par.Current[Index] := Client;
      // ������ ������������
      par.CashmenStat[Index].Start(SimTime);
      // ������������� ������� ��������� ������������
      TFinishedEvent.Create(Index).
          ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime));
    end;
  end;
  // ������������� �������� ���������� �������
  TArrivalEvent.Create.
      ActivateDelay(rndClient.Exponential(MeanArrivalInterval));
  Finish;
end;

{ TFinishedEvent }

constructor TFinishedEvent.Create(Idx: Integer);
begin
  Index := Idx;
  inherited Create;
end;

procedure TFinishedEvent.DefaultEventProc;
var
  par : TBank;
  Client : TClient;
begin
  par := Parent as TBank;
  // ������� ���������� �� ������������ ������������
  Client := par.Current[Index];
  par.TimeStat.AddData(SimTime - Client.StartingTime);
  par.DepartStat.AddData(SimTime);
  par.CashmenStat[Index].Finish(SimTime);
  // ������� ������� �� �������
  par.ClientStat.Finish(SimTime);
  Client.Free;
  // ���� � ������� ������� ���� �������
  if par.Queue[index].Size > 0 then
  begin
    // ������� ������� � ��������� �� ������������
    Client := par.Queue[Index].First as TClient;
    par.Current[Index] := Client;
    Client.Remove;
    // ������ ������������
    par.CashmenStat[Index].Start(SimTime);
    // ������������� ������� ��������� ������������
    TFinishedEvent.Create(Index).
        ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime));
    // ���� � ������ ������� �������� ������, ��� � �������, ��� ������� �� 2
    if par.Queue[1 - Index].Size >= par.Queue[Index].Size + MinQueueDiff then
    begin
      // ��������� ������� �� ������ ������� � �������
      Client := par.Queue[1 - Index].Last as TClient;
      Client.Insert(par.Queue[Index]);
      Inc(par.JerkCount);
    end;
  end
  else
    // ���� �������� ���, ������ ��������
    par.Current[Index] := nil;
  Finish;
end;

{ TBank }

destructor TBank.Destroy;
var
  i : Integer;
begin
  for i := 0 to CashCount - 1 do
    Current[i].Free;
  SetLength(Current, 0);
  BalksStat.Free;
  for i := 0 to CashCount - 1 do
    CashmenStat[i].Free;
  SetLength(CashmenStat, 0);
  ClientStat.Free;
  DepartStat.Free;
  TimeStat.Free;
  for i := 0 to CashCount - 1 do
    Queue[i].Free;
  SetLength(Queue, 0);
  inherited;
end;

procedure TBank.Init;
var
  i : Integer;
begin
  inherited;
  BalksStat := TTimeBetStatistics.Create;
  SetLength(CashmenStat, CashCount);
  for i := 0 to CashCount - 1 do
    CashmenStat[i] := TServiceStatistics.Create;
  ClientStat := TActionStatistics.Create;
  SetLength(Current, CashCount);
  for i := 0 to CashCount - 1 do
    Current[i] := nil;
  DepartStat := TTimeBetStatistics.Create;
  JerkCount := 0;
  IncomeCount := 0;
  SetLength(Queue, CashCount);
  for i := 0 to CashCount - 1 do
    Queue[i] := TList.Create;
  TimeStat := TStatistics.Create;
end;

procedure TBank.RunSimulation;
var
  i, j : Integer;
begin
  // �������� �������� ������������ �������
  for i := 0 to CashCount - 1 do
  begin
    // ������� ������� � ��������� ��� �� ������������
    Current[i] := TClient.Create(0);
    Inc(IncomeCount);
    // ������������� ������� ��������� ������������
    TFinishedEvent.Create(i).
        ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime));
    CashmenStat[i].Start(0);
    ClientStat.Start(0);
    for j := 0 to InitClientCount - 1 do
    begin
      // ������� ������� � ��������� ��� � �������
      TClient.Create(0).Insert(Queue[i]);
      Inc(IncomeCount);
      ClientStat.Start(0);
    end;
  end;
  // ������������� �������� ���������� �������
  TArrivalEvent.Create.ActivateDelay(FirstClientArrivalTime);
  // ������� ��������� ��������
  Hold(SimulationTime);
  StopStat;
end;

procedure TBank.StopStat;
var
  i : Integer;
begin
  inherited;
  for i := 0 to CashCount - 1 do
    CashmenStat[i].StopStat(SimTime);
  ClientStat.StopStat(SimTime);
  for i := 0 to CashCount - 1 do
    Queue[i].StopStat(SimTime);
end;

end.
