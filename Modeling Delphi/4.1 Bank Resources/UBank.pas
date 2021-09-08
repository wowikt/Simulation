unit UBank;

interface
uses USimulation;

type
  // Класс TClient - процесс, моделирующий клиента банка
  TClient = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TClientGenerator - процесс, порождающий клиентов банка
  TClientGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TBankSimulation - моделирование работы банка
  TBankSimulation = class(TSimulation)
  public
    // Ресурс кассира
    Cashman : TResource;
    // Генератор клиентов
    Generator : TClientGenerator;
    // Статистика и гистограмма по времени пребывания
    //   клиентов в банке
    InBankTime : TStatistics;
    InBankHist : THistogram;
    // Счетчик клиентов, обслуженных без ожидания
    NotWaited : Integer;
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
  // Средний интервал между прибытием клиентов
  MeanClientInterval : Double = 5;
  // Границы времени обслуживания
  MinCashTime : Double = 2;
  MaxCashTime : Double = 6;
  // Параметры гистограммы
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

  // Дождаться освобождения кассира
  GetResource(par.Cashman);

  // Приступить к выполнению действия
  // Если клиент не ждал, учесть его
  if StartingTime = SimTime then
    Inc(par.NotWaited);

  // Выполнить обслуживание
  Hold(rndCashman.Uniform(MinCashTime, MaxCashTime));

  // Учесть полное время пребывания в банке
  InTime := SimTime - StartingTime;
  par.InBankTime.AddData(InTime);
  par.InBankHist.AddData(InTime);

  // Если все клиенты обслужены, завершить работу
  if par.InBankTime.Count = MaxClientCount then
    par.ActivateDelay(0);
  ReleaseResource(par.Cashman);

  // Встать в очередь завершенных процессов
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
  // Запустить процесс создания клиентов
  Generator.ActivateDelay(0);
  // Ждать конца имитации
  Passivate;
  StopStat;
end;

procedure TBankSimulation.StopStat;
begin
  inherited;
  Cashman.StopStat(SimTime);
end;

end.
