unit UBank;

interface
uses USimulation;

type
  // ����� TClient ������������ �������, �������������� � �����
  TClient = class(TEventHandler)
  public
    // ����� �������, �������������� �������
    Index : Integer;
    StartingTime : Double;
    procedure DefaultEventProc; override;
    procedure ServiceFinished;
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

procedure TClient.DefaultEventProc;
var
  par : TBank;
begin
  par := Parent as TBank;
//  ClearFinished;
  // ������ ����� ������
  StartingTime := SimTime;
  if StartingTime < 10 then
    WriteLn('New client at ', StartingTime : 5 : 3);
  Inc(par.IncomeCount);
  // ���� ��� ������� ��������� �� �������
  if (par.Queue[0].Size = MaxQueueSize) and
      (par.Queue[1].Size = MaxQueueSize) then
  begin
    // ������� ������� �� �������
    par.BalksStat.AddData(SimTime);
    if StartingTime < 10 then
      WriteLn('Balked.');
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
    if SimTime < 10 then
      WriteLn('Inserted in queue ', Index);
    // ������ � ���
    Insert(par.Queue[Index]);
    // ���� ��������� ������ ��������
    if par.Current[Index] = nil then
    begin
      // ������� ������� �� ������� � ��������� ��� �� ������������
      Remove;
      par.Current[Index] := Self;
      // ������ ������������
      par.CashmenStat[Index].Start(SimTime);
      // ������������� ������� ��������� ������������
      ReactivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime),
          ServiceFinished);
    end
    else
      Suspend;
  end;
  // ������������� �������� ���������� �������
  TClient.Create.ActivateDelay(rndClient.Exponential(MeanArrivalInterval));
end;

procedure TClient.ServiceFinished;
var
  par : TBank;
  Client : TClient;
  st : Double;
begin
  par := Parent as TBank;
  // ������� ���������� �� ������������ ������������
  st := SimTime;
  par.TimeStat.AddData(SimTime - StartingTime);
  par.DepartStat.AddData(SimTime);
  par.CashmenStat[Index].Finish(st);
  if st < 10 then
    WriteLn('Client at index ', Index, ' finished at ', st : 5 : 3);
  // ������� ������� �� �������
  par.ClientStat.Finish(SimTime);
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
    Client.
        ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime),
        Client.ServiceFinished);
    // ���� � ������ ������� �������� ������, ��� � �������, ��� ������� �� 2
    if par.Queue[1 - Index].Size >= par.Queue[Index].Size + MinQueueDiff then
    begin
      // ��������� ������� �� ������ ������� � �������
      Client := par.Queue[1 - Index].Last as TClient;
      Client.Insert(par.Queue[Index]);
      Client.Index := Index;
      Inc(par.JerkCount);
    end;
  end
  else
    // ���� �������� ���, ������ ��������
    par.Current[Index] := nil;
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
  clt : TClient;
begin
  // �������� �������� ������������ �������
  for i := 0 to CashCount - 1 do
  begin
    // ������� ������� � ��������� ��� �� ������������
    Current[i] := TClient.Create;
    Current[i].StartingTime := 0;
    Current[i].Index := i;
    Inc(IncomeCount);
    // ������������� ������� ��������� ������������
    Current[i].
        ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime),
        Current[i].ServiceFinished);
    CashmenStat[i].Start(0);
    ClientStat.Start(0);
    for j := 0 to InitClientCount - 1 do
    begin
      // ������� ������� � ��������� ��� � �������
      clt := TClient.Create;
      clt.Insert(Queue[i]);
      clt.StartingTime := 0;
      clt.Index := i;
      Inc(IncomeCount);
      ClientStat.Start(0);
    end;
  end;
  // ������������� �������� ���������� �������
  TClient.Create.ActivateDelay(FirstClientArrivalTime);
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
