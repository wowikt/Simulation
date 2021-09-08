unit UTools;

interface
uses USimulation;

type
  // Перечисление TMachineState обозначает состояния станка
  TMachineState = (msFree, msPreparing, msWorking, msRepaired);

  // Класс TDetail имитирует процесс прохождения детали через станок
  TDetail = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TDetailGenerator - генератор деталей
  TDetailGenerator = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TFaults управляет поломками станка
  TFaults = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TTools имитирует работу станка с поломками
  TTools = class(TSimulation)
  public
    // Генератор заданий
    Generator : TDetailGenerator;
    // Управление поломками
    Faults : TFaults;
    // Ресурс станка
    Tool : TResource;
    // Состояние станка
    State : TMachineState;
    // Статистика по времени пребывания детали в системе
    TimeStat : TStatistics;
    // Статистика по действию наладки
    PrepStat : TActionStatistics;
    PrepTimeStat : TStatistics;
    // Статистика по основному действию
    OperStat : TActionStatistics;
    OperTimeStat : TStatistics;
    // Статистика по ремонту станка
    RepairStat : TActionStatistics;
    RepairTimeStat : TStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure Init; override;
    procedure RunSimulation; override;
  end;

var
  // Генераторы случайных чисел
  rndDetail,
  rndFaults : TRandom;
  // Количество очередей ресурса станка
  ResQueueCount : Integer = 2;
  // Индексы очередей ресурса
  FaultQueueIndex : Integer = 0;
  DetailQueueIndex : Integer = 1;
  // Границы времени подготовительного действия
  MinPrepTime : Double = 0.2;
  MaxPrepTime : Double = 0.5;
  // Параметры времени основного действия
  MeanMainTime : Double = 0.5;
  DeviationMainTime : Double = 0.1;
  // Средний интервал между поступлением деталей
  MeanDetailInterval : Double = 1;
  // Параметры времени поломок
  MeanFaultInterval : Double = 20;
  DeviationFaultInterval : Double = 2;
  // Параметры времени ремонта
  MeanRepairIntervalTime : Double = 0.75;
  RepairIntervalCount : Integer = 3;
  // Время имитации
  SimulationTime : Double = 500;

implementation

{ TDetail }

procedure TDetail.RunProcess;
var
  par : TTools;
  ActionStartTime : Double;
begin
  par := Parent as TTools;
  // Получить доступ к станку
  GetResource(par.Tool, 1, DetailQueueIndex);
  // Состояние станка - наладка
  par.State := msPreparing;
  // Выполнить наладку
  par.PrepStat.Start(SimTime);
  ActionStartTime := SimTime;
  Hold(rndDetail.Uniform(MinPrepTime, MaxPrepTime));
  par.PrepStat.Finish(SimTime);
  par.PrepTimeStat.AddData(SimTime - ActionStartTime);
  // Состояние станка - основное действие
  par.State := msWorking;
  // Выполнить основное действие
  par.OperStat.Start(SimTime);
  ActionStartTime := SimTime;
  Hold(rndDetail.Normal(MeanMainTime, DeviationMainTime));
  par.OperStat.Finish(SimTime);
  par.OperTimeStat.AddData(SimTime - ActionStartTime);
  // Состояние станка - свободен
  par.State := msFree;
  // Освободить станок
  ReleaseResource(par.Tool);
  // Собрать статистику по времени пребывания в системе
  par.TimeStat.AddData(SimTime - StartingTime);
  Finish;
end;

{ TDetailGenerator }

procedure TDetailGenerator.RunProcess;
begin
  while True do
  begin
    ClearFinished;
    // Создать задание и запустить процесс
    TDetail.Create.ActivateDelay(0);
    // Подождать перед созданием следующего
    Hold(rndDetail.Exponential(MeanDetailInterval));
  end;
end;

{ TFaults }

procedure TFaults.RunProcess;
var
  par : TTools;
  RepairStartTime : Double;
  LastState : TMachineState;
begin
  par := Parent as TTools;
  while True do
  begin
    // Ожидать следующей поломки
    Hold(rndFaults.Normal(MeanFaultInterval, DeviationFaultInterval));
    // Прервать текущее действие
    PreemptResource(par.Tool, FaultQueueIndex);
    // Приостановить сбор статистики действия
    case par.State of
    msPreparing :
      par.PrepStat.Preempt(SimTime);
    msWorking :
      par.OperStat.Preempt(SimTime);
    end;
    // Запомнить предыдущее состояние станка и задать состояние ремонта
    LastState := par.State;
    par.State := msRepaired;
    // Начать ремонт
    RepairStartTime := SimTime;
    par.RepairStat.Start(SimTime);
    Hold(rndFaults.Erlang(MeanRepairIntervalTime, RepairIntervalCount));
    par.RepairStat.Finish(SimTime);
    par.RepairTimeStat.AddData(SimTime - RepairStartTime);
    // Восстановить предыдущее состояние
    par.State := LastState;
    // Возобновить сбор статистики
    case par.State of
    msPreparing :
      par.PrepStat.Resume(SimTime);
    msWorking :
      par.OperStat.Resume(SimTime);
    end;
    // Закончить ремонт
    ReleaseResource(par.Tool);
  end;
end;

{ TTools }

destructor TTools.Destroy;
begin
  Faults.Free;
  Generator.Free;
  PrepStat.Free;
  PrepTimeStat.Free;
  OperStat.Free;
  OperTimeStat.Free;
  RepairStat.Free;
  RepairTimeStat.Free;
  TimeStat.Free;
  Tool.Free;
  inherited;
end;

procedure TTools.Init;
begin
  inherited;
  Faults := TFaults.Create;
  Generator := TDetailGenerator.Create;
  TimeStat := TStatistics.Create;
  PrepStat := TActionStatistics.Create;
  PrepTimeStat := TStatistics.Create;
  OperStat := TActionStatistics.Create;
  OperTimeStat := TStatistics.Create;
  RepairStat := TActionStatistics.Create;
  RepairTimeStat := TStatistics.Create;
  Tool := TResource.Create(1, 0, ResQueueCount);
end;

procedure TTools.RunSimulation;
begin
  State := msFree;
  Generator.ActivateDelay(0);
  Faults.ActivateDelay(0);
  // Ожидать окончания имитации
  Hold(SimulationTime);
  StopStat;
end;

procedure TTools.StopStat;
begin
  inherited;
  OperStat.StopStat(SimTime);
  PrepStat.StopStat(SimTime);
  RepairStat.StopStat(SimTime);
  Tool.StopStat(SimTime);
end;

end.
