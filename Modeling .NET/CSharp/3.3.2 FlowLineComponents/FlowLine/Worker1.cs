using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace FlowLine
{
    /// <summary>
    /// Класс WorkPlace1 имитирует обслуживание на первом рабочем месте
    /// </summary>
    internal class WorkPlace1 : Component
    {
        /// <summary>
        /// Текущее обслуживаемое изделие
        /// </summary>
        private Piece pc;

        /// <summary>
        /// Событие начала работы
        /// </summary>
        public override void StartEvent()
        {
            FlowLine flPar = Parent as FlowLine;
            // ОЖидать появления изделий в очереди
            if (flPar.Queue1.Empty())
            {
                return;
            }
            // Зафиксировать начало обслуживания
            flPar.Worker1Stat.Start(SimTime());
            // Извлечь первое изделие из очереди
            pc = flPar.Queue1.First as Piece;
            pc.StartRunning();
            // Выполнить обслуживание
            ReactivateDelay(FlowLine.RandWorker1.Exponential(Params.Worker1MeanTime), ServiceFinishedEvent);
        }

        /// <summary>
        /// Событие окончания обслуживания
        /// </summary>
        public override void ServiceFinishedEvent()
        {
            FlowLine flPar = Parent as FlowLine;
            // Зафиксировать окончание обслуживания
            flPar.Worker1Stat.Finish(SimTime());
            // Если очередь к второму рабочему месту заполнена
            if (flPar.Queue2.Size >= Params.Queue2MaxSize)
            {
                // Зафиксировать начало блокировки
                flPar.Worker1Stat.StartBlock(SimTime());
                NextEvent = BlockFinishEvent;
                return;
            }
            // Поместить изделие в очередь ко второму рабочему месту
            pc.Insert(flPar.Queue2);
            // Запустить обслуживание на втором рабочем месте
            flPar.Worker2.Activate();
            // Перейти к извлечению следующего изделия
            Reactivate(StartEvent);
        }

        /// <summary>
        /// Событие окончания блокировки
        /// </summary>
        public void BlockFinishEvent()
        {
            FlowLine flPar = Parent as FlowLine;
            // Ожидать появления места в очереди
            if (flPar.Queue2.Size >= Params.Queue2MaxSize)
            {
                return;
            }
            // Зафиксировать окончание блокировки
            flPar.Worker1Stat.FinishBlock(SimTime());
            // Поместить изделие в очередь ко второму рабочему месту
            pc.Insert(flPar.Queue2);
            // Запустить обслуживание на втором рабочем месте
            flPar.Worker2.Activate();
            Reactivate(StartEvent);
        }
    }
}
