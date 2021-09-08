using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс SchedulableComponent является основой для создания компонентов - 
    /// объектов имитации, способных планировать события в календаре
    /// </summary>
    public class SchedulableComponent : Component, ISchedulable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SchedulableComponent()
        {
            OnStartEvent = StartEvent;
            OnNextScheduledEvent = OnStartEvent;
            FirstEvent = true;
        }

        /// <summary>
        /// Признак того, является ли следующее событие первым в жизненном цикле компонента.
        /// Непосредственно перед обработкой первого события устанавливается имитационное время 
        /// начала работы компонента.
        /// </summary>
        private bool FirstEvent;

        /// <summary>
        /// Задает следующее обрабатываемое событие, не планируя его в календаре
        /// </summary>
        protected EventProc NextSceduledEvent
        {
            set
            {
                OnNextScheduledEvent = value;
            }
        }

        /// <summary>
        /// Ссылка на событийную процедуру для следующего планируемого события
        /// </summary>
        public EventProc OnNextScheduledEvent
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Ссылка на событийную процедуру для события начала работы компонента
        /// </summary>
        public EventProc OnStartEvent
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Ссылка на уведомление о событии
        /// </summary>
        public SchedulableEventNotice Event
        {
            get;
            set;
        }

        /// <summary>
        /// Проверка, назначено ли событие компоненту
        /// </summary>
        public bool Idle
        {
            get
            {
                return Event == null;
            }
        }

        /// <summary>
        /// Помещает запись уведомления об активации компонента непосредственно после текущего.
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        public void Activate()
        {
            ActivateDelay(0, OnNextScheduledEvent);
        }

        /// <summary>
        /// Помещает запись уведомления об активации компонента непосредственно после текущего с указанием событийного метода.
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="nextProc">Метод обработки планируемого события</param>
        protected void Activate(EventProc nextProc)
        {
            ActivateDelay(0, nextProc);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента непосредственно после указанной записи
        /// </summary>
        /// <param name="en">Запись уведомления о событии, после которой следует поместить запись нового события</param>
        internal void ActivateAfter(SchedulableEventNotice en)
        {
            Event = new ComponentEventNotice(en.EventTime, this);
            Event.InsertAfter(en);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента непосредственно после записи указанного процесса
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, после которого следует активировать компонент</param>
        public void ActivateAfter(ISchedulable sch)
        {
            ActivateAfter(sch, OnNextScheduledEvent);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента непосредственно после указанной записи с указанием событийного метода
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, после которого следует активировать компонент</param>
        /// <param name="nextProc">Метод обработки планируемого события</param>
        protected void ActivateAfter(ISchedulable sch, EventProc nextProc)
        {
            if (!Idle && !Event.IsFirst)
            {
                return;
            }
            if (sch.Idle)
            {
                throw new ESimulationException("EventNotice.ActivateAfter(): нельзя активировать после пассивного процесса");
            }
            if (nextProc.Target != this)
            {
                throw new ESimulationException("EventNotice.ActivateAfter(): объект может активировать только события, связанные с собой");
            }
            OnNextScheduledEvent = nextProc;
            ActivateAfter(sch.Event);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента в указанное время
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="t">Имитационное время активации компонента</param>
        public void ActivateAt(double t)
        {
            ActivateAt(t, OnNextScheduledEvent);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента в указанное время с указанием событийного метода
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="t">Имитационное время активации компонента</param>
        /// <param name="nextProc">Метод обработки планируемого события</param>
        protected void ActivateAt(double t, EventProc nextProc)
        {
            if (!Idle && !Event.IsFirst)
            {
                return;
            }
            if (nextProc.Target != this)
            {
                throw new ESimulationException("EventNotice.ActivateAt(): объект может активировать только события, связанные с собой");
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            OnNextScheduledEvent = nextProc;
            Event = new ComponentEventNotice(t, this);
            Event.Insert(Parent.Calendar);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента непосредственно перед указанной записью
        /// </summary>
        /// <param name="en">Запись уведомления о событии, перед которой следует поместить запись нового события</param>
        internal void ActivateBefore(SchedulableEventNotice en)
        {
            Event = new ComponentEventNotice(en.EventTime, this);
            Event.InsertBefore(en);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента непосредственно перед записью указанного процесса
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, перед которым следует активировать компонент</param>
        public void ActivateBefore(ISchedulable sch)
        {
            ActivateBefore(sch, OnNextScheduledEvent);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента непосредственно 
        /// перед записью указанного процесса с указанием событийного метода
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, перед которым следует активировать компонент</param>
        /// <param name="nextProc">Метод обработки планируемого события</param>
        protected void ActivateBefore(ISchedulable sch, EventProc nextProc)
        {
            if (!Idle && !Event.IsFirst)
            {
                return;
            }
            if (sch.Idle)
            {
                throw new ESimulationException("EventNotice.ActivateBefore(): нельзя активировать после пассивного процесса");
            }
            if (nextProc.Target != this)
            {
                throw new ESimulationException("EventNotice.ActivateBefore(): объект может активировать только события, связанные с собой");
            }
            OnNextScheduledEvent = nextProc;
            ActivateBefore(sch.Event);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента с указанной задержкой времени
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        public void ActivateDelay(double dt)
        {
            ActivateAt(SimTime() + dt, OnNextScheduledEvent);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента с указанной задержкой времени с указанием событийного метода
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        /// <param name="nextProc">Метод обработки планируемого события</param>
        protected void ActivateDelay(double dt, EventProc nextProc)
        {
            ActivateAt(SimTime() + dt, nextProc);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента в указанное время с приоритетом
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="t">Имитационное время активации компонента</param>
        public void ActivatePriorAt(double t)
        {
            ActivatePriorAt(t, OnNextScheduledEvent);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента в указанное время 
        /// с указанием событийного метода с приоритетом
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="t">Имитационное время активации компонента</param>
        /// <param name="nextProc">Метод обработки планируемого события</param>
        protected void ActivatePriorAt(double t, EventProc nextProc)
        {
            if (!Idle && !Event.IsFirst)
            {
                return;
            }
            if (nextProc.Target != this)
            {
                throw new ESimulationException("EventNotice.ActivatePriorAt(): объект может активировать только события, связанные с собой");
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            OnNextScheduledEvent = nextProc;
            Event = new ComponentEventNotice(t, this);
            Event.InsertPrior(Parent.Calendar);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента 
        /// с указанной задержкой времени с приоритетом
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        public void ActivatePriorDelay(double dt)
        {
            ActivatePriorAt(SimTime() + dt, OnNextScheduledEvent);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента 
        /// с указанной задержкой времени с указанием событийного метода с приоритетом
        /// <para>Если компонент находится в активном состоянии, создается новая запись уведомления</para>
        /// <para>Если компонент находится в приостановленном состоянии, ничего не делает</para>
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        /// <param name="nextProc">Метод обработки планируемого события</param>
        protected void ActivatePriorDelay(double dt, EventProc nextProc)
        {
            ActivatePriorAt(SimTime() + dt, nextProc);
        }

        /// <summary>
        /// Удаление завершенных процессов. Используется для оптимизации количества потоков в программе
        /// <para>Если в программе не используются процессы, а есть только компоненты, 
        /// этот метод, а также GoFinished() и StartRunning(), не нужны</para>
        /// </summary>
        public void ClearFinished()
        {
            Parent.Finished.Clear();
        }

        /// <summary>
        /// Включение компонента в список завершенных для последующего автоматического удаления.
        /// <para>Представлен для совместимости с классом Process 
        /// с целью более простого перехода от процессов к компонентам. 
        /// Реально в этом методе нет необходимости, посткольку в .NET 
        /// неиспользуемые объекты удаляются автоматически при сборке мусора</para>
        /// <para>Если в программе не используются процессы, а есть только компоненты, 
        /// этот метод, а также ClearFinished() и StartRunning(), не нужны</para>
        /// </summary>
        public void GoFinished()
        {
            Insert(Parent.Finished);
        }

        /// <summary>
        /// Событийный метод начала работы компонента. 
        /// <para>При создании компонента ссылка на этот метод помечается как очередной.</para>
        /// <para>Если компонент обрабатывает единственное событие, другие событийные методы не нужны</para>
        /// <para>Если алгоритм работы компонента предусматривает несколько событий,
        /// данный метод будет соответствовать первому событию. Для обработки других событий 
        /// в классе компонента необходимо предусмотреть соответствующие методы
        /// с аналогичной сигнатурой</para>
        /// </summary>
        protected internal virtual void StartEvent()
        {
        }

        /// <summary>
        /// Событийный метод окончания работы компонента. 
        /// <para>Если логика работы компонента подразумевает исполнение 
        /// завершающих действий по окончании его жизненного цикла, 
        /// следует переопределить этот метод.</para>
        /// <para>Если работа компонента завершается после прохождения по сети,
        /// можно указать данный метод как очередной  при входе в сеть 
        /// (например, присвоив его свойству NextScheduledEvent компонента 
        /// или указав параметром в методе входа в сеть), а сам метод
        /// переопределять только в случае, если при завершении работы
        /// требуется выполнить некоторые дополнительные действия
        /// (например, сбор статистики)</para>
        /// </summary>
        protected internal virtual void FinishEvent()
        {
        }

        /// <summary>
        /// Исключает уведомление о событии для компонента из календаря
        /// </summary>
        public void Passivate()
        {
            if (Event != null)
            {
                Event.Remove();
                Event = null;
            }
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента, 
        /// в том числе приостановленного, непосредственно после текущего.
        /// <para>Если компонент находится в активном состоянии 
        /// (то есть, является текущим), создается новая запись уведомления</para>
        /// </summary>
        public void Reactivate()
        {
            ReactivateDelay(0, OnNextScheduledEvent);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента, 
        /// в том числе приостановленного, непосредственно после текущего 
        /// с указанием событийного метода.
        /// <para>Если компонент находится в активном состоянии 
        /// (то есть, является текущим), создается новая запись уведомления</para>
        /// </summary>
        /// <param name="nextProc">Событийный метод обработки планируемого события</param>
        public void Reactivate(EventProc nextProc)
        {
            ReactivateDelay(0, nextProc);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// непосредственно после записи указанного процесса
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, 
        /// порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, после записи которого следует активировать компонент</param>
        public void ReactivateAfter(ISchedulable sch)
        {
            ReactivateAfter(sch, OnNextScheduledEvent);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// непосредственно после записи указанного процесса
        /// с указанием событийного метода.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, 
        /// порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, после записи которого следует активировать компонент</param>
        /// <param name="nextProc">Событийный метод обработки планируемого события</param>
        public void ReactivateAfter(ISchedulable sch, EventProc nextProc)
        {
            if (Idle || Event.IsFirst)
            {
                ActivateAfter(sch, nextProc);
                return;
            }
            if (sch.Idle)
            {
                throw new ESimulationException("EventNotice.ReactivateAfter(): нельзя активировать после пассивного процесса");
            }
            if (nextProc.Target != this)
            {
                throw new ESimulationException("EventNotice.ReactivateAfter(): объект может активировать только события, связанные с собой");
            }
            OnNextScheduledEvent = nextProc;
            ReactivateAfter(sch.Event);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// непосредственно после указанной записи
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, 
        /// порождается исключение</para>
        /// </summary>
        /// <param name="en">Запись, после которой следует активировать текущую</param>
        internal void ReactivateAfter(SchedulableEventNotice en)
        {
            Event.EventTime = en.EventTime;
            Event.InsertAfter(en);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента на указанное время
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        public void ReactivateAt(double t)
        {
            ReactivateAt(t, OnNextScheduledEvent);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента на указанное время
        /// с указанием событийного метода.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        /// <param name="nextProc">Событийный метод обработки планируемого события</param>
        public void ReactivateAt(double t, EventProc nextProc)
        {
            if (Idle || Event.IsFirst)
            {
                ActivateAt(t, nextProc);
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            Event.SetTime(t);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// непосредственно перед записью указанного процесса
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, 
        /// порождается исключение</para>
        /// </summary>
        /// <param name="en">Процесс, перед записью которого следует активировать компонент</param>
        internal void ReactivateBefore(SchedulableEventNotice en)
        {
            Event.EventTime = en.EventTime;
            Event.InsertBefore(en);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// непосредственно перед записью указанного процесса
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, 
        /// порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, перед записью которого следует активировать компонент</param>
        public void ReactivateBefore(ISchedulable sch)
        {
            ReactivateBefore(sch, OnNextScheduledEvent);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// непосредственно перед записью указанного процесса
        /// с указанием событийного метода.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// <para>Если процесс-параметр находится в пассивном состоянии, 
        /// порождается исключение</para>
        /// </summary>
        /// <param name="sch">Процесс, перед записью которого следует активировать компонент</param>
        /// <param name="nextProc">Событийный метод обработки планируемого события</param>
        public void ReactivateBefore(ISchedulable sch, EventProc nextProc)
        {
            if (Idle || Event.IsFirst)
            {
                ActivateBefore(sch, nextProc);
                return;
            }
            if (sch.Idle)
            {
                throw new ESimulationException("EventNotice.ReactivateBefore(): нельзя активировать после пассивного процесса");
            }
            if (nextProc.Target != this)
            {
                throw new ESimulationException("EventNotice.ReactivateBefore(): объект может активировать только события, связанные с собой");
            }
            OnNextScheduledEvent = nextProc;
            ReactivateBefore(sch.Event);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// с указанной задержкой времени относительно текущего процесса.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="dt">Имитационное время активации компонента</param>
        public void ReactivateDelay(double dt)
        {
            ReactivateAt(SimTime() + dt, OnNextScheduledEvent);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// с указанной задержкой времени относительно текущего процесса.
        /// с указанием событийного метода.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="dt">Имитационное время активации компонента</param>
        /// <param name="nextProc">Событийный метод обработки планируемого события</param>
        public void ReactivateDelay(double dt, EventProc nextProc)
        {
            ReactivateAt(SimTime() + dt, nextProc);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента на указанное время 
        /// с приоритетом
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        public void ReactivatePriorAt(double t)
        {
            ReactivatePriorAt(t, OnNextScheduledEvent);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента на указанное время
        /// с указанием событийного метода с приоритетом.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="t">Имитационное время активации процесса</param>
        /// <param name="nextProc">Событийный метод обработки планируемого события</param>
        public void ReactivatePriorAt(double t, EventProc nextProc)
        {
            if (Idle || Event.IsFirst)
            {
                ActivatePriorAt(t, nextProc);
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            Event.SetTimePrior(t);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// с указанной задержкой времени относительно текущего процесса с приоритетом.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="dt">Имитационное время активации компонента</param>
        public void ReactivatePriorDelay(double dt)
        {
            ReactivatePriorAt(SimTime() + dt, OnNextScheduledEvent);
        }

        /// <summary>
        /// Перемещает запись уведомления об активации компонента 
        /// с указанной задержкой времени относительно текущего процесса с приоритетом.
        /// с указанием событийного метода.
        /// <para>Если компонент находится в активном или пассивном состоянии, 
        /// создает новую запись уведомления</para>
        /// </summary>
        /// <param name="dt">Имитационное время активации компонента</param>
        /// <param name="nextProc">Событийный метод обработки планируемого события</param>
        public void ReactivatePriorDelay(double dt, EventProc nextProc)
        {
            ReactivatePriorAt(SimTime() + dt, nextProc);
        }

        /// <summary>
        /// Обработка события компонента
        /// </summary>
        /// <returns>Ссылка на данный объект</returns>
        internal virtual object Run()
        {
            if (FirstEvent)
            {
                StartingTime = SimTime();
                FirstEvent = false;
            }
            OnNextScheduledEvent();
            return new ContinueSim();
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
        /// Постановка компонента в очередь и перевод его в режим ожидания
        /// </summary>
        /// <param name="q">Очередь ожидания</param>
        /// <returns><para>false, если процесс не поставлен в очередь.</para> 
        /// <para>true, если процесс успешно поставлен в очередь</para></returns>
        public bool Wait(Queue q)
        {
            return Wait(q, OnNextScheduledEvent);
        }

        /// <summary>
        /// Постановка компонента в очередь и перевод его в режим ожидания
        /// с указанием событийного метода
        /// </summary>
        /// <param name="q">Очередь ожидания</param>
        /// <param name="nextProc">Событийный метод, который будет по умолчанию 
        /// установлен при следующем планировании события</param>
        /// <returns><para>false, если процесс не поставлен в очередь.</para> 
        /// <para>true, если процесс успешно поставлен в очередь</para></returns>
        public bool Wait(Queue q, EventProc nextProc)
        {
            if (q.MaxSize == 0 || q.Size < q.MaxSize)
            {
                Insert(q);
                OnNextScheduledEvent = nextProc;
                Passivate();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Инициирует запуск обслуживающего действия
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
            Insert(serv.Queue);
            return true;
        }

        /// <summary>
        /// Инициирует запуск обслуживающего действия
        /// </summary>
        /// <param name="serv">Обслуживающее действие</param>
        /// <param name="dt">Планируемая продолжительность действия</param>
        /// <param name="finish">Метод, выполняемый при завершении действия</param>
        /// <returns>true, если компонент был успешно поставлен в очередь 
        /// ожидания действия. false, если компонент не удалось поставить в очередь 
        /// по причине ее заполненности.</returns>
        public bool DoService(Service serv, double dt, EventProc finish)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            TimeLeft = dt;
            OnNextScheduledEvent = finish;
            serv.Agents.ActivateFirst();
            Insert(serv.Queue);
            return true;
        }

        /// <summary>
        /// Инициирует запуск обслуживающего действия.
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
            Insert(serv.Queue);
            return true;
        }

        /// <summary>
        /// Инициирует запуск обслуживающего действия.
        /// Продолжительность действия определяется делегатом, находящимся 
        /// в объекте действия. Если делегат отсутствует, порождается исключение.
        /// </summary>
        /// <param name="serv">Обслуживающее действие</param>
        /// <param name="finish">Метод, выполняемый при завершении действия</param>
        /// <returns>true, если компонент был успешно поставлен в очередь 
        /// ожидания действия. false, если компонент не удалось поставить в очередь 
        /// по причине ее заполненности.</returns>
        public bool DoService(Service serv, EventProc finish)
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
            OnNextScheduledEvent = finish;
            serv.Agents.ActivateFirst();
            Insert(serv.Queue);
            return true;
        }

        /// <summary>
        /// Инициирует запуск обслуживающего действия.
        /// Продолжительность действия определяется делегатом, находящимся 
        /// в объекте действия. Если делегат отсутствует, порождается исключение.
        /// </summary>
        /// <param name="serv">Обслуживающее действие</param>
        /// <param name="finish">Метод, выполняемый при завершении действия</param>
        /// <param name="start">Метод, выполняемый в момент начала обслуживания</param>
        /// <returns>true, если компонент был успешно поставлен в очередь 
        /// ожидания действия. false, если компонент не удалось поставить в очередь 
        /// по причине ее заполненности.</returns>
        public bool DoService(Service serv, EventProc finish, EventProc start)
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
            OnNextScheduledEvent = finish;
            serv.Agents.ActivateFirst();
            Insert(serv.Queue);
            return true;
        }
    }
}
