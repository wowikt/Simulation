unit UTraffic;

interface
uses USimulation;

type
  // ����������� ��������
  TDirection = (dirLeftRight, dirRightLeft);

  // ����� TGenerator - ��������� �����������
  TGenerator = class(TProcess)
  public
    constructor Create(Dir : TDirection; Mean : Double);
  protected
    // ����������� �������� �����������
    Direction : TDirection;
    // ������� �������� ��������
    MeanInterval : Double;
    procedure RunProcess; override;
  end;

  // ����� TCar - ����������
  TCar = class(TProcess)
  public
    constructor Create(Dir : TDirection);
  protected
    // ����������� ��������
    Direction : TDirection;
    procedure RunProcess; override;
  end;

  // ����� TLights - �������, ����������� �����������
  TLights = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TTraffic - �������� �������������� ������� ������
  TTraffic = class(TSimulation)
  public
    // ���������� �����������
    Gen : array [TDirection] of TGenerator;
    // �������, ����������� �����������
    Lights : TLights;
    // �������, ����������� ������������ �����������
    LightRes : array [TDirection] of TResource;
    // �������, ������������ ���������
    LightGate : array [TDirection] of TGate;
    // ���������� �� ������� �������� ����� �����������
    WaitStat : array [TDirection] of TStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndCar : TRandom;
  // ����� ��������� ��������� ���������� ������� �����������
  OpenTime : array [TDirection] of Double = (60, 45);
  // ����� ����� ��������� ������� �����������
  CarPauseTime : Double = 2;
  // ������������ ������������ �������
  RedLightTime : Double = 55;
  // ������� ��������� ����� ��������� �����������
  MeanLeftInterval : Double = 9;
  MeanRightInterval : Double = 12;
  // ����� ��������
  SimulationTime : Double = 3600;

implementation

{ TGenerator }

constructor TGenerator.Create(Dir: TDirection; Mean: Double);
begin
  Direction := Dir;
  MeanInterval := Mean;
  inherited Create;
end;

procedure TGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // ������� ����������
    TCar.Create(Direction).ActivateDelay(0);
    // ��������� �������� ����������
    Hold(rndCar.Exponential(MeanInterval));
  end;
end;

{ TCar }

constructor TCar.Create(Dir: TDirection);
begin
  Direction := Dir;
  inherited Create;
end;

procedure TCar.RunProcess;
var
  par : TTraffic;
begin
  par := Parent as TTraffic;
  // ������� ����������� �������
  GetResource(par.LightRes[Direction]);
  // ������� ������������ ������� ���������
  WaitGate(par.LightGate[Direction]);
  // ������������� ���������� �� ������� ��������
  par.WaitStat[Direction].AddData(SimTime - StartingTime);
  // �������� ���� ���������
  Hold(CarPauseTime);
  // ��������� ������ ���������� ����������
  ReleaseResource(par.LightRes[Direction]);
  Finish;
end;

{ TLights }

procedure TLights.RunProcess;
var
  par : TTraffic;
begin
  par := Parent as TTraffic;
  while True do
  begin
    // ������� ���������� ������� �����������
    Hold(RedLightTime);
    // ������� �������� ��� �������� ����� �������
    OpenGate(par.LightGate[dirLeftRight]);
    // ������� �����
    Hold(OpenTime[dirLeftRight]);
    // ������� ��������
    CloseGate(par.LightGate[dirLeftRight]);
    // ������� ���������� ������� �����������
    Hold(RedLightTime);
    // ������� �������� ��� �������� ������ ������
    OpenGate(par.LightGate[dirRightLeft]);
    // ������� �����
    Hold(OpenTime[dirRightLeft]);
    // ������� ��������
    CloseGate(par.LightGate[dirRightLeft]);
  end;
end;

{ TTraffic }

destructor TTraffic.Destroy;
begin
  Gen[dirLeftRight].Free;
  Gen[dirRightLeft].Free;
  WaitStat[dirLeftRight].Free;
  WaitStat[dirRightLeft].Free;
  Lights.Free;
  LightGate[dirLeftRight].Free;
  LightGate[dirRightLeft].Free;
  LightRes[dirLeftRight].Free;
  LightRes[dirRightLeft].Free;
  inherited;
end;

procedure TTraffic.Init;
begin
  inherited;
  Gen[dirLeftRight] := TGenerator.Create(dirLeftRight, MeanLeftInterval);
  Gen[dirRightLeft] := TGenerator.Create(dirRightLeft, MeanRightInterval);
  LightGate[dirLeftRight] := TGate.Create;
  LightGate[dirRightLeft] := TGate.Create;
  LightRes[dirLeftRight] := TResource.Create;
  LightRes[dirRightLeft] := TResource.Create;
  Lights := TLights.Create;
  WaitStat[dirLeftRight] := TStatistics.Create;
  WaitStat[dirRightLeft] := TStatistics.Create;
end;

procedure TTraffic.RunSimulation;
begin
  Gen[dirLeftRight].ActivateDelay(0);
  Gen[dirRightLeft].ActivateDelay(0);
  Lights.ActivateDelay(0);
  // ��������� ��������� ��������
  Hold(SimulationTime);
  StopStat;
end;

procedure TTraffic.StopStat;
begin
  inherited;
  LightGate[dirLeftRight].StopStat(SimTime);
  LightGate[dirRightLeft].StopStat(SimTime);
  LightRes[dirLeftRight].StopStat(SimTime);
  LightRes[dirRightLeft].StopStat(SimTime);
end;

end.
