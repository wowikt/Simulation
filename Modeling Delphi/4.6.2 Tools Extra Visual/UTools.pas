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
    // Ресурс основного и дополнительного станков
    Tool : TResource;
    ExtraTool : TResource;
    // Состояние основного и дополнительного станков
    State : TMachineState;
    ExtraState : TMachineState;
    // Статистика по времени пребывания детали в системе
    TimeStat : TStatistics;
    // Статистика по действию наладки для основного и дополнительного станков
    PrepStat : TActionStatistics;
    PrepTimeStat : TStatistics;
    ExtraPrepStat : TActionStatistics;
    ExtraPrepTimeStat : TStatistics;
    // Статистика по основному действию для основного и дополнительного станков
    OperStat : TActionStatistics;
    OperTimeStat : TStatistics;
    ExtraOperStat : TActionStatistics;
    ExtraOperTimeStat : TStatistics;
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
  VisTimeStep : Double = 0.1;

implementation

{ TDetail }

procedure TDetail.RunProcess;
var
  par : TTools;
  ActionStartTime : Double;
  OperTime : Double;
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
  // Если наладка прервана, перейти на дополнительный станок
  if TimeLeft > 0 then
  begin
    // Работа начинается заново
    TimeLeft := 0;
    // Получить ресурс дополниельного станка
    GetResource(par.ExtraTool);
    // Начать наладку на дополнительном станке
    par.ExtraState := msPreparing;
    par.ExtraPrepStat.Start(SimTime);
    ActionStartTime := SimTime;
    Hold(rndDetail.Uniform(MinPrepTime, MaxPrepTime) * 2);
    // Закончить наладку
    par.ExtraPrepStat.Finish(SimTime);
    par.ExtraPrepTimeStat.AddData(SimTime - ActionStartTime);
    // Начать основное действие
    par.ExtraState := msWorking;
    par.ExtraOperStat.Start(SimTime);
    ActionStartTime := SimTime;
    Hold(rndDetail.Normal(MeanMainTime, DeviationMainTime) * 2);
    // Закончить действие
    par.ExtraOperStat.Finish(SimTime);
    par.ExtraOperTimeStat.AddData(SimTime - ActionStartTime);
    par.ExtraState := msFree;
    // Освободить дополнительный станок
    ReleaseResource(par.ExtraTool);
  end
  else
  begin
    // Закончить наладку на основном станке
    par.PrepStat.Finish(SimTime);
    par.PrepTimeStat.AddData(SimTime - ActionStartTime);
    // Состояние станка - основное действие
    par.State := msWorking;
    // Выполнить основное действие
    par.OperStat.Start(SimTime);
    ActionStartTime := SimTime;
    Hold(rndDetail.Normal(MeanMainTime, DeviationMainTime));
    // Если основное действие было прервано
    if TimeLeft > 0 then
    begin
      // Запомнить оставшееся время выполнения основного действия
      OperTime := TimeLeft;
      TimeLeft := 0;
      // Получить ресурс дополнтельного станка
      GetResource(par.ExtraTool);
      // Начать наладку
      par.ExtraState := msPreparing;
      par.ExtraPrepStat.Start(SimTime);
      ActionStartTime := SimTime;
      Hold(rndDetail.Uniform(MinPrepTime, MaxPrepTime) * 2);
      // Закончить наладку
      par.ExtraPrepStat.Finish(SimTime);
      par.ExtraPrepTimeStat.AddData(SimTime - ActionStartTime);
      // Начать основное действие
      par.ExtraState := msWorking;
      par.ExtraOperStat.Start(SimTime);
      ActionStartTime := SimTime;
      Hold(OperTime * 2);
      // Закончить основное действие
      par.ExtraOperStat.Finish(SimTime);
      par.ExtraOperTimeStat.AddData(SimTime - ActionStartTime);
      par.ExtraState := msFree;
      // Освободить ресурс
      ReleaseResource(par.ExtraTool);
    end
    else
    begin
      // Закончить действие на основном станке
      par.OperStat.Finish(SimTime);
      par.OperTimeStat.AddData(SimTime - ActionStartTime);
      // Состояние станка - свободен
      par.State := msFree;
      // Освободить станок
      ReleaseResource(par.Tool);
    end;
  end;
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
begin
  par := Parent as TTools;
  while True do
  begin
    // Ожидать следующей поломки
    Hold(rndFaults.Normal(MeanFaultInterval, DeviationFaultInterval));
    // Прервать текущее действие
    PreemptResourceNoWait(par.Tool, FaultQueueIndex);
    // Остановить сбор статистики действия
    case par.State of
    msPreparing :
      par.PrepStat.Preempt(SimTime);
    msWorking :
      par.OperStat.Preempt(SimTime);
    end;
    // Задать состояние ремонта
    par.State := msRepaired;
    // Начать ремонт
    RepairStartTime := SimTime;
    par.RepairStat.Start(SimTime);
    Hold(rndFaults.Erlang(MeanRepairIntervalTime, RepairIntervalCount));
    par.RepairStat.Finish(SimTime);
    par.RepairTimeStat.AddData(SimTime - RepairStartTime);
    // Станок свободен
    par.State := msFree;
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
  ExtraPrepStat.Free;
  ExtraPrepTimeStat.Free;
  OperStat.Free;
  OperTimeStat.Free;
  ExtraOperStat.Free;
  ExtraOperTimeStat.Free;
  RepairStat.Free;
  RepairTimeStat.Free;
  TimeStat.Free;
  Tool.Free;
  ExtraTool.Free;
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
  ExtraPrepStat := TActionStatistics.Create;
  ExtraPrepTimeStat := TStatistics.Create;
  OperStat := TActionStatistics.Create;
  OperTimeStat := TStatistics.Create;
  ExtraOperStat := TActionStatistics.Create;
  ExtraOperTimeStat := TStatistics.Create;
  RepairStat := TActionStatistics.Create;
  RepairTimeStat := TStatistics.Create;
  Tool := TResource.Create(1, 0, ResQueueCount);
  ExtraTool := TResource.Create;
  MakeVisualizator(VisTimeStep);
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
  ExtraOperStat.StopStat(SimTime);
  PrepStat.StopStat(SimTime);
  ExtraPrepStat.StopStat(SimTime);
  RepairStat.StopStat(SimTime);
  Tool.StopStat(SimTime);
  ExtraTool.StopStat(SimTime);
end;

end.
