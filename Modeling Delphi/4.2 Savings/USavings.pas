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
    destructor Destroy; override;
    procedure StopStat; override;
    procedure ClearStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  rndCustomer : TRandom;
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
  OrderTime : Double = 3;
  // ����� ������������� �� ������� ���������� = 1 ���
  PreSimulationTime : Double = 52;
  // ����� ��������� ������������� = 5 ���
  MainSimulationTime : Double = 260;

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
  OrderCount : Integer;
begin
  par := Parent as TSavings;
  while True do
  begin
    // ���� ������� ���������� ���������� ������ ��������� ����������� 
    if par.CurrentAvailable < OrderLimit then
    begin
      // ��������� ����� ������ �������������
      OrderCount := InvPosition - par.CurrentAvailable;
      // ������������ ������� ����������
      par.CurrentAvailable := InvPosition;
      par.InvPosStat.AddData(par.CurrentAvailable, SimTime);
      // ��������� ���������� ������
      Hold(OrderTime);
      // ������������� ���������� �� ���������� ������
      par.SafetyStockStat.AddData(par.Radios.Available);
      // ������������ ���������� �������
      ReleaseResource(par.Radios, OrderCount);
      // ������� ���������� ����� �� ��������� ��������
      Hold(CheckInterval - OrderTime);
    end
    else
      // ����� ��������� ��������
      Hold(CheckInterval);
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
end;

destructor TSavings.Destroy;
begin
  Check.Free;
  CustGen.Free;
  InvPosStat.Free;
  SafetyStockStat.Free;
  LostSalesStat.Free;
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
