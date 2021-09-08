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

  TOrder = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  TSavings = class(TSimulation)
  public
    // Ресурс по имеющимся в наличии приемникам
    Radios : TResource;
    // Статистика по интервалам между отказами от покупки
    LostSalesStat : TTimeBetStatistics;
    // Статистика по запасам на момент исполнения заказа
    SafetyStockStat : TStatistics;
    // Статистика по наличию приемников на складе
    InvPosStat : TIntervalStatistics;
    // Генератор покупателей
    CustGen : TCustomerGenerator;
    // Процесс проверки состояния склада
    Check : TCheck;
    // Учетное количество приемников
    CurrentAvailable : Integer;
    // Количество сделанных покупок
    BuysCount : Integer;
    // Статистика по покупателям, сделавшим предварительный заказ
    WaitStat : TStatistics;
    // Процесс выполнения заказа поставщику
    Order : TOrder;
    destructor Destroy; override;
    procedure StopStat; override;
    procedure ClearStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  rndCustomer : TRandom;
  rndOrder : TRandom;
  // Нижняя граница заказа
  OrderLimit : Integer = 18;
  // Верхняя граница заказа
  InvPosition : Integer = 72;
  // Средний интервал прибытия покупателей
  MeanCustTime : Double = 0.2;
  // Вероятность ожидания приотсутствии товара
  OrderProb : Double = 0.2;
  // Интервал проверки
  CheckInterval : Double = 4;
  // Время исполнения заказа
  MeanOrderTime : Double = 3;
  DeviationOrderTime : Double = 1;
  MinOrderTime : Double = 2;
  // Время моделирования до очистки статистики = 1 год
  PreSimulationTime : Double = 52;
  // Время основного моделирования = 5 лет
  MainSimulationTime : Double = 260;
  VisTimeStep : Double = 0.1;

implementation

{ TCustomerGenerator }

procedure TCustomerGenerator.RunProcess;
begin
  while True do
  begin
    // Удалить завершенные процессы (если есть)
    ClearFinished;
    // Создать покупателя
    TCustomer.Create.ActivateDelay(0);
    // Покупатели прибывают с экспоненциально распределенным интервалом
    Hold(rndCustomer.Exponential(MeanCustTime));
  end;
end;

{ TCustomer }

procedure TCustomer.RunProcess;
var
  par : TSavings;
begin
  par := Parent as TSavings;
  // Покупатель приобретает приемник, если:
  //   1. Приемники имеются в наличии
  //   2. Приемников нет, тогда он с вероятностью OrderProb
  //      встает в очередь ожидания
  if (par.Radios.Available > 0) or rndCustomer.Draw(OrderProb) then
  begin
    // Уменьшить учетное количество приемников
    //   (отрицательное число означает наличие заказов, ожидающих выполнения)
    Dec(par.CurrentAvailable);
    par.InvPosStat.AddData(par.CurrentAvailable, SimTime);
    // Приобрести приемник или ждать, пока они не появятся в наличии
    GetResource(par.Radios);
    // Покупка выполнена
    Inc(par.BuysCount);
    // Если покупатель ожидал заказа, учесть его
    if SimTime > StartingTime then
      par.WaitStat.AddData(SimTime - StartingTime);
  end
  else
    // Зафиксировать статистику по отказам
    par.LostSalesStat.AddData(SimTime);
  // Встать в очередь завершенных процессов
  Finish;
end;

{ TCheck }

procedure TCheck.RunProcess;
var
  par : TSavings;
begin
  par := Parent as TSavings;
  while True do
  begin
    // Если учетное количество приемников меньше предельно допустимого 
    if par.CurrentAvailable < OrderLimit then
      // Запустить процесс выполнения заказа поставщику
      par.Order.ActivateDelay(0);
    Hold(CheckInterval);
  end;
end;

{ TOrder }

procedure TOrder.RunProcess;
var
  par : TSavings;
  OrderCount : Integer;
begin
  par := Parent as TSavings;
  while True do
  begin
    // Вычислить объем заказа поставщику
    OrderCount := InvPosition - par.CurrentAvailable;
    // Восстановить учетное количество
    par.CurrentAvailable := InvPosition;
    par.InvPosStat.AddData(par.CurrentAvailable, SimTime);
    // ДОждаться выполнения заказа
    Hold(Max(MinOrderTime, rndOrder.Normal(MeanOrderTime, DeviationOrderTime)));
    // Зафиксировать статистику по резервному запасу
    par.SafetyStockStat.AddData(par.Radios.Available);
    // Восстановить количество ресурса
    ReleaseResource(par.Radios, OrderCount);
    // Ожидать следующего заказа
    Passivate;
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
  WaitStat.Clear;
  BuysCount := 0;
end;

destructor TSavings.Destroy;
begin
  Check.Free;
  CustGen.Free;
  Order.Free;
  InvPosStat.Free;
  SafetyStockStat.Free;
  LostSalesStat.Free;
  WaitStat.Free;
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
  Order := TOrder.Create;
  WaitStat := TStatistics.Create;
  BuysCount := 0;
  MakeVisualizator(VisTimeStep);
end;

procedure TSavings.RunSimulation;
begin
  CustGen.ActivateDelay(0);
  Check.ActivateDelay(0);
  // Предварительная имитация
  Hold(PreSimulationTime);
  // Очистка статистики
  ClearStat;
  // Продолжение имитации
  Hold(MainSimulationTime);
  // Коррекция статистики
  StopStat;
end;

procedure TSavings.StopStat;
begin
  inherited;
  InvPosStat.StopStat(SimTime);
  Radios.StopStat(SimTime);
end;

end.
