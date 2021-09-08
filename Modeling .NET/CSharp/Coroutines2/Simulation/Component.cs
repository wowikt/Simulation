using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Делегат, описывающий событийные методы компонентов
    /// </summary>
    public delegate void EventProc();

    /// <summary>
    /// Уведомление о событии, связанное с объектом-компонентом
    /// </summary>
    internal class ComponentEventNotice : EventNotice
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="time">Время события</param>
        /// <param name="comp">Компонент, обрабатывающий событие</param>
        public ComponentEventNotice(double time, Component comp) : base(time)
        {
            Comp = comp;
        }

        /// <summary>
        /// Компонент, обрабатывающий событие
        /// </summary>
        internal Component Comp;

        public override void Finish()
        {
            if (Comp != null)
            {
                Comp.Event = null;
            }
            base.Finish();
        }

        /// <summary>
        /// Обработка события
        /// </summary>
        /// <returns></returns>
        public override object RunEvent()
        {
            Global.CurrSim.CurrentSimTime = EventTime;
            // Исполнить событийный метод компонента
            object res = Comp.Run();
            // Если уведомление не было перемещено (реактивировано), удалить его из календаря
            if (IsFirst)
            {
                Remove();
                // Если в компоненте не назначено другое уведомление о событии, отсоединить уведомление от компонента
                if (Comp.Event == this)
                {
                    Comp.Event = null;
                }
            }
            // Результат - ссылка на компонент-обработчик
            return res;
        }
    }

    /// <summary>
    /// Класс Component является основой для создания компонентов - 
    /// объектов имитации, способных обрабатывать события
    /// </summary>
    public class Component : Link, ISchedulable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Component()
        {
            Parent = Global.CurrSim;
            OnStartEvent = StartEvent;
            OnServiceFinishedEvent = ServiceFinishedEvent;
            OnNextEvent = OnStartEvent;
            StartingTime = SimTime();
            FirstEvent = true;
        }

        /// <summary>
        /// Признак того, является ли следующее событие первым в жизненном цикле компонента.
        /// Непосредственно перед обработкой первого события устанавливается имитационное время 
        /// начала работы компонента.
        /// </summary>
        private bool FirstEvent;

        /// <summary>
        /// Ссылка на событийную процедуру для следующего события
        /// </summary>
        internal EventProc OnNextEvent;

        /// <summary>
        /// Ссылка на событийную процедуру для события начала работы компонента
        /// </summary>
        protected internal EventProc OnStartEvent;

        /// <summary>
        /// Ссылка на событийную процедуру для события окончания обслуживания
        /// </summary>
        protected internal EventProc OnServiceFinishedEvent;

        /// <summary>
        /// Ссылка на родительскую имитацию
        /// </summary>
        public SimProc Parent;

        /// <summary>
        /// Время создания объекта. Устанавливается автоматически в конструкторе. 
        /// При необходимости впоследствии может быть изменено.
        /// </summary>
        public double StartingTime;

        /// <summary>
        /// Ссылка на уведомление о событии
        /// </summary>
        public EventNotice Event
        {
            get;
            internal set;
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
            ActivateDelay(0, OnNextEvent);
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
        internal void ActivateAfter(EventNotice en)
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
            ActivateAfter(sch, OnNextEvent);
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
            OnNextEvent = nextProc;
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
            ActivateAt(t, OnNextEvent);
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
            OnNextEvent = nextProc;
            Event = new ComponentEventNotice(t, this);
            Event.Insert(Parent.Calendar);
        }

        /// <summary>
        /// Создает запись уведомления об активации компонента непосредственно перед указанной записью
        /// </summary>
        /// <param name="en">Запись уведомления о событии, перед которой следует поместить запись нового события</param>
        internal void ActivateBefore(EventNotice en)
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
            ActivateBefore(sch, OnNextEvent);
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
            OnNextEvent = nextProc;
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
            ActivateAt(SimTime() + dt, OnNextEvent);
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
            ActivatePriorAt(t, OnNextEvent);
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
            OnNextEvent = nextProc;
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
            ActivatePriorAt(SimTime() + dt, OnNextEvent);
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
        /// Событийный метод окончания обслуживания. 
        /// <para>При планировании обслуживающего действия ссылка на этот метод 
        /// помечается как очередной, если не указан иной метод.</para>
        /// <para>Если в жизненном цикле компонента имеется только одно 
        /// обслуживающее действие, следует переопределить только этот метод.</para>
        /// <para>Если алгоритм работы компонента подразумевает несколько 
        /// обслуживающих действий, для окончания каждого из них должен быть 
        /// предусмотрен метод с аналогичной сигнатурой</para>
        /// </summary>
        public virtual void ServiceFinishedEvent()
        {
        }

        /// <summary>
        /// Событийный метод начала работы процесса. 
        /// <para>При создании компонента ссылка на этот метод помечается как очередной.</para>
        /// <para>Если компонент обрабатывает единственное событие, другие событийные методы не нужны</para>
        /// <para>Если алгоритм работы компонента предусматривает несколько событий,
        /// данный метод будет соответствовать первому событию. Для обработки других событий 
        /// в классе компонента необходимо предусмотреть соответствующие методы
        /// с аналогичной сигнатурой</para>
        /// </summary>
        public virtual void StartEvent()
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
            ReactivateDelay(0, OnNextEvent);
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
            ReactivateAfter(sch, OnNextEvent);
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
            OnNextEvent = nextProc;
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
        internal void ReactivateAfter(EventNotice en)
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
            ReactivateAt(t, OnNextEvent);
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
        internal void ReactivateBefore(EventNotice en)
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
            ReactivateBefore(sch, OnNextEvent);
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
            OnNextEvent = nextProc;
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
            ReactivateAt(SimTime() + dt, OnNextEvent);
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
            ReactivatePriorAt(t, OnNextEvent);
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
            ReactivatePriorAt(SimTime() + dt, OnNextEvent);
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
            OnNextEvent();
            return new ContinueSim();
        }

        /// <summary>
        /// Возвращает текущее имитационное время. 
        /// Обращается к родительскому процесу имитации.
        /// </summary>
        /// <returns>Текущее имитационное время</returns>
        public double SimTime()
        {
            return Parent.SimTime();
        }

        /// <summary>
        /// Включение компонента в список свободных. 
        /// Используется также для извлечения компонента из очереди ожидания
        /// <para>Представлен для совместимости с классом Process 
        /// с целью более простого перехода от процессов к компонентам. 
        /// Реально в этом методе нет необходимости, поскольку в .NET 
        /// объекты, на которые нет ссылок, удаляются автоматически при сборке мусора</para>
        /// <para>Если в программе не используются процессы, а есть только компоненты, 
        /// этот метод, а также ClearFinished() и GoFinished(), не нужны</para>
        /// </summary>
        public void StartRunning()
        {
            Insert(Parent.Running);
        }

        /// <summary>
        /// Постановка компонента в очередь и перевод его в режим ожидания
        /// </summary>
        /// <param name="q">Очередь ожидания</param>
        public void Wait(List q)
        {
            Wait(q, OnNextEvent);
        }

        /// <summary>
        /// Постановка компонента в очередь и перевод его в режим ожидания
        /// с указанием событийного метода
        /// </summary>
        /// <param name="q">Очередь ожидания</param>
        /// <param name="nextProc">Событийный метод, который будет по умолчанию 
        /// установлен при следующем планировании события</param>
        public void Wait(List q, EventProc nextProc)
        {
            Insert(q);
            OnNextEvent = nextProc;
            Passivate();
        }
    }

    /// <summary>
    /// Компонент-визуализатор. По истечении заданного интервала времени передает управление главному потоку программы
    /// </summary>
    internal class Visualizer : Component
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="interval">Интервал срабатывания визуализатора</param>
        internal Visualizer(double interval)
        {
            Interval = interval;
        }

        /// <summary>
        /// Промежуток имитационного времени между срабатываниями визуализатора
        /// </summary>
        internal double Interval;

        /// <summary>
        /// Обработка события визуализатора.
        /// <para>Активирует новое событие с заданным интервалом времени и возвращает пустой указатель</para>
        /// </summary>
        /// <returns>null</returns>
        internal override object Run()
        {
            ActivateDelay(Interval);
            return new StopSim();
        }
    }

    /// <summary>
    /// Класс Collector определяет компонент-сборщик завершенных процессов.
    /// Завершаемый процесс в конце своей работы должен встать в список
    /// завершенных процессов, выполнив метод GoFinished().
    /// Какой-либо из процессов имитации должен периодически вызывать метод
    /// ClearFinished(), который активирует данный объект.
    /// Одноименный метод класса Component не активирует данный компонент,
    /// а очищает список завершенных процессов непосредственно
    /// </summary>
    internal class Collector : Component
    {
        /// <summary>
        /// Событийный метод очистки списка завершенных процессов
        /// </summary>
        public override void StartEvent()
        {
            ClearFinished();
        }
    }
}
