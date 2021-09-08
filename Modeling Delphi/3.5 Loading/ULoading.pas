unit ULoading;

interface
uses USimulation;

type
  // ����� THeap - ���� �����.
  //   �� ��������, ������ ������ ������
  THeap = TLink;

  // ����� TBulldozer  - ���������
  TBulldozer = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� Tloader - ���������
  TLoader = class(TProcess)
  private
    // ������� ����� ��������
    MeanWorkTime : Double;
    // ������ ����� ����������
    Stat : TServiceStatistics;
  public
    constructor Create(AStat : TServiceStatistics; AWork : Double);
  protected
    procedure RunProcess; override;
  end;

  // ����� TTruck - ��������
  TTruck = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // ����� TLoading - ������������� �������� ���������
  TLoading = class(TSimulation)
  public
    // ������� ���, ������� � ���������
    HeapQueue : TList;
    // ������� ����������, ��������� ������
    TrucksQueue : TList;
    // ������� �����������, ��������� ������
    LoadersQueue : TList;
    // ���������� �� �����������
    LoadersStat : array of TServiceStatistics;
    // ���������
    Bulldozer : TBulldozer;
    // ������� ���������� ������
    Finished : Boolean;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndBulldozer,
  rndLoader,
  rndTruck : TRandom;
  ModelingTime : Double = 480;
  MinHeapQueueSize : Integer = 2;
  HeapParamInterval : Double = 4;
  HeapParamCount : Integer = 2;
  LoaderReturnTime : Double = 5;
  TruckForwardMean : Double = 22;
  TruckForwardDeviation : Double = 3;
  TruckUnloadMin : Double = 2;
  TruckUnloadMax : Double = 8;
  TruckBackMean : Double = 18;
  TruckBackDeviation : Double = 3;
  TrucksCount : Integer = 4;
  LoadersCount : Integer = 2;
  LoaderTimeMean : array [0 .. 1] of Double = (12, 14);

implementation

{ TBulldozer }

procedure TBulldozer.RunProcess;
var
  par : TLoading;
begin
  // ������ �� ������ ������������ � ������ �������������.
  //   ��� ������������� � ��������� ��� �����
  par := Parent as TLoading;
  while SimTime <= ModelingTime do
  begin
    // ������� ���� ����� � ��������� �� � �������
    THeap.Create.Insert(par.HeapQueue);
    // ���� ��� ���� �� ����������� ����������
    if par.HeapQueue.Size >= MinHeapQueueSize then
      // ������������ ������� ���������� (���� ����)
      ActivateDelay(par.LoadersQueue, 0);
    Hold(rndBulldozer.Erlang(HeapParamInterval, HeapParamCount));
  end;
  // ������ ���������
  par.Finished := True;
end;

{ TLoader }

constructor TLoader.Create(AStat: TServiceStatistics; AWork : Double);
begin
  Stat := AStat;
  MeanWorkTime := AWork;
  inherited Create;
end;

procedure TLoader.RunProcess;
var
  par : TLoading;
  trk : TTruck;
  i : Integer;
begin
  // �������, ������������ ������ ����������
  par := Parent as TLoading;
  while True do
  begin
    // ������� ������� �� ����� ������������ ���������� ���
    //   � ���������� ���������
    while (par.HeapQueue.Size < MinHeapQueueSize) or par.TrucksQueue.Empty do
    begin
      // ���� ��������� �������� ������,
      //   �������� ������ ������������ ����� ��� � ��� ��������� ��������
      if par.Finished and (par.HeapQueue.Size < MinHeapQueueSize) and
          (par.TrucksQueue.Size = 4) then
      begin
        // ��������� ������ ����������� ���������
        //   ������������� ��������
        par.ActivateDelay(0);
        Passivate;
      end
      else
        // ����� - �����
        Passivate;
    end;
    // ������ ������������
    // ������������� ������ ������
    Stat.Start(SimTime);
    // ������� �� ������� ������ ��������
    trk := par.TrucksQueue.First as TTruck;
    trk.StartRunning;
    StartRunning;
    // ������ ���� �� ������ �������
    for i := 1 to MinHeapQueueSize do
      par.HeapQueue.First.Free;
    // ��������� ��������
    Hold(rndLoader.Exponential(MeanWorkTime));
    // ��������� ������
    Stat.Finish(SimTime);
    // ������������ ��������
    trk.ActivateDelay(0);
    // �����������
    Hold(LoaderReturnTime);
    // ������ � ������� �������� �� ��������� �����
    Insert(par.LoadersQueue);
  end;
end;

{ TTruck }

procedure TTruck.RunProcess;
var
  par : TLoading;
begin
  // �������, ������������ ������ ���������
  par := Parent as TLoading;
  while True do
  begin
    // �������� �������� ����� ��������
    // ���������
    Hold(rndTruck.Normal(TruckForwardMean, TruckForwardDeviation));
    // ���������
    Hold(rndTruck.Uniform(TruckUnloadMin, TruckUnloadMax));
    // �����������
    Hold(rndTruck.Normal(TruckBackMean, TruckBackDeviation));
    // ������������ ��������� ���������
    ActivateDelay(par.LoadersQueue, 0);
    // ������ � ������� ��������
    Wait(par.TrucksQueue);
  end;
end;

{ TLoading }

destructor TLoading.Destroy;
var
  i : Integer;
begin
  Bulldozer.Free;
  for i := 0 to LoadersCount - 1 do
    LoadersStat[i].Free;
  TrucksQueue.Free;
  LoadersQueue.Free;
  HeapQueue.Free;
  inherited;
end;

procedure TLoading.Init;
var
  i : Integer;
begin
  inherited;
  // ������� ������ ����������, ����������� � ���
  TrucksQueue := TList.Create;
  LoadersQueue := TList.Create;
  HeapQueue := TList.Create;
  // ������ �������� �����������
  SetLength(LoadersStat, LoadersCount);
  for i := 0 to LoadersCount - 1 do
    LoadersStat[i] := TServiceStatistics.Create(1);
  // ������� ���������� � ��������� �� � �������
  for i := 0 to LoadersCount - 1 do
    TLoader.Create(LoadersStat[i], LoaderTimeMean[i]).Insert(LoadersQueue);
  // ������� ��������� � ��������� �� � �������
  for i := 1 to TrucksCount do
    TTruck.Create.Insert(TrucksQueue);
  // ������� ���������
  Bulldozer := TBulldozer.Create;
  // ������� ������ �� �������
  Finished := False;
end;

procedure TLoading.RunSimulation;
begin
  // ��������� ���������
  Bulldozer.ActivateDelay(0);
  // ������� ���������
  Passivate;
  // ��������������� ����������
  StopStat;
end;

procedure TLoading.StopStat;
begin
  inherited;
  LoadersStat[0].StopStat(SimTime);
  LoadersStat[1].StopStat(SimTime);
  TrucksQueue.StopStat(SimTime);
  LoadersQueue.StopStat(SimTime);
  HeapQueue.StopStat(SimTime);
end;

end.
