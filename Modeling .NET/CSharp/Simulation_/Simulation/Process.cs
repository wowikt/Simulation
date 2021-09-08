using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
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
            Parent = Global.CurrSim;
            // Если родительский процесс существует (то есть, данный процесс - не процесс имитации),
            //   встать в список свободных процессов
            if (Parent != null && Parent != this)
            {
                StartRunning();
            }
            OnServiceStartingEvent = ServiceStartingEvent;
            // Уведомления о событии нет
            (this as ISchedulable).Event = null;
            //Global.CurrProc = this;
        }

        /// <summary>
        /// Уведомление о событии, связанное с процессом
        /// </summary>
        EventNotice ISchedulable.Event
        {
            get;
            set;
        }

        /// <summary>
        /// Ссылка на метод обработки события начала обслуживающего действия
        /// </summary>
        public EventProc OnServiceStartingEvent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Процесс имитации, в рамках которой выполняется данный процесс
        /// </summary>
        protected internal IMainSimulation Parent;

        /// <summary>
        /// Имитационное время начала работы процесса
        /// </summary>
        public double StartingTime;

        /// <summary>
        /// Имитационное время, оставшееся до выполнения незавершенного действия.
        /// Используется, если текущее действие было перехвачено другим процессом.
        /// </summary>
        public double TimeLeft
        {
            get;
            set;
        }

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
                    return ((this as ISchedulable).Event as EventNotice).EventTime;
            }
        }

        /// <summary>
        /// Указывает, находится ли процесс в пассивном или завершенном состоянии
        /// </summary>
        public bool Idle
        {
            get
            {
                return (this as ISchedulable).Event == null;
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
            (this as ISchedulable).Event = new ProcessEventNotice(en.EventTime, this);
            (this as ISchedulable).Event.InsertAfter(en);
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
            (this as ISchedulable).Event = new ProcessEventNotice(t, this);
            (this as ISchedulable).Event.Insert(Parent.Calendar);
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
            (this as ISchedulable).Event = new ProcessEventNotice(en.EventTime, this);
            (this as ISchedulable).Event.InsertBefore(en);
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
            (this as ISchedulable).Event = new ProcessEventNotice(t, this);
            (this as ISchedulable).Event.InsertPrior(Parent.Calendar);
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
            EventNotice en = (this as ISchedulable).Event;
            if ((this as ISchedulable).Event != null)
            {
                ((this as ISchedulable).Event as ProcessEventNotice).Proc = null;
                (this as ISchedulable).Event = null;
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
            EventNotice pen = (this as ISchedulable).Event;
            ((this as ISchedulable).Event as ProcessEventNotice).Proc = null;
            (this as ISchedulable).Event = null;
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                ((this as ISchedulable).Event as EventNotice).EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertAfter(en);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertAfter(en);
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                (this as ISchedulable).Event.SetTime(t);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.SetTime(t);
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                (this as ISchedulable).Event.EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertBefore(en);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertBefore(en);
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                (this as ISchedulable).Event.SetTimePrior(t);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.SetTimePrior(t);
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
            EventNotice en = (this as ISchedulable).Event;
            ((this as ISchedulable).Event as ProcessEventNotice).Proc = null;
            (this as ISchedulable).Event = null;
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
        /// Стандартный метод обработки обслуживающего действия.
        /// В данном классе ничего не делает.
        /// </summary>
        public virtual void ServiceStartingEvent()
        {
        }

        /// <summary>
        /// Постановка процесса в очередь и перевод его в режим ожидания
        /// <para>Если очередь имеет максимальный размер, процесс 
        /// не ставится в очередь</para>
        /// </summary>
        /// <param name="q">Очередь ожидания</param>
        /// <returns><para>false, если процесс не поставлен в очередь.</para> 
        /// <para>true, если процесс успешно поставлен и возобновлен после ожидания</para></returns>
        public bool Wait(List q)
        {
            if (q.MaxSize == 0 || q.Size < q.MaxSize)
            {
                Insert(q);
                Passivate();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Выполняет обслуживающее действие
        /// </summary>
        /// <param name="serv">Обслуживающее действие</param>
        /// <param name="dt">Планируемая продолжительность действия</param>
        /// <returns>true, если компонент был успешно поставлен в очередь 
        /// ожидания действия. false, если компонент не удалось поставить в очередь 
        /// по причине ее заполненности.</returns>
        public bool DoService(Service serv, double dt)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            TimeLeft = dt;
            serv.Agents.ActivateFirst();
            Wait(serv.Queue);
            return true;
        }

        /// <summary>
        /// Выполняет обслуживающее действие
        /// </summary>
        /// <param name="serv">Обслуживающее действие</param>
        /// <param name="dt">Планируемая продолжительность действия</param>
        /// <param name="start">Метод, выполняемый в момент начала обслуживания</param>
        /// <returns>true, если компонент был успешно поставлен в очередь 
        /// ожидания действия. false, если компонент не удалось поставить в очередь 
        /// по причине ее заполненности.</returns>
        public bool DoService(Service serv, double dt, EventProc start)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            TimeLeft = dt;
            OnServiceStartingEvent = start;
            serv.Agents.ActivateFirst();
            Wait(serv.Queue); 
            return true;
        }

        /// <summary>
        /// Выполняет обслуживающее действие.
        /// Продолжительность действия определяется делегатом, находящимся 
        /// в объекте действия. Если делегат отсутствует, порождается исключение.
        /// </summary>
        /// <param name="serv">Обслуживающее действие</param>
        /// <returns>true, если компонент был успешно поставлен в очередь 
        /// ожидания действия. false, если компонент не удалось поставить в очередь 
        /// по причине ее заполненности.</returns>
        public bool DoService(Service serv)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            if (serv.Duration == null)
            {
                throw new ESimulationException(
                    "DoService(): у объекта действия не задан делегат, вычисляющий длительность обслуживания");
            }
            TimeLeft = serv.Duration();
            serv.Agents.ActivateFirst();
            Wait(serv.Queue);
            return true;
        }

        /// <summary>
        /// Выполняет обслуживающее действие.
        /// Продолжительность действия определяется делегатом, находящимся 
        /// в объекте действия. Если делегат отсутствует, порождается исключение.
        /// </summary>
        /// <param name="serv">Обслуживающее действие</param>
        /// <param name="start">Метод, выполняемый в момент начала обслуживания</param>
        /// <returns>true, если компонент был успешно поставлен в очередь 
        /// ожидания действия. false, если компонент не удалось поставить в очередь 
        /// по причине ее заполненности.</returns>
        public bool DoService(Service serv, EventProc start)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            if (serv.Duration == null)
            {
                throw new ESimulationException(
                    "DoService(): у объекта действия не задан делегат, вычисляющий длительность обслуживания");
            }
            TimeLeft = serv.Duration();
            OnServiceStartingEvent = start;
            serv.Agents.ActivateFirst();
            Wait(serv.Queue);
            return true;
        }
    }
}
