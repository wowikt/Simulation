unit UTVControl;

interface
uses USimulation;

// Модель поточной линии по контролю телевизоров
type
  // Класс TTVSet - проверяемый телевизор
  TTVSet = class(TLink)
  public
    StartingTime : Double;
  end;

  TTVSetGenerator = class(TProcess)
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
    Inspectors : array of TProcess;
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
    // Генератор объектов
    Generator : TTVSetGenerator;
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
  InspectorCount : Integer = 2;
  SimulationTime : Double = 480;

implementation

{ TTVSetGenerator }

procedure TTVSetGenerator.RunProcess;
var
  par : TTVControl;
  tv : TTVSet;
begin
  par := Parent as TTVControl;
  while True do
  begin
    ClearFinished;
    // Создать новый телевизор
    tv := TTVSet.Create;
    // Зафиксировать время прибытия
    tv.StartingTime := SimTime;
    // Поместить телевизор в очередь проверки
    tv.Insert(par.InspectionQueue);
    ActivateDelay(par.Inspectors, 0);
    // Подождать до следующего
    Hold(rndTVSet.Uniform(MinCreationDelay, MaxCreationDelay));
  end;
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
    // Извлечь из очереди первый телевизор
    Piece := par.InspectionQueue.First as TTVSet;
    Piece.Insert(par.RunningObjects);
    // Зафиксировать начало работы
    par.InspectorsStat.Start(SimTime);
    // Проверка
    Hold(rndInspector.Uniform(MinInspectionTime, MaxInspectionTime));
    // Зафиксировать конец работы
    par.InspectorsStat.Finish(SimTime);
    // С вероятность NoAdjustmentProb телевизор исправен
    if rndInspector.Draw(NoAdjustmentProb) then
    begin
      // Внести статистику о времени пребывания в системе
      par.TimeInSystemStat.AddData(SimTime - Piece.StartingTime);
      // Удалить телевизор
      Piece.Insert(par.FinishedObjects);
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
    Piece.Insert(par.RunningObjects);
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
var
  i : Integer;
begin
  Generator.Free;
  TimeInSystemStat.Free;
  InspectorsStat.Free;
  AdjustmentStat.Free;
  for i := 0 to InspectorCount - 1 do
    Inspectors[i].Free;
  Adjuster.Free;
  InspectionQueue.Free;
  AdjustmentQueue.Free;
  inherited;
end;

procedure TTVControl.Init;
var
  i : Integer;
begin
  inherited;
  InspectionQueue := TList.Create;
  Adjuster := TAdjuster.Create;
  AdjustmentQueue := TList.Create;
  Generator := TTVSetGenerator.Create;
  // Создать контролеров
  SetLength(Inspectors, InspectorCount);
  for i := 0 to InspectorCount - 1 do
    Inspectors[i] := TInspector.Create;
  TimeInSystemStat := TStatistics.Create;
  InspectorsStat := TServiceStatistics.Create(InspectorCount);
  AdjustmentStat := TServiceStatistics.Create(1);
end;

procedure TTVControl.RunSimulation;
begin
  Generator.ActivateDelay(0);
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
