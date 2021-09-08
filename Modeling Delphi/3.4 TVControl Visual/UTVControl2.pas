unit UTVControl;

interface
uses USimulation;

// Модель поточной линии по контролю телевизоров
type
  // Класс TTVSet - проверяемый телевизор
  TTVSet = class(TProcess)
  public
    IncomeTime : Double;
  protected
    procedure RunProcess; override;
  end;

  // Класс TInspector - контролер
  TInspector = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TAdjuster  - настройщик
  TAdjuster = class(TProcess)
  protected
    procedure RunProcess; override;
  end;

  // Класс TTVControl - имитация линия контроля
  TTVControl = class(TSimulation)
  public
    // Очередь телевизоров на проверку
    InspectionQueue : TList;
    // Очередь свободных проверяющих
    Inspectors : array [0 .. 1] of TProcess;
    // Настройщик
    Adjuster : TAdjuster;
    // Очередь на настройку
    AdjustmentQueue : TList;
    // Статистика по времени нахождения в системе
    TimeInSystemStat : TStatistics;
    // Статистика по занятости проверяющих
    InspectorsStat : TServiceStatistics;
    // Статистика по занятости настройщика
    AdjustmentStat : TServiceStatistics;
    destructor Destroy; override;
    procedure StopStat; override;
  protected
    procedure RunSimulation; override;
    procedure Init; override;
  end;

var
  rndTVSet,
  rndInspector,
  rndAdjuster : TRandom;
  MinCreationDelay : Double = 3.5;
  MaxCreationDelay : Double = 7.5;
  MinInspectionTime : Double = 6;
  MaxInspectionTime : Double = 12;
  NoAdjustmentProb : Double = 0.85;
  MinAdjustmentTime : Double = 20;
  MaxAdjustmentTime : Double = 40;
  SimulationTime : Double = 480;

implementation

{ TTVSet }

procedure TTVSet.RunProcess;
var
  par : TTVControl;
begin
  par := Parent as TTVControl;
  // Жизненный цикл проверяемого телевизора
  // Зафиксировать время прибытия
  IncomeTime := SimTime;
  ClearFinished;
  // Запланировать прибытие следующего телевизора
  TTVSet.Create.
      ActivateDelay(rndTVSet.Uniform(MinCreationDelay, MaxCreationDelay));
  ActivateDelay(par.Inspectors, 0);
  // Ожидать обслуживания
  Wait(par.InspectionQueue);
end;

{ TInspector }

procedure TInspector.RunProcess;
var
  Piece : TTVSet;
  par : TTVControl;
begin
  // Работа проверяющего
  par := Parent as TTVControl;
  while True do
  begin
    // Если нет телевизоров для проверки
    while par.InspectionQueue.Empty do
      // Встать в очередь и ждать прибытия
      Passivate;
    // Зафиксировать начало работы
    par.InspectorsStat.Start(SimTime);
    // Извлечь из очереди первый телевизор
    Piece := par.InspectionQueue.First as TTVSet;
    Piece.StartRunning;
    // Проверка
    Hold(rndInspector.Uniform(MinInspectionTime, MaxInspectionTime));
    // Зафиксировать начало работы
    par.InspectorsStat.Finish(SimTime);
    // С вероятность NoAdjustmentProb телевизор исправен
    if rndInspector.Draw(NoAdjustmentProb) then
    begin
      // Внести статистику о времени пребывания в системе
      par.TimeInSystemStat.AddData(SimTime - Piece.IncomeTime);
      // Удалить телевизор
      Piece.Finish;
    end
    else
    begin
      // Поместить телевизор в очередь на настройку
      Piece.Insert(par.AdjustmentQueue);
      // Дать сигнал настройщику
      par.Adjuster.ActivateDelay(0);
    end;
  end;
end;

{ TAdjuster }

procedure TAdjuster.RunProcess;
var
  par : TTVControl;
  Piece : TTVSet;
begin
  // Работа настройщика
  par := Parent as TTVControl;
  while True do
  begin
    // Если нет телевизоров для настройки, ожидать
    while par.AdjustmentQueue.Empty do
      Passivate;
    // Извлечь первый телевизор из очереди
    Piece := par.AdjustmentQueue.First as TTVSet;
    Piece.StartRunning;
    // Зафиксировать начало работы
    par.AdjustmentStat.Start(SimTime);
    // Выполнить работу
    Hold(rndAdjuster.Uniform(MinAdjustmentTime, MaxAdjustmentTime));
    // Зафиксировать конец работы
    par.AdjustmentStat.Finish(SimTime);
    // Поместить телевизор в очередь на проверку
    Piece.Insert(par.InspectionQueue);
  end;
end;

{ TTVControl }

destructor TTVControl.Destroy;
begin
  TimeInSystemStat.Free;
  InspectorsStat.Free;
  AdjustmentStat.Free;
  Inspectors[0].Free;
  Inspectors[1].Free;
  Adjuster.Free;
  InspectionQueue.Free;
  AdjustmentQueue.Free;
  inherited;
end;

procedure TTVControl.Init;
begin
  inherited;
  InspectionQueue := TList.Create;
  Adjuster := TAdjuster.Create;
  AdjustmentQueue := TList.Create;
  // Поместить двух проверяющих в очередь
  Inspectors[0] := TInspector.Create;
  Inspectors[1] := TInspector.Create;
  TimeInSystemStat := TStatistics.Create;
  InspectorsStat := TServiceStatistics.Create(2, 0, 0);
  AdjustmentStat := TServiceStatistics.Create(1, 0, 0);
  MakeVisualizator(0.2);
end;

procedure TTVControl.RunSimulation;
begin
  TTVSet.Create.ActivateDelay(0);
  // Время моделирования
  Hold(SimulationTime);
  // Скорректировать статистики
  StopStat;
end;

procedure TTVControl.StopStat;
begin
  inherited;
  InspectionQueue.StopStat(SimTime);
  AdjustmentQueue.StopStat(SimTime);
  InspectorsStat.StopStat(SimTime);
  AdjustmentStat.StopStat(SimTime);
end;

end.
