unit UBank;

interface
uses USimulation;

type
  // Класс TClient представляет клиента, обслуживаемого в банке
  TClient = class(TLink)
  public
    StartingTime : Double;
    constructor Create(Start : Double);
  end;

  // Класс TArrivalEvent - событийный процесс
  //   для события прибытия клиентов в банк
  TArrivalEvent = class(TEventHandler)
  public
    procedure DefaultEventProc; override;
  end;

  // Класс TFinishedEvent - событийный процесс
  //   для события заершения обслуживания
  TFinishedEvent = class(TEventHandler)
  public
    // Номер кассира, завершившего обслуживание
    Index : Integer;
    constructor Create(Idx : Integer);
    procedure DefaultEventProc; override;
  end;

  // Класс TBank - модель банка для автомобилистов
  TBank = class(TSimulation)
  public
    // Очереди к кассирам
    Queue : array of TList;
    // Обслуживаемые клиенты
    Current : array of TClient;
    // Статистика по загруженности кассиров
    CashmenStat : array of TServiceStatistics;
    // Статистика по числу клиентов в банке
    ClientStat : TActionStatistics;
    // Статистика по интервалам времени между отъездами клиентов
    DepartStat : TTimeBetStatistics;
    // Статистика по времени пребывания клиентов в банке
    TimeStat : TStatistics;
    // Статистика по интервалам времени между отказами
    BalksStat : TTimeBetStatistics;
    // Количество переходов между очередями
    JerkCount : Integer;
    // Количество клиентов, прибывших в банк
    IncomeCount : Integer;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndClient : TRandom;
  // Максимальная длина очереди
  MaxQueueSize : Integer = 3;
  // Средний интервал прибытия клиентов
  MeanArrivalInterval : Double = 0.5;
  // Параметры времени обслуживания
  MeanClientTime : Double = 1;
  DeviationClientTime : Double = 0.3;
  // Минимальная разница в длинах очередей, чтобы был возможен переход
  MinQueueDiff : Integer = 2;
  // Количество касс
  CashCount : Integer = 2;
  // Исходное количество клиентов в каждой очереди
  InitClientCount : Integer = 2;
  // Время прибытия первого клиента
  FirstClientArrivalTime : Double = 0.1;
  // Время моделирования
  SimulationTime : Double = 1000;

implementation

{ TClient }

constructor TClient.Create(Start : Double);
begin
  StartingTime := Start;
end;

{ TArrivalEvent }

procedure TArrivalEvent.DefaultEventProc;
var
  par : TBank;
  Client : TClient;
  Index : Integer;
begin
  par := Parent as TBank;
  ClearFinished;
  // Создать нового клиента
  Client := TClient.Create(SimTime);
  Inc(par.IncomeCount);
  // Если обе очереди заполнены до предела
  if (par.Queue[0].Size = MaxQueueSize) and
      (par.Queue[1].Size = MaxQueueSize) then
  begin
    // Удалить клиента из системы
    par.BalksStat.AddData(SimTime);
    Client.Free;
  end
  else
  begin
    // Увеличить число клиентов в системе
    par.ClientStat.Start(SimTime);
    // Выбрать более короткую очередь
    if par.Queue[0].Size <= par.Queue[1].Size then
      Index := 0
    else
      Index := 1;
    // Поместить клиента в нее
    Client.Insert(par.Queue[Index]);
    // Если выбранный кассир свободен
    if par.Current[Index] = nil then
    begin
      // Извлечь клиента из очереди и поставить его на обслуживание
      Client.Remove;
      par.Current[Index] := Client;
      // Начать обслуживание
      par.CashmenStat[Index].Start(SimTime);
      // Запланировать событие окончания обслуживания
      TFinishedEvent.Create(Index).
          ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime));
    end;
  end;
  // Запланировать прибытие следующего клиента
  TArrivalEvent.Create.
      ActivateDelay(rndClient.Exponential(MeanArrivalInterval));
  Finish;
end;

{ TFinishedEvent }

constructor TFinishedEvent.Create(Idx: Integer);
begin
  Index := Idx;
  inherited Create;
end;

procedure TFinishedEvent.DefaultEventProc;
var
  par : TBank;
  Client : TClient;
begin
  par := Parent as TBank;
  // Собрать статистику по завершенному обслуживанию
  Client := par.Current[Index];
  par.TimeStat.AddData(SimTime - Client.StartingTime);
  par.DepartStat.AddData(SimTime);
  par.CashmenStat[Index].Finish(SimTime);
  // Удалить клиента из системы
  par.ClientStat.Finish(SimTime);
  Client.Free;
  // Если в текущей очереди есть клиенты
  if par.Queue[index].Size > 0 then
  begin
    // Извлечь первого и поставить на обслуживание
    Client := par.Queue[Index].First as TClient;
    par.Current[Index] := Client;
    Client.Remove;
    // Начать обслуживание
    par.CashmenStat[Index].Start(SimTime);
    // Запланировать событие окончания обслуживания
    TFinishedEvent.Create(Index).
        ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime));
    // Если в другой очереди клиентов больше, чем в текущей, как минимум на 2
    if par.Queue[1 - Index].Size >= par.Queue[Index].Size + MinQueueDiff then
    begin
      // Перевести клиента из другой очереди в текущую
      Client := par.Queue[1 - Index].Last as TClient;
      Client.Insert(par.Queue[Index]);
      Inc(par.JerkCount);
    end;
  end
  else
    // Если клиентов нет, кассир свободен
    par.Current[Index] := nil;
  Finish;
end;

{ TBank }

destructor TBank.Destroy;
var
  i : Integer;
begin
  for i := 0 to CashCount - 1 do
    Current[i].Free;
  SetLength(Current, 0);
  BalksStat.Free;
  for i := 0 to CashCount - 1 do
    CashmenStat[i].Free;
  SetLength(CashmenStat, 0);
  ClientStat.Free;
  DepartStat.Free;
  TimeStat.Free;
  for i := 0 to CashCount - 1 do
    Queue[i].Free;
  SetLength(Queue, 0);
  inherited;
end;

procedure TBank.Init;
var
  i : Integer;
begin
  inherited;
  BalksStat := TTimeBetStatistics.Create;
  SetLength(CashmenStat, CashCount);
  for i := 0 to CashCount - 1 do
    CashmenStat[i] := TServiceStatistics.Create;
  ClientStat := TActionStatistics.Create;
  SetLength(Current, CashCount);
  for i := 0 to CashCount - 1 do
    Current[i] := nil;
  DepartStat := TTimeBetStatistics.Create;
  JerkCount := 0;
  IncomeCount := 0;
  SetLength(Queue, CashCount);
  for i := 0 to CashCount - 1 do
    Queue[i] := TList.Create;
  TimeStat := TStatistics.Create;
end;

procedure TBank.RunSimulation;
var
  i, j : Integer;
begin
  // Создание исходной конфигурации системы
  for i := 0 to CashCount - 1 do
  begin
    // Создать клиента и поставить его на обслуживание
    Current[i] := TClient.Create(0);
    Inc(IncomeCount);
    // Запланировать событие окончания обслуживания
    TFinishedEvent.Create(i).
        ActivateDelay(rndClient.Normal(MeanClientTime, DeviationClientTime));
    CashmenStat[i].Start(0);
    ClientStat.Start(0);
    for j := 0 to InitClientCount - 1 do
    begin
      // Создать клиента и поставить его в очередь
      TClient.Create(0).Insert(Queue[i]);
      Inc(IncomeCount);
      ClientStat.Start(0);
    end;
  end;
  // Запланировать прибытие очередного клиента
  TArrivalEvent.Create.ActivateDelay(FirstClientArrivalTime);
  // ОЖидать окончания имитации
  Hold(SimulationTime);
  StopStat;
end;

procedure TBank.StopStat;
var
  i : Integer;
begin
  inherited;
  for i := 0 to CashCount - 1 do
    CashmenStat[i].StopStat(SimTime);
  ClientStat.StopStat(SimTime);
  for i := 0 to CashCount - 1 do
    Queue[i].StopStat(SimTime);
end;

end.
