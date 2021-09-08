unit USavings;

interface
uses USimulation;

type
  TCustomerGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TCustomer = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TCheck = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TOrder = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TSavings = class(TSimulation)
  public
    // ������ �� ��������� � ������� ����������
    Radios : TResource;
    // ���������� �� ���������� ����� �������� �� �������
    LostSalesStat : TTimeBetStatistics;
    // ���������� �� ������� �� ������ ���������� ������
    SafetyStockStat : TStatistics;
    // ���������� �� ������� ���������� �� ������
    InvPosStat : TIntervalStatistics;
    // ��������� �����������
    CustGen : TCustomerGenerator;
    // ������� �������� ��������� ������
    Check : TCheck;
    // ������� ���������� ����������
    CurrentAvailable : Integer;
    // ���������� ��������� �������
    BuysCount : Integer;
    // ���������� �� �����������, ��������� ��������������� �����
    WaitStat : TStatistics;
    // ������� ���������� ������ ����������
    Order : TOrder;
    destructor Destroy; override;
    procedure StopStat; override;
    procedure ClearStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  rndCustomer : TRandom;
  rndOrder : TRandom;
  // ������ ������� ������
  OrderLimit : Integer = 18;
  // ������� ������� ������
  InvPosition : Integer = 72;
  // ������� �������� �������� �����������
  MeanCustTime : Double = 0.2;
  // ����������� �������� ������������� ������
  OrderProb : Double = 0.2;
  // �������� ��������
  CheckInterval : Double = 4;
  // ����� ���������� ������
  MeanOrderTime : Double = 3;
  DeviationOrderTime : Double = 1;
  MinOrderTime : Double = 2;
  // ����� ������������� �� ������� ���������� = 1 ���
  PreSimulationTime : Double = 52;
  // ����� ��������� ������������� = 5 ���
  MainSimulationTime : Double = 260;
  VisTimeStep : Double = 0.1;

implementation

{ TCustomerGenerator }

procedure TCustomerGenerator.RunProcess;
begin
  while True do
  begin
    // ������� ����������� �������� (���� ����)
    ClearFinished;
    // ������� ����������
    TCustomer.Create.ActivateDelay(0);
    // ���������� ��������� � ��������������� �������������� ����������
    Hold(rndCustomer.Exponential(MeanCustTime));
  end;
end;

{ TCustomer }

procedure TCustomer.RunProcess;
var
  par : TSavings;
begin
  par := Parent as TSavings;
  // ���������� ����������� ��������, ����:
  //   1. ��������� ������� � �������
  //   2. ���������� ���, ����� �� � ������������ OrderProb
  //      ������ � ������� ��������
  if (par.Radios.Available > 0) or rndCustomer.Draw(OrderProb) then
  begin
    // ��������� ������� ���������� ����������
    //   (������������� ����� �������� ������� �������, ��������� ����������)
    Dec(par.CurrentAvailable);
    par.InvPosStat.AddData(par.CurrentAvailable, SimTime);
    // ���������� �������� ��� �����, ���� ��� �� �������� � �������
    GetResource(par.Radios);
    // ������� ���������
    Inc(par.BuysCount);
    // ���� ���������� ������ ������, ������ ���
    if SimTime > StartingTime then
      par.WaitStat.AddData(SimTime - StartingTime);
  end
  else
    // ������������� ���������� �� �������
    par.LostSalesStat.AddData(SimTime);
  // ������ � ������� ����������� ���������
  Finish;
end;

{ TCheck }

procedure TCheck.RunProcess;
var
  par : TSavings;
begin
  par := Parent as TSavings;
  while True do
  begin
    // ���� ������� ���������� ���������� ������ ��������� ����������� 
    if par.CurrentAvailable < OrderLimit then
      // ��������� ������� ���������� ������ ����������
      par.Order.ActivateDelay(0);
    Hold(CheckInterval);
  end;
end;

{ TOrder }

procedure TOrder.RunProcess;
var
  par : TSavings;
  OrderCount : Integer;
begin
  par := Parent as TSavings;
  while True do
  begin
    // ��������� ����� ������ ����������
    OrderCount := InvPosition - par.CurrentAvailable;
    // ������������ ������� ����������
    par.CurrentAvailable := InvPosition;
    par.InvPosStat.AddData(par.CurrentAvailable, SimTime);
    // ��������� ���������� ������
    Hold(Max(MinOrderTime, rndOrder.Normal(MeanOrderTime, DeviationOrderTime)));
    // ������������� ���������� �� ���������� ������
    par.SafetyStockStat.AddData(par.Radios.Available);
    // ������������ ���������� �������
    ReleaseResource(par.Radios, OrderCount);
    // ������� ���������� ������
    Passivate;
  end;
end;

{ TSavings }

procedure TSavings.ClearStat;
begin
  inherited;
  InvPosStat.Clear(SimTime);
  SafetyStockStat.Clear;
  LostSalesStat.Clear;
  Radios.ClearStat(SimTime);
  WaitStat.Clear;
  BuysCount := 0;
end;

destructor TSavings.Destroy;
begin
  Check.Free;
  CustGen.Free;
  Order.Free;
  InvPosStat.Free;
  SafetyStockStat.Free;
  LostSalesStat.Free;
  WaitStat.Free;
  Radios.Free;
  inherited;
end;

procedure TSavings.Init;
begin
  inherited;
  Check := TCheck.Create;
  CustGen := TCustomerGenerator.Create;
  CurrentAvailable := InvPosition;
  InvPosStat := TIntervalStatistics.Create(CurrentAvailable);
  SafetyStockStat := TStatistics.Create;
  LostSalesStat := TTimeBetStatistics.Create;
  Radios := TResource.Create(InvPosition);
  Order := TOrder.Create;
  WaitStat := TStatistics.Create;
  BuysCount := 0;
  MakeVisualizator(VisTimeStep);
end;

procedure TSavings.RunSimulation;
begin
  CustGen.ActivateDelay(0);
  Check.ActivateDelay(0);
  // ��������������� ��������
  Hold(PreSimulationTime);
  // ������� ����������
  ClearStat;
  // ����������� ��������
  Hold(MainSimulationTime);
  // ��������� ����������
  StopStat;
end;

procedure TSavings.StopStat;
begin
  inherited;
  InvPosStat.StopStat(SimTime);
  Radios.StopStat(SimTime);
end;

end.
