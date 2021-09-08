using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс уведомленияо событии, связанном с возобновлением процесса
    /// </summary>
    internal class ProcessEventNotice : EventNotice
    {
        /// <summary>
        /// Конструктор. Записывает значения параметров в поля объекта
        /// </summary>
        /// <param name="time">Время наступления события</param>
        /// <param name="proc">Процесс, активируемый при наступлении события</param>
        public ProcessEventNotice(double time, Process proc) : base(time)
        {
            Proc = proc;
        }

        /// <summary>
        /// Процесс, который активируется при наступлении события
        /// </summary>
        internal Process Proc;

        public override void Finish()
        {
            if (Proc != null)
            {
                if (Proc is SimProc)
                {
                    Proc.Event = null;
                    Proc = null;
                }
                else if (Proc.Event == this)
                {
                    Proc.Event = null;
                    Proc.Finish();
                    Proc.Dispose();
                    Proc = null;
                }
            }
            base.Finish();
        }

        /// <summary>
        /// Обработка события, связанного с процессом
        /// </summary>
        public override object RunEvent()
        {
            Global.CurrSim.CurrentSimTime = EventTime;
            Global.CurrProc = Proc;
            return Proc.Resume();
        }

        /// <summary>
        /// Отображение содержимого уведомления о событии в текстовом виде
        /// </summary>
        /// <returns>Класс процесса и время запланированного события</returns>
        public override string ToString()
        {
            return "Уведомление о событии процесса " + Proc.ToString() + " в " + EventTime.ToString();
        }
    }

    /// <summary>
    /// Класс Process - базовый класс процесса, выполняемого при исполнении имитации
    /// </summary>
    public class Process : Coroutine, ISchedulable
    {
        /// <summary>
        /// Конструктор. Устанавливает ссылку на родительскую имитацию.
        /// </summary>
        public Process()
        {
            // Получить ссылку на родительский процесс имитации
            // Пустая ссылка на родителя может быть только у главного процесса имитации
            if (Owner == null || Owner is SimProc)
                Parent = Owner as SimProc;
            else
                // Если ссылка непустая и не имитация, то это обычный процесс
                Parent = (Owner as Process).Parent;
            // Если родительский процесс существует (то есть, данный процесс - не процесс имитации),
            //   встать в список свободных процессов
            if (Parent != null && Parent != this)
            {
                StartRunning();
            }
            // Уведомления о событии нет
            Event = null;
            //Global.CurrProc = this;
        }

        /// <summary>
        /// Уведомление о событии, связанное с процессом
        /// </summary>
        public EventNotice Event
        {
            get;
            internal set;
        }

        /// <summary>
        /// Процесс имитации, в рамках которой выполняется данный процесс
        /// </summary>
        protected internal SimProc Parent;

        /// <summary>
        /// Имитационное время начала работы процесса
        /// </summary>
        public double StartingTime;

        /// <summary>
        /// Имитационное время, оставшееся до выполнения незавершенного действия.
        /// Используется, если текущее действие было перехвачено другим процессом.
        /// </summary>
        public double TimeLeft;

        /// <summary>
        /// Время наступления события данного процесса. 
        /// Если события нет, большая отрицательная величина (-1e300).
        /// </summary>
        public double EventTime
        {
            get
            {
                if (Idle)
                    return -1e300;
                else
                    return (Event as EventNotice).EventTime;
            }
        }

        /// <summary>
        /// Указывает, находится ли процесс в пассивном или завершенном состоянии
        /// </summary>
        public bool Idle
        {
            get
            {
                return Event == null;
            }
        }

        /// <summary>
        /// Помещает запись уведомления об активации процесса непосредственно после текущего.
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// </summary>
        public void Activate()
        {
            ActivateDelay(0);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса непосредственно после указанной записи
        /// </summary>
        /// <param name="en">Запись уведомления о событии, после которой следует поместить запись нового события</param>
        internal void ActivateAfter(EventNotice en)
        {
            if (!Idle)
            {
                return;
            }
            Event = new ProcessEventNotice(en.EventTime, this);
            Event.InsertAfter(en);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса непосредственно после записи указанного процесса
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="p">Процесс, после которого следует активировать данный</param>
        public void ActivateAfter(ISchedulable p)
        {
            if (!Idle)
            {
                return;
            }
            if (p.Idle)
            {
                throw new ESimulationException("Нельзя выполнить ActivateAfter(p) после пассивного процесса");
            }
            ActivateAfter(p.Event);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса в указанное время
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        public void ActivateAt(double t)
        {
            if (!Idle)
            {
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            Event = new ProcessEventNotice(t, this);
            Event.Insert(Parent.Calendar);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса непосредственно перед указанной записью
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="en">Запись уведомления о событии, перед которой следует поместить запись нового события</param>
        internal void ActivateBefore(EventNotice en)
        {
            if (!Idle)
            {
                return;
            }
            Event = new ProcessEventNotice(en.EventTime, this);
            Event.InsertBefore(en);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса непосредственно перед записью указанного процесса
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="p">Процесс, перед которым следует активировать данный</param>
        public void ActivateBefore(ISchedulable p)
        {
            if (!Idle)
            {
                return;
            }
            if (p.Idle)
            {
                throw new ESimulationException("Нельзя выполнить ActivateAfter(p) после пассивного процесса");
            }
            ActivateBefore(p.Event);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса с указанной задержкой времени
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        public void ActivateDelay(double dt)
        {
            ActivateAt(SimTime() + dt);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса в указанное время с приоритетом по отношению к процессам,
        /// запланированным ранее на то же время
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        public void ActivatePriorAt(double t)
        {
            if (!Idle)
            {
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            Event = new ProcessEventNotice(t, this);
            Event.InsertPrior(Parent.Calendar);
        }

        /// <summary>
        /// Создает запись уведомления об активации процесса с указанной задержкой времени с приоритетом по отношению к процессам,
        /// запланированным ранее на то же время
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        public void ActivatePriorDelay(double dt)
        {
            ActivatePriorAt(SimTime() + dt);
        }

        /// <summary>
        /// Удаление завершенных процессов. 
        /// Используется для оптимизации количества потоков в программе
        /// </summary>
        public void ClearFinished()
        {
            Parent.Collect.Activate();
        }

        /// <summary>
        /// Основной алгоритм работы процесса. Должен быть переопределен в производном классе.
        /// В данном классе ничего не делает.
        /// </summary>
        protected override void Execute()
        {
        }

        /// <summary>
        /// Завершение работы процесса. Удаляет уведомление о событии из календаря.
        /// В переопределенном методе производного класса ОБЯЗАТЕЛЬНО должен вызываться ПОСЛЕДНИМ.
        /// </summary>
        public override void Finish()
        {
            EventNotice en = Event;
            if (Event != null)
            {
                (Event as ProcessEventNotice).Proc = null;
                Event = null;
                en.Finish();
            }
            base.Finish();
        }

        /// <summary>
        /// Включение процесса в список завершенных для последующего автоматического удаления.
        /// <para>Любой процесс, не завершаемый явно извне методом Finish(), должен завершать свою работу посредством этого метода</para>
        /// </summary>
        public void GoFinished()
        {
            Insert(Parent.Finished);
        }

        /// <summary>
        /// Обрабатывает все события, запланированные на текущее время, и возобновляет процесс
        /// <para>Эквивалентно вызову Hold(0)</para>
        /// </summary>
        public void Hold()
        {
            Hold(0);
        }

        /// <summary>
        /// Приостанавливает процесс на заданный промежуток времени
        /// </summary>
        /// <param name="dt">Время приостановки процесса</param>
        public void Hold(double dt)
        {
            ReactivateDelay(dt);
        }

        /// <summary>
        /// Инициализация процесса
        /// <para>Может использоваться для создания внутренних объектов процесса, которые должны создаваться в контексте процесса</para>
        /// <para>В отличие от конструктора, исполняется в контексте потока данного процесса</para>
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// Переводит процесс в пассивное состояние, удаляя его запись уведомления о событии из календаря
        /// <para>Если процесс является текущим, управление передается следующему по порядку процессу</para>
        /// </summary>
        public void Passivate()
        {
            if (Idle)
            {
                return;
            }
            EventNotice pen = Event;
            (Event as ProcessEventNotice).Proc = null;
            Event = null;
            pen.Finish();
            if (Global.CurrProc == this)
            {
                RunNextProc();
            }
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса, в том числе приостановленного, непосредственно после текущего.
        /// <para>Если процесс находится в активном состоянии (то есть, является текущим), ничего не делает</para>
        /// </summary>
        public void Reactivate()
        {
            ReactivateDelay(0);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса непосредственно после указанной записи
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="en">Запись, после которой следует активировать текущую</param>
        internal void ReactivateAfter(EventNotice en)
        {
            if (Idle)
            {
                ActivateAfter(en);
                return;
            }
            if (Event.IsFirst)
            {
                (Event as EventNotice).EventTime = en.EventTime;
                Event.InsertAfter(en);
                RunNextProc();
            }
            else
            {
                Event.EventTime = en.EventTime;
                Event.InsertAfter(en);
            }
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса непосредственно после записи указанного процесса
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="p">Процесс, перед которым следует активировать данный</param>
        public void ReactivateAfter(ISchedulable p)
        {
            if (Idle)
            {
                ActivateAfter(p);
                return;
            }
            if (p.Idle)
            {
                throw new ESimulationException("Нельзя выполнить ReactivateAfter(p) после пассивного процесса");
            }
            ReactivateAfter(p.Event);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса на указанное время
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        public void ReactivateAt(double t)
        {
            if (Idle)
            {
                ActivateAt(t);
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            if (Event.IsFirst)
            {
                Event.SetTime(t);
                RunNextProc();
            }
            else
            {
                Event.SetTime(t);
            }
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса непосредственно перед указанной записью
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="en">Запись, перед которой следует активировать текущую</param>
        internal void ReactivateBefore(EventNotice en)
        {
            if (Idle)
            {
                ActivateBefore(en);
                return;
            }
            if (Event.IsFirst)
            {
                Event.EventTime = en.EventTime;
                Event.InsertBefore(en);
                RunNextProc();
            }
            else
            {
                Event.EventTime = en.EventTime;
                Event.InsertBefore(en);
            }
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса непосредственно перед записью указанного процесса
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="p">Процесс, перед которым следует активировать данный</param>
        public void ReactivateBefore(ISchedulable p)
        {
            if (Idle)
            {
                ActivateBefore(p);
                return;
            }
            if (p.Idle)
            {
                throw new ESimulationException("Нельзя выполнить ReactivateBefore(p) после пассивного процесса");
            }
            ReactivateBefore(p.Event);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса с указанной задержкой времени относительно текущего процесса.
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// </summary>
        /// <param name="dt">Имитационное время активации процесса</param>
        public void ReactivateDelay(double dt)
        {
            ReactivateAt(SimTime() + dt);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса на указанное время с приоритетом.
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        public void ReactivatePriorAt(double t)
        {
            if (Idle)
            {
                ActivatePriorAt(t);
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            if (Event.IsFirst)
            {
                Event.SetTimePrior(t);
                RunNextProc();
            }
            else
            {
                Event.SetTimePrior(t);
            }
        }

        /// <summary>
        /// Перемещает запись уведомления об активации процесса с указанной задержкой времени относительно текущего процесса с приоритетом.
        /// <para>Если процесс находится в пассивном состоянии, создает новую запись уведомления</para>
        /// <para>Если процесс является активным, он приостанавливается</para>
        /// </summary>
        /// <param name="dt">Имитационное время активации процесса</param>
        public void ReactivatePriorDelay(double dt)
        {
            ReactivatePriorAt(SimTime() + dt);
        }

        /// <summary>
        /// Организует исполнение метода Execute() в контексте процесса
        /// </summary>
        protected override void Run()
        {
            Global.CurrProc = this;
            Init();
            StartingTime = SimTime();
            Execute();
            TerminatedState = true;
            EventNotice en = Event;
            (Event as ProcessEventNotice).Proc = null;
            Event = null;
            en.Finish();
            //Remove();
        }

        /// <summary>
        /// Переход к следующему событию в календаре
        /// </summary>
        private void RunNextProc()
        {
            Yield(new ContinueSim());
        }

        /// <summary>
        /// Возвращает текущее имитационное время. 
        /// Обращается к родительскому процесу имитации.
        /// </summary>
        /// <returns>Текущее имитационное время</returns>
        public virtual double SimTime()
        {
            return Parent.SimTime();
        }

        /// <summary>
        /// Включение процесса в список свободных. Используется также 
        /// для извлечения процесса из очереди ожидания 
        /// (при этом он все равно попадает в список свободных процессов)
        /// </summary>
        public void StartRunning()
        {
            Insert(Parent.Running);
        }

        /// <summary>
        /// Постановка процесса в очередь и перевод его в режим ожидания
        /// </summary>
        /// <param name="q">Очередь ожидания</param>
        public void Wait(List q)
        {
            Insert(q);
            Passivate();
        }
    }

    /// <summary>
    /// Фиктивный класс, предназначенный для передачи от обработчика события информации
    /// о том, что имитация должна быть приостановлена
    /// </summary>
    internal class StopSim
    {
    }

    /// <summary>
    /// Фиктивный класс, предназначенный для передачи от обработчика события информации
    /// о том, что имитация должна продолжаться
    /// </summary>
    internal class ContinueSim
    {
    }

    /// <summary>
    /// Класс SimProc - главный процесс имитации
    /// </summary>
    public class SimProc : Process
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SimProc()
        {
            Global.CurrSim = this;
            CurrentSimTime = 0;
            Parent = this;
            Calendar = new List(CalendarOrder, 0, "Календарь");
            Finished = new List();
            Running = new List();
            Event = new ProcessEventNotice(0, this);
            Event.InsertFirst(Calendar);
        }

        /// <summary>
        /// Календарь событий
        /// </summary>
        public List Calendar;

        /// <summary>
        /// Сборщик завершенных процессов
        /// </summary>
        internal Collector Collect;

        /// <summary>
        /// Текущее имитационное время
        /// </summary>
        public double CurrentSimTime;

        /// <summary>
        /// Диспетчер, управляющий работой данной имитации
        /// </summary>
        internal SimulationDispatcher Dispatcher;

        /// <summary>
        /// Список завершенных процессов для периодического удаления
        /// </summary>
        public List Finished;

        /// <summary>
        /// Список свободных процессов
        /// </summary>
        public List Running;

        internal static StopSim Stopped = new StopSim();

        /// <summary>
        /// Процесс-визуализатор
        /// </summary>
        internal Visualizer VisProc;

        /// <summary>
        /// Шаг визуализации
        /// </summary>
        public double VisualInterval
        {
            get
            {
                if (VisProc == null)
                {
                    return 0;
                }
                else
                {
                    return VisProc.Interval;
                }
            }
            set
            {
                if (VisProc == null)
                {
                    VisProc = new Visualizer(value);
                }
                else
                {
                    VisProc.Interval = value;
                }
            }
        }

        /// <summary>
        /// Стандартная функция сравнения для календаря событий. 
        /// Используется при вставке уведомлений о событиях в календарь без приоритета
        /// </summary>
        /// <param name="a">Ссылка на вставляемое уведомление</param>
        /// <param name="b">Ссылка на очередное уведомление списка</param>
        /// <returns>Результат сравнения</returns>
        private static bool CalendarOrder(ILink a, ILink b)
        {
            return (a as EventNotice).EventTime < (b as EventNotice).EventTime;
        }

        /// <summary>
        /// Очистка статистики
        /// </summary>
        public virtual void ClearStat()
        {
            Calendar.ClearStat();
        }

        /// <summary>
        /// Вывод на консоль календаря событий. Используется с целью диагностики при отладке программ
        /// </summary>
        public void DumpEventQueue()
        {
            EventNotice en = Calendar.First as EventNotice;
            while (en != null)
            {
                Console.WriteLine(en);
                en = en.Next as EventNotice;
            }
        }

        /// <summary>
        /// Завершение имитации
        /// </summary>
        public override void Finish()
        {
            Finished.Finish();
            Running.Finish();
            Calendar.Finish();
            base.Finish();
        }

        /// <summary>
        /// Исполняемый метод процесса имитации. Никогда не должен вызываться прямо или переопределяться
        /// </summary>
        protected override void Run()
        {
            Global.CurrProc = this;
            Global.CurrSim = this;
            Collect = new Collector();
            Init();
            StartingTime = SimTime();
            Collect.Activate();
            if (VisProc != null)
            {
                VisProc.Activate();
            }
            Execute();
            TerminatedState = true;
            StopStat();
            GC.Collect();
            Yield(new StopSim());
            //while (true)
                //Yield(Dispatcher);
        }

        /// <summary>
        /// Возвращает текущее имитационное время данной имитации
        /// </summary>
        /// <returns>Имитационное время</returns>
        public override double SimTime()
        {
            return CurrentSimTime;
        }

        /// <summary>
        /// Запуск процесса имитации. Фактически, запускается диспетчер, управляющий имитацией
        /// </summary>
        public void Start()
        {
            object res;
            int i = 0;
            Global.CurrSim = this;
            Global.CurrProc = this;
            do
            {
                // Получить очередную запись календаря событий
                EventNotice en = Calendar.First as EventNotice;
                // Исполнить событие
                res = en.RunEvent();
                i++;
            }
            while (!(res is StopSim));
            // Если результат исполнения - пустой указатель, приостановить исполнение
            Global.CurrProc = null;
        }

        /// <summary>
        /// Коррекция статистики к текущему моменту
        /// </summary>
        public virtual void StopStat()
        {
            Calendar.StopStat();
        }
    }
}
