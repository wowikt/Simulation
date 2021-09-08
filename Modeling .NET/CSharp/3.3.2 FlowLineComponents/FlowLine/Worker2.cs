using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace FlowLine
{
    /// <summary>
    /// Класс WorkPlace2 имитирует обслуживание изделия на втором рабочем месте
    /// </summary>
    internal class WorkPlace2 : Component
    {
        /// <summary>
        /// Текущее обрабатываемое изделие
        /// </summary>
        Piece pc;

        /// <summary>
        /// Событие начала работы
        /// </summary>
        public override void StartEvent()
        {
            FlowLine flPar = Parent as FlowLine;
            // ОЖидать появления изделий в очереди
            if (flPar.Queue2.Empty())
            {
                return;
            }
            // Зафиксировать начало обслуживания
            flPar.Worker2Stat.Start(SimTime());
            // Извлечь изделие из очереди
            pc = flPar.Queue2.First as Piece;
            pc.StartRunning();
            // Уведомить первого рабочего об освобождении места в очереди
            flPar.Worker1.Activate();
            // Выполнить обслуживание
            ReactivateDelay(FlowLine.RandWorker2.Exponential(Params.Worker2MeanTime), ServiceFinishedEvent);
        }

        /// <summary>
        /// Событие окончания обслуживания
        /// </summary>
        public override void ServiceFinishedEvent()
        {
            FlowLine flPar = Parent as FlowLine;
            // Собрать статистику по времени пребвания изделия в системе
            flPar.TimeInSystemStat.AddData(SimTime() - pc.StartingTime);
            flPar.TimeHist.AddData(SimTime() - pc.StartingTime);
            // Зафиксировать окончание обслуживания
            flPar.Worker2Stat.Finish(SimTime());
            // Перейти к извлечению следующего изделия
            Reactivate(StartEvent);
        }
    }
}
