using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс ServiceAgent представляет компоненты, выполняющий обслуживающее действие
    /// </summary>
    internal class ServiceAgent : Component
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="myService">Сслыка на действие, управляющее агентом</param>
        public ServiceAgent(Service myService)
        {
            MyService = myService;
        }

        /// <summary>
        /// Ссылка на действие, управляющее агентом
        /// </summary>
        internal Service MyService;

        /// <summary>
        /// Обслуживаемый компонент
        /// </summary>
        private ISchedulable Serviced;

        /// <summary>
        /// Признак занятости обслуживающего действия
        /// </summary>
        public bool Busy
        {
            get;
            internal set;
        }

        /// <summary>
        /// Событие начала работы
        /// </summary>
        public override void StartEvent()
        {
            if (MyService.Queue.Empty())
            {
                return;
            }
            Serviced = MyService.Queue.First as ISchedulable;
            Serviced.StartRunning();
            Busy = true;
            if (MyService.PrevService != null)
            {
                MyService.PrevService.Agents.ActivateFirst();
            }
            Serviced.OnServiceStartingEvent();
            MyService.ServiceStat.Start(SimTime());
            ReactivateDelay(Serviced.TimeLeft, ServiceFinishedEvent);
            Serviced.TimeLeft = 0;
        }

        /// <summary>
        /// Событие окончания обслуживания
        /// </summary>
        public override void ServiceFinishedEvent()
        {
            MyService.ServiceStat.Finish(SimTime());
            if (MyService.NextService != null && MyService.NextService.Queue.MaxSize > 0 && 
                MyService.NextService.Queue.Size >= MyService.NextService.Queue.MaxSize)
            {
                MyService.ServiceStat.StartBlock(SimTime());
                NextEvent = BlockFinishEvent;
                return;
            }
            Serviced.Activate();
            Serviced = null;
            Busy = false;
            Reactivate(StartEvent);
        }

        /// <summary>
        /// Событие окончания блокировки
        /// </summary>
        public void BlockFinishEvent()
        {
            if (MyService.NextService.Queue.Size >= MyService.NextService.Queue.MaxSize)
            {
                return;
            }
            MyService.ServiceStat.FinishBlock(SimTime());
            Serviced.Activate();
            Serviced = null;
            Busy = false;
            Reactivate(StartEvent);
        }
    }
}
