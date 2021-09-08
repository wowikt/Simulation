unit UGarage;

interface
uses USimulation;

// ������������� ������� ������������ ������������ �����������

type
  // ����� TCar - ������������� ����������
  TCar = class(TProcess)
  protected
    procedure RunProcess; override;
  public
    WaitingStart : Double;
    CarNum : Integer;
    constructor Create;
  end;

  // ����� TBrigade - ������������� �������
  TBrigade = class(TProcess)
  protected
    procedure RunProcess; override;
  public
    constructor Create;
  end;

  // ����� TGarage - ������� ���������
  TGarage = class(TSimulation)
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  public
    Brigade : TBrigade;
    Parking : TList;
    NotServiced : TList;
    BrigadeStat : TServiceStatistics;
    constructor Create;
    destructor Destroy; override;
  end;

var
  ModelingTime,             // ����� �������������
  MeanServiceTime,          // ������� ����� ������������
  DeviationServiceTime,     // ����������� ���������� ������� ������������
  CarsPerTimeUnit : Double; // ������������� ������ ����������� �����������
  CarCount,                 // ������� �����������
  ParkingPlaces,            // ����� ���� �� �������
  NoWaitCount : Integer;    // ����� �����������, ����������� ��� ��������

  // ���������� ��������� �����
  rndCar,                   // ��� ��������� ��������
  rndService : TRandom;     // ��� ������� ������������   

implementation

{ TCar }

constructor TCar.Create;
begin
  inherited;
end;

procedure TCar.RunProcess;
var
  par : TGarage;
begin
  par := Parent as TGarage;
  // �������� ����� ����������
  Inc(CarCount);
  CarNum := CarCount;

  // ���� �� ������� ���� �����
  if par.Parking.Size < ParkingPlaces then
  begin
    // ������ � ������� �� ������������
    InsertLast(par.Parking);

    WaitingStart := SimTime;

    // ������������ �������
    ActivateAfter(par.Brigade, Self);
  end
  else
    // ������ � ������ �������������
    InsertLast(par.NotServiced);

  // ������������� �������� ���������� ����������
  ActivateDelay(TCar.Create, rndCar.NegExp(CarsPerTimeUnit));
end;

{ TBrigade }

constructor TBrigade.Create;
begin
  inherited;
end;

procedure TBrigade.RunProcess;
var
  WorkStart, Waited : Double;
  Client : TCar;
  par : TGarage;
begin
  par := Parent as TGarage;
  StopRunning;
  while True do
  begin
    // ������� ��� ������������� (���� ����)
    par.NotServiced.Clear;

    // ���� ������� �����
    while par.Parking.Empty do
      // ������� �������� ����������
      Passivate;

    // ��������� ����� ������ ������
    par.BrigadeStat.Start(SimTime);

    // ������� �� ������� ������� �������
    Client := (Parent as TGarage).Parking.First as TCar;

    // ����� �������� ������� �� ������ ������������
    Waited := SimTime - Client.WaitingStart;
    Client.Free;

    // ���� �� ����
    if Waited = 0 then
      // ��������� ������� �� ���������
      Inc(NoWaitCount);

    // ������������
    Hold(rndService.Normal(MeanServiceTime, DeviationServiceTime));

    // ��������� ����� ������������
    par.BrigadeStat.Finish(SimTime);
  end;
end;

{ TGarage }

constructor TGarage.Create;
begin
  inherited;
end;

destructor TGarage.Destroy;
begin
  Brigade.Free;
  BrigadeStat.Free;
  Parking.Free;
  NotServiced.Free;
  inherited;
end;

procedure TGarage.Init;
begin
  inherited;
  Brigade := TBrigade.Create;
  Parking := TList.Create(SimTime);
  NotServiced := TList.Create;
  BrigadeStat := TServiceStatistics.Create(1, 0, 0);
  CarCount := 0;
  NoWaitCount := 0;
end;

procedure TGarage.RunSimulation;
begin
  // ��������� ������ ���������� � �������
  Activate(TCar.Create);

  // �������� �� ����� �������������
  Hold(ModelingTime);
  // ������������� ����������
  BrigadeStat.StopStat(SimTime);
  Parking.LengthStat.StopStat(SimTime);

  Detach;
end;

end.
