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
    Cash : TCashman;
    CashStat : TServiceStatistics;
    TimeStat : TStatistics;
    // ���������� �� ���������� ����������� � �������� ����
    PeopleStat : TActionStatistics;
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
  // ��������� �������� ����� �������
  MinBuysCount : Integer = 2;
  MaxBuysCount : Integer = 16;
  // ������� �������� ��������� �����������
  MeanCustInterval : Double = 2;
  // ������� ����� ��������� ����� �������
  TimePerBuy : Double = 0.2;
  // ������ ����� �������������
  SimulationTime : Double = 480;

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
  par.PeopleStat.Finish(SimTime);
  // ������������ �������
  par.Cash.ActivateDelay(0);
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
      // ���������� �������� - ������ ��������
      par.CashStat.Finish(SimTime);
      par.TimeStat.AddData(SimTime - Cust.StartingTime);
      // ������������ ��� ����������
      Cust.ActivateDelay(0);
    end;
    // ����� ���������� ����������
    Passivate;
  end;
end;

{ TGenerator }

procedure TGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // ������� ������ ���������� � ��������� ��� � �������
    TCustomer.Create.ActivateDelay(0);
    // ��������� ����� ��������� ����������
    Hold(rndCust.Exponential(MeanCustInterval));
  end;
end;

{ TShop }

destructor TShop.Destroy;
begin
  Cash.Free;
  Generator.Free;
  CashStat.Free;
  TimeStat.Free;
  PeopleStat.Free;
  Queue.Free;
  inherited;
end;

procedure TShop.Init;
begin
  inherited;
  CashStat := TServiceStatistics.Create(1);
  PeopleStat := TActionStatistics.Create;
  Queue := TList.Create;
  Cash := TCashman.Create;
  Generator := TGenerator.Create;
  TimeStat := TStatistics.Create;
end;

procedure TShop.RunSimulation;
begin
  // ��������� ���������
  Generator.ActivateDelay(0);
  // ����� ��������� �������������
  Hold(SimulationTime);
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
