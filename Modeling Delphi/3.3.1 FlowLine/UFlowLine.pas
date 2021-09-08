unit UFlowLine;

interface
uses USimulation;

type
  // Класс TPiece - изделие
  TPiece = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс Tworker1 - первое рабочее место
  TWorker1 = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TWorker2 - второе рабочее место
  TWorker2 = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TFlowLineSimulation - имитация поточного производства
  TFlowLineSimulation = class(TSimulation)
  public
    // Статистика по отказам
    Balks : TTimeBetStatistics;
    // Статистика по времени нахождения в системе
    TimeInSystem : TStatistics;
    // Гистограмма по времени нахождения в системе
    TimeHist : TUniformHistogram;
    // Первое рабочее место
    Worker1 : TWorker1;
    // Второе рабочее место
    Worker2 : TWorker2;
    // Очередь изделий, ожидающих обслуживания
    //   на первом рабочем месте
    Queue1 : TList;
    // Очередь изделий, ожидающих обслуживания
    //   на втором рабочем месте
    Queue2 : TList;
    // Статистика по первой операции
    Stat1 : TServiceStatistics;
    // Статистика по второй операции
    Stat2 : TServiceStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  rndPiece : TRandom;
  rndWorker1 : TRandom;
  rndWorker2 : TRandom;
  Queue1Size : Integer = 4;
  Queue2Size : Integer = 2;
  PieceMeanInterval : Double = 0.4;
  Worker1MeanTime : Double = 0.25;
  Worker2MeanTime : Double = 0.5;
  HistMin : Double = 0;
  HistStep : Double = 0.5;
  HistStepCount : Integer = 20;
  SimulationTime : Double = 300;

implementation

{ TPiece }

procedure TPiece.RunProcess;
var
  par : TFlowLineSimulation;
begin
  par := Parent as TFlowLineSimulation;
  ClearFinished;
  // Запланировать прибытие следующего изделия
  TPiece.Create.ActivateDelay(rndPiece.Exponential(PieceMeanInterval));
  // Если очередь к первому рабочему месту не заполнена
  if par.Queue1.Size < Queue1Size then
  begin
    // Активировать обслуживание
    par.Worker1.ActivateDelay(0);
    // Встать в очередь ожидания
    Wait(par.Queue1);
  end
  else
  begin
    // Добавить статистику по отказам
    par.Balks.AddData(SimTime);
  end;
  Finish;
end;

{ TWorker1 }

procedure TWorker1.RunProcess;
var
  Piece : TPiece;
  par : TFlowLineSimulation;
begin
  par := Parent as TFlowLineSimulation;
  // Первое рабочее место
  while True do
  begin
    // Ждать появления изделий в очереди
    while par.Queue1.Empty do
      Passivate;
    // Начать операцию
    par.Stat1.Start(SimTime);
    // Извлечь очередное изделие
    Piece := par.Queue1.First as TPiece;
    Piece.StartRunning;
    // Выполнить обслуживание
    Hold(rndWorker1.Exponential(Worker1MeanTime));
    // Закончить операцию
    par.Stat1.Finish(SimTime);
    // Если очередь ко второму рабочему месту заполнена
    if par.Queue2.Size >= Queue2Size then
    begin
      // Заблокировать процесс
      par.Stat1.StartBlock(SimTime);
      // Ожидать наличия места в очереди к второму рабочему месту
      while par.Queue2.Size >= Queue2Size do
        Passivate;
      // Разблокировать
      par.Stat1.FinishBlock(SimTime);
    end;
    // Поместить изделие в очередь
    Piece.Insert(par.Queue2);
    // Активировать второй процесс
    par.Worker2.ActivateDelay(0);
  end;
end;

{ TWorker2 }

procedure TWorker2.RunProcess;
var
  Piece : TPiece;
  par : TFlowLineSimulation;
begin
  par := Parent as TFlowLineSimulation;
  // Второе рабочее место
  while True do
  begin
    // Ожидать появления изделий в очереди
    while par.Queue2.Empty do
      Passivate;
    // Начать операцию
    par.Stat2.Start(SimTime);
    // Извлечь изделие
    Piece := par.Queue2.First as TPiece;
    Piece.StartRunning;
    // Активировать первого работника на случай, если он был заблокирован
    par.Worker1.ActivateDelay(0);
    // Выполнить обслуживание
    Hold(rndWorker2.Exponential(Worker2MeanTime));
    // Зафиксировать статистику по времени пребывания в системе
    par.TimeInSystem.AddData(SimTime - Piece.StartingTime);
    par.TimeHist.AddData(SimTime - Piece.StartingTime);
    // Закончить операцию
    par.Stat2.Finish(SimTime);
    // Активировать изделие для завершения работы
    Piece.ActivateDelay(0);
  end;
end;

{ TFlowLineSimulation }

destructor TFlowLineSimulation.Destroy;
begin
  Worker1.Free;
  Worker2.Free;
  Stat1.Free;
  Stat2.Free;
  Balks.Free;
  TimeInSystem.Free;
  TimeHist.Free;
  Queue1.Free;
  Queue2.Free;
  inherited;
end;

procedure TFlowLineSimulation.Init;
begin
  inherited;
  Balks := TTimeBetStatistics.Create;
  TimeInSystem := TStatistics.Create;
  TimeHist := TUniformHistogram.Create(HistMin, HistStep, HistStepCount);
  Queue1 := TList.Create;
  Queue2 := TList.Create;
  Worker1 := TWorker1.Create;
  Worker2 := TWorker2.Create;
  Stat1 := TServiceStatistics.Create(1);
  Stat2 := TServiceStatistics.Create(1);
end;

procedure TFlowLineSimulation.RunSimulation;
begin
  // Поместить первое изделие в момент 0
  TPiece.Create.ActivateDelay(0);
  // Выполнение моделирования
  Hold(SimulationTime);
  StopStat;
end;

procedure TFlowLineSimulation.StopStat;
begin
  inherited;
  Queue1.StopStat(SimTime);
  Queue2.StopStat(SimTime);
  Stat1.StopStat(SimTime);
  Stat2.StopStat(SimTime);
end;

end.
