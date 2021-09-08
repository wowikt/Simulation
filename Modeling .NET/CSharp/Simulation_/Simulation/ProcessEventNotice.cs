using System;
using System.Collections.Generic;
using System.Linq;
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
        public ProcessEventNotice(double time, Process proc)
            : base(time)
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
                    (Proc as ISchedulable).Event = null;
                    Proc = null;
                }
                else if ((Proc as ISchedulable).Event == this)
                {
                    (Proc as ISchedulable).Event = null;
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
            Global.GlobalSimTime = EventTime;
            Global.CurrProc = Proc;
            object res = Proc.Resume();
            Global.CurrProc = null;
            return res;
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
}
