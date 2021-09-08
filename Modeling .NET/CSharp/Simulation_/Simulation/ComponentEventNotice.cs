using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
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
        public ComponentEventNotice(double time, Component comp)
            : base(time)
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
                (Comp as ISchedulable).Event = null;
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
            Global.GlobalSimTime = EventTime;
            // Исполнить событийный метод компонента
            object res = Comp.Run();
            // Если уведомление не было перемещено (реактивировано), удалить его из календаря
            if (IsFirst)
            {
                Remove();
                // Если в компоненте не назначено другое уведомление о событии, отсоединить уведомление от компонента
                if ((Comp as ISchedulable).Event == this)
                {
                    (Comp as ISchedulable).Event = null;
                }
            }
            // Результат - ссылка на компонент-обработчик
            return res;
        }

        /// <summary>
        /// Отображение содержимого уведомления о событии в текстовом виде
        /// </summary>
        /// <returns>Класс процесса и время запланированного события</returns>
        public override string ToString()
        {
            return "Уведомление о событии компонента " + Comp.ToString() + " в " + EventTime.ToString();
        }
    }
}
