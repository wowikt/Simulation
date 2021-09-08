unit UBank;

interface
uses USimulation;

type
  // ����� TClient - �������, ������������ ������� �����
  TClient = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TClientGenerator - �������, ����������� �������� �����
  TClientGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TBankSimulation - ������������� ������ �����
  TBankSimulation = class(TSimulation)
  public
    // ������ �������
    Cashman : TResource;
    // ��������� ��������
    Generator : TClientGenerator;
    // ���������� � ����������� �� ������� ����������
    //   �������� � �����
    InBankTime : TStatistics;
    InBankHist : THistogram;
    // ������� ��������, ����������� ��� ��������
    NotWaited : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  // ������� ��������� �����
  rndClient : TRandom;
  rndCashman : TRandom;
  // ���������� ������������� ��������
  MaxClientCount : Integer = 100;
  // ������� �������� ����� ��������� ��������
  MeanClientInterval : Double = 5;
  // ������� ������� ������������
  MinCashTime : Double = 2;
  MaxCashTime : Double = 6;
  // ��������� �����������
  HistMin : Double = 2;
  HistStep : Double = 2;
  HistStepCount : Integer = 20;

implementation

{ TClient }

procedure TClient.RunProcess;
var
  par : TBankSimulation;
  InTime : Double;
begin
  par := Parent as TBankSimulation;

  // ��������� ������������ �������
  GetResource(par.Cashman);

  // ���������� � ���������� ��������
  // ���� ������ �� ����, ������ ���
  if StartingTime = SimTime then
    Inc(par.NotWaited);

  // ��������� ������������
  Hold(rndCashman.Uniform(MinCashTime, MaxCashTime));

  // ������ ������ ����� ���������� � �����
  InTime := SimTime - StartingTime;
  par.InBankTime.AddData(InTime);
  par.InBankHist.AddData(InTime);

  // ���� ��� ������� ���������, ��������� ������
  if par.InBankTime.Count = MaxClientCount then
    par.ActivateDelay(0);
  ReleaseResource(par.Cashman);

  // ������ � ������� ����������� ���������
  Finish;
end;

{ TClientGenerator }

procedure TClientGenerator.RunProcess;
var
  i : Integer;
begin
  for i := 1 to MaxClientCount do
  begin
    ClearFinished;
    TClient.Create.ActivateDelay(0);
    Hold(rndClient.Exponential(MeanClientInterval));
  end;
end;


{ TBankSimulation }

destructor TBankSimulation.Destroy;
begin
  Generator.Free;
  InBankTime.Free;
  InBankHist.Free;
  Cashman.Free;
  inherited;
end;

procedure TBankSimulation.Init;
begin
  inherited;
  Cashman := TResource.Create;
  Generator := TClientGenerator.Create;
  InBankTime := TStatistics.Create;
  InBankHist := TUniformHistogram.Create(HistMin, HistStep, HistStepCount);
  NotWaited := 0;
end;

procedure TBankSimulation.RunSimulation;
begin
  // ��������� ������� �������� ��������
  Generator.ActivateDelay(0);
  // ����� ����� ��������
  Passivate;
  StopStat;
end;

procedure TBankSimulation.StopStat;
begin
  inherited;
  Cashman.StopStat(SimTime);
end;

end.
