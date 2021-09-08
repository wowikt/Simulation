unit UShop;

interface
uses USimulation;

// Моделирование работы магазина

type
  // Класс TCustomer - покупатель
  TCustomer = class(TProcess)
  public
    // Количество покупок
    BuysCount : Integer;
  protected
    procedure RunProcess; override;
  end;

  // Класс TCashman - кассир
  TCashman = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TGenerator - генератор покупателей
  TGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TShop - имитация работы магазина
  TShop = class(TSimulation)
  public
    Queue : TList;
    Generator : TGenerator;
    Cash : TCashman;
    CashStat : TServiceStatistics;
    TimeStat : TStatistics;
    // Статистика по нахождению покупателей в торговом зале
    PeopleStat : TActionStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  // Генераторы случайных чисел
  rndCust,                 // Прибытие покупателей
  rndService : TRandom;    // Время обслуживания
  // Границы времени выбора покупок
  MinShoppingTime : Double = 2;
  MaxShoppingTime : Double = 12;
  // Граничные значения числа покупок
  MinBuysCount : Integer = 2;
  MaxBuysCount : Integer = 16;
  // Средний интервал прибылтия покупателей
  MeanCustInterval : Double = 2;
  // Среднее время обработки одной покупки
  TimePerBuy : Double = 0.2;
  // Полное время моделирования
  SimulationTime : Double = 480;

implementation

{ TCustomer }

procedure TCustomer.RunProcess;
var
  par : TShop;
begin
  par := Parent as TShop;
  StartRunning;
  // Задать число покупок
  BuysCount := rndService.NextInt(MinBuysCount, MaxBuysCount);
  // Выбор покупок

  par.PeopleStat.Start(SimTime);
  Hold(rndService.Uniform(MinShoppingTime, MaxShoppingTime));
  par.PeopleStat.Finish(SimTime);
  // Активировать кассира
  par.Cash.ActivateDelay(0);
  // Встать в очередь к кассе
  Wait(par.Queue);
  // По окончании обслуживания завершить работу
  Finish;
end;

{ TCashman }

procedure TCashman.RunProcess;
var
  Cust : TCustomer;
  par : TShop;
begin
  par := Parent as TShop;
  while True do
  begin
    while not par.Queue.Empty do
    begin
      // Обратиться к очередному покупателю
      Cust := par.Queue.First as TCustomer;
      // Извлечь из очереди
      Cust.StartRunning;
      // Кассир занят
      par.CashStat.Start(SimTime);
      // Рассчитать покупателя
      Hold(rndService.Erlang(TimePerBuy, Cust.BuysCount));
      // Покупатель обслужен - кассир свободен
      par.CashStat.Finish(SimTime);
      par.TimeStat.AddData(SimTime - Cust.StartingTime);
      // Активировать его завершение
      Cust.ActivateDelay(0);
    end;
    // Ждать очередного покупателя
    Passivate;
  end;
end;

{ TGenerator }

procedure TGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // Создать нового покупателя и поместить его в систему
    TCustomer.Create.ActivateDelay(0);
    // Подождать перед созданием следующего
    Hold(rndCust.Exponential(MeanCustInterval));
  end;
end;

{ TShop }

destructor TShop.Destroy;
begin
  Cash.Free;
  Generator.Free;
  CashStat.Free;
  TimeStat.Free;
  PeopleStat.Free;
  Queue.Free;
  inherited;
end;

procedure TShop.Init;
begin
  inherited;
  CashStat := TServiceStatistics.Create(1);
  PeopleStat := TActionStatistics.Create;
  Queue := TList.Create;
  Cash := TCashman.Create;
  Generator := TGenerator.Create;
  TimeStat := TStatistics.Create;
end;

procedure TShop.RunSimulation;
begin
  // Запустить генератор
  Generator.ActivateDelay(0);
  // Ждать окончания моделирования
  Hold(SimulationTime);
  // Закончить статистику
  StopStat;
end;

procedure TShop.StopStat;
begin
  inherited;
  CashStat.StopStat(SimTime);
  Queue.StopStat(SimTime);
  PeopleStat.StopStat(SimTime);
end;

end.
