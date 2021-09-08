unit UShop;

interface
uses USimulation;

// ������������� ������ ��������

type
  // ����� TCustomer - ����������
  TCustomer = class(TProcess)
  public
    // ���������� �������
    BuysCount : Integer;
  protected
    procedure RunProcess; override;
  end;

  // ����� TCashman - ������
  TCashman = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TGenerator - ��������� �����������
  TGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TShop - �������� ������ ��������
  TShop = class(TSimulation)
  public
    Queue : TList;
    Generator : TGenerator;
    Cash : array of TProcess;
    CashStat : TServiceStatistics;
    TimeStat : TStatistics;
    // ���������� �� ���������� ����������� � �������� ����
    PeopleStat : TActionStatistics;
    TotalBuys : Integer;
    CanceledCust : Integer;
    CanceledBuys : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  // ���������� ��������� �����
  rndCust,                 // �������� �����������
  rndService : TRandom;    // ����� ������������
  // ������� ������� ������ �������
  MinShoppingTime : Double = 2;
  MaxShoppingTime : Double = 12;
  // ������� ���������� ������� �� ��������� ����
  MinSecondShoppingTime : Double = 2;
  MaxSecondShoppingTime : Double = 6;
  // ��������� �������� ����� �������
  MinBuysCount : Integer = 2;
  MaxBuysCount : Integer = 16;
  // ������������ �������� ���������� ����� �������
  MaxSecondBuysCount : Integer = 3;
  // ������� �������� ��������� �����������
  MeanCustInterval : Double = 1.3;
  // ������� ����� ��������� ����� �������
  TimePerBuy : Double = 0.4;
  // ������ ����� �������������
  SimulationTime : Double = 480;
  // ���������� ����
  CashCount : Integer = 3;
  VisTimeStep : Double = 0.5;
  // ����������� ������
  CancelProb : Double = 0.2;
  // ����������� �������������� �������
  ExtraBuyProb : Double = 0.3;
  // ������� ����� ������������ �������������� �������
  ExtraBuyTime : Double = 0.5;

implementation

{ TCustomer }

procedure TCustomer.RunProcess;
var
  par : TShop;
begin
  par := Parent as TShop;
  StartRunning;
  // ������ ����� �������
  BuysCount := rndService.NextInt(MinBuysCount, MaxBuysCount);
  // ����� �������

  par.PeopleStat.Start(SimTime);
  Hold(rndService.Uniform(MinShoppingTime, MaxShoppingTime));
  // ���� ������� ����� ������������ ������
  while par.Queue.Size = 10 do
    // ���� ����� �� �������
    if rndCust.Draw(CancelProb) then
    begin
      Inc(par.CanceledCust);
      Inc(par.CanceledBuys, BuysCount);
      par.PeopleStat.Finish(SimTime);
      Finish;
      Exit;
    end
    // ���� ��������� ������ �� ��������� ����
    else
    begin
      Hold(rndService.Uniform(MinSecondShoppingTime, MaxSecondShoppingTime));
      Inc(BuysCount, rndCust.NextInt(MaxSecondBuysCount));
    end;
  par.PeopleStat.Finish(SimTime);
  // ������������ ��������
  ActivateDelay(par.Cash, 0);
  // ������ � ������� � �����
  Wait(par.Queue);
  // �� ��������� ������������ ��������� ������
  Finish;
end;

{ TCashman }

procedure TCashman.RunProcess;
var
  Cust : TCustomer;
  par : TShop;
begin
  par := Parent as TShop;
  while True do
  begin
    while not par.Queue.Empty do
    begin
      // ���������� � ���������� ����������
      Cust := par.Queue.First as TCustomer;
      // ������� �� �������
      Cust.StartRunning;
      // ������ �����
      par.CashStat.Start(SimTime);
      // ���������� ����������
      Hold(rndService.Erlang(TimePerBuy, Cust.BuysCount));
      Inc(par.TotalBuys, Cust.BuysCount);
      // ���� ����������� �������������� �������
      if rndCust.Draw(ExtraBuyProb) then
      begin
        Hold(rndService.Exponential(ExtraBuyTime));
        Inc(par.TotalBuys);
      end;
      // ���������� �������� - ������ ��������
      par.CashStat.Finish(SimTime);
      par.TimeStat.AddData(SimTime - Cust.StartingTime);
      // ������������ ��� ����������
      Cust.ActivateDelay(0);
    end;
    if (SimTime >= SimulationTime) and (par.PeopleStat.Running = 0) and
        (par.CashStat.Running = 0) then
      par.ActivateDelay(0);
    // ����� ���������� ����������
    Passivate;
  end;
end;

{ TGenerator }

procedure TGenerator.RunProcess;
var
  StartCust, i : Integer;
  par : TShop;
begin
  par := Parent as TShop;
  StartCust := rndCust.Poisson(3);
  for i := 1 to StartCust - 1 do
    TCustomer.Create.ActivateDelay(0);
  while SimTime < SimulationTime do
  begin
    ClearFinished;
    // ������� ������ ���������� � ��������� ��� � �������
    TCustomer.Create.ActivateDelay(0);
    // ��������� ����� ��������� ����������
    Hold(rndCust.Exponential(MeanCustInterval));
  end;
  if par.Queue.Empty and (par.PeopleStat.Running = 0) and
      (par.CashStat.Running = 0) then
    par.ActivateDelay(0);
end;

{ TShop }

destructor TShop.Destroy;
var i : Integer;
begin
  for i := 0 to CashCount - 1 do
    Cash[i].Free;
  Generator.Free;
  CashStat.Free;
  TimeStat.Free;
  PeopleStat.Free;
  Queue.Free;
  inherited;
end;

procedure TShop.Init;
var
  i : Integer;
begin
  inherited;
  CashStat := TServiceStatistics.Create(CashCount);
  PeopleStat := TActionStatistics.Create;
  Queue := TList.Create;
  SetLength(Cash, CashCount);
  for i := 0 to CashCount - 1 do
    Cash[i] := TCashman.Create;
  Generator := TGenerator.Create;
  TimeStat := TStatistics.Create;
  MakeVisualizator(VisTimeStep);
  CanceledCust := 0;
  CanceledBuys := 0;
  TotalBuys := 0;
end;

procedure TShop.RunSimulation;
begin
  // ��������� ���������
  Generator.ActivateDelay(0);
  // ����� ��������� �������������
  Passivate;
  // ��������� ����������
  StopStat;
end;

procedure TShop.StopStat;
begin
  inherited;
  CashStat.StopStat(SimTime);
  Queue.StopStat(SimTime);
  PeopleStat.StopStat(SimTime);
end;

end.