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
    internal class WorkPlace1 : Process
    {
        // Алгоритм процесса
        protected override void Execute()
        {
            FlowLine flPar = Parent as FlowLine;
            while (true)
            {
                // ОЖидать появления изделий в очереди
                while (flPar.Queue1.Empty())
                {
                    Passivate();
                }
                // Зафиксировать начало обслуживания
                flPar.Worker1Stat.Start(SimTime());
                // Извлечь первое изделие из очереди
                Piece pc = flPar.Queue1.First as Piece;
                pc.StartRunning();
                // Выполнить обслуживание
                Hold(FlowLine.RandWorker1.Exponential(Params.Worker1MeanTime));
                // Зафиксировать окончание обслуживания
                flPar.Worker1Stat.Finish(SimTime());
                // Если очередь к второму рабочему месту заполнена
                if (flPar.Queue2.Size >= Params.Queue2MaxSize)
                {
                    // Зафиксировать начало блокировки
                    flPar.Worker1Stat.StartBlock(SimTime());
                    // Ожидать появления места в очереди
                    while (flPar.Queue2.Size >= Params.Queue2MaxSize)
                    {
                        Passivate();
                    }
                    // Зафиксировать окончание блокировки
                    flPar.Worker1Stat.FinishBlock(SimTime());
                }
                // Поместить изделие в очередь ко второму рабочему месту
                pc.Insert(flPar.Queue2);
                // Запустить обслуживание на втором рабочем месте
                flPar.Worker2.Activate();
            }
        }
    }
}
