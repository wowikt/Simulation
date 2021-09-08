unit UPort;

interface
uses USimulation;

type
  // �������, ����������� ������ �������
  TTanker = class(TProcess)
  public
    // ��� �������
    TankerType : Integer;
    constructor Create(TType : Integer);
  protected
    procedure RunProcess; override;
  private
    // ���������� ��������� �������� ������� � �������
    function GetBerthAndTug : Boolean;
  end;

  // ��������� �������� 0-2 �����
  TTankerGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ���������� �������� 3 ����
  TTanker3Generator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ������� ������
  TStorm = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // �������� ������ �����
  TPort = class(TSimulation)
  public
    // �������
    Berth : TResource;
    // ������
    Tug : TResource;
    // ���������� ��������
    Gen : TTankerGenerator;
    Gen3 : TTanker3Generator;
    // �����
    Storm : TStorm;
    // ���������� �� ������� ���������� �������� �� �����
    TimeStat : array of TStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

const
  TypeCount = 4;

var
  rndTanker : TRandom;
  rndStorm : TRandom;
  // ������� ������� �������� �� �����
  MinLoadingTime : array [0 .. TypeCount - 1] of Double = (16, 21, 32, 18);
  MaxLoadingTime : array [0 .. TypeCount - 1] of Double = (20, 27, 40, 24);
  // ������� ������� �������� �������� ���� 3
  MinTripTime : Double = 216;
  MaxTripTime : Double = 264;
  // ����������� ������� �������� ����� 0-2
  TypeProb : array [0 .. TypeCount - 3] of Double = (0.25, 0.8);
  // ������� ��������� ������� ����� ��������� ��������
  MinInterval : Double = 4;
  MaxInterval : Double = 18;
  // �������� ��������������� �������� �������� ���� 3
  Interval3 : Double = 48;
  // ���������� �������� ���� 3
  Type3Count : Integer = 5;
  // �������� ����� ��������
  StormInterval : Double = 48;
  // ������� ������� ������
  MinStormTime : Double = 2;
  MaxStormTime : Double = 6;
  // ���������� ��������
  BerthCount : Integer = 3;
  // ����� �������� (1 ���)
  SimulationTime : Double = 8760;

implementation

{ TTanker }

constructor TTanker.Create(TType: Integer);
begin
  TankerType := TType;
  inherited Create;
end;

function TTanker.GetBerthAndTug: Boolean;
var
  par : TPort;
begin
  par := Parent as TPort;
  // ���������� ��������� �������� ������� � �������
  // ���� ��� ������� ������� � �������
  if (par.Berth.Available > 0) and (par.Tug.Available > 0) then
  begin
    // �������� �� 1 ������� �������
    par.Berth.Get;
    par.Tug.Get;
    // ���������� True
    Result := True;
  end
  else
    // �������� �������� ��� � �������, ���������� False
    Result := False;
end;

procedure TTanker.RunProcess;
var
  par : TPort;
  ArriveTime : Double;
begin
  par := Parent as TPort;
  while True do
  begin
    // ��������� ����� ��������
    ArriveTime := SimTime;
    // �������� ��������� ������ � ������
    GetResource(par.Tug, GetBerthAndTug);
    // ��������� ������������
    Hold(1);
    // ���������� ������
    ReleaseResource(par.Tug);
    // ��������� ��������. ����� ���������� ������� �� ���� �������
    Hold(rndTanker.Uniform(MinLoadingTime[TankerType],
        MaxLoadingTime[TankerType]));
    // �������� ������ ��� �����������
    GetResource(par.Tug, 1, 1);
    // ��������� �����������
    Hold(1);
    // ���������� ������
    ReleaseResource(par.Berth);
    // ���������� ������
    ReleaseResource(par.Tug);
    // ������������� ���������� ������� ���������� �� ���� �������
    par.TimeStat[TankerType].AddData(SimTime - ArriveTime);
    // ���� ������ ���� 0-2, ��������� ������
    if TankerType < TypeCount - 1 then
      Break;
    // ��������� ������� �� ��������� � �������
    Hold(rndTanker.Uniform(MinTripTime, MaxTripTime));
  end;
  Finish;
end;

{ TTankerGenerator }

procedure TTankerGenerator.RunProcess;
var
  TType : Integer;
begin
  while True do
  begin
    // �������� ������ ����������� ���������
    ClearFinished;
    // �������� ������� ��� �������
    TType := rndTanker.TableIndex(TypeProb);
    // ������� ������
    TTanker.Create(TType).ActivateDelay(0);
    // ������� �������� ���������� �������
    Hold(rndTanker.Uniform(MinInterval, MaxInterval));
  end;
end;

{ TTanker3Generator }

procedure TTanker3Generator.RunProcess;
var
  i : Integer;
begin
  // ����� ��������� 5 �������� 3 ����
  for i := 1 to Type3Count do
  begin
    // ������� ������ 3 ����
    TTanker.Create(3).ActivateDelay(0);
    // ������� �������� ���������� �������
    Hold(Interval3);
  end;
end;

{ TStorm }

procedure TStorm.RunProcess;
var
  par : TPort;
begin
  par := Parent as TPort;
  while True do
  begin
    // ����� �� ���������� ������
    Hold(rndStorm.Exponential(StormInterval));
    // ������� ������ ��������� �� ����� ������
    ChangeResource(par.Tug, -1);
    // ������������ ������
    Hold(rndStorm.Uniform(MinStormTime, MaxStormTime));
    // ���������� ������ ����� ������
    ChangeResource(par.Tug, 1);
  end;
end;

{ TPort }

destructor TPort.Destroy;
var
  i : Integer;
begin
  Gen.Free;
  Gen3.Free;
  Storm.Free;
  Berth.Free;
  Tug.Free;
  for i := 0 to TypeCount - 1 do
    TimeStat[i].Free;
  inherited;
end;

procedure TPort.Init;
var
  i : Integer;
begin
  inherited;
  Berth := TResource.Create(BerthCount);
  Tug := TResource.Create(1, 0, 2);
  Gen := TTankerGenerator.Create;
  Gen3 := TTanker3Generator.Create;
  Storm := TStorm.Create;
  SetLength(TimeStat, TypeCount);
  for i := 0 to TypeCount - 1 do
    TimeStat[i] := TStatistics.Create;
end;

procedure TPort.RunSimulation;
begin
  // ��������� ��������
  Gen.ActivateDelay(0);
  Gen3.ActivateDelay(0);
  Storm.ActivateDelay(0);
  // ����� �������� (1 ���)
  Hold(SimulationTime);
  StopStat;
end;

procedure TPort.StopStat;
begin
  inherited;
  Berth.StopStat(SimTime);
  Tug.StopStat(SimTime);
end;

end.
