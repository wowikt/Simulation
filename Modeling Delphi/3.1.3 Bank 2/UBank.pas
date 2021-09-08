unit UBank;

interface
uses USimulation;

type
  // Класс TClient - процесс, моделирующий клиента банка
  TClient = class(TProcess)
  public
    Inserted : Boolean;
    constructor Create; overload;
    constructor Create(AInserted : Boolean); overload;
  protected
    procedure RunProcess; override;
  end;

  // Класс TClientGenerator - процесс, порождающий клиентов банка
  TClientGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TCashman - процесс, моделирующий работу кассира
  TCashman = class(TProcess)
  public
    Client : TClient;
    constructor Create(AClient : TClient);
  protected
    procedure RunProcess; override;
  end;

  // Класс TBankSimulation - моделирование работы банка
  TBankSimulation = class(TSimulation)
  public
    Cashmen : array of TProcess;
    Generator : TClientGenerator;
    InBankTime : TStatistics;
    InBankHist : THistogram;
    CashStat : TServiceStatistics;
    Queue : TList;
    NotWaited : Integer;
    NotServiced : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  // Датчики случайных чисел
  rndClient : TRandom;
  rndCashman : TRandom;
  // Количество обслуживаемых клиентов
  MaxClientCount : Integer = 100;
  // Максимальная длина очереди
  MaxQueueLength : Integer = 10;
  // Начальное количество клиентов в очереди
  StartClientNum : Integer = 2;
  // Средний интервал прибытия клиентов
  MeanClientInterval : Double = 5;
  // Момент прибытия первого клиента
  FirstClientArrival : Double = 5;
  // Границы времени обслуживания кассиром
  MinCashTime : Double = 6;
  MaxCashTime : Double = 12;
  // Количество касс
  CashCount : Integer = 2;
  // Ширина интервала гистограммы
  HistInterval : Double = 2;
  // Количество интервалов гистограммы
  HistIntervalCount : Integer = 20;

implementation

{ TClient }

constructor TClient.Create(AInserted: Boolean);
begin
  Inserted := AInserted;
  inherited Create;
end;

constructor TClient.Create;
begin
  Inserted := False;
  inherited Create;
end;

procedure TClient.RunProcess;
var
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;

  // Если клиент был внедрен в систему принудительно
  if Inserted then
  begin
    // Сразу ожидать обслуживания
    Passivate;
  end
  // Встать в очередь
  else if par.Queue.Size < MaxQueueLength then
  begin
    Insert(par.Queue);
    // Активировать кассиров
    ActivateDelay(par.Cashmen, 0);

    // Ждать обслуживания
    Passivate;
  end
  else
    // Очередь заполнена - увеличить счетчик отказов
    Inc(par.NotServiced);
  // Встать в очередь завершенных процессов
  Finish;
end;

{ TClientGenerator }

procedure TClientGenerator.RunProcess;
begin
  // Подождать перед появлением первого клиента
  Hold(FirstClientArrival);
  while True do
  begin
    ClearFinished;
    TClient.Create.ActivateDelay(0);
    Hold(rndClient.Exponential(MeanClientInterval));
  end;
end;

{ TCashman }

constructor TCashman.Create(AClient : TClient);
begin
  Client := AClient;
  inherited Create;
end;

procedure TCashman.RunProcess;
var
  InTime : Double;
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;

  // Передать управление клиенту, чтобы он запомнил
  //   время запуска
  Client.StartRunning;
  Client.ActivateDelay(0);
  Hold(0);
  // Приступить к работе с ним
  while True do
  begin
    // Если клиент не ждал, учесть его
    if Client.StartingTime = SimTime then
      Inc(par.NotWaited);

    // Выполнить обслуживание
    par.CashStat.Start(SimTime);
    Hold(rndCashman.Uniform(MinCashTime, MaxCashTime));
    par.CashStat.Finish(SimTime);

    // Учесть полное время пребывания в банке
    InTime := SimTime - Client.StartingTime;
    par.InBankTime.AddData(InTime);
    par.InBankHist.AddData(InTime);

    // Возобновить процесс клиента, дав ему возможность
    //   закончить работу
    Client.ActivateDelay(0);
    // Если достигнуто предельное число клиентов,
    //   завершить работу
    if par.CashStat.Finished = MaxClientCount then
    begin
      par.ActivateDelay(0);
      Passivate;
    end;

    // Если очередь пуста, ждать прибытия клиента
    while par.Queue.Empty do
      Passivate;

    // Извлечь первого клиента из очереди
    Client := par.Queue.First as TClient;
    Client.StartRunning;
  end;
end;

{ TBankSimulation }

destructor TBankSimulation.Destroy;
var
  i : Integer;
begin
  for i := 0 to CashCount - 1 do
    Cashmen[i].Free;
  Queue.Free;
  InBankTime.Free;
  InBankHist.Free;
  CashStat.Free;
  inherited;
end;

procedure TBankSimulation.Init;
var
  i : Integer;
begin
  inherited;
  Queue := TList.Create;
  SetLength(Cashmen, CashCount);
  for i := 0 to CashCount - 1 do
    Cashmen[i] := TCashman.Create(TClient.Create(True));
  for i := 1 to StartClientNum do
    TClient.Create(True).Insert(Queue);
  Generator := TClientGenerator.Create;
  InBankTime := TStatistics.Create;
  CashStat := TServiceStatistics.Create(CashCount);
  InBankHist :=
      TUniformHistogram.Create(MinCashTime, HistInterval, HistIntervalCount);
  NotWaited := 0;
  NotServiced := 0;
end;

procedure TBankSimulation.RunSimulation;
begin
  // Запустить процесс создания клиентов
  Generator.ActivateDelay(0);
  // Активировать кассиров
  ActivateAllDelay(Cashmen, 0);
  // Активировать клиентов, принудительно поставленных в очередь,
  //  чтобы они запомнили время начала работы
  ActivateAllDelay(Queue, 0);
  // Ждать конца имитации
  Passivate;
  StopStat;
end;

procedure TBankSimulation.StopStat;
begin
  inherited;
  Queue.StopStat(SimTime);
  CashStat.StopStat(SimTime);
end;

end.

