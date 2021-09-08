using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс ServiceAgent представляет компонент, выполняющий обслуживающее действие
    /// </summary>
    internal class ServiceAgent : SchedulableComponent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="myService">Ссылка на действие, управляющее агентом</param>
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
        private IComponent Serviced;

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
        protected internal override void StartEvent()
        {
            // Проверить наличие очередного компонента в очереди
            if (MyService.Queue.Empty())
            {
                return;
            }
            // Извлечь первый компонент
            Serviced = MyService.Queue.First as IComponent;
            Serviced.StartRunning();
            // Агент занят обслуживанием
            Busy = true;
            // Если данному действию предшествовал другое, сообщить
            //   о начале действия для разблокировки возможно заблокированного действия
            if (MyService.PrevService != null)
            {
                MyService.PrevService.Agents.ActivateFirst();
            }
            // Уведомить компонент о начале обслуживания
            Serviced.OnActionStartingEvent(MyService);
            // Зафиксировать начало действия в статистике
            MyService.ServiceStat.Start(SimTime());
            // Запланировать окончание обслуживания
            ReactivateDelay(Serviced.TimeLeft, FinishedEvent);
            Serviced.TimeLeft = 0;
        }

        /// <summary>
        /// Событие окончания обслуживания
        /// </summary>
        protected internal void FinishedEvent()
        {
            // Зафиксировать в статистике окончание обслуживания
            MyService.ServiceStat.Finish(SimTime());
            // Если указано следующее действие, проверить возможность 
            //   помещения компонента в его очередь
            if (MyService.NextService != null && MyService.NextService.Queue.MaxSize > 0 && 
                MyService.NextService.Queue.Size >= MyService.NextService.Queue.MaxSize)
            {
                // Если поместить нельзя, начать блокировку
                MyService.ServiceStat.StartBlock(SimTime());
                NextSceduledEvent = BlockFinishEvent;
                return;
            }
            // По окончании обслуживания активировать обслуженный компонент
            Serviced.OnActionFinishedEvent(MyService);
            Serviced = null;
            // Агент свободен
            Busy = false;
            // Перейти к извлечению следующего компонента
            Reactivate(StartEvent);
        }

        /// <summary>
        /// Событие окончания блокировки
        /// </summary>
        public void BlockFinishEvent()
        {
            // Проверить возможность разблокировки
            if (MyService.NextService.Queue.Size >= MyService.NextService.Queue.MaxSize)
            {
                return;
            }
            // Разблокировать действие
            MyService.ServiceStat.FinishBlock(SimTime());
            // По окончании обслуживания активировать обслуженный компонент
            Serviced.OnActionFinishedEvent(MyService);
            Serviced = null;
            // Агент свободен
            Busy = false;
            // Перейти к извлечению следующего компонента
            Reactivate(StartEvent);
        }
    }
}
