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
    internal class WorkPlace2 : Process
    {
        /// <summary>
        /// Алгоритм обслуживания
        /// </summary>
        protected override void Execute()
        {
            FlowLine flPar = Parent as FlowLine;
            while (true)
            {
                // ОЖидать появления изделий в очереди
                while (flPar.Queue2.Empty())
                {
                    Passivate();
                }
                // Зафиксировать начало обслуживания
                flPar.Worker2Stat.Start(SimTime());
                // Извлечь изделие из очереди
                Piece pc = flPar.Queue2.First as Piece;
                pc.StartRunning();
                // Уведомить первого рабочего об освобождении места в очереди
                flPar.Worker1.Activate();
                // Выполнить обслуживание
                Hold(FlowLine.RandWorker2.Exponential(Params.Worker2MeanTime));
                // Собрать статистику по времени пребвания изделия в системе
                flPar.TimeInSystemStat.AddData(SimTime() - pc.StartingTime);
                flPar.TimeHist.AddData(SimTime() - pc.StartingTime);
                // Зафиксировать окончание обслуживания
                flPar.Worker2Stat.Finish(SimTime());
            }
        }
    }
}
