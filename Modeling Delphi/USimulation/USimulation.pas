unit USimulation;

interface

uses
  Classes, SysUtils, Grids, Types;

const
  LocalStackSize = 65536;

type
  ESimulationException = class(Exception)
  public
    Msg : string;
    constructor Create(AMsg : string);
  end;

  TLink = class;
  TList = class;

  // ????? TLinkage - ??????? ????? ?????? ???????? ??????, ????? ??????
  //   ??? ??????????? ??????? ?????????????
  TLinkage = class
  protected
    FPrev, FNext : TLinkage;
    FHeader : TList;
  public
    constructor Create;
    function Next : TLink;
    function Prev : TLink;
  end;

  TCompareFunc = function(A, B : TLink) : Boolean;

  // ????? TLink = ?????????? ?????? ???????? ??????
  TLink = class(TLinkage)
  private
    InsertTime : Double;
  protected
    function GetHeader : TList; 
  public
    destructor Destroy; override;
    // ?????????? ?????? ?? ??????
    procedure Remove;
    // ??????? ?????? ? ??????: ?? ? ????? ????????? ??????, ? ? ????? ??????
    procedure InsertBefore(L : TLinkage);
    procedure InsertAfter(L : TLinkage);
    procedure InsertLast(L : TList);
    procedure InsertFirst(L : TList);
    procedure Insert(L : TList); overload;
    procedure Insert(L : TList; Order : TcompareFunc); overload;
  end;

  TStatistics = class;
  TIntervalStatistics = class;

  // ????? TList - ???????????? ?????? ??????
  TList = class(TLinkage)
  private
    OrderFunc : TCompareFunc;
    FSize : Integer;
    procedure SetOrderFunc(NewOrderFunc : TCompareFunc);
  public
    LengthStat : TIntervalStatistics;
    WaitStat : TStatistics;
    constructor Create; overload;
    constructor Create(SimTime : Double); overload;
    constructor Create(Order : TCompareFunc); overload;
    constructor Create(Order : TCompareFunc; SimTime : Double); overload;
    destructor Destroy; override;
    function First : TLink;
    function Last : TLink;
    function Empty : Boolean;
    function Size : Integer;
    procedure Clear;
    procedure StopStat; overload;
    procedure StopStat(NewTime : Double); overload;
    procedure ClearStat; overload;
    procedure ClearStat(NewTime : Double); overload;
    property Order : TCompareFunc write SetOrderFunc;
  end;

  // ????? TRunningObject - ??????????? ??????? ?????
  //   ??? ??????????? ????????-??????????.
  //   ???????????? ?????????? ???????????? ???????????
  //   ??????????? SwitchTo ? Detach
  //   ? ??????????? ?????? ??????????? ???????????????
  //   ???????????? ?????? Execute. ??? ????????? ?????? ???? ???????? ?????????:
  //   1. ???????? ?????????? ????????, ????????? ?????????? ?????????.
  //   2. Detach;
  //   3. ??????? ???? ????????.
  //   4. Detach;
  TLocalStack = array [0 .. LocalStackSize - 1] of Byte;
  TRunningObject = class(TLink)
  private
    { Private declarations }
    SavedSP : Cardinal;     // ??????????? ????????? ?????
    FTerminated : Boolean;  // ??????? ?????????? ????????
    LocalStack : ^TLocalStack;  // ????????? ???? (????????) ????????
    procedure Run;
  protected
    Owner : TRunningObject;
    procedure Execute; virtual; abstract;
  public
    constructor Create;
    destructor Destroy; override;
    property Terminated : Boolean read FTerminated;
  end;

  TProcess = class;

  // ????? TEventNotice - ?????? ?????? ???????
  TEventNotice = class(TLink)
  private
    EventTime : Double;
    Proc : TProcess;
    constructor Create(ETime : Double; AProc : TProcess);
    procedure InsertPrior(l : TList);
    procedure SetTime(NewTime : Double);
    procedure SetTimePrior(NewTime : Double);
  public
    destructor Destroy; override;
  end;

  TSimulation = class;
  TResource = class;
  TGate = class;

  TGetResourceFunc = function : Boolean of object;

  // ????? TProcess - ??????????? ?????, ???????????? ???????
  //   ??? ???????? ???????????? ?????? ??????????? ?????????????? ????? RunProcess
  TProcess = class(TRunningObject)
  private
    Event : TEventNotice;
    procedure RunNextProc;
  protected
    Parent : TSimulation;
    procedure Init; virtual;
    procedure Execute; override;
    procedure RunProcess; virtual; abstract;
    function SimTime : Double; virtual;
    procedure StartRunning;
    procedure Finish;
    procedure ClearFinished;
    procedure Activate(const Procs : array of TProcess); overload;
    procedure Activate(Procs : TList); overload;
    procedure ActivateAt(Procs : TList; t : Double); overload;
    procedure ActivateAt(const Procs : array of TProcess; t : Double); overload;
    procedure ActivatePriorAt(const Procs : array of TProcess;
        t : Double); overload;
    procedure ActivatePriorAt(Procs : TList; t : Double); overload;
    procedure ActivateDelay(const Procs : array of TProcess;
        t : Double); overload;
    procedure ActivateDelay(Procs : TList; t : Double); overload;
    procedure ActivatePriorDelay(const Procs : array of TProcess;
        t : Double); overload;
    procedure ActivatePriorDelay(Procs : TList; t : Double); overload;
    procedure ActivateAfter(const Procs : array of TProcess;
        p : TProcess); overload;
    procedure ActivateAfter(Procs : TList; p : TProcess); overload;
    procedure ActivateBefore(const Procs : array of TProcess;
        p : TProcess); overload;
    procedure ActivateBefore(Procs : TList; p : TProcess); overload;
    procedure ActivateAll(const Procs : array of TProcess); overload;
    procedure ActivateAll(Procs : TList); overload;
    procedure ActivateAllAt(Procs : TList; t : Double); overload;
    procedure ActivateAllAt(const Procs : array of TProcess; t : Double); overload;
    procedure ActivateAllPriorAt(const Procs : array of TProcess;
        t : Double); overload;
    procedure ActivateAllPriorAt(Procs : TList; t : Double); overload;
    procedure ActivateAllDelay(const Procs : array of TProcess;
        t : Double); overload;
    procedure ActivateAllDelay(Procs : TList; t : Double); overload;
    procedure ActivateAllPriorDelay(const Procs : array of TProcess;
        t : Double); overload;
    procedure ActivateAllPriorDelay(Procs : TList; t : Double); overload;
    procedure ActivateAllAfter(const Procs : array of TProcess;
        p : TProcess); overload;
    procedure ActivateAllAfter(Procs : TList; p : TProcess); overload;
    procedure ActivateAllBefore(const Procs : array of TProcess;
        p : TProcess); overload;
    procedure ActivateAllBefore(Procs : TList; p : TProcess); overload;
    procedure Reactivate(Proc : TProcess); overload;
    procedure ReactivateAt(Proc : TProcess; t : Double); overload;
    procedure ReactivatePriorAt(Proc : TProcess; t : Double); overload;
    procedure ReactivateDelay(Proc : TProcess; t : Double); overload;
    procedure ReactivatePriorDelay(Proc : TProcess; t : Double); overload;
    procedure ReactivateAfter(Proc, p : TProcess); overload;
    procedure ReactivateBefore(Proc, p : TProcess); overload;
  public
    StartingTime : Double;
    TimeLeft : Double;
    constructor Create;
    destructor Destroy; override;
    function Idle : Boolean;
    function EventTime : Double;
    function NextEvent : TProcess;
    procedure Passivate;
    procedure Hold(t : Double);
    procedure Wait(l : TList);
    procedure Activate; overload;
    procedure ActivateAt(t : Double); overload;
    procedure ActivatePriorAt(t : Double); overload;
    procedure ActivateDelay(t : Double); overload;
    procedure ActivatePriorDelay(t : Double); overload;
    procedure ActivateAfter(p : TProcess); overload;
    procedure ActivateBefore(p : TProcess); overload;
    procedure Reactivate; overload;
    procedure ReactivateAt(t : Double); overload;
    procedure ReactivatePriorAt(t : Double); overload;
    procedure ReactivateDelay(t : Double); overload;
    procedure ReactivatePriorDelay(t : Double); overload;
    procedure ReactivateAfter(p : TProcess); overload;
    procedure ReactivateBefore(p : TProcess); overload;
    procedure GetResource(Res : TResource; Count, Index : Integer); overload;
    procedure GetResource(Res : TResource; Count : Integer); overload;
    procedure GetResource(Res : TResource); overload;
    procedure GetResource(Res : TResource; GetRes : TGetResourceFunc;
        Index : Integer); overload;
    procedure GetResource(Res : TResource; GetRes : TGetResourceFunc); overload;
    procedure PreemptResource(Res : TResource; Index : Integer); overload;
    procedure PreemptResource(Res : TResource); overload;
    procedure PreemptResourceNoWait(Res : TResource; Index : Integer); overload;
    procedure PreemptResourceNoWait(Res : TResource); overload;
    procedure ReleaseResource(Res : TResource; Count : Integer); overload;
    procedure ReleaseResource(Res : TResource); overload;
    procedure ChangeResource(Res : TResource; Count : Integer);
    procedure WaitGate(Gate : TGate);
    procedure CloseGate(Gate : TGate);
    procedure OpenGate(Gate : TGate);
  end;

  TVisualizator = class(TProcess)
  public
    DeltaT : Double;
    constructor Create(dt : Double);
  protected
    procedure RunProcess; override;
  end;

  // ????? TSimulation - ??????????? ?????, ???????????? ????????
  //   ??? ???????? ???????????? ?????? ??????????? ?????????????? ?????
  //   RunSimulation
  TSimulation = class(TProcess)
  private
    FSimTime : Double;
    FLastCleared : Double;
  public
    Visualizator : TVisualizator;
    Calendar : TList;
    constructor Create;
    destructor Destroy; override;
    function SimTime : Double; override;
    procedure StopStat; virtual;
    procedure ClearStat; virtual;
    property LastCleared : Double read FLastCleared;
  protected
    RunningObjects : TList;
    FinishedObjects : TList;
    procedure Execute; override;
    procedure RunSimulation; virtual; abstract;
    procedure Init; override;
    procedure Finalize; virtual;
    procedure MakeVisualizator(dt : Double);
  end;

  // ????? TStatistics ???????????? ??? ????? ???????? ??????????
  TStatistics = class
  private
    SumX : Double;
    SumX_2 : Double;
    FMin : Double;
    FMax : Double;
    FCount : Integer;
  public
    constructor Create;
    procedure AddData(NewX : Double);
    procedure Clear;
    function Mean : Double;
    function Deviation : Double;
    function Disperse : Double;
    property Min : Double read FMin;
    property Max : Double read FMax;
    property Count : Integer read FCount;
  end;

  // ????? TTimeBetStatistics ???????????? ??? ?????
  //   ???????? ?????????? ?? ?????????? ??????? ????? ?????????
  TTimeBetStatistics = class
  private
    SumX : Double;
    SumX_2 : Double;
    FMin : Double;
    FMax : Double;
    LastTime : Double;
    FCount : Integer;
  public
    constructor Create;
    procedure AddData; overload;
    procedure AddData(NewTime : Double); overload;
    procedure Clear;
    function Mean : Double;
    function Deviation : Double;
    function Disperse : Double;
    property Min : Double read FMin;
    property Max : Double read FMax;
    property Count : Integer read FCount;
  end;

  // ????? TIntervalStatistic ???????????? ??? ?????
  //   ???????????? (?????????) ??????????
  TIntervalStatistics = class
  private
    SumX : Double;
    SumX_2 : Double;
    LastTime : Double;
    LastX : Double;
    FMin : Double;
    FMax : Double;
    FTotalTime : Double;
  public
    constructor Create(InitX, InitTime : Double); overload;
    constructor Create(InitX : Double); overload;
    procedure AddData(NewX, NewTime : Double); overload;
    procedure AddData(NewX : Double); overload;
    procedure StopStat(NewTime : Double); overload;
    procedure StopStat; overload;
    procedure Clear(NewTime : Double); overload;
    procedure Clear; overload;
    function Mean : Double;
    function Deviation : Double;
    function Disperse : Double;
    property Min : Double read FMin;
    property Max : Double read FMax;
    property Current : Double read LastX;
    property TotalTime : Double read FTotalTime;
  end;

  // ????? TActionStatistic ???????????? ??? ?????
  //   ?????????? ?? ?????????
  TActionStatistics = class
  private
    SumX : Double;
    SumX_2 : Double;
    LastTime : Double;
    LastX : Integer;
    FMax : Integer;
    FFinished : Integer;
    FTotalTime : Double;
  public
    constructor Create(InitX : Integer; InitTime : Double); overload;
    constructor Create(InitX : Integer); overload;
    constructor Create; overload;
    procedure Start(NewTime : Double); overload;
    procedure Start; overload;
    procedure Finish(NewTime : Double); overload;
    procedure Finish; overload;
    procedure Preempt(NewTime : Double); overload;
    procedure Preempt; overload;
    procedure Resume(NewTime : Double); overload;
    procedure Resume; overload;
    procedure StopStat(NewTime : Double); overload;
    procedure StopStat; overload;
    procedure Clear(NewTime : Double); overload;
    procedure Clear; overload;
    function Mean : Double;
    function Deviation : Double;
    function Disperse : Double;
    property Max : Integer read FMax;
    property Finished : Integer read FFinished;
    property Running : Integer read LastX;
    property TotalTime : Double read FTotalTime;
  end;

  // ????? TServiceStatistic ???????????? ??? ?????
  //   ?????????? ?? ????????????? ?????????
  TServiceStatistics = class
  private
    SumX : Double;
    SumX_2 : Double;
    LastTime : Double;
    LastUtil : Integer;
    SumBlockage : Double;
    LastBlockage : Integer;
    LastBlockTime : Double;
    FMaxBusy : Integer;
    FMinBusy : Integer;
    FFinished : Integer;
    LastIdleStart : Double;
    LastIdleTime : Double;
    LastBusyStart : Double;
    LastBusyTime : Double;
    FMaxIdleTime : Double;
    FMaxBusyTime : Double;
    DeviceCount : Integer;
    FTotalTime : Double;
  public
    constructor Create(Devices, InitUtil : Integer;
        InitTime : Double); overload;
    constructor Create(Devices : Integer); overload;
    constructor Create; overload;
    procedure Start(NewTime : Double); overload;
    procedure Start; overload;
    procedure Finish(NewTime : Double); overload;
    procedure Finish; overload;
    procedure StartBlock(NewTime : Double); overload;
    procedure StartBlock; overload;
    procedure FinishBlock(NewTime : Double); overload;
    procedure FinishBlock; overload;
    procedure StopStat(NewTime : Double); overload;
    procedure StopStat; overload;
    procedure Clear(NewTime : Double); overload;
    procedure Clear; overload;
    function Mean : Double;
    function MeanBlockage : Double;
    function Deviation : Double;
    function Disperse : Double;
    property MaxBusy : Integer read FMaxBusy;
    property MinBusy : Integer read FMinBusy;
    property Finished : Integer read FFinished;
    property Running : Integer read LastUtil;
    property MaxIdleTime : Double read FMaxIdleTime;
    property MaxBusyTime : Double read FMaxBusyTime;
    property Devices : Integer read DeviceCount;
    property TotalTime : Double read FTotalTime;
    property Blocked : Integer read LastBlockage; 
  end;

  // ????? TRandom ??? ????????? ??????????????????? ????????? ????????
  TRandom = class
  private
    FSeed : Integer;
    HasNextNormal : Boolean;
    NextNormal : Double;
  public
    constructor Create; overload;
    constructor Create(Seed : Integer); overload;
    function Draw(A : Double) : Boolean;
    function NextInt : Integer; overload;
    function NextInt(High : Integer) : Integer; overload;
    function NextInt(Low, High : Integer) : Integer; overload;
    function NextFloat : Double;
    function Uniform(A, B : Double) : Double;
    function TableIndex(Table : array of Double) : Integer;
    function Normal(Mean, Sigma : Double) : Double;
    function PSNorm(Mean, Sigma : Double; Count : Integer) : Double;
    function Triangular(A, Moda, B : Double) : Double;
    function LogNormal(Mean, Sigma : Double) : Double;
    function Exponential(Mean : Double) : Double;
    function Poisson(Lambda : Double) : Integer;
    function NegExp(Mean : Double) : Double;
    function Erlang(Mean : Double; K : Integer) : Double;
    function Gamma(Beta, Alpha : Double) : Double;
    function Beta(Alpha, Betta : Double) : Double; overload;
    function Beta(Min, Mean, Max, Sigma : Double) : Double; overload;
    function Weibull(Beta, Alpha : Double) : Double;
  end;

  // ????? THistogram ??? ?????????? ????????????? ????????
  //   ?? ??????????
  THistogram = class
  protected
    FTotalCount : Integer;
    function GetCount(i : Integer) : Integer; virtual; abstract;
    function GetCumulativeCount(i : Integer) : Integer; virtual; abstract;
    function GetPercent(i : Integer) : Double; virtual; abstract;
    function GetCumulativePercent(i : Integer) : Double; virtual; abstract;
    function GetLowerBound(i : Integer) : Double; virtual; abstract;
    function GetUpperBound(i : Integer) : Double; virtual; abstract;
    function GetIntervalCount : Integer; virtual; abstract;
    procedure SetLowerBound(i : Integer; Value : Double); virtual; abstract;
    procedure SetUpperBound(i : Integer; Value : Double); virtual; abstract;
    procedure SetIntervalCount(NewCount : Integer); virtual; abstract;
  public
    constructor Create;
    procedure Clear; virtual; abstract;
    procedure AddData(d : Double); virtual; abstract;
    property Count[i : Integer] : Integer read GetCount;
    property Percent[i : Integer] : Double read GetPercent;
    property CumulativeCount[i : Integer] : Integer read GetCumulativeCount;
    property CumulativePercent[i : Integer] : Double read GetCumulativePercent;
    property LowerBound[i : Integer] : Double read GetLowerBound
        write SetLowerBound;
    property UpperBound[i : Integer] : Double read GetUpperBound
        write SetUpperBound;
    property IntervalCount : Integer read GetIntervalCount
        write SetIntervalCount;
    property TotalCount : Integer read FTotalCount;
  end;

  // ????? TUniformHistogram ??? ?????????? ????????????? ????????
  //   ?? ?????????? ?????? ?????
  TUniformHistogram = class(THistogram)
  private
    FLow : Double;        // ?????? ??????? ??????? ????????? ?????????
    FStep : Double;       // ?????? ??????? ?????????
    FIntervalCount : Integer;  // ?????????? ???????? ??????????
    Data : array of Integer;   // ?????? ????????? ??????
  protected
    function GetCount(i : Integer) : Integer; override;
    function GetPercent(i : Integer) : Double; override;
    function GetCumulativeCount(i : Integer) : Integer; override;
    function GetCumulativePercent(i : Integer) : Double; override;
    function GetLowerBound(i : Integer) : Double; override;
    function GetUpperBound(i : Integer) : Double; override;
    function GetIntervalCount : Integer; override;
    procedure SetLowerBound(i : Integer; Value : Double); override;
    procedure SetUpperBound(i : Integer; Value : Double); override;
    procedure SetIntervalCount(NewCount : Integer); override;
  public
    constructor Create(ALow, AStep : Double; AIntervalCount : Integer);
    procedure Clear; override;
    procedure AddData(d : Double); override;
  end;

  // ????? TArrayHistogram ??? ?????????? ????????????? ????????
  //   ?? ??????????, ???????? ???????? ????????? ????????
  TArrayHistogram = class(THistogram)
  private
    FIntervalCount : Integer;  // ?????????? ???????? ??????????
    Data : array of Integer;   // ?????? ????????? ??????
    Bounds : array of Double;  // ?????? ????????? ????????
    function IntervalIndex(Value : Double) : Integer;
  protected
    function GetCount(i : Integer) : Integer; override;
    function GetPercent(i : Integer) : Double; override;
    function GetCumulativeCount(i : Integer) : Integer; override;
    function GetCumulativePercent(i : Integer) : Double; override;
    function GetLowerBound(i : Integer) : Double; override;
    function GetUpperBound(i : Integer) : Double; override;
    function GetIntervalCount : Integer; override;
    procedure SetLowerBound(i : Integer; Value : Double); override;
    procedure SetUpperBound(i : Integer; Value : Double); override;
    procedure SetIntervalCount(NewCount : Integer); override;
  public
    constructor Create(ABounds : array of Double);
    procedure Clear; override;
    procedure AddData(d : Double); override;
  end;

  // ????? TResource ??? ?????????? ???????????? ?????????
  TResource = class
  private
    FCapacity : Integer;
    FBusy : Integer;
    FQueueCount : Integer;
    FAvailStat : TIntervalStatistics;
    FBusyStat : TIntervalStatistics;
    FQueue : array of TList;
    FPriority : TCompareFunc;
    function GetQueue(Index : Integer) : TList;
  public
    CurrentProc : TProcess;
    CurrentIndex : Integer;
    constructor Create(InitCap, InitBusy, QueCnt : Integer;
        StartTime : Double; PriorFunc : TCompareFunc); overload;
    constructor Create(InitCap, InitBusy, QueCnt : Integer;
        StartTime : Double); overload;
    constructor Create(InitCap, InitBusy, QueCnt : Integer;
        PriorFunc : TCompareFunc); overload;
    constructor Create(InitCap, InitBusy, QueCnt : Integer); overload;
    constructor Create(InitCap : Integer; StartTime : Double;
        PriorFunc : TCompareFunc); overload;
    constructor Create(InitCap : Integer; StartTime : Double); overload;
    constructor Create(InitCap : Integer;
        PriorFunc : TCompareFunc); overload;
    constructor Create(InitCap : Integer); overload;
    constructor Create(PriorFunc : TCompareFunc); overload;
    constructor Create; overload;
    destructor Destroy; override;
    function Available : Integer;
    function Get(cnt : Integer; NewTime : Double) : Boolean; overload;
    function Get(cnt : Integer) : Boolean; overload;
    function Get : Boolean; overload;
    procedure Release(cnt : Integer; NewTime : Double); overload;
    procedure Release(cnt : Integer); overload;
    procedure Release; overload;
    procedure Add(cnt : Integer; NewTime : Double); overload;
    procedure Add(cnt : Integer); overload;
    procedure Add; overload;
    procedure Sub(cnt : Integer; NewTime : Double); overload;
    procedure Sub(cnt : Integer); overload;
    procedure Sub; overload;
    procedure ClearStat(NewTime : Double); overload;
    procedure ClearStat; overload;
    procedure StopStat(NewTime : Double); overload;
    procedure StopStat; overload;
    function PreemptedProcs(Index : Integer) : Integer; overload;
    function PreemptedProcs : Integer; overload;
    property Busy : Integer read FBusy;
    property QueueCount : Integer read FQueueCount;
    property Capacity : Integer read FCapacity;
    property AvailStat : TIntervalStatistics read FAvailStat;
    property BusyStat : TIntervalStatistics read FBusyStat;
    property Queue[Idx : Integer] : TList read GetQueue;
    property Priority : TCompareFunc read FPriority;
  end;

  // ????? TGate ??? ????????????? ????????, ?????????????????? ????????
  TGate = class
  private
    FState : Boolean;
    FStat : TIntervalStatistics;
    FQueue : TList;
  public
    constructor Create(InitState : Boolean; StartTime : Double); overload;
    constructor Create(InitState : Boolean); overload;
    constructor Create; overload;
    destructor Destroy; override;
    procedure Open(NewTime : Double); overload;
    procedure Open; overload;
    procedure Close(NewTime : Double); overload;
    procedure Close; overload;
    procedure StopStat(NewTime : Double); overload;
    procedure StopStat; overload;
    procedure ClearStat(NewTime : Double); overload;
    procedure ClearStat; overload;
    property State : Boolean read FState;
    property Stat : TIntervalStatistics read FStat;
    property Queue : TList read FQueue;
  end;

// ?????????? ????????? ??? ?????????? ?????????????
//  SwitchTo  - ????????? ??????? ??????? ? ???????????? ?????????
procedure SwitchTo(Proc : TRunningObject);
//  Detach - ????????? ??????? ??????? ? ???????????? ??? ?????????
procedure Detach;

procedure DumpEventQueue;

procedure RunSimulation(sim : TSimulation);
function Chars(Count : Integer; Ch : Char) : string;
function Min(a, b : Double) : Double; overload;
function Max(a, b : Double) : Double; overload;
function Min(a, b : Integer) : Integer; overload;
function Max(a, b : Integer) : Integer; overload;

procedure WriteStat(Header : string; Stat : TStatistics); overload;
procedure WriteStat(Header : string; Stat : TIntervalStatistics); overload;
procedure WriteStat(Header : string; Stat : TTimeBetStatistics); overload;
procedure WriteStat(Header : string; Stat : TActionStatistics); overload;
procedure WriteStat(Header : string; Stat : TServiceStatistics); overload;
procedure WriteStat(Header : string; Queue : TList); overload;
procedure WriteStat(Header : string; Res : TResource); overload;
procedure WriteStat(Header : string; Gate : TGate); overload;
procedure WriteHist(Header : string; Hist : THistogram);

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TStatistics); overload;
procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TIntervalStatistics); overload;
procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TTimeBetStatistics); overload;
procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TActionStatistics); overload;
procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TServiceStatistics); overload;
procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Queue : array of TList); overload;
procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Res : array of TResource); overload;
procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Gate : array of TGate); overload;
procedure DrawHistCell(Grid : TDrawGrid; ACol, ARow : Integer; Rect : TRect;
    Hist : THistogram);

implementation
uses Graphics;

var
  GlobalSP : Cardinal;
  CurrentProc : TRunningObject = nil;
  CurrentSim : TSimulation = nil;

{ TRunningObject }

// ???????????.
// ? ????????????? ??????????? ??????? ??????????? ?????? ?????????? ?????????
constructor TRunningObject.Create;
begin
  inherited Create;
  Owner := CurrentProc;
  FTerminated := False;
  New(LocalStack);
  Run;
end;

// ??????????
destructor TRunningObject.Destroy;
begin
  Dispose(LocalStack);
  inherited;
end;

// ????????????? ?????????? ????? ? ??????? ? ??????????
procedure TRunningObject.Run;
var
  OldSP : Cardinal;
begin
  asm
    push dword ptr [ebp-8]   // ??????? EBX
    push edx
    push esi
    push edi
    push ebp
    mov eax, esp
    mov OldSP, esp
  end;
  // ?????????? ????????? ????????-?????????
  if Owner = nil then
    GlobalSP := OldSP
  else
    Owner.SavedSP := OldSP;
  // ???????????? ? ?????? ?????????
  CurrentProc := Self;
  asm
    mov eax, [esi+LocalStack]
    mov esp, eax
    add esp, LocalStackSize
  end;
  Execute;
  FTerminated := True;
end;

// ???????????? ? ?????? ???????????
procedure SwitchTo(Proc : TRunningObject);
var
  OldSP, NewSP : Cardinal;
begin
  asm
    push ebx
    push edx
    push esi
    push edi
    push ebp
    mov eax, esp
    mov OldSP, esp
  end;
  if CurrentProc = nil then
    GlobalSP := OldSP
  else
    CurrentProc.SavedSP := OldSP;
  if Proc = nil then
    NewSP := GlobalSP
  else if Proc.Terminated then
    Exit   // ??????, ??????? ???????????? ?? ??????????? ???????
    // ????? ??????? ????????? ????????? ??????????
  else
    NewSP := Proc.SavedSP;
  CurrentProc := Proc;
{  if (Proc = nil) or (Proc is TSimulation) then
    CurrentSim := Proc as TSimulation;
}
  asm
    mov eax, NewSP
    mov esp, eax
    pop ebp
    pop edi
    pop esi
    pop edx
    pop ebx
  end;
end;

// ?????????? ???????? ? ???????????? ?? ?????????? ???????
procedure Detach;
var
  OldSP, NewSP : Cardinal;
begin
  asm
    push ebx
    push edx
    push esi
    push edi
    push ebp
    mov eax, esp
    mov OldSP, esp
  end;
  if CurrentProc = nil then
    // ?????????? ? ????????? ??????????? ???????? ??????????
    Exit
  else
    CurrentProc.SavedSP := OldSP;
  if CurrentProc.Owner = nil then
    NewSP := GlobalSP
  else if CurrentProc.Owner.Terminated then
    Exit   // ??????, ??????? ???????????? ?? ??????????? ???????
    // ????? ??????? ????????? ????????? ??????????
  else
    NewSP := CurrentProc.Owner.SavedSP;
  CurrentProc := CurrentProc.Owner;
{  if (CurrentProc = nil) or (CurrentProc is TSimulation) then
    CurrentSim := CurrentProc as TSimulation
  else if CurrentProc is TProcess then
    CurrentSim := (CurrentProc as TProcess).Parent;
}
  asm
    mov eax, NewSP
    mov esp, eax
    pop ebp
    pop edi
    pop esi
    pop edx
    pop ebx
  end;
end;

{ TLinkage }

constructor TLinkage.Create;
begin
  FPrev := nil;
  FNext := nil;
  FHeader := nil;
end;

function TLinkage.Next: TLink;
begin
  if FNext = nil then
    Result := nil
  else if FNext is TLink then
    Result := FNext as TLink
  else
    Result := nil;
end;

function TLinkage.Prev: TLink;
begin
  if FPrev = nil then
    Result := nil
  else if FPrev is TLink then
    Result := FPrev as TLink
  else
    Result := nil;
end;

{ TLink }

destructor TLink.Destroy;
begin
  // ????? ????????? ?????? ?? ????? ???????? ?? ???????? ??????
  Remove;
  inherited;
end;

// ????????? ???????????? ?????? ??????
function TLink.GetHeader: TList;
begin
  Result := FHeader;
end;

// ??????? ?????? ? ??????
procedure TLink.Insert(L: TList);
var
  lnk : TLink;
begin
  // ???? ?? ?????? ??????? ????????????, ???????? ? ?????
  if @L.OrderFunc = nil then
    InsertLast(L)
  else
  begin
    // ????? ????? ??????? ? ???????????? ? ???????? ????????????
    lnk := L.First;
    while lnk <> nil do
    begin
      if L.OrderFunc(Self, lnk) then
        Break;
      lnk := lnk.Next;
    end;
    // ???? ?????? ????????
    if lnk = nil then
      // ???????? ? ?????
      InsertLast(L)
    else
      // ????? ???????? ????? ????????? ???????
      InsertBefore(lnk);
  end;
end;

// ??????? ?????? ? ??????
//   ? ???????????? ? ???????????? ???????? ????????????
procedure TLink.Insert(L: TList; Order: TCompareFunc);
var
  lnk : TLink;
begin
  // ????? ????? ???????
  lnk := L.First;
  while lnk <> nil do
  begin
    if Order(Self, lnk) then
      Break;
    lnk := lnk.Next;
  end;
  // ???? ?????? ????????
  if lnk = nil then
    // ???????? ? ?????
    InsertLast(L)
  else
    // ????? ???????? ????? ????????? ???????
    InsertBefore(lnk);
end;

// ??????? ?????? ? ?????? ????? ????????? ??????
procedure TLink.InsertAfter(L: TLinkage);
begin
  Remove; 
  InsertTime := CurrentSim.SimTime;
  FPrev := L;
  FNext := L.FNext;
  FNext.FPrev := Self;
  L.FNext := Self;
  FHeader := FPrev.FHeader;
  Inc(FHeader.FSize);
  FHeader.LengthStat.AddData(FHeader.Size, CurrentSim.SimTime);
end;

// ??????? ?????? ? ?????? ????? ????????? ???????
procedure TLink.InsertBefore(L: TLinkage);
begin
  Remove;
  InsertTime := CurrentSim.SimTime;
  FNext := L;
  FPrev := L.FPrev;
  FPrev.FNext := Self;
  L.FPrev := Self;
  FHeader := FPrev.FHeader;
  Inc(FHeader.FSize);
  FHeader.LengthStat.AddData(FHeader.Size, CurrentSim.SimTime);
end;

// ??????? ?????? ? ?????? ?? ?????? ?????
procedure TLink.InsertFirst(L: TList);
begin
  InsertAfter(L);
end;

// ??????? ?????? ? ?????? ?? ????????? ?????
procedure TLink.InsertLast(L: TList);
begin
  InsertBefore(L);
end;

// ?????????? ?????? ?? ??????
procedure TLink.Remove;
begin
  if FNext <> nil then
  begin
    Dec(FHeader.FSize);
    FNext.FPrev := FPrev;
    FPrev.FNext := FNext;
    FNext := nil;
    FPrev := nil;
    FHeader.LengthStat.AddData(FHeader.Size, CurrentSim.SimTime);
    FHeader.WaitStat.AddData(CurrentSim.SimTime - InsertTime);
    FHeader := nil;
  end;
end;

{ TList }

// ??????? ?????? ? ????????? ???? ??????????? ? ??? ?????
procedure TList.Clear;
begin
  while not Empty do
    First.Free;
end;

// ??????? ?????????? ??????
procedure TList.ClearStat;
begin
  LengthStat.Clear(CurrentSim.SimTime);
  WaitStat.Clear;
end;

// ??????? ?????????? ?????? ? ????????? ? ??????? ???????
procedure TList.ClearStat(NewTime: Double);
begin
  LengthStat.Clear(NewTime);
  WaitStat.Clear;
end;

// ??????????? ???????????
constructor TList.Create;
begin
  FNext := Self;
  FPrev := Self;
  FSize := 0;
  FHeader := Self;
  OrderFunc := nil;
  LengthStat := TIntervalStatistics.Create(0, CurrentSim.SimTime);
  WaitStat := TStatistics.Create;
end;

// ??????????? ? ????????? ?? ???????
constructor TList.Create(SimTime: Double);
begin
  FNext := Self;
  FPrev := Self;
  FSize := 0;
  FHeader := Self;
  OrderFunc := nil;
  LengthStat := TIntervalStatistics.Create(0, SimTime);
  WaitStat := TStatistics.Create;
end;

// ??????????? ? ????????? ??????? ????????????
constructor TList.Create(Order: TCompareFunc);
begin
  FNext := Self;
  FPrev := Self;
  FSize := 0;
  FHeader := Self;
  OrderFunc := Order;
  LengthStat := TIntervalStatistics.Create(0, CurrentSim.SimTime);
  WaitStat := TStatistics.Create;
end;

// ??????????? ? ????????? ?? ??????? ? ????????? ??????? ????????????
constructor TList.Create(Order: TCompareFunc; SimTime: Double);
begin
  FNext := Self;
  FPrev := Self;
  FSize := 0;
  FHeader := Self;
  OrderFunc := Order;
  LengthStat := TIntervalStatistics.Create(0, SimTime);
  WaitStat := TStatistics.Create;
end;

destructor TList.Destroy;
begin
  // ??? ???????? ?????? ????????? ??? ??????????? ? ??? ???????
  Clear;
  LengthStat.Free;
  WaitStat.Free;
end;

//  ???????? ?????? ?? ???????
function TList.Empty: Boolean;
begin
  Result := (FNext = Self);
end;

// ????????? ?? ?????? ??????
function TList.First: TLink;
begin
  Result := Next;
end;

// ????????? ?? ????????? ??????
function TList.Last: TLink;
begin
  Result := Prev;
end;

// ????????? ??????? ????????????
procedure TList.SetOrderFunc(NewOrderFunc: TCompareFunc);
begin
  // ??????? ????? ??????????????? ?? ????? ?????? ????
  //   ? ?????? ??? ??????? ??????
  if (@OrderFunc <> nil) then
    raise ESimulationException.Create('Cannot redefine Order of a list');
  if not Empty then
    raise ESimulationException.Create(
        'Cannot define Order for a non-empty list');
  OrderFunc := NewOrderFunc;
end;

// ?????????? ??????? ??????
function TList.Size: Integer;
begin
  Result := FSize;
end;

// ????????? ?????????? ? ???????? ???????
procedure TList.StopStat(NewTime: Double);
begin
  LengthStat.StopStat(NewTime);
end;

procedure TList.StopStat;
begin
  StopStat(CurrentSim.SimTime);
end;

{ TProcess }

// ????????? ????????
procedure TProcess.Activate(const Procs: array of TProcess);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call Activate(Procs) from within other class');
  ActivateBefore(Procs, CurrentProc as TProcess);
end;

procedure TProcess.Activate;
begin
  ActivateBefore(CurrentProc as TProcess);
end;

procedure TProcess.Activate(Procs: TList);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call Activate(Procs) from within other class');
  ActivateBefore(Procs, CurrentProc as TProcess);
end;

procedure TProcess.ActivateAll(Procs: TList);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAll(Procs) from within other class');
  ActivateAllBefore(Procs, CurrentProc as TProcess);
end;

procedure TProcess.ActivateAll(const Procs: array of TProcess);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAll(Procs) from within other class');
  ActivateAllBefore(Procs, CurrentProc as TProcess);
end;

// ????????? ???????? ????? ?????????
procedure TProcess.ActivateAfter(const Procs : array of TProcess; p: TProcess);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAfter(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateAfter(Procs, p) after idle process');
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      Procs[i].ActivateAfter(p);
      Break;
    end;
end;

procedure TProcess.ActivateAfter(p: TProcess);
begin
  if not Idle then
    Exit;
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateAfter(p) after idle process');
  Event := TEventNotice.Create(p.Event.EventTime, Self);
  Event.InsertAfter(p.Event);
end;

procedure TProcess.ActivateAfter(Procs: TList; p: TProcess);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAfter(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateAfter(Procs, p) after idle process');
  lnk := Procs.First;
  // ????? ??????? ????????????????? ????????
  while (lnk <> nil) and not ((lnk is TProcess) and (lnk as TProcess).Idle) do
    lnk := lnk.Next;
  if lnk <> nil then
    (lnk as TProcess).ActivateAfter(p);
end;

procedure TProcess.ActivateAllAfter(Procs: TList; p: TProcess);
var
  lnk : TLink;
  PrevProc : TProcess;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllAfter(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateAllAfter(Procs, p) after idle process');
  PrevProc := p;
  lnk := Procs.First;
  while lnk <> nil do
  begin
    if (lnk is TProcess) and (lnk as TProcess).Idle then
    begin
      (lnk as TProcess).ActivateAfter(PrevProc);
      PrevProc := lnk as TProcess;
    end;
    lnk := lnk.Next;
  end;
end;

procedure TProcess.ActivateAllAfter(const Procs: array of TProcess;
  p: TProcess);
var
  i : Integer;
  PrevProc : TProcess;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllAfter(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateAllAfter(Procs, p) after idle process');
  PrevProc := p;
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      Procs[i].ActivateAfter(PrevProc);
      PrevProc := Procs[i];
    end;
end;

// ????????? ???????? ? ???????? ?????
procedure TProcess.ActivateAt(const Procs : array of TProcess; t: Double);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAt(Procs, t) from within other class');
  if t < SimTime then
    t := SimTime;
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      Procs[i].ActivateAt(t);
      Break;
    end;
end;

procedure TProcess.ActivateAt(t: Double);
begin
  if not Idle then
    Exit;
  if t < SimTime then
    t := SimTime;
  Event := TEventNotice.Create(t, Self);
  Event.Insert(Parent.Calendar);
end;

procedure TProcess.ActivateAt(Procs: TList; t: Double);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAt(Procs) from within other class');
  lnk := Procs.First;
  // ????? ??????? ????????????????? ????????
  while (lnk <> nil) and not ((lnk is TProcess) and (lnk as TProcess).Idle) do
    lnk := lnk.Next;
  if t < SimTime then
    t := SimTime;
  if lnk <> nil then
    (lnk as TProcess).ActivateAt(t);
end;

procedure TProcess.ActivateAllAt(const Procs: array of TProcess;
  t: Double);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAt(Procs, t) from within other class');
  if t < SimTime then
    t := SimTime;
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      Procs[i].ActivateAt(t);
    end;
end;

procedure TProcess.ActivateAllAt(Procs: TList; t: Double);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAt(Procs) from within other class');
  lnk := Procs.First;
  if t < SimTime then
    t := SimTime;
  while lnk <> nil do
  begin
    if (lnk is TProcess) and (lnk as TProcess).Idle then
      (lnk as TProcess).ActivateAt(t);
    lnk := lnk.Next;
  end;
end;

// ????????? ???????? ????? ????????
procedure TProcess.ActivateBefore(const Procs : array of TProcess; p: TProcess);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateBefore(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateBefore(Procs, p) before idle process');
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      Procs[i].ActivateBefore(p);
      Break;
    end;
end;

procedure TProcess.ActivateBefore(p: TProcess);
begin
  if not Idle then
    Exit;
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateBefore(p) before idle process');
  Event := TEventNotice.Create(p.Event.EventTime, Self);
  Event.InsertBefore(p.Event);
  if p = CurrentProc then
    RunNextProc;
end;

procedure TProcess.ActivateBefore(Procs: TList; p: TProcess);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateBefore(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateBefore(Procs, p) before idle process');
  lnk := Procs.First;
  // ????? ??????? ????????????????? ????????
  while (lnk <> nil) and not ((lnk is TProcess) and (lnk as TProcess).Idle) do
    lnk := lnk.Next;
  if lnk <> nil then
    (lnk as TProcess).ActivateBefore(p);
end;

procedure TProcess.ActivateAllBefore(const Procs: array of TProcess;
  p: TProcess);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllBefore(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateAllBefore(Procs, p) before idle process');
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      Procs[i].Event := TEventNotice.Create(p.Event.EventTime, Procs[i]);
      Procs[i].Event.InsertBefore(p.Event);
    end;
  if p = CurrentProc then
    RunNextProc;
end;

procedure TProcess.ActivateAllBefore(Procs: TList; p: TProcess);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllBefore(Procs, p) from within other class');
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot call ActivateAllBefore(Procs, p) before idle process');
  lnk := Procs.First;
  // ????? ??????? ????????????????? ????????
  while lnk <> nil do
  begin
    if (lnk is TProcess) and (lnk as TProcess).Idle then
    begin
      (lnk as TProcess).Event :=
          TEventNotice.Create(p.Event.EventTime, (lnk as TProcess));
      (lnk as TProcess).Event.InsertBefore(p.Event);
    end;
    lnk := lnk.Next;
  end;
  if p = CurrentProc then
    RunNextProc;
end;

// ????????? ???????? ? ???????? ?????????
procedure TProcess.ActivateDelay(const Procs : array of TProcess; t: Double);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateDelay(Procs, t) from within other class');
  if t < 0 then
    t := 0;
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      Procs[i].ActivateAt(SimTime + t);
      Break;
    end;
end;

procedure TProcess.ActivateDelay(t: Double);
begin
  ActivateAt(SimTime + t);
end;

procedure TProcess.ActivateDelay(Procs: TList; t: Double);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateDelay(Procs, t) from within other class');
  lnk := Procs.First;
  // ????? ??????? ????????????????? ????????
  while (lnk <> nil) and not ((lnk is TProcess) and (lnk as TProcess).Idle) do
    lnk := lnk.Next;
  if t < 0 then
    t := 0;
  if lnk <> nil then
    (lnk as TProcess).ActivateDelay(t);
end;

procedure TProcess.ActivateAllDelay(const Procs: array of TProcess;
  t: Double);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllDelay(Procs, t) from within other class');
  if t < 0 then
    t := 0;
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
      Procs[i].ActivateAt(SimTime + t);
end;

procedure TProcess.ActivateAllDelay(Procs: TList; t: Double);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllDelay(Procs, t) from within other class');
  if t < 0 then
    t := 0;
  lnk := Procs.First;
  while lnk <> nil do
  begin
    if (lnk is TProcess) and (lnk as TProcess).Idle then
      (lnk as TProcess).ActivateDelay(t);
    lnk := lnk.Next;
  end;
end;

// ????????? ???????? ? ???????? ????? ? ???????????
procedure TProcess.ActivatePriorAt(const Procs : array of TProcess; t: Double);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivatePriorAt(Procs, t) from within other class');
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      if t < SimTime then
        t := SimTime;
      Procs[i].ActivatePriorAt(t);
      Break;
    end;
end;

procedure TProcess.ActivatePriorAt(t: Double);
begin
  if not Idle then
    Exit;
  if t < SimTime then
    t := SimTime;
  Event := TEventNotice.Create(t, Self);
  Event.InsertPrior(Parent.Calendar);
  if t = SimTime then
    RunNextProc;
end;

procedure TProcess.ActivatePriorAt(Procs: TList; t: Double);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateProirAt(Procs) from within other class');
  lnk := Procs.First;
  // ????? ??????? ????????????????? ????????
  while (lnk <> nil) and not ((lnk is TProcess) and (lnk as TProcess).Idle) do
    lnk := lnk.Next;
  if t < SimTime then
    t := SimTime;
  if lnk <> nil then
    (lnk as TProcess).ActivatePriorAt(t);
end;

procedure TProcess.ActivateAllPriorAt(const Procs: array of TProcess;
  t: Double);
var
  i : Integer;
  PrevProc : TProcess;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllPriorAt(Procs, t) from within other class');
  if t < SimTime then
    t := SimTime;
  if t = SimTime then
    ActivateAllBefore(Procs, CurrentProc as TProcess)
  else
  begin
    PrevProc := nil;
    for i := 0 to Length(Procs) - 1 do
      if Procs[i].Idle then
      begin
        if PrevProc = nil then
          Procs[i].ActivatePriorAt(t)
        else
          Procs[i].ActivateAfter(PrevProc);
        PrevProc := Procs[i];
      end;
  end;
end;

procedure TProcess.ActivateAllPriorAt(Procs: TList; t: Double);
var
  lnk : TLink;
  PrevProc : TProcess;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllProirAt(Procs) from within other class');
  if t < SimTime then
    t := SimTime;
  if t = SimTime then
    ActivateAllBefore(Procs, CurrentProc as TProcess)
  else
  begin
    PrevProc := nil;
    lnk := Procs.First;
    while lnk <> nil do
    begin
      if (lnk is TProcess) and (lnk as TProcess).Idle then
      begin
        if PrevProc = nil then
          (lnk as TProcess).ActivatePriorAt(t)
        else
          (lnk as TProcess).ActivateAfter(PrevProc);
        PrevProc := lnk as TProcess;
      end;
      lnk := lnk.Next;
    end;
  end;
end;

// ????????? ???????? ? ???????? ????????? ? ???????????
procedure TProcess.ActivatePriorDelay(const Procs : array of TProcess; t: Double);
var
  i : Integer;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivatePriorDelay(Procs, t) from within other class');
  for i := 0 to Length(Procs) - 1 do
    if Procs[i].Idle then
    begin
      if t < 0 then
        t := 0;
      Procs[i].ActivatePriorAt(SimTime + t);
      Break;
    end;
end;

procedure TProcess.ActivatePriorDelay(t: Double);
begin
  if not Idle then
    Exit;
  if t < 0 then
    t := 0;
  Event := TEventNotice.Create(SimTime + t, Self);
  Event.InsertPrior(Parent.Calendar);
  if t = 0 then
    RunNextProc;
end;

procedure TProcess.ActivatePriorDelay(Procs: TList; t: Double);
var
  lnk : TLink;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivatePriorDelay(Procs, t) from within other class');
  lnk := Procs.First;
  // ????? ??????? ????????????????? ????????
  while (lnk <> nil) and not ((lnk is TProcess) and (lnk as TProcess).Idle) do
    lnk := lnk.Next;
  if t < 0 then
    t := 0;
  if lnk <> nil then
    (lnk as TProcess).ActivatePriorDelay(t);
end;

procedure TProcess.ActivateAllPriorDelay(const Procs: array of TProcess;
  t: Double);
var
  i : Integer;
  PrevProc : TProcess;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllPriorDelay(Procs, t) from within other class');
  if t < 0 then
    t := 0;
  if t = 0 then
    ActivateAllBefore(Procs, CurrentProc as TProcess)
  else
  begin
    PrevProc := nil;
    for i := 0 to Length(Procs) - 1 do
      if Procs[i].Idle then
      begin
        if PrevProc = nil then
          Procs[i].ActivatePriorDelay(t)
        else
          Procs[i].ActivateAfter(PrevProc);
        PrevProc := Procs[i];
      end;
  end;
end;

procedure TProcess.ActivateAllPriorDelay(Procs: TList; t: Double);
var
  lnk : TLink;
  PrevProc : TProcess;
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ActivateAllProirAt(Procs) from within other class');
  if t < 0 then
    t := 0;
  if t = 0 then
    ActivateAllBefore(Procs, CurrentProc as TProcess)
  else
  begin
    PrevProc := nil;
    lnk := Procs.First;
    while lnk <> nil do
    begin
      if (lnk is TProcess) and (lnk as TProcess).Idle then
      begin
        if PrevProc = nil then
          (lnk as TProcess).ActivatePriorDelay(t)
        else
          (lnk as TProcess).ActivateAfter(PrevProc);
        PrevProc := lnk as TProcess;
      end;
      lnk := lnk.Next;
    end;
  end;
end;

// ????????? ???????? ???????
procedure TProcess.ChangeResource(Res: TResource; Count: Integer);
var
  i : Integer;
begin
  // ???????? ???????? ???????
  Res.Add(Count, SimTime);
  // ???? ???????? ?????????, ???????????? ????????, ????????? ???????
  if Count > 0 then
  begin
    for i := 0 to Res.QueueCount - 1 do
    begin
      // ???????? ??????? ????????????? ????????.
      //   ?? ??????? ???????????, ?????? ??? ???????? ???????
      //   ???????? ?????? ??? ??????? ????????? ????????
      if Res.PreemptedProcs(i) > 0 then
        raise ESimulationException.Create(
            'Cannot change resource having preempted actions');
      ActivateAllDelay(Res.Queue[i], 0);
    end;
  end;
end;

// ??????? ????????, ??????????? ???? ??????
procedure TProcess.ClearFinished;
begin
  Parent.FinishedObjects.Clear;
end;

// ???????? ???????
procedure TProcess.CloseGate(Gate: TGate);
begin
  Gate.Close(SimTime);
end;

// ???????????.
//   ? ??????????? ??????? ?????????????? ??????????? ???????????
//   ?????? ?????????? ?????????
constructor TProcess.Create;
begin
  inherited Create;
end;

// ??????????.
//   ? ??????????? ??????? ?????????????? ?????????? ???????????
//   ?????? ?????????? ?????????
destructor TProcess.Destroy;
begin
  if Event <> nil then
     Event.Free;
  inherited;
end;

// ????? ???????? ??? ?????????? ??????? ????????
function TProcess.EventTime: Double;
begin
  if not Idle then
    Result := Event.EventTime
  else
    // ??? ?????????? ???????? ???????? ??????????? ????????
    Result := -1E300;
end;

// ??????? ????????? ????????.
//   ? ??????????? ????????? ?? ???????????.
//   ?????? ????? ???????????????? ????? RunProcess
procedure TProcess.Execute;
begin
  // ?????????????
  Init;
  // ????????? ? ??????? ????????? ?????????
  StartRunning;
  // ?????????? ????????????
  Detach;
  // ??????????? ??????? ?????? ??????????
  StartingTime := SimTime;
  // ?????????? ????????
  RunProcess;
  // ??????????
  FTerminated := True;
  // ?? ?????? ????????????????? ????????????? ???????????? ????????
  while True do
    Passivate;
end;

// ????????? ??????? ? ?????? ???????????
procedure TProcess.Finish;
begin
  Remove;
  Insert(Parent.FinishedObjects);
end;

// ?????? ???????
procedure TProcess.GetResource(Res: TResource; Count, Index: Integer);
begin
  // ?????? ? ??????? ????????
  Insert(Res.Queue[Index]);
  // ???? ??????? ????????? ??????? ?? ????? ???????
  while not Res.Get(Count, SimTime) do
    Passivate;
  // ????????? ??????? ??? ??????? (???????????? ??? ?????????)
  Res.CurrentProc := Self;
  Res.CurrentIndex := Index;
  // ?????? ? ?????? ????????? ????????? (??????? ??????? ????????)
  StartRunning;
end;

procedure TProcess.GetResource(Res: TResource; Count: Integer);
begin
  GetResource(Res, Count, 0);
end;

procedure TProcess.GetResource(Res: TResource);
begin
  GetResource(Res, 1, 0);
end;

procedure TProcess.GetResource(Res: TResource;
  GetRes: TGetResourceFunc; Index: Integer);
begin
  // ?????? ? ??????? ????????
  Insert(Res.Queue[Index]);
  // ???? ??????? ????????? ??????? ?? ????? ???????
  while not GetRes do
    Passivate;
  // ????????? ??????? ??? ??????? (???????????? ??? ?????????)
  Res.CurrentProc := Self;
  Res.CurrentIndex := Index;
  // ?????? ? ?????? ????????? ????????? (??????? ??????? ????????)
  StartRunning;
end;

procedure TProcess.GetResource(Res: TResource;
  GetRes: TGetResourceFunc);
begin
  GetResource(Res, GetRes, 0);
end;

// ????? ???????? ?? ???????? ?????
procedure TProcess.Hold(t: Double);
begin
  ReactivateDelay(t);
end;

// ???????? ?? ??????? ??????????
function TProcess.Idle: Boolean;
begin
  Result := Event = nil;
end;

// ???????????? ????????.
//   ? ??????????? ???????, ???? ????????????????, ?????????????? ?????
//   ?????????? ??????
procedure TProcess.Init;
begin
  // ??????? ?????? ?? ???????????? ??????? ????????
  if (Owner <> nil) and not (Owner is TSimulation) then
    Parent := (Owner as TProcess).Parent
  else
    Parent := Owner as TSimulation;
  // ?????????? ? ????????? ?????????
  Event := nil;
  TimeLeft := 0;
end;

// ???????, ???????? ??????????? ????????? ???????
function TProcess.NextEvent: TProcess;
begin
  if not Idle then
    Result := (Event.Next as TEventNotice).Proc
  else
    Result := nil;
end;

// ??????? ?????? ? ???????????? ??? ????????, ????????? ??? ????????
procedure TProcess.OpenGate(Gate: TGate);
begin
  Gate.Open(SimTime);
  ActivateAllDelay(Gate.Queue, 0);
end;

// ????????? ??????? ?? ????????? ???????
procedure TProcess.Passivate;
begin
  if Idle then
    Exit;
  Event.Free;
  Event := nil;
  if Self = CurrentProc then
    RunNextProc;
end;

// ??????????? ??????
procedure TProcess.PreemptResource(Res: TResource; Index: Integer);
var
  Proc : TProcess;
begin
  Proc := Res.CurrentProc;
  // ???????? ??????? ???????? ??? ?????????? ????????? ???????:
  // 1. ?????? ????? ????????? ????????
  // 2. ?????? ? ?????? ?????? ?????
  // 3. ?????? ????? ????????? ?????????
  //    (?? ????, ?????????, ??????? ??? ????????? ????????)
  // 4. ??????????????? ??????? ???????? ? ????? ???????????? ???????,
  //    ??? ??????????????? (?? ????, ? ??????? ? ??????? ????????)
  // 5. ??? ????????? ??????????? ???????? ??????? ????????? ??????????
  //    ?????? ????????? ???????????????? ????????
  // ??? ???????????? ?????? ?? ???? ??????? ?????? ?? ????? ???? ??????????,
  //    ? ??????? ?????? ?????? ? ??????? ???????? ???????
  if (Proc = nil) or (Res.Capacity <> 1) or (Res.Available > 0) or
      Proc.Idle or (Index > Res.CurrentIndex) or
      ((Index = Res.CurrentIndex) and (@Res.Priority <> nil) and
      not Res.Priority(Self, Proc)) then
    GetResource(Res, 1, Index)
  else
  begin
    // ???????? ???????
    // ????????? ?????, ?????????? ?? ?????????? ????????
    Proc.TimeLeft := Proc.Event.EventTime - SimTime;
    // ????????? ????????????? ??????? ?? ?????? ???????
    Proc.Event.Free;
    Proc.Event := nil;
    // ????????? ????????????? ??????? ?? ?????? ?????
    //   ? ?????? ??? ??????????? ????????
    Proc.InsertFirst(Res.Queue[Res.CurrentIndex]);
    // ?????????? ? ????? ????????? ??????
    //  (? ??????? ?????? ?? ?????????? ???????? ????????,
    //  ?? ???????????? ????? ???? ?????? ??? ???????? ??????????)
    Res.Release(1, SimTime);
    Res.Get(1, SimTime);
    // ????????? ??????????????? ??????? ??? ???????? ????????? ???????
    Res.CurrentProc := Self;
    Res.CurrentIndex := Index;
    // ????? ?? ??????? ??????? (???? ??????? ????)
    StartRunning;
  end;
end;

procedure TProcess.PreemptResource(Res: TResource);
begin
  PreemptResource(Res, 0);
end;

// ??????????? ??????, ?? ??????? ????????????? ??????? ? ??????? ????????
procedure TProcess.PreemptResourceNoWait(Res: TResource; Index: Integer);
var
  Proc : TProcess;
begin
  Proc := Res.CurrentProc;
  // ???????? ??????? ???????? ??? ?????????? ????????? ???????:
  // 1. ?????? ????? ????????? ????????
  // 2. ?????? ? ?????? ?????? ?????
  // 3. ?????? ????? ????????? ?????????
  //    (?? ????, ?????????, ??????? ??? ????????? ????????)
  // 4. ??????????????? ??????? ???????? ? ????? ???????????? ???????,
  //    ??? ??????????????? (?? ????, ? ??????? ? ??????? ????????)
  // 5. ??? ????????? ??????????? ???????? ??????? ????????? ??????????
  //    ?????? ????????? ???????????????? ????????
  // ??? ???????????? ?????? ?? ???? ??????? ?????? ?? ????? ???? ??????????,
  //    ? ??????? ?????? ?????? ? ??????? ???????? ???????
  if (Proc = nil) or (Res.Capacity <> 1) or (Res.Available > 0) or
      Proc.Idle or (Index > Res.CurrentIndex) or
      ((Index = Res.CurrentIndex) and (@Res.Priority <> nil) and
      not Res.Priority(Self, Proc)) then
    GetResource(Res, 1, Index)
  else
  begin
    // ???????? ???????
    // ????????? ?????, ?????????? ?? ?????????? ????????
    Proc.TimeLeft := Proc.Event.EventTime - SimTime;
    // ??????????? ????????????? ???????,
    //   ?? ????????? ????????? ???????? ????????
    Proc.ReactivateDelay(0);
    // ?????????? ? ????? ????????? ??????
    //  (? ??????? ?????? ?? ?????????? ???????? ????????,
    //  ?? ???????????? ????? ???? ?????? ??? ???????? ??????????)
    Res.Release(1, SimTime);
    Res.Get(1, SimTime);
    // ????????? ??????????????? ??????? ??? ???????? ????????? ???????
    Res.CurrentProc := Self;
    Res.CurrentIndex := Index;
    // ????? ?? ??????? ??????? (???? ??????? ????)
    StartRunning;
  end;
end;

procedure TProcess.PreemptResourceNoWait(Res: TResource);
begin
  PreemptResourceNoWait(Res, 0);
end;

// ?????????????? ??????? ????? ???????
procedure TProcess.Reactivate(Proc : TProcess);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call Reactivate(Proc) from within other class');
  ReactivateBefore(Proc, Self);
end;

procedure TProcess.Reactivate;
begin
  ReactivateBefore(CurrentProc as TProcess);
end;

// ?????????????? ??????? ????? ?????????
procedure TProcess.ReactivateAfter(Proc, p: TProcess);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ReactivateAfter(Proc, p) from within other class');
  if Proc.Idle then
    Proc.ActivateAfter(p)
  else
    Proc.ReactivateAfter(p);
end;

procedure TProcess.ReactivateAfter(p: TProcess);
begin
  if Idle then
  begin
    ActivateAfter(p);
    Exit;
  end;
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot ReactivateAfter(p) after idle process');
  Event.SetTime(p.Event.EventTime);
  if Self = CurrentProc then
    RunNextProc;
end;

// ?????????????? ??????? ? ???????? ?????
procedure TProcess.ReactivateAt(Proc : TProcess; t: Double);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ReactivateAt(Proc, t) from within other class');
  if Proc.Idle then
    Proc.ActivateAt(t)
  else
    Proc.ReactivateAt(t);
end;

procedure TProcess.ReactivateAt(t: Double);
begin
  if Idle then
  begin
    ActivateAt(t);
    Exit;
  end;
  if t < SimTime then
    t := SimTime;
  Event.SetTime(t);
  if Self = CurrentProc then
    RunNextProc;
end;

// ?????????????? ??????? ????? ????????
procedure TProcess.ReactivateBefore(Proc, p: TProcess);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ReactivateBefore(Proc, p) from within other class');
  if Proc.Idle then
    Proc.ActivateBefore(p)
  else
    Proc.ReactivateBefore(p);
end;

procedure TProcess.ReactivateBefore(p: TProcess);
begin
  if Idle then
  begin
    ActivateBefore(p);
    Exit;
  end;
  if p.Idle then
    raise ESimulationException.Create(
        'Cannot ReactivateBefore(p) before idle process');
  Event.SetTimePrior(p.EventTime);
  if (Self = CurrentProc) or (p = CurrentProc) then
    RunNextProc;
end;

// ?????????????? ??????? ? ?????????
procedure TProcess.ReactivateDelay(Proc : TProcess; t: Double);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ReactivateDelay(Proc, t) from within other class');
  if Proc.Idle then
    Proc.ActivateDelay(t)
  else
    Proc.ReactivateDelay(t);
end;

procedure TProcess.ReactivateDelay(t: Double);
begin
  if Idle then
  begin
    ActivateDelay(t);
    Exit;
  end;
  if t < 0 then
    t := 0;
  Event.SetTime(SimTime + t);
  if Self = CurrentProc then
    RunNextProc;
end;

// ?????????????? ??????? ? ???????? ????? ? ???????????
procedure TProcess.ReactivatePriorAt(Proc : TProcess; t: Double);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ReactivatePriorAt(Proc, t) from within other class');
  if Proc.Idle then
    Proc.ActivatePriorAt(t)
  else
    Proc.ReactivatePriorAt(t);
end;

procedure TProcess.ReactivatePriorAt(t: Double);
begin
  if Idle then
  begin
    ActivatePriorAt(t);
    Exit;
  end;
  if t < SimTime then
    t := SimTime;
  Event.SetTimePrior(t);
  if Self = CurrentProc then
    RunNextProc;
end;

// ?????????????? ??????? ? ???????? ????????? ? ???????????
procedure TProcess.ReactivatePriorDelay(Proc : TProcess; t: Double);
begin
  if Self <> CurrentProc then
    raise ESimulationException.Create(
        'Cannot call ReactivatePriorDelay(Proc, t) from within other class');
  if Proc.Idle then
    Proc.ActivatePriorDelay(t)
  else
    Proc.ReactivateDelay(t);
end;

procedure TProcess.ReactivatePriorDelay(t: Double);
begin
  if Idle then
  begin
    ActivatePriorDelay(t);
    Exit;
  end;
  if t < 0 then
    t := 0;
  Event.SetTimePrior(SimTime + t);
  if (Self = CurrentProc) or (t = 0) then
    RunNextProc;
end;

// ???????????? ???????
procedure TProcess.ReleaseResource(Res: TResource; Count: Integer);
var
  i, Index : Integer;
  Proc : TProcess;
begin
  Proc := nil;
  Index := -1;
  // ????? ??????????? ????????.
  // ???? ??????? ????, ?? ????????? ? ?????? ?????-???? ???????
  for i := 0 to Res.QueueCount - 1 do
    if Res.PreemptedProcs(i) > 0 then
    begin
      Proc := (Res.Queue[i].First as TProcess);
      Index := i;
      Break;
    end;
  // ???? ??????? ????
  if Proc <> nil then
  begin
    // ????????????? ?????? ????? ???? ?????? ????????? ????????
    if Count <> 1 then
      raise ESimulationException.Create(
          'Can release only 1 unit of preempted resource');
    // ?????????? ?????? ? ????? ?????? ???
    Res.Release(Count, SimTime);
    Res.Get(1, SimTime);
    // ???????? ?????? ??????????????? ????????
    Res.CurrentProc := Proc;
    Res.CurrentIndex := Index;
    // ??????? ??????? ?? ???????
    Proc.StartRunning;
    // ????????? ??????? ????????
    Proc.ActivateDelay(Proc.TimeLeft);
    // ???????? ??? ??? ?? ??????????
    Proc.TimeLeft := 0;
  end
  else
  begin
    // ???? ??????????? ???????? ???,
    //   ?????????, ????????????? ?? ?????? ?????????????
    if Count > 0 then
    begin
      // ?????????? ??????
      Res.Release(Count, SimTime);
      // ??????? ??????? ?? ?? ????????
      Res.CurrentProc := nil;
      Res.CurrentIndex := -1;
    end;
    // ???????????? ??? ????????, ????????? ???????
    for i := 0 to Res.QueueCount - 1 do
      ActivateAllDelay(Res.Queue[i], 0);
  end;
end;

procedure TProcess.ReleaseResource(Res: TResource);
begin
  ReleaseResource(Res, 1);
end;

// ??????? ? ?????????? ????????
procedure TProcess.RunNextProc;
begin
  Parent.FSimTime := (Parent.Calendar.First as TEventNotice).EventTime;
  SwitchTo((Parent.Calendar.First as TEventNotice).Proc);
end;

// ??????? ???????????? ?????
function TProcess.SimTime : Double;
begin
  Result := Parent.SimTime;
end;

// ????????? ???????? ? ?????? ?????????
procedure TProcess.StartRunning;
begin
  Remove;
  InsertLast(Parent.RunningObjects);
end;

// ?????????? ? ??????? ????????
procedure TProcess.Wait(l: TList);
begin
  Insert(l);
  Passivate;
end;

{ TEventNotice }

// ??????? ???????????? ????????? ???????
function CalendarOrder(A, B : TLink) : Boolean;
begin
  Result := (A as TEventNotice).EventTime < (B as TEventNotice).EventTime;
end;

// ??????? ???????????? ??? ???????????? ??????? ???????
function CalendarOrderPrior(A, B : TLink) : Boolean;
begin
  Result := (A as TEventNotice).EventTime <= (B as TEventNotice).EventTime;
end;

// ???????? ??????????? ? ???????
constructor TEventNotice.Create(ETime: Double; AProc: TProcess);
begin
  inherited Create;
  EventTime := ETime;
  Proc := AProc;
end;

// ???????? ??????????? ? ???????
destructor TEventNotice.Destroy;
begin
  Proc.Event := nil;
  inherited Destroy;
end;

// ??????? ??????????? ? ??????? ? ????????? ? ???????????
procedure TEventNotice.InsertPrior(l: TList);
begin
  Insert(l, CalendarOrderPrior);
end;

// ????????? ??????? ???????
procedure TEventNotice.SetTime(NewTime: Double);
var
  lst : TList;
begin
  lst := GetHeader;
  Remove;
  EventTime := NewTime;
  Insert(lst);
end;

// ????????? ??????? ??????? ? ???????????
procedure TEventNotice.SetTimePrior(NewTime: Double);
var
  lst : TList;
begin
  lst := GetHeader;
  Remove;
  EventTime := NewTime;
  InsertPrior(lst);
end;

{ TSimulation }

// ???????????.
//   ? ??????????? ?????? ??????????? ?????? ?????????? ?????????
constructor TSimulation.Create;
begin
  FSimTime := 0;
  FLastCleared := 0;
  CurrentSim := Self;
  Calendar := TList.Create(CalendarOrder, 0);
  RunningObjects := TList.Create;
  FinishedObjects := TList.Create;
  Visualizator := nil;
  inherited Create;
end;

// ??????????.
//   ? ??????????? ?????? ??????????? ?????? ?????????? ?????????
destructor TSimulation.Destroy;
begin
  Visualizator.Free;
  RunningObjects.Free;
  FinishedObjects.Free;
  Calendar.Free;
  inherited;
end;

// ??????? ????????? ?????????.
//   ? ??????????? ?????? ?? ????????????????.
//   ?????? ????? ???????????????? ????? RunSimulation
procedure TSimulation.Execute;
begin
  Init;
  Detach;
  if Visualizator <> nil then
    Visualizator.ActivateDelay(0);
  RunSimulation;
  Finalize;
  // ??????????
  FTerminated := True;
  while True do
    Detach;
end;

// ?????????? ?????? ?????????.
//   ???? ? ??????????? ?????? ????????????????,
//   ?????????????? ????? ?????????? ?????????
procedure TSimulation.Finalize;
begin
  FTerminated := True;
end;

// ????????? ???????? ?????????.
//   ???? ? ??????????? ?????? ????????????????,
//   ?????????????? ????? ?????????? ??????
procedure TSimulation.Init;
begin
  inherited;
  Parent := Self;
  Event := TEventNotice.Create(0, Self);
  Event.Insert(Calendar);
end;

// ???????????? ?????
function TSimulation.SimTime : Double;
begin
  Result := FSimTime;
end;

procedure TSimulation.ClearStat;
begin
  FLastCleared := SimTime;
  Calendar.ClearStat(SimTime);
end;

procedure TSimulation.StopStat;
begin
  Calendar.StopStat(SimTime);
end;

{ TStatistics }

// ?????????? ???????? ?????? ? ???????? ??????????
procedure TStatistics.AddData(NewX: Double);
begin
  // ???? ??? ?????? ??????? ??????
  if Count = 0 then
  begin
    // ???????????????? ??????????
    FMin := NewX;
    FMax := NewX;
  end
  // ?????? ???????
  else if NewX > FMax then
    FMax := NewX
  else if NewX < FMin then
    FMin := NewX;
  Inc(FCount);
  SumX := SumX + NewX;
  SumX_2 := SumX_2 + Sqr(NewX);
end;

// ??????? ??????????
procedure TStatistics.Clear;
begin
  FCount := 0;
  SumX := 0;
  SumX_2 := 0;
  FMin := 0;
  FMax := 0;
end;

// ???????????. ????????????? ??????????
constructor TStatistics.Create;
begin
  FCount := 0;
  SumX := 0;
  SumX_2 := 0;
  FMin := 0;
  FMax := 0;
end;

// ??????????? ??????????
function TStatistics.Deviation: Double;
begin
  Result := Sqrt(Disperse);
end;

// ?????????
function TStatistics.Disperse: Double;
begin
  if Count <= 1 then
    Result := 0
  else
    Result := (SumX_2 - Count * Sqr(Mean)) / (Count - 1);
end;

// ??????? ????????
function TStatistics.Mean: Double;
begin
  if Count = 0 then
    Result := 0
  else
    Result := SumX / Count;
end;

{ TIntervalStatistics }

// ?????????? ???????? ?????? ? ???????????? ??????????
procedure TIntervalStatistics.AddData(NewX, NewTime: Double);
var
  dt : Double;
begin
  // ?????????? ??????? ? ??????? ?????????? ??????????
  dt := NewTime - LastTime;
  if dt < 0 then
    // ????????? ????????
    raise ESimulationException.Create(
        'Cannot add interval statistics data prior to last time');
  // ???? ? ??????? ????????? ?????? ?????? ????????? ?????
  if dt > 0 then
  begin
    // ???? ??? ?????? ??????? ??????
    if TotalTime = 0 then
    begin
      // ???????????????? ??????????
      FMin := LastX;
      FMax := LastX;
    end
    // ????????? ????????? ????????
    else if LastX > FMax then
      FMax := LastX
    else if LastX < FMin then
      FMin := LastX;
    // ?????? ?????????? ??????? ? ?????????? ?????????
    //   (LastX) ? ??????? ??????? dt
    FTotalTime := FTotalTime + dt;
    SumX := SumX + LastX * dt;
    SumX_2 := SumX_2 + Sqr(LastX) * dt;
  end;
  // ???????? ?????????
  LastX := NewX;
  LastTime := NewTime;
end;

procedure TIntervalStatistics.AddData(NewX: Double);
begin
  AddData(NewX, CurrentSim.SimTime);
end;

// ??????? ??????????
procedure TIntervalStatistics.Clear(NewTime : Double);
begin
  SumX := 0;
  SumX_2 := 0;
  FTotalTime := 0;
  LastTime := NewTime;
  FMin := 0;
  FMax := 0;
end;

procedure TIntervalStatistics.Clear;
begin
  Clear(CurrentSim.SimTime);
end;

// ????????????
constructor TIntervalStatistics.Create(InitX, InitTime: Double);
begin
  LastX := InitX;
  LastTime := InitTime;
  FTotalTime := 0;
  SumX := 0;
  SumX_2 := 0;
  FMin := 0;
  FMax := 0;
end;

constructor TIntervalStatistics.Create(InitX: Double);
begin
  LastX := InitX;
  LastTime := CurrentSim.SimTime;
  FTotalTime := 0;
  SumX := 0;
  SumX_2 := 0;
  FMin := 0;
  FMax := 0;
end;

// ??????????? ??????????
function TIntervalStatistics.Deviation: Double;
begin
  if Disperse < 0 then
    Result := 0
  else
    Result := Sqrt(Disperse);
end;

// ?????????
function TIntervalStatistics.Disperse: Double;
begin
  if TotalTime = 0 then
    Result := 0
  else
    Result := SumX_2 / TotalTime - Sqr(Mean);
end;

// ?????????????? ????????
function TIntervalStatistics.Mean: Double;
begin
  if TotalTime = 0 then
    Result := 0
  else
    Result := SumX / TotalTime;
end;

// ????????? ?????????? ? ???????? ???????
procedure TIntervalStatistics.StopStat(NewTime: Double);
begin
  AddData(LastX, NewTime);
end;

procedure TIntervalStatistics.StopStat;
begin
  StopStat(CurrentSim.SimTime);
end;

{ TRandom }

function TRandom.Beta(Alpha, Betta: Double): Double;
var
  G1, G2 : Double;
begin
  G1 := Gamma(1, Alpha);
  G2 := Gamma(1, Betta);
  Result := G1 / (G1 + G2);
end;

function TRandom.Beta(Min, Mean, Max, Sigma: Double): Double;
var
  Alpha, Betta, M, S2 : Double;
begin
  M := (Mean - Min) / (Max - Min);
  S2 := Sqr(Sigma) / Sqr(Max - Min);
  Alpha := M * (M - Sqr(M) - S2) / S2;
  Betta := Alpha * (1 - M) / M;
  Result := Beta(Alpha, Betta) * (Max - Min) + Min;
end;

constructor TRandom.Create;
begin
  Randomize;
  FSeed := Trunc(Random * $7FFFFFFF) or 1;
  NextInt;
end;

constructor TRandom.Create(Seed: Integer);
begin
  FSeed := Seed;
  NextInt;
end;

function TRandom.Draw(A: Double): Boolean;
begin
  Result := NextFloat < A;
end;

function TRandom.Erlang(Mean: Double; K: Integer): Double;
var
  i : Integer;
  Res : Double;
begin
  Res := 1;
  for i := 1 to K do
    Res := Res * (1 - NextFloat);
  Result := -Mean * Ln(Res);
end;

function TRandom.Exponential(Mean: Double): Double;
begin
  Result := NegExp(1 / Mean);
end;

function TRandom.Gamma(Beta, Alpha: Double): Double;
var
  b, X, Y, W, Prod : Double;
  a, i : Integer;
begin
  if Abs(Round(Alpha) - Alpha) < 1e-10 * Abs(Alpha) then
    Result := Erlang(Beta, Round(Alpha))
  else if Alpha < 1 then
  begin
    repeat
      X := Exp(Ln(1 - NextFloat) / Alpha);
      Y := Exp(Ln(1 - NextFloat) / (1 - Alpha));
    until X + Y <= 1;
    W := X / (X + Y);
    Result := -W * Ln(1 - NextFloat) * Beta;
  end
  else if Alpha < 5 then
  begin
    a := Trunc(Alpha);
    b := Alpha - a;
    repeat
      Prod := 1;
      for i := 1 to a do
        Prod := Prod * (1 - NextFloat);
      X := - Alpha / a * Ln(Prod);
    until 1 - NextFloat <= Exp(Ln(X / Alpha) * b) * Exp(-b * (X / Alpha - 1));
    Result := X * Beta;
  end
  else if 1 - NextFloat >= Alpha - Trunc(Alpha) then
    Result := Erlang(Beta, Trunc(Alpha))
  else
    Result := Erlang(Beta, Trunc(Alpha) + 1);
end;

function TRandom.LogNormal(Mean, Sigma: Double): Double;
var
  SigmaN2, MeanN : Double;
begin
  SigmaN2 := Ln(Sqr(Sigma) / Sqr(Mean) + 1);
  MeanN := Ln(Mean) - SigmaN2 / 2;
  Result := Exp(Normal(MeanN, Sqrt(SigmaN2)));
end;

function TRandom.NegExp(Mean: Double): Double;
begin
  Result := -Ln(1 - NextFloat) / Mean;
end;

function TRandom.NextFloat: Double;
begin
  Result := (NextInt / $40000000) / 2;
end;

function TRandom.NextInt: Integer;
var
  L : Int64;
const
  a = 843314861;
  c = 453816693;
  m2 = 1073741824;
begin
  L := FSeed;
  L := L * a + c;
  FSeed := Integer(L);
  if FSeed < 0 then
  begin
    Inc(FSeed, m2);
    Inc(FSeed, m2);
  end;
  Result := FSeed;
end;

function TRandom.NextInt(High: Integer): Integer;
begin
  Result := Int64(NextInt) * High div $40000000 div 2;
end;

function TRandom.NextInt(Low, High: Integer): Integer;
begin
  Result := NextInt(High - Low) + Low;
end;

function Phi(Alpha : Double) : Double;
var
  Theta : Double;
begin
  Theta := Sqrt(-2 * Ln(Alpha));
  Result := (2.515517 + 0.802853 * Theta + 0.010328 * Sqr(Theta)) /
      (1 + 1.432788 * Theta + 0.189269 * Sqr(Theta) +
      0.001308 * Sqr(Theta) * Theta) - Theta;
end;

function TRandom.Normal(Mean, Sigma: Double): Double;
var
  Rnd1, Rnd2, W : Double;
begin
  if HasNextNormal then
  begin
    HasNextNormal := False;
    Result := NextNormal * Sigma + Mean;
  end
  else
  begin
    HasNextNormal := True;
    repeat
      Rnd1 := 2 * NextFloat - 1;
      Rnd2 := 2 * NextFloat - 1;
      W := Sqr(Rnd1) + Sqr(Rnd2);
    until W <= 1;
    NextNormal := Rnd2 * Sqrt(-2 * Ln(W) / W);
    Result := Rnd1 * Sqrt(-2 * Ln(W) / W) * Sigma + Mean;
  end;
end;

function TRandom.Poisson(Lambda: Double): Integer;
var
  i : Integer;
  Prod, Border : Double;
begin
  Border := Exp(-Lambda);
  Prod := NextFloat;
  i := 0;
  while Prod >= Border do
  begin
    Prod := Prod * NextFloat;
    Inc(i);
  end;
  Result := i;
end;

function TRandom.PSNorm(Mean, Sigma: Double; Count: Integer): Double;
var
  Sum : Double;
  i : Integer;
begin
  Sum := 0;
  for i := 1 to Count do
    Sum := Sum + NextFloat;
  Sum := Sqrt(12 / Count) * (Sum - Count / 2);
  Sum := Sum + (Sum * Sum * Sum - 3 * Sum) / 20 / Count;
  Result := Mean + Sigma * Sum;
end;

// ????????? ?????????? ??????? ?? ??????? Table ??????, ???
//  Table[i - 1] <= Random < Table[i]
//  0, ???? Random < Table[0]
//  N, ???? Random >= Table[N - 1], N - ?????? ???????
//  ?????? Table ?????? ???? ?????????? ?? ???????????!
function TRandom.TableIndex(Table: array of Double): Integer;
var
  L, R, M : Integer;
  Rnd : Double;
begin
  if Length(Table) = 0 then
    Result := 0
  else
  begin
    Rnd := NextFloat;
    // ???????? ????? ?????????? ???????? ??? ??????? ????????
    L := 0;
    R := Length(Table) - 1;
    while L < R do
    begin
      M := (L + R) div 2;
      if Rnd > Table[M] then
        L := M + 1
      else
        R := M;
    end;
    if Rnd < Table[L] then
      Result := L
    else
      // ????????, ?????? ??????????, ???????????
      //   ?????????? ?????????.
      // ????????, ??????????? ?????????, ????? ?????????
      //   ?????? ? ????????? ?????????
      Result := L + 1
  end;
end;

function TRandom.Triangular(A, Moda, B: Double): Double;
var
  Rnd : Double;
begin
  Rnd := NextFloat;
  if Rnd <= (Moda - A) / (B - A) then
    Result := A + Sqrt((Moda - A) * (B - A) * Rnd)
  else
    Result := B - Sqrt((B - Moda) * (B - A) * (1 - Rnd));
end;

function TRandom.Uniform(A, B: Double): Double;
begin
  Result := NextFloat * (B - A) + A;
end;

function TRandom.Weibull(Beta, Alpha: Double): Double;
begin
  Result := Exp(Ln(-Beta * Ln(1 - NextFloat)) / Alpha);
end;

{ TUniformHistogram }

procedure TUniformHistogram.AddData(d: Double);
var
  iStep : Integer;
begin
  if d >= FLow then
  begin
    iStep := Trunc((d - FLow) / FStep) + 1;
    if iStep > FIntervalCount + 1 then
      iStep := FIntervalCount + 1;
  end
  else
    iStep := 0;
  Inc(Data[iStep]);
  Inc(FTotalCount);
end;

procedure TUniformHistogram.Clear;
var
  i : Integer;
begin
  for i := 0 to FIntervalCount + 1 do
    Data[i] := 0;
  FTotalCount := 0;
end;

constructor TUniformHistogram.Create(ALow, AStep: Double;
  AIntervalCount: Integer);
begin
  FLow := ALow;
  FStep := AStep;
  FIntervalCount := AIntervalCount;
  SetLength(Data, FIntervalCount + 2);
  Clear;
end;

function TUniformHistogram.GetCount(i: Integer): Integer;
begin
  if i <= 0 then
    Result := Data[0]
  else if i >= FIntervalCount + 1 then
    Result := Data[FIntervalCount + 1]
  else
    Result := Data[i];
end;

function TUniformHistogram.GetCumulativeCount(i: Integer): Integer;
var
  j, Sum : Integer;
begin
  Sum := 0;
  for j := 0 to i do
    if j <= FIntervalCount + 1 then
      Sum := Sum + Data[j];
  Result := Sum;
end;

function TUniformHistogram.GetCumulativePercent(i: Integer): Double;
begin
  if FTotalCount = 0 then
    Result := 0
  else
    Result := GetCumulativeCount(i) / FTotalCount;
end;

function TUniformHistogram.GetIntervalCount: Integer;
begin
  Result := FIntervalCount;
end;

function TUniformHistogram.GetLowerBound(i: Integer): Double;
begin
  if i <= 0 then
    Result := -1e300
  else if i >= FIntervalCount + 1 then
    Result := FLow + FStep * FIntervalCount
  else
    Result := FLow + FStep * (i - 1);
end;

function TUniformHistogram.GetPercent(i: Integer): Double;
begin
  if FTotalCount = 0 then
    Result := 0
  else
    Result := GetCount(i) / FTotalCount;
end;

function TUniformHistogram.GetUpperBound(i: Integer): Double;
begin
  if i <= 0 then
    Result := FLow
  else if i >= FIntervalCount + 1 then
    Result := 1e300
  else
    Result := FLow + FStep * i;
end;

procedure TUniformHistogram.SetIntervalCount(NewCount: Integer);
begin
  FIntervalCount := NewCount;
  SetLength(Data, FIntervalCount + 2);
  Clear;
end;

procedure TUniformHistogram.SetLowerBound(i: Integer; Value: Double);
begin
end;

procedure TUniformHistogram.SetUpperBound(i: Integer; Value: Double);
begin
end;

procedure DumpEventQueue;
var
  lk : TLinkage;
  en : TEventNotice;
begin
  lk := CurrentSim.Calendar.First;
  en := lk as TEventNotice;
  while lk <> nil do
  begin
    Write(en.EventTime : 5 : 2, ' : ', en.Proc.ClassName, ';');
    lk := lk.Next;
    en := lk as TEventNotice;
  end;
  WriteLn;
end;

{ THistogram }

constructor THistogram.Create;
begin
  FTotalCount := 0;
end;

{ TTimeBetStatistics }

procedure TTimeBetStatistics.AddData(NewTime: Double);
var
  dt : Double;
begin
  if Count < 0 then
    LastTime := NewTime
  else
  begin
    dt := NewTime - LastTime;
    if Count = 0 then
    begin
      FMax := dt;
      FMin := dt;
    end
    else if dt > FMax then
      FMax := dt
    else if dt < FMin then
      FMin := dt;
    SumX := SumX + dt;
    SumX_2 := SumX_2 + Sqr(dt);
    LastTime := NewTime;
  end;
  Inc(FCount);
end;

procedure TTimeBetStatistics.AddData;
begin
  if CurrentProc is TProcess then
    AddData((CurrentProc as TProcess).SimTime)
  else
    raise ESimulationException.Create(
        'Cannot add time data with no current process');
end;

procedure TTimeBetStatistics.Clear;
begin
  LastTime := -1;
  FCount := -1;
  SumX := 0;
  SumX_2 := 0;
end;

constructor TTimeBetStatistics.Create;
begin
  LastTime := -1;
  FCount := -1;
  SumX := 0;
  SumX_2 := 0;
end;

function TTimeBetStatistics.Deviation: Double;
begin
  Result := Sqrt(Disperse);
end;

function TTimeBetStatistics.Disperse: Double;
begin
  if Count <= 1 then
    Result := 0
  else
    Result := (SumX_2 - Count * Sqr(Mean)) / (Count - 1);
end;

function TTimeBetStatistics.Mean: Double;
begin
  if Count <= 0 then
    Result := 0
  else
    Result := SumX / Count;
end;

{ TActionStatistics }

procedure TActionStatistics.Clear(NewTime : Double);
begin
  FTotalTime := 0;
  FFinished := 0;
  FMax := 0;
  SumX := 0;
  SumX_2 := 0;
  LastTime := NewTime;
end;

procedure TActionStatistics.Clear;
begin
  Clear((CurrentProc as TProcess).SimTime);
end;

constructor TActionStatistics.Create(InitX: Integer; InitTime: Double);
begin
  FTotalTime := 0;
  FFinished := 0;
  FMax := InitX;
  LastTime := InitTime;
  LastX := InitX;
  SumX := 0;
  SumX_2 := 0;
end;

constructor TActionStatistics.Create(InitX: Integer);
begin
  FTotalTime := 0;
  FFinished := 0;
  FMax := InitX;
  LastTime := 0;
  LastX := InitX;
  SumX := 0;
  SumX_2 := 0;
end;

constructor TActionStatistics.Create;
begin
  FTotalTime := 0;
  FFinished := 0;
  FMax := 0;
  LastTime := 0;
  LastX := 0;
  SumX := 0;
  SumX_2 := 0;
end;

function TActionStatistics.Deviation: Double;
begin
  Result := Sqrt(Disperse);
end;

function TActionStatistics.Disperse: Double;
begin
  if TotalTime = 0 then
    Result := 0
  else
    Result := SumX_2 / TotalTime - Sqr(Mean);
end;

procedure TActionStatistics.Finish(NewTime: Double);
var
  dt : Double;
begin
  if NewTime > LastTime then
  begin
    dt := NewTime - LastTime;
    LastTime := NewTime;
    SumX := SumX + LastX * dt;
    SumX_2 := SumX_2 + Sqr(LastX) * dt;
    FTotalTime := FTotalTime + dt;
    if LastX > FMax then
      FMax := LastX;
  end;
  Dec(LastX);
  Inc(FFinished);
end;

procedure TActionStatistics.Finish;
begin
  Finish((CurrentProc as TProcess).SimTime);
end;

function TActionStatistics.Mean: Double;
begin
  if TotalTime = 0 then
    Result := 0
  else
    Result := SumX / TotalTime;
end;

procedure TActionStatistics.Preempt(NewTime: Double);
var
  dt : Double;
begin
  if NewTime > LastTime then
  begin
    dt := NewTime - LastTime;
    LastTime := NewTime;
    SumX := SumX + LastX * dt;
    SumX_2 := SumX_2 + Sqr(LastX) * dt;
    FTotalTime := FTotalTime + dt;
    if LastX > FMax then
      FMax := LastX;
  end;
  Dec(LastX);
end;

procedure TActionStatistics.Preempt;
begin
  Preempt((CurrentProc as TProcess).SimTime);
end;

procedure TActionStatistics.Resume(NewTime: Double);
begin
  Start(NewTime);
end;

procedure TActionStatistics.Resume;
begin
  Start;
end;

procedure TActionStatistics.Start(NewTime: Double);
var
  dt : Double;
begin
  if NewTime > LastTime then
  begin
    dt := NewTime - LastTime;
    LastTime := NewTime;
    SumX := SumX + LastX * dt;
    SumX_2 := SumX_2 + Sqr(LastX) * dt;
    FTotalTime := FTotalTime + dt;
  end;
  Inc(LastX);
end;

procedure TActionStatistics.Start;
begin
  Start((CurrentProc as TProcess).SimTime);
end;

procedure TActionStatistics.StopStat(NewTime: Double);
var
  dt : Double;
begin
  if NewTime > LastTime then
  begin
    dt := NewTime - LastTime;
    LastTime := NewTime;
    SumX := SumX + LastX * dt;
    SumX_2 := SumX_2 + Sqr(LastX) * dt;
    FTotalTime := FTotalTime + dt;
  end;
end;

procedure TActionStatistics.StopStat;
begin
  StopStat((CurrentProc as TProcess).SimTime);
end;

{ TServiceStatistics }

procedure TServiceStatistics.Clear(NewTime: Double);
begin
  FFinished := 0;
  FMaxBusyTime := 0;
  FMaxIdleTime := 0;
  FMaxBusy := LastUtil;
  FMinBusy := LastUtil;
  LastBlockTime := 0;
  LastBusyStart := NewTime;
  LastBusyTime := 0;
  LastIdleTime := 0;
  LastIdleStart := NewTime;
  LastTime := NewTime;
  SumBlockage := 0;
  SumX := 0;
  SumX_2 := 0;
  FTotalTime := 0;
end;

procedure TServiceStatistics.Clear;
begin
  Clear((CurrentProc as TProcess).SimTime);
end;

constructor TServiceStatistics.Create(Devices, InitUtil: Integer;
  InitTime: Double);
begin
  DeviceCount := Devices;
  FFinished := 0;
  FMaxBusyTime := 0;
  FMaxIdleTime := 0;
  FMaxBusy := 0;
  FMinBusy := Devices;
  LastBlockage := 0;
  LastBlockTime := 0;
  LastBusyStart := InitTime;
  LastBusyTime := 0;
  LastIdleTime := 0;
  LastIdleStart := InitTime;
  LastTime := InitTime;
  LastUtil := InitUtil;
  SumBlockage := 0;
  SumX := 0;
  SumX_2 := 0;
  FTotalTime := 0;
end;

constructor TServiceStatistics.Create(Devices: Integer);
begin
  DeviceCount := Devices;
  FFinished := 0;
  FMaxBusyTime := 0;
  FMaxIdleTime := 0;
  FMaxBusy := 0;
  FMinBusy := Devices;
  LastBlockage := 0;
  LastBlockTime := 0;
  LastBusyStart := 0;
  LastBusyTime := 0;
  LastIdleTime := 0;
  LastIdleStart := 0;
  LastTime := 0;
  LastUtil := 0;
  SumBlockage := 0;
  SumX := 0;
  SumX_2 := 0;
  FTotalTime := 0;
end;

constructor TServiceStatistics.Create;
begin
  DeviceCount := 1;
  FFinished := 0;
  FMaxBusyTime := 0;
  FMaxIdleTime := 0;
  FMaxBusy := 0;
  FMinBusy := Devices;
  LastBlockage := 0;
  LastBlockTime := 0;
  LastBusyStart := 0;
  LastBusyTime := 0;
  LastIdleTime := 0;
  LastIdleStart := 0;
  LastTime := 0;
  LastUtil := 0;
  SumBlockage := 0;
  SumX := 0;
  SumX_2 := 0;
  FTotalTime := 0;
end;

function TServiceStatistics.Deviation: Double;
begin
  Result := Sqrt(Disperse);
end;

function TServiceStatistics.Disperse: Double;
begin
  if TotalTime = 0 then
    Result := 0
  else
    Result := SumX_2 / TotalTime - Sqr(Mean);
end;

procedure TServiceStatistics.Finish(NewTime: Double);
var
  dt : Double;
begin
  dt := NewTime - LastTime;
  if dt < 0 then
    dt := 0;
  if dt > 0 then
  begin
    SumX := SumX + LastUtil * dt;
    SumX_2 := SumX_2 + Sqr(LastUtil) * dt;
    if LastUtil > FMaxBusy then
      FMaxBusy := LastUtil
    else if LastUtil < FMinBusy then
      FMinBusy := LastUtil;
    FTotalTime := FTotalTime + dt;
    LastTime := NewTime;
  end;
  Dec(LastUtil);
  Inc(FFinished);
  if LastUtil = 0 then
  begin
    dt := NewTime - LastBusyStart;
    if LastIdleTime = 0 then
      LastBusyTime := LastBusyTime + dt
    else
      LastBusyTime := dt;
    if LastBusyTime > FMaxBusyTime then
      FMaxBusyTime := LastBusyTime;
    LastIdleStart := NewTime;
  end;
end;

procedure TServiceStatistics.Finish;
begin
  Finish((CurrentProc as TProcess).SimTime);
end;

procedure TServiceStatistics.FinishBlock(NewTime: Double);
var
  dt : Double;
begin
  dt := NewTime - LastBlockTime;
  if dt < 0 then
    dt := 0;
  if dt > 0 then
  begin
    SumBlockage := SumBlockage + dt * LastBlockage;
    LastBlockTime := NewTime;
  end;
  Dec(LastBlockage);
end;

procedure TServiceStatistics.FinishBlock;
begin
  FinishBlock((CurrentProc as TProcess).SimTime);
end;

function TServiceStatistics.Mean: Double;
begin
  if TotalTime = 0 then
    Result := 0
  else
    Result := SumX / TotalTime;
end;

function TServiceStatistics.MeanBlockage: Double;
begin
  if TotalTime = 0 then
    Result := 0
  else
    Result := SumBlockage / TotalTime;
end;

procedure TServiceStatistics.Start(NewTime: Double);
var
  dt : Double;
begin
  dt := NewTime - LastTime;
  if dt < 0 then
    dt := 0;
  if dt > 0 then
  begin
    SumX := SumX + LastUtil * dt;
    SumX_2 := SumX_2 + Sqr(LastUtil) * dt;
    if LastUtil > FMaxBusy then
      FMaxBusy := LastUtil
    else if LastUtil < FMinBusy then
      FMinBusy := LastUtil;
    FTotalTime := FTotalTime + dt;
    LastTime := NewTime;
  end;
  Inc(LastUtil);
  if LastUtil = 1 then
  begin
    dt := NewTime - LastIdleStart;
    if LastBusyTime = 0 then
      LastIdleTime := LastIdleTime + dt
    else
      LastIdleTime := dt;
    if LastIdleTime > FMaxIdleTime then
      FMaxIdleTime := LastIdleTime;
    LastBusyStart := NewTime;
  end;
end;

procedure TServiceStatistics.Start;
begin
  Start((CurrentProc as TProcess).SimTime);
end;

procedure TServiceStatistics.StartBlock(NewTime: Double);
var
  dt : Double;
begin
  dt := NewTime - LastBlockTime;
  if dt < 0 then
    dt := 0;
  if dt > 0 then
  begin
    SumBlockage := SumBlockage + dt * LastBlockage;
    LastBlockTime := NewTime;
  end;
  Inc(LastBlockage);
end;

procedure TServiceStatistics.StartBlock;
begin
  StartBlock((CurrentProc as TProcess).SimTime);
end;

procedure TServiceStatistics.StopStat(NewTime: Double);
var
  dt : Double;
begin
  dt := NewTime - LastTime;
  if dt < 0 then
    Exit;
  if dt > 0 then
  begin
    SumX := SumX + LastUtil * dt;
    SumX_2 := SumX_2 + Sqr(LastUtil) * dt;
    if LastUtil > FMaxBusy then
      FMaxBusy := LastUtil
    else if LastUtil < FMinBusy then
      FMinBusy := LastUtil;
    FTotalTime := FTotalTime + dt;
    dt := NewTime - LastBlockTime;
    SumBlockage := SumBlockage + dt * LastBlockage;
    LastTime := NewTime;
    LastBlockTime := NewTime;
  end;
  if LastUtil = 0 then
  begin
    dt := NewTime - LastIdleStart;
    if LastBusyTime = 0 then
      LastIdleTime := LastIdleTime + dt
    else
      LastIdleTime := dt;
    if LastIdleTime > FMaxIdleTime then
      FMaxIdleTime := LastIdleTime;
    LastBusyTime := 0;
    LastIdleStart := NewTime;
  end
  else
  begin
    dt := NewTime - LastBusyStart;
    if LastIdleTime = 0 then
      LastBusyTime := LastBusyTime + dt
    else
      LastBusyTime := dt;
    if LastBusyTime > FMaxBusyTime then
      FMaxBusyTime := LastBusyTime;
    LastIdleTime := 0;
    LastBusyStart := NewTime;
  end;
end;

procedure TServiceStatistics.StopStat;
begin
  StopStat((CurrentProc as TProcess).SimTime);
end;

procedure WriteStat(Header : string; Stat : TStatistics);
begin
  WriteLn(Header);
  WriteLn('Average = ', Stat.Mean : 6 : 3, ' +- ', Stat.Deviation : 6 : 3);
  WriteLn('Min = ', Stat.Min : 6 : 3, ', max = ', Stat.Max : 6 : 3);
  WriteLn('Total = ', Stat.Count : 4, ' values');
end;

procedure WriteStat(Header : string; Stat : TIntervalStatistics);
begin
  WriteLn(Header);
  WriteLn('Average = ', Stat.Mean : 6 : 3, ' +- ', Stat.Deviation : 6 : 3);
  WriteLn('Min = ', Stat.Min : 6 : 3, ', max = ', Stat.Max : 6 : 3);
  WriteLn('Total = ', Stat.TotalTime : 6 : 3, ' time units, current value = ',
      Stat.LastX : 6 : 3);
end;

procedure WriteStat(Header : string; Stat : TTimeBetStatistics);
begin
  WriteLn(Header);
  WriteLn('Average = ', Stat.Mean : 6 : 3, ' +- ', Stat.Deviation : 6 : 3);
  WriteLn('Min = ', Stat.Min : 6 : 3, ', max = ', Stat.Max : 6 : 3);
  WriteLn('Total = ', Stat.Count : 4, ' values');
end;

procedure WriteStat(Header : string; Stat : TActionStatistics);
begin
  WriteLn(Header);
  WriteLn('Average = ', Stat.Mean : 6 : 3, ' +- ', Stat.Deviation : 6 : 3);
  WriteLn('Max = ', Stat.Max : 4);
  WriteLn('Current running = ', Stat.Running : 4, ', completed = ',
      Stat.Finished : 4);
end;

procedure WriteStat(Header : string; Stat : TServiceStatistics);
begin
  WriteLn(Header);
  WriteLn('Device count = ', Stat.Devices : 4);
  WriteLn('Average usage = ', Stat.Mean : 6 : 3, ' +- ', Stat.Deviation : 6 : 3);
  WriteLn('Current running = ', Stat.Running : 4, ', completed = ',
      Stat.Finished : 4);
  WriteLn('Average blockage = ', Stat.MeanBlockage : 6 : 3);
  if Stat.Devices = 1 then
    WriteLn('Max idle time = ', Stat.MaxIdleTime : 6 : 3,
        ', max busy time = ', Stat.MaxBusyTime : 6 : 3)
  else
    WriteLn('Max idle devices = ', Stat.Devices - Stat.MinBusy : 4,
        ', max busy devices = ', Stat.MaxBusy : 4);
end;

procedure WriteStat(Header : string; Queue : TList);
begin
  WriteLn(Header);
  WriteLn('Average length = ', Queue.LengthStat.Mean : 6 : 3, ' +- ',
      Queue.LengthStat.Deviation : 6 : 3);
  WriteLn('Max = ', Queue.LengthStat.Max : 2 : 0, ', current = ',
      Queue.Size : 2);
  WriteLn('Average waiting time = ', Queue.WaitStat.Mean : 6 : 3);
end;

procedure WriteStat(Header : string; Res : TResource);
begin
  WriteLn(Header);
  WriteLn('Current capacity = ', Res.Capacity);
  WriteLn('Utilization = ', Res.BusyStat.Mean : 6 : 3, ' +- ',
      Res.BusyStat.Deviation : 6 : 3);
  WriteLn('Max utilization = ', Res.BusyStat.Max : 3 : 0, ', current = ',
      Res.Busy);
  WriteLn('Current available = ', Res.Available, ', average = ',
      Res.AvailStat.Mean : 6 : 3);
  WriteLn('Min available = ', Res.AvailStat.Min : 3 : 0, ', max = ',
    Res.AvailStat.Max : 3 : 0);
end;

procedure WriteStat(Header : string; Gate : TGate);
begin
  WriteLn(Header);
  if Gate.State then
    WriteLn('Current state = OPEN, open percentage = ', Gate.Stat.Mean : 6 : 3)
  else
    WriteLn('Current state = CLOSED, open percentage = ',
        Gate.Stat.Mean : 6 : 3);
end;

procedure WriteHist(Header : string; Hist : THistogram);
var
  i, j, pctCount, cumCount : Integer;
  strGraph : string;
begin
  WriteLn(Header);
  cumCount := Round(Hist.CumulativePercent[0] * 40) - 1;
  pctCount := Round(Hist.Percent[0] * 40);
  strGraph := '';
  for j := 1 to cumCount do
    strGraph := strGraph + ' ';
  if cumCount >= 0 then
    strGraph := strGraph + 'O';
  for j := 1 to pctCount do
    strGraph[j] := '*';
  WriteLn(' -INF - ', Hist.UpperBound[0] : 5 : 2, ' : ', Hist.Count[0] : 3,
      ' (', Hist.Percent[0] * 100 : 5 : 2, '%), ',
      Hist.CumulativePercent[0] * 100 : 6 : 2, '% ', strGraph);
  for i := 1 to Hist.IntervalCount do
  begin
    cumCount := Round(Hist.CumulativePercent[i] * 40) - 1;
    pctCount := Round(Hist.Percent[i] * 40);
    strGraph := '';
    for j := 1 to cumCount do
      strGraph := strGraph + ' ';
    if cumCount >= 0 then
      strGraph := strGraph + 'O';
    for j := 1 to pctCount do
      strGraph[j] := '*';
    WriteLn(Hist.LowerBound[i] : 5 : 2, ' - ',
        Hist.UpperBound[i] : 5 : 2, ' : ', Hist.Count[i] : 3,
        ' (', Hist.Percent[i] * 100 : 5 : 2, '%), ',
        Hist.CumulativePercent[i] * 100 : 6 : 2, '% ', strGraph);
  end;
  cumCount := Round(Hist.CumulativePercent[Hist.IntervalCount + 1] * 40) - 1;
  pctCount := Round(Hist.Percent[Hist.IntervalCount + 1] * 40);
  strGraph := '';
  for j := 1 to cumCount do
    strGraph := strGraph + ' ';
  if cumCount >= 0 then
    strGraph := strGraph + 'O';
  for j := 1 to pctCount do
    strGraph[j] := '*';
  WriteLn(Hist.LowerBound[Hist.IntervalCount + 1] : 5 : 2,
      ' -  +INF : ', Hist.Count[Hist.IntervalCount + 1] : 3,
      ' (', Hist.Percent[Hist.IntervalCount + 1] * 100 : 5 : 2, '%), ',
      Hist.CumulativePercent[Hist.IntervalCount + 1] * 100 : 6 : 2, '% ',
      strGraph);
end;

{ TResource }

procedure TResource.Add(cnt: Integer; NewTime: Double);
begin
  FCapacity := FCapacity + cnt;
  if FBusy >= FCapacity then
    AvailStat.AddData(0, NewTime)
  else
    AvailStat.AddData(FCapacity - FBusy, NewTime);
end;

procedure TResource.Add(cnt: Integer);
begin
  FCapacity := FCapacity + cnt;
  if FBusy >= FCapacity then
    AvailStat.AddData(0, CurrentSim.SimTime)
  else
    AvailStat.AddData(FCapacity - FBusy, CurrentSim.SimTime);
end;

procedure TResource.Add;
begin
  Inc(FCapacity);
  if FBusy >= FCapacity then
    AvailStat.AddData(0, CurrentSim.SimTime)
  else
    AvailStat.AddData(FCapacity - FBusy, CurrentSim.SimTime);
end;

function TResource.Available: Integer;
begin
  if FBusy >= FCapacity then
    Result := 0
  else
    Result := FCapacity - FBusy;
end;

constructor TResource.Create(InitCap, InitBusy, QueCnt: Integer;
  StartTime: Double);
var
  i : Integer;
begin
  FCapacity := InitCap;
  FBusy := InitBusy;
  FQueueCount := QueCnt;
  SetLength(FQueue, QueCnt);
  for i := 0 to QueCnt - 1 do
    FQueue[i] := TList.Create(StartTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      StartTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, StartTime);
  FPriority := nil;
end;

constructor TResource.Create(InitCap, InitBusy, QueCnt: Integer;
  PriorFunc: TCompareFunc);
var
  i : Integer;
begin
  FCapacity := InitCap;
  FBusy := InitBusy;
  FQueueCount := QueCnt;
  SetLength(FQueue, QueCnt);
  for i := 0 to QueCnt - 1 do
    FQueue[i] := TList.Create(CurrentSim.SimTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      CurrentSim.SimTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, CurrentSim.SimTime);
  FPriority := PriorFunc;
end;

constructor TResource.Create(InitCap: Integer; StartTime: Double);
begin
  FCapacity := InitCap;
  FBusy := 0;
  FQueueCount := 1;
  SetLength(FQueue, 1);
  FQueue[0] := TList.Create(StartTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      StartTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, StartTime);
  FPriority := nil;
end;

constructor TResource.Create(InitCap, InitBusy, QueCnt: Integer;
  StartTime: Double; PriorFunc: TCompareFunc);
var
  i : Integer;
begin
  FCapacity := InitCap;
  FBusy := InitBusy;
  FQueueCount := QueCnt;
  SetLength(FQueue, QueCnt);
  for i := 0 to QueCnt - 1 do
    FQueue[i] := TList.Create(StartTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      StartTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, StartTime);
  FPriority := PriorFunc;
end;

constructor TResource.Create(InitCap, InitBusy, QueCnt: Integer);
var
  i : Integer;
begin
  FCapacity := InitCap;
  FBusy := InitBusy;
  FQueueCount := QueCnt;
  SetLength(FQueue, QueCnt);
  for i := 0 to QueCnt - 1 do
    FQueue[i] := TList.Create(CurrentSim.SimTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      CurrentSim.SimTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, CurrentSim.SimTime);
  FPriority := nil;
end;

constructor TResource.Create(InitCap: Integer; StartTime: Double;
  PriorFunc: TCompareFunc);
begin
  FCapacity := InitCap;
  FBusy := 0;
  FQueueCount := 1;
  SetLength(FQueue, 1);
  FQueue[0] := TList.Create(StartTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      StartTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, StartTime);
  FPriority := PriorFunc;
end;

constructor TResource.Create;
begin
  FCapacity := 1;
  FBusy := 0;
  FQueueCount := 1;
  SetLength(FQueue, 1);
  FQueue[0] := TList.Create(CurrentSim.SimTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      CurrentSim.SimTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, CurrentSim.SimTime);
  FPriority := nil;
end;

procedure TResource.ClearStat(NewTime: Double);
var
  i : Integer;
begin
  FAvailStat.Clear(NewTime);
  FBusyStat.Clear(NewTime);
  for i := 0 to FQueueCount - 1 do
    FQueue[i].ClearStat(NewTime);
end;

procedure TResource.ClearStat;
var
  i : Integer;
begin
  FAvailStat.Clear(CurrentSim.SimTime);
  FBusyStat.Clear(CurrentSim.SimTime);
  for i := 0 to FQueueCount - 1 do
    FQueue[i].ClearStat(CurrentSim.SimTime);
end;

constructor TResource.Create(PriorFunc: TCompareFunc);
begin
  FCapacity := 1;
  FBusy := 0;
  FQueueCount := 1;
  SetLength(FQueue, 1);
  FQueue[0] := TList.Create(CurrentSim.SimTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      CurrentSim.SimTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, CurrentSim.SimTime);
  FPriority := PriorFunc;
end;

constructor TResource.Create(InitCap: Integer);
begin
  FCapacity := InitCap;
  FBusy := 0;
  FQueueCount := 1;
  SetLength(FQueue, 1);
  FQueue[0] := TList.Create(CurrentSim.SimTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      CurrentSim.SimTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, CurrentSim.SimTime);
  FPriority := nil;
end;

constructor TResource.Create(InitCap: Integer;
  PriorFunc: TCompareFunc);
begin
  FCapacity := InitCap;
  FBusy := 0;
  FQueueCount := 1;
  SetLength(FQueue, 1);
  FQueue[0] := TList.Create(CurrentSim.SimTime);
  FAvailStat := TIntervalStatistics.Create(FCapacity - FBusy,
      CurrentSim.SimTime);
  FBusyStat := TIntervalStatistics.Create(FBusy, CurrentSim.SimTime);
  FPriority := PriorFunc;
end;

destructor TResource.Destroy;
var
  i : Integer;
begin
  for i := 0 to FQueueCount - 1 do
    Queue[i].Free;
  AvailStat.Free;
  BusyStat.Free;
  inherited;
end;

function TResource.Get(cnt: Integer; NewTime: Double): Boolean;
begin
  if cnt <= 0 then
  begin
    Result := True;
    Exit;
  end;
  if Available >= cnt then
  begin
    Inc(FBusy, cnt);
    AvailStat.AddData(FCapacity - FBusy, NewTime);
    BusyStat.AddData(FBusy, NewTime);
    Result := True;
  end
  else
    Result := False;
end;

function TResource.Get(cnt: Integer): Boolean;
begin
  if cnt <= 0 then
  begin
    Result := True;
    Exit;
  end;
  if Available >= cnt then
  begin
    Inc(FBusy, cnt);
    AvailStat.AddData(FCapacity - FBusy, CurrentSim.SimTime);
    BusyStat.AddData(FBusy, CurrentSim.SimTime);
    Result := True;
  end
  else
    Result := False;
end;

function TResource.Get: Boolean;
begin
  if Available >= 1 then
  begin
    Inc(FBusy);
    AvailStat.AddData(FCapacity - FBusy, CurrentSim.SimTime);
    BusyStat.AddData(FBusy, CurrentSim.SimTime);
    Result := True;
  end
  else
    Result := False;
end;

function TResource.GetQueue(Index: Integer): TList;
begin
  Result := FQueue[Index];
end;

function TResource.PreemptedProcs(Index: Integer): Integer;
var
  i : Integer;
  Proc : TProcess;
begin
  i := 0;
  Proc := Queue[Index].First as TProcess;
  while (Proc <> nil) and (Proc.TimeLeft > 0) do
  begin
    Inc(i);
    Proc := Proc.Next as TProcess;
  end;
  Result := i;
end;

function TResource.PreemptedProcs: Integer;
var
  i : Integer;
  Proc : TProcess;
begin
  i := 0;
  Proc := Queue[0].First as TProcess;
  while (Proc <> nil) and (Proc.TimeLeft > 0) do
  begin
    Inc(i);
    Proc := Proc.Next as TProcess;
  end;
  Result := i;
end;

procedure TResource.Release(cnt: Integer; NewTime: Double);
begin
  if cnt <= 0 then
    Exit;
  Dec(FBusy, cnt);
  BusyStat.AddData(FBusy, NewTime);
  if FCapacity > FBusy then
    AvailStat.AddData(FCapacity - FBusy, NewTime);
end;

procedure TResource.Release(cnt: Integer);
begin
  if cnt <= 0 then
    Exit;
  Dec(FBusy, cnt);
  BusyStat.AddData(FBusy, CurrentSim.SimTime);
  if FCapacity > FBusy then
    AvailStat.AddData(FCapacity - FBusy, CurrentSim.SimTime);
end;

procedure TResource.Release;
begin
  Dec(FBusy);
  BusyStat.AddData(FBusy, CurrentSim.SimTime);
  if FCapacity > FBusy then
    AvailStat.AddData(FCapacity - FBusy, CurrentSim.SimTime);
end;

procedure TResource.StopStat(NewTime: Double);
var
  i : Integer;
begin
  AvailStat.StopStat(NewTime);
  BusyStat.StopStat(NewTime);
  for i := 0 to FQueueCount - 1 do
    Queue[i].StopStat(NewTime);
end;

procedure TResource.StopStat;
var
  i : Integer;
begin
  AvailStat.StopStat(CurrentSim.SimTime);
  BusyStat.StopStat(CurrentSim.SimTime);
  for i := 0 to FQueueCount - 1 do
    Queue[i].StopStat(CurrentSim.SimTime);
end;

procedure TResource.Sub(cnt: Integer; NewTime: Double);
begin
  Add(-cnt, NewTime);
end;

procedure TResource.Sub(cnt: Integer);
begin
  Add(-cnt);
end;

procedure TResource.Sub;
begin
  Add(-1);
end;

{ TGate }

procedure TGate.ClearStat(NewTime: Double);
begin
  Stat.Clear(NewTime);
  Queue.ClearStat(NewTime);
end;

procedure TGate.ClearStat;
begin
  Stat.Clear(CurrentSim.SimTime);
  Queue.ClearStat(CurrentSim.SimTime);
end;

procedure TGate.Close(NewTime: Double);
begin
  FState := False;
  Stat.AddData(0, NewTime);
end;

procedure TGate.Close;
begin
  Close(CurrentSim.SimTime);
end;

constructor TGate.Create(InitState: Boolean; StartTime: Double);
begin
  FState := InitState;
  FQueue := TList.Create(StartTime);
  if FState then
    FStat := TIntervalStatistics.Create(1, StartTime)
  else
    FStat := TIntervalStatistics.Create(0, StartTime);
end;

constructor TGate.Create(InitState: Boolean);
var
  StartTime : Double;
begin
  FState := InitState;
  StartTime := CurrentSim.SimTime;
  FQueue := TList.Create(StartTime);
  if FState then
    FStat := TIntervalStatistics.Create(1, StartTime)
  else
    FStat := TIntervalStatistics.Create(0, StartTime);
end;

constructor TGate.Create;
var
  StartTime : Double;
begin
  FState := False;
  StartTime := CurrentSim.SimTime;
  FQueue := TList.Create(StartTime);
  FStat := TIntervalStatistics.Create(0, StartTime);
end;

destructor TGate.Destroy;
begin
  Queue.Free;
  Stat.Free;
  inherited;
end;

procedure TGate.Open(NewTime: Double);
begin
  FState := True;
  Stat.AddData(1, NewTime);
end;

procedure TGate.Open;
begin
  Open(CurrentSim.SimTime);
end;

procedure TProcess.WaitGate(Gate: TGate);
begin
  Insert(Gate.Queue);
  while not Gate.State do
    Passivate;
  StartRunning;
end;

procedure TGate.StopStat(NewTime: Double);
begin
  Stat.StopStat(NewTime);
  Queue.StopStat(NewTime);
end;

procedure TGate.StopStat;
begin
  Stat.StopStat(CurrentSim.SimTime);
  Queue.StopStat(CurrentSim.SimTime);
end;

{ TVisualizator }

constructor TVisualizator.Create(dt: Double);
begin
  DeltaT := dt;
  inherited Create;
end;

procedure TVisualizator.RunProcess;
begin
  while True do
  begin
    Hold(DeltaT);
    SwitchTo(nil);
  end;
end;

procedure TSimulation.MakeVisualizator(dt: Double);
begin
  if Visualizator = nil then
    Visualizator := TVisualizator.Create(dt)
  else
    Visualizator.DeltaT := dt;
end;

procedure RunSimulation(sim : TSimulation);
begin
  SwitchTo(sim.Visualizator);
end;

function Chars(Count : Integer; Ch : Char) : string;
var
  s : string;
  i : Integer;
begin
  s := '';
  for i := 1 to Count do
    s := s + ch;
  Result := s;
end;

function Min(a, b : Double) : Double;
begin
  if a < b then
    Result := a
  else
    Result := b;
end;

function Max(a, b : Double) : Double;
begin
  if a > b then
    Result := a
  else
    Result := b;
end;

function Min(a, b : Integer) : Integer;
begin
  if a < b then
    Result := a
  else
    Result := b;
end;

function Max(a, b : Integer) : Integer;
begin
  if a > b then
    Result := a
  else
    Result := b;
end;

{ ESimulationException }

constructor ESimulationException.Create(AMsg: string);
begin
  Msg := AMsg;
end;

{ TArrayHistogram }

procedure TArrayHistogram.AddData(d: Double);
begin
  Inc(Data[IntervalIndex(d)]);
  Inc(FTotalCount);
end;

procedure TArrayHistogram.Clear;
var
  i : Integer;
begin
  for i := 0 to FIntervalCount + 1 do
    Data[i] := 0;
end;

constructor TArrayHistogram.Create(ABounds: array of Double);
var
  i : Integer;
begin
  FIntervalCount := Length(ABounds) - 1;
  // ????????? ??????????????? ??????? ??????
  for i := 0 to FIntervalCount - 1 do
    if ABounds[i] > ABounds[i + 1] then
      raise ESimulationException.Create('Array histogram bounds not ordered');
  SetLength(Bounds, Length(ABounds));
  for i := 0 to FIntervalCount do
    Bounds[i] := ABounds[i];
  SetLength(Data, FIntervalCount + 2);
  for i := 0 to FIntervalCount + 1 do
    Data[i] := 0;
end;

function TArrayHistogram.GetCount(i: Integer): Integer;
begin
  if i <= 0 then
    Result := Data[0]
  else if i > FIntervalCount then
    Result := Data[FIntervalCount + 1]
  else
    Result := Data[i];
end;

function TArrayHistogram.GetCumulativeCount(i: Integer): Integer;
var
  j, Sum : Integer;
begin
  Sum := 0;
  for j := 0 to i do
    if j <= FIntervalCount + 1 then
      Sum := Sum + Data[j];
  Result := Sum;
end;

function TArrayHistogram.GetCumulativePercent(i: Integer): Double;
begin
  Result := GetCumulativeCount(i) / FTotalCount;
end;

function TArrayHistogram.GetIntervalCount: Integer;
begin
  Result := FIntervalCount;
end;

function TArrayHistogram.GetLowerBound(i: Integer): Double;
begin
  if i <= 0 then
    Result := -1e300
  else if i >= FIntervalCount + 1 then
    Result := Bounds[FIntervalCount]
  else
    Result := Bounds[i - 1];
end;

function TArrayHistogram.GetPercent(i: Integer): Double;
begin
  Result := GetCount(i) / FTotalCount;
end;

function TArrayHistogram.GetUpperBound(i: Integer): Double;
begin
  if i <= 0 then
    Result := Bounds[0]
  else if i >= FIntervalCount + 1 then
    Result := 1e300
  else
    Result := Bounds[i];
end;

function TArrayHistogram.IntervalIndex(Value: Double): Integer;
var
  L, R, M : Integer;
begin
  L := 0;
  R := FIntervalCount;
  while L < R do
  begin
    M := (L + R) div 2;
    if Value > Bounds[M] then
      L := M + 1
    else
      R := M;
  end;
  if Value = Bounds[L] then
    Result := L + 1
  else
    Result := L;
end;

// ???????? ????? ?????????? ??????????,
//   ??? ???????? ???????? ??? ??????????
procedure TArrayHistogram.SetIntervalCount(NewCount: Integer);
begin
  raise ESimulationException.Create(
      'Cannot change array histogram interval count');
end;

// ???????? ??????? ?????????? ??????????,
//   ??? ???????? ???????? ??? ??????????
procedure TArrayHistogram.SetLowerBound(i: Integer; Value: Double);
begin
  raise ESimulationException.Create(
      'Cannot change array histogram bounds');
end;

procedure TArrayHistogram.SetUpperBound(i: Integer; Value: Double);
begin
  raise ESimulationException.Create(
      'Cannot change array histogram bounds');
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TStatistics);
var
  i : Integer;
begin
  Grid.RowCount := Length(Stat) + 1;
  Grid.ColCount := 6;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '??????????';
  Grid.Cells[1, 0] := '???????';
  Grid.Cells[2, 0] := '??????????';
  Grid.Cells[3, 0] := '???????';
  Grid.Cells[4, 0] := '????????';
  Grid.Cells[5, 0] := '??????????';
  for i := 0 to Length(Stat) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    Grid.Cells[1, i + 1] := Format('%5.4f', [Stat[i].Mean]);
    Grid.Cells[2, i + 1] := Format('%5.4f', [Stat[i].Deviation]);
    Grid.Cells[3, i + 1] := Format('%5.4f', [Stat[i].Min]);
    Grid.Cells[4, i + 1] := Format('%5.4f', [Stat[i].Max]);
    Grid.Cells[5, i + 1] := Format('%d', [Stat[i].Count]);
  end;
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TIntervalStatistics);
var
  i : Integer;
begin
  Grid.RowCount := Length(Stat) + 1;
  Grid.ColCount := 7;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '??????????';
  Grid.Cells[1, 0] := '???????';
  Grid.Cells[2, 0] := '??????????';
  Grid.Cells[3, 0] := '???????';
  Grid.Cells[4, 0] := '????????';
  Grid.Cells[5, 0] := '????????';
  Grid.Cells[6, 0] := '??????';
  for i := 0 to Length(Stat) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    Grid.Cells[1, i + 1] := Format('%.4f', [Stat[i].Mean]);
    Grid.Cells[2, i + 1] := Format('%.4f', [Stat[i].Deviation]);
    Grid.Cells[3, i + 1] := Format('%.4f', [Stat[i].Min]);
    Grid.Cells[4, i + 1] := Format('%.4f', [Stat[i].Max]);
    Grid.Cells[5, i + 1] := Format('%.4f', [Stat[i].TotalTime]);
    Grid.Cells[6, i + 1] := Format('%.4f', [Stat[i].LastX]);
  end;
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TTimeBetStatistics);
var
  i : Integer;
begin
  Grid.RowCount := Length(Stat) + 1;
  Grid.ColCount := 6;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '??????????';
  Grid.Cells[1, 0] := '???????';
  Grid.Cells[2, 0] := '??????????';
  Grid.Cells[3, 0] := '???????';
  Grid.Cells[4, 0] := '????????';
  Grid.Cells[5, 0] := '??????????';
  for i := 0 to Length(Stat) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    Grid.Cells[1, i + 1] := Format('%5.4f', [Stat[i].Mean]);
    Grid.Cells[2, i + 1] := Format('%5.4f', [Stat[i].Deviation]);
    Grid.Cells[3, i + 1] := Format('%5.4f', [Stat[i].Min]);
    Grid.Cells[4, i + 1] := Format('%5.4f', [Stat[i].Max]);
    Grid.Cells[5, i + 1] := Format('%d', [Stat[i].Count]);
  end;
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TActionStatistics);
var
  i : Integer;
begin
  Grid.RowCount := Length(Stat) + 1;
  Grid.ColCount := 6;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '????????';
  Grid.Cells[1, 0] := '???????';
  Grid.Cells[2, 0] := '??????????';
  Grid.Cells[3, 0] := '????????';
  Grid.Cells[4, 0] := '??????';
  Grid.Cells[5, 0] := '?????????';
  for i := 0 to Length(Stat) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    Grid.Cells[1, i + 1] := Format('%5.4f', [Stat[i].Mean]);
    Grid.Cells[2, i + 1] := Format('%5.4f', [Stat[i].Deviation]);
    Grid.Cells[3, i + 1] := Format('%d', [Stat[i].Max]);
    Grid.Cells[4, i + 1] := Format('%d', [Stat[i].LastX]);
    Grid.Cells[5, i + 1] := Format('%d', [Stat[i].Finished]);
  end;
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Stat : array of TServiceStatistics);
var
  i : Integer;
begin
  Grid.RowCount := Length(Stat) + 1;
  Grid.ColCount := 9;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '????????';
  Grid.Cells[1, 0] := '??????????';
  Grid.Cells[2, 0] := '???????';
  Grid.Cells[3, 0] := '??????????';
  Grid.Cells[4, 0] := '??????';
  Grid.Cells[5, 0] := '??. ????./???????';
  Grid.Cells[6, 0] := '????. ???????';
  Grid.Cells[7, 0] := '????. ??????';
  Grid.Cells[8, 0] := '?????????';
  for i := 0 to Length(Stat) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    Grid.Cells[1, i + 1] := Format('%d', [Stat[i].Devices]);
    Grid.Cells[2, i + 1] := Format('%5.4f', [Stat[i].Mean]);
    Grid.Cells[3, i + 1] := Format('%5.4f', [Stat[i].Deviation]);
    Grid.Cells[4, i + 1] := Format('%d', [Stat[i].LastUtil]);
    Grid.Cells[5, i + 1] := Format('%5.4f', [Stat[i].MeanBlockage]);
    if Stat[i].Devices > 1 then
      Grid.Cells[6, i + 1] := Format('%d', [Stat[i].Devices - Stat[i].MinBusy])
    else
      Grid.Cells[6, i + 1] := Format('%5.4f', [Stat[i].MaxIdleTime]);
    if Stat[i].Devices > 1 then
      Grid.Cells[7, i + 1] := Format('%d', [Stat[i].MaxBusy])
    else
      Grid.Cells[7, i + 1] := Format('%5.4f', [Stat[i].MaxBusyTime]);
    Grid.Cells[8, i + 1] := Format('%d', [Stat[i].Finished]);
  end;
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Queue : array of TList);
var
  i : Integer;
begin
  Grid.RowCount := Length(Queue) + 1;
  Grid.ColCount := 6;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '???????';
  Grid.Cells[1, 0] := '???????';
  Grid.Cells[2, 0] := '??????????';
  Grid.Cells[3, 0] := '????????';
  Grid.Cells[4, 0] := '??????';
  Grid.Cells[5, 0] := '??. ?????';
  for i := 0 to Length(Queue) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    Grid.Cells[1, i + 1] := Format('%5.4f', [Queue[i].LengthStat.Mean]);
    Grid.Cells[2, i + 1] := Format('%5.4f', [Queue[i].LengthStat.Deviation]);
    Grid.Cells[3, i + 1] := Format('%2.0f', [Queue[i].LengthStat.Max]);
    Grid.Cells[4, i + 1] := Format('%2.0f', [Queue[i].LengthStat.LastX]);
    Grid.Cells[5, i + 1] := Format('%5.4f', [Queue[i].WaitStat.Mean]);
  end;
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Res : array of TResource);
var
  i : Integer;
begin
  Grid.RowCount := Length(Res) + 1;
  Grid.ColCount := 10;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '??????';
  Grid.Cells[1, 0] := '????????';
  Grid.Cells[2, 0] := '??. ??????.';
  Grid.Cells[3, 0] := '??????????';
  Grid.Cells[4, 0] := '????. ??????.';
  Grid.Cells[5, 0] := '?????? ??????.';
  Grid.Cells[6, 0] := '?????? ????.';
  Grid.Cells[7, 0] := '??. ????.';
  Grid.Cells[8, 0] := '???. ????.';
  Grid.Cells[9, 0] := '????. ????.';
  for i := 0 to Length(Res) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    Grid.Cells[1, i + 1] := Format('%d', [Res[i].Capacity]);
    Grid.Cells[2, i + 1] := Format('%5.4f', [Res[i].BusyStat.Mean]);
    Grid.Cells[3, i + 1] := Format('%5.4f', [Res[i].BusyStat.Deviation]);
    Grid.Cells[4, i + 1] := Format('%2.0f', [Res[i].BusyStat.Max]);
    Grid.Cells[5, i + 1] := Format('%5.4f', [Res[i].BusyStat.LastX]);
    Grid.Cells[6, i + 1] := Format('%5.4f', [Res[i].AvailStat.LastX]);
    Grid.Cells[7, i + 1] := Format('%5.4f', [Res[i].AvailStat.Mean]);
    Grid.Cells[8, i + 1] := Format('%2.0f', [Res[i].AvailStat.Min]);
    Grid.Cells[9, i + 1] := Format('%2.0f', [Res[i].AvailStat.Max]);
  end;
end;

procedure ShowStat(Grid : TStringGrid; const Headers : array of string;
    const Gate : array of TGate);
var
  i : Integer;
begin
  Grid.RowCount := Length(Gate) + 1;
  Grid.ColCount := 3;
  Grid.FixedCols := 1;
  Grid.FixedRows := 1;
  Grid.Cells[0, 0] := '??????';
  Grid.Cells[1, 0] := '??????';
  Grid.Cells[2, 0] := '??????';
  for i := 0 to Length(Gate) - 1 do
  begin
    if i < Length(Headers) then
      Grid.Cells[0, i + 1] := Headers[i];
    if Gate[i].State then
      Grid.Cells[1, i + 1] := '??????'
    else
      Grid.Cells[1, i + 1] := '??????';
    Grid.Cells[2, i + 1] := Format('%5.4f', [Gate[i].Stat.Mean]);
  end;
end;

procedure DrawHistCell(Grid : TDrawGrid; ACol, ARow : Integer; Rect : TRect;
    Hist : THistogram);
var
  s : string;
  x : Integer;
  OldColor : TColor;
begin
  if ARow = 0 then
  begin
    s := '';
    case ACol of
    0 :
      s := '??';
    1 :
      s := '??';
    2 :
      s := '????????';
    3 :
      s := '???????';
    4 :
      s := '??????????';
    5 :
      s := '???????????';
    end;
    Grid.Canvas.TextOut((Rect.Left + Rect.Right -
        Grid.Canvas.TextWidth(s)) div 2, Rect.Top + 1, s);
  end
  else
  begin
    s := '';
    case ACol of
    0 :
      if ARow = 1 then
        s := '-INF'
      else
        s := Format('%.2f', [Hist.LowerBound[ARow - 1]]);
    1 :
      if ARow = Hist.IntervalCount + 2 then
        s := '+INF'
      else
        s := Format('%.2f', [Hist.UpperBound[ARow - 1]]);
    2 :
      s := Format('%d', [Hist.Count[ARow - 1]]);
    3 :
      s := Format('%.2f%%', [Hist.Percent[ARow - 1] * 100]);
    4 :
      s := Format('%.2f%%', [Hist.CumulativePercent[ARow - 1] * 100]);
    end;
    if ACol < 5 then
      Grid.Canvas.TextOut(Rect.Right - Grid.Canvas.TextWidth(s) - 2,
          Rect.Top + 1, s)
    else
      with Grid.Canvas do
      begin
        x := Round(Hist.Percent[ARow - 1] * (Rect.Right - Rect.Left));
        OldColor := Brush.Color;
        Brush.Color := clRed;
        Rectangle(Rect.Left, Rect.Top, Rect.Left + x, Rect.Bottom);
        x := Round(Hist.CumulativePercent[ARow - 1] * (Rect.Right - Rect.Left));
        Brush.Color := clBlue;
        Ellipse(Rect.Left + x - 5, (Rect.Top + Rect.Bottom) div 2 - 5,
            Rect.Left + x + 5, (Rect.Top + Rect.Bottom) div 2 + 5);
        Brush.Color := OldColor;
        OldColor := Pen.Color;
        Pen.Width := 3;
        Pen.Color := clBlue;
        if (ARow > 1) and (ARow >= Grid.TopRow) then
        begin
          if ARow > Grid.TopRow then
          begin
            x := Round(Hist.CumulativePercent[ARow - 2] *
                (Rect.Right - Rect.Left));
            MoveTo(Rect.Left + x, (Rect.Top * 3 - Rect.Bottom) div 2);
          end
          else
          begin
            x := Round((Hist.CumulativePercent[ARow - 2] +
                Hist.CumulativePercent[ARow - 1]) / 2 *
                (Rect.Right - Rect.Left));
            MoveTo(Rect.Left + x, Rect.Top);
          end;
          x := Round(Hist.CumulativePercent[ARow - 1] *
              (Rect.Right - Rect.Left));
          LineTo(Rect.Left + x, (Rect.Top + Rect.Bottom) div 2);
        end;
        Pen.Width := 1;
        Pen.Color := clDkGray;
        MoveTo((Rect.Left * 3 + Rect.Right) div 4, Rect.Top);
        LineTo((Rect.Left * 3 + Rect.Right) div 4, Rect.Bottom);
        MoveTo((Rect.Left + Rect.Right) div 2, Rect.Top);
        LineTo((Rect.Left + Rect.Right) div 2, Rect.Bottom);
        MoveTo((Rect.Left + Rect.Right * 3) div 4, Rect.Top);
        LineTo((Rect.Left + Rect.Right * 3) div 4, Rect.Bottom);
        Pen.Color := OldColor;
      end;
  end;
end;

end.

