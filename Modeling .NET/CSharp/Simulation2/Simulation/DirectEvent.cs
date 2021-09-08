using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс DirectEvent позволяет прямо планировать события, связанные с любыми объектами.
    /// События, спланированные с помощью этого класса, не предназначены для
    /// последующих манипуляций - удаления, реактивации и т. п.
    /// </summary>
    public class DirectEvent : SchedulableEventNotice, IActivatable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="proc">Метод обработки планируемого события</param>
        public DirectEvent(EventProc proc) : base(0)
        {
            Proc = proc;
        }

        /// <summary>
        /// Событийный метод
        /// </summary>
        internal EventProc Proc;

        /// <summary>
        /// Помещает запись уведомления об активации процесса непосредственно после текущего.
        /// <para>Если процесс находится в активном или приостановленном состоянии, ничего не делает</para>
        /// </summary>
        public void Activate()
        {
            ActivateDelay(0);
        }

        /// <summary>
        /// Помещает запись уведомления о событии непосредственно после указанной записи
        /// </summary>
        /// <param name="en">Запись, после которой следует активировать событие</param>
        internal void ActivateAfter(SchedulableEventNotice en)
        {
            EventTime = en.EventTime;
            InsertAfter(en);
        }

        /// <summary>
        /// Помещает запись уведомления о событии непосредственно после записи указанного процесса
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="act">Процесс, после которого следует активировать событие</param>
        public void ActivateAfter(ISchedulable act)
        {
            if (act.Idle)
            {
                throw new ESimulationException("Нельзя выполнить ActivateAfter(p) после пассивного процесса");
            }
            ActivateAfter(act.Event);
        }

        /// <summary>
        /// Помещает запись уведомления о событии в указанное время
        /// </summary>
        /// <param name="t">Имитационное время активации события</param>
        public void ActivateAt(double t)
        {
            if (t < SimTime())
            {
                t = SimTime();
            }
            EventTime = t;
            Insert(Global.CurrSim.Calendar);
        }

        /// <summary>
        /// Помещает запись уведомления о событии непосредственно перед указанной записью
        /// </summary>
        /// <param name="en">Запись, перед которой следует активировать событие</param>
        internal void ActivateBefore(SchedulableEventNotice en)
        {
            EventTime = en.EventTime;
            InsertBefore(en);
        }

        /// <summary>
        /// Помещает запись уведомления о событии непосредственно перед записью указанного процесса
        /// <para>Если процесс-параметр находится в пассивном или завершенном состоянии, порождается исключение</para>
        /// </summary>
        /// <param name="act">Процесс, перед которым следует активировать событие</param>
        public void ActivateBefore(ISchedulable act)
        {
            if (act.Idle)
            {
                throw new ESimulationException("Нельзя выполнить ActivateAfter(p) после пассивного процесса");
            }
            ActivateBefore(act.Event);
        }

        /// <summary>
        /// Помещает запись уведомления о событии с указанной задержкой времени
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        public void ActivateDelay(double dt)
        {
            ActivateAt(SimTime() + dt);
        }

        /// <summary>
        /// Помещает запись уведомления о событии в указанное время с приоритетом
        /// </summary>
        /// <param name="t">Имитационное время активации события</param>
        public void ActivatePriorAt(double t)
        {
            if (t < SimTime())
            {
                t = SimTime();
            }
            EventTime = t;
            InsertPrior(Global.CurrSim.Calendar);
        }

        /// <summary>
        /// Помещает запись уведомления о событии с указанной задержкой времени с приоритетом
        /// </summary>
        /// <param name="dt">Задержка относительно текущего имитационного времени</param>
        public void ActivatePriorDelay(double dt)
        {
            ActivatePriorAt(SimTime() + dt);
        }

        /// <summary>
        /// ОБработка события
        /// </summary>
        /// <returns>Объект класса ContinueSim</returns>
        public override object RunEvent()
        {
            Global.CurrSim.CurrentSimTime = EventTime;
            Proc();
            Remove();
            return new ContinueSim();
        }

        /// <summary>
        /// Возвращает текущее имитацинное время
        /// </summary>
        /// <returns>Имитационное время</returns>
        internal double SimTime()
        {
            return Global.SimTime();
        }
    }
}
