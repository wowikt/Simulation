unit UShop;

interface
uses USimulation, Graphics;

// ������������� ������ ��������

type
  // ����� TCustomer - ����������
  TCustomer = class(TProcess)
  public
    // ���������� �������
    BuysCount : Integer;
    CurrentBuys : Double;
    X : Double;
    Y : Double;
    dx : Double;
    dy : Double;
    Color : TColor;
    BuysColor : TColor;
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
    InShop : TList;
    Generator : TGenerator;
    Cash : TCashman;
    CashStat : TServiceStatistics;
    TimeStat : TStatistics;
    CashCustomer : TCustomer;
    CashStart : Double;
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
  // ������� �������� �������� �����������
  MeanCustInterval : Double = 2;
  // ������� ����� ��������� ����� �������
  TimePerBuy : Double = 0.2;
  // ������ ����� �������������
  SimulationTime : Double = 480;
  VisTimeStep : Double = 0.04;
  SpriteX : Integer = 48;
  SpriteY : Integer = 48;
  MinX : Integer;
  MinY : Integer;
  MaxX : Integer;
  MaxY : Integer;
  CashX : Integer;
  CashY : Integer;
  StartX : Integer;
  StartY : Integer;
  BuysLeft : Integer = 24;
  BuysRight : Integer = 45;
  BuysBottom : Integer = 45;
  BuysMaxHeight : Integer = 42;
  SpriteStep : Integer = 52;
  DeviationX : Double = 5;
  DeviationY : Double = 10;
  MaxDDX : Double = 5;
  MaxDDY : Double = 5;

implementation
uses Windows;

{ TCustomer }

procedure TCustomer.RunProcess;
var
  par : TShop;
begin
  par := Parent as TShop;
  Insert(par.InShop);
  X := StartX;
  Y := StartY;
  dx := 0;
  dy := 0;
  CurrentBuys := 0;
  Color := RGB(rndService.NextInt(256), rndService.NextInt(256),
      rndService.NextInt(256));
  BuysColor := RGB(rndService.NextInt(256), rndService.NextInt(256),
      rndService.NextInt(256));
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
      par.CashCustomer := Cust;
      par.CashStart := SimTime;
      // ������ �����
      par.CashStat.Start(SimTime);
      // ���������� ����������
      Hold(rndService.Erlang(TimePerBuy, Cust.BuysCount));
      // ���������� �������� - ������ ��������
      par.CashStat.Finish(SimTime);
      par.TimeStat.AddData(SimTime - Cust.StartingTime);
      par.CashCustomer := nil;
      // ������������ ��� ����������
      Cust.ActivateDelay(0);
    end;
    if (SimTime >= SimulationTime) and (par.InShop.Size = 0) then
      par.ActivateDelay(0);
    // ����� ���������� ����������
    Passivate;
  end;
end;

{ TGenerator }

procedure TGenerator.RunProcess;
begin
  while SimTime < SimulationTime do
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
  InShop.Free;
  inherited;
end;

procedure TShop.Init;
begin
  inherited;
  CashStat := TServiceStatistics.Create(1);
  PeopleStat := TActionStatistics.Create;
  Queue := TList.Create;
  InShop := TList.Create;
  Cash := TCashman.Create;
  Generator := TGenerator.Create;
  TimeStat := TStatistics.Create;
  CashCustomer := nil;
  MakeVisualizator(VisTimeStep);
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