unit UQuarry;

interface
uses USimulation;

type
  TExcavator = class(TProcess)
  public
    // ??????? ??????????
    Queue : TList;
    // ??????????
    Stat : TServiceStatistics;
    constructor Create(AQueue : TList; AStat : TServiceStatistics);
  protected
    procedure RunProcess; override;
  end;

  TTruck = class(TProcess)
  public
    // ??????? ? ???????????
    Queue : TList;
    // ???????????? ??????????
    Excavator : TProcess;
    // ????????????????
    Tonnage : Double;
    // ????? ????????
    LoadingTime : Double;
    // ????? ????????? ???????
    TripTime : Double;
    // ????? ?????????
    UnloadingTime : Double;
    constructor Create(Que : TList; Ex : TProcess;
        Tng, LdTm, TrTm, UnldTm : Double);
  protected
    procedure RunProcess; override;
  end;

  TMill = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TQuarry = class(TSimulation)
  public
    // ??????? ? ????????????
    ExcavatorQueue : array of TList;
    // ??????? ? ????????????
    MillQueue : TList;
    // ???????????
    Excavator : array of TProcess;
    // ????????????
    Mill : TMill;
    // ?????????? ?? ???????? ??????????
    ReturnStat : TActionStatistics;
    // ?????????? ?? ????????????
    ExcavatorStat : array of TServiceStatistics;
    // ?????????? ?? ????????????
    MillStat : TServiceStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndExcavator,
  rndMill : TRandom;
  ExcavatorCount : Integer = 3;
  HeavyTonnage : Double = 50;
  LightTonnage : Double = 20;
  HeavyLoadingTime : Double = 10;
  LightLoadingTime : Double = 5;
  HeavyTripTime : Double = 2;
  LightTripTime : Double = 1.5;
  HeavyUnloadingTime : Double = 4;
  LightUnloadingTime : Double = 2;
  SimulationTime : Double = 480;
  InitLightTrucks : Integer = 2;
  InitHeavyTrucks : Integer = 1;

implementation

{ TExcavator }

constructor TExcavator.Create(AQueue : TList; AStat : TServiceStatistics);
begin
  Queue := AQueue;
  Stat := AStat;
  inherited Create;
end;

procedure TExcavator.RunProcess;
var
  trk : TTruck;
begin
  // ?????? ???????????
  while True do
  begin
    // ??????? ????????? ?????????? ? ???????
    while Queue.Empty do
      Passivate;
    // ????? ?????? ????????
    trk := Queue.First as TTruck;
    trk.StartRunning;
    // ???????? ? ??????? ??????????
    Stat.Start(SimTime);
    Hold(rndExcavator.Exponential(trk.LoadingTime));
    Stat.Finish(SimTime);
    // ????????? ????????
    trk.ActivateDelay(0);
  end;
end;

{ TTruck }

constructor TTruck.Create(Que : TList; Ex : TProcess;
    Tng, LdTm, TrTm, UnldTm: Double);
begin
  Queue := Que;
  Excavator := Ex;
  Tonnage := Tng;
  LoadingTime := LdTm;
  TripTime := TrTm;
  UnloadingTime := UnldTm;
  inherited Create;
end;

procedure TTruck.RunProcess;
var
  par : TQuarry;
begin
  par := Parent as TQuarry;
  while True do
  begin
    // ???????? ???????? ? ???????????? ? ????????????
    Hold(TripTime + 1);
    // ???????????? ?????????
    par.Mill.ActivateDelay(0);
    // ?????? ? ??????? ? ????????????
    Wait(par.MillQueue);
    // ???????? ???? ? ??????? ??????????
    par.ReturnStat.Start(SimTime);
    Hold(TripTime);
    par.ReturnStat.Finish(SimTime);
    // ???????????? ???? ??????????
    Excavator.ActivateDelay(0);
    // ?????? ? ??????? ? ?????? ???????????
    Wait(Queue);
  end;
end;

{ TMill }

procedure TMill.RunProcess;
var
  par : TQuarry;
  trk : TTruck;
begin
  // ??????? ?????? ????????????
  par := Parent as TQuarry;
  while True do
  begin
    // ??????? ??????????? ?????????? ?? ????????
    while par.MillQueue.Empty do
      Passivate;
    // ??????? ?????? ???????? ?? ???????
    trk := par.MillQueue.First as TTruck;
    trk.StartRunning;
    // ????????? ? ??????? ??????????
    par.MillStat.Start(SimTime);
    Hold(rndMill.Exponential(trk.UnloadingTime));
    par.MillStat.Finish(SimTime);
    // ????????? ???????? ? ???????? ????
    trk.ActivateDelay(0);
  end;
end;

{ TQuarry }

destructor TQuarry.Destroy;
var
  i : Integer;
begin
  for i := 0 to ExcavatorCount - 1 do
    Excavator[i].Free;
  SetLength(Excavator, 0);
  Mill.Free;
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorStat[i].Free;
  SetLength(ExcavatorStat, 0);
  MillStat.Free;
  ReturnStat.Free;
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorQueue[i].Free;
  SetLength(ExcavatorQueue, 0);
  MillQueue.Free;
  inherited;
end;

// ??????? ????????? ??? ??????? ? ??????? ???????? ?????????
function MillQueueFunc(A, B : TLink) : Boolean;
begin
  // ??????????????? ????????? ????? ????????????
  Result := (A as TTruck).Tonnage > (B as TTruck).Tonnage;
end;

procedure TQuarry.Init;
var
  i, j : Integer;
begin
  inherited;
  // ??????? ??????? ? ????????????
  SetLength(ExcavatorQueue, ExcavatorCount);
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorQueue[i] := TList.Create;
  // ??????? ? ???????????? ??????????? ?? ????????????????
  MillQueue := TList.Create(MillQueueFunc);
  // ??????? ??????? ????? ??????????
  SetLength(ExcavatorStat, ExcavatorCount);
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorStat[i] := TServiceStatistics.Create(1);
  MillStat := TServiceStatistics.Create(1);
  ReturnStat := TActionStatistics.Create;
  // ??????? ????????
  SetLength(Excavator, ExcavatorCount);
  for i := 0 to ExcavatorCount - 1 do
    Excavator[i] := TExcavator.Create(ExcavatorQueue[i], ExcavatorStat[i]);
  Mill := TMill.Create;
  // ??????? ????????? ? ????????? ?? ? ??????? ? ????????????
  for i := 0 to ExcavatorCount - 1 do
  begin
    // ??????? ??????
    for j := 1 to InitLightTrucks do
      TTruck.Create(ExcavatorQueue[i], Excavator[i], LightTonnage,
          LightLoadingTime, LightTripTime,
          LightUnloadingTime).Insert(ExcavatorQueue[i]);
    // ????? ???????
    for j := 1 to InitHeavyTrucks do
      TTruck.Create(ExcavatorQueue[i], Excavator[i], HeavyTonnage,
          HeavyLoadingTime, HeavyTripTime,
          HeavyUnloadingTime).Insert(ExcavatorQueue[i]);
  end;
end;

procedure TQuarry.RunSimulation;
begin
  // ???????????? ??? ???????????
  ActivateAllDelay(Excavator, 0);
  // ??????? ????????? ????????
  Hold(SimulationTime);
  StopStat;
end;

procedure TQuarry.StopStat;
var
  i : Integer;
begin
  inherited;
  for i := 0 to ExcavatorCount - 1 do
    ExcavatorStat[i].StopStat(SimTime);
  MillStat.StopStat(SimTime);
  ReturnStat.StopStat(SimTime);
end;

end.
