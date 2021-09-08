using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace TVControl
{
    /// <summary>
    /// Класс Inspector моделирует работу проверяющего в модели контроля телевизоров
    /// </summary>
    internal class Inspector : Process
    {
        /// <summary>
        /// Алгоритм проверки
        /// </summary>
        protected override void Execute()
        {
            TVControlSim tvcPar = Parent as TVControlSim;
            while (true)
            {
                // ОЖидать появления телевизоров
                while (tvcPar.InspectionQueue.Empty())
                {
                    Passivate();
                }
                // Извлечь первый телевизор из очереди
                TVSet tv = tvcPar.InspectionQueue.First as TVSet;
                tv.Insert(tvcPar.Running);
                tvcPar.InspectorsStat.Start(SimTime());
                // Выполнить проверку
                Hold(TVControlSim.RandInspector.Uniform(Params.MinInspectionTime, 
                    Params.MaxInspectionTime));
                tvcPar.InspectorsStat.Finish(SimTime());
                // Если телевизор исправен
                if (TVControlSim.RandInspector.Draw(Params.NoAdjustmentProb))
                {
                    // Зафиксировать статистику по времени пребывания в системе
                    tvcPar.TimeInSystemStat.AddData(SimTime() - tv.StartingTime);
                }
                else
                {
                    // Поместить телевизор в очередь к настройщику
                    tv.Insert(tvcPar.AdjustmentQueue);
                    // Активировать настройщика
                    tvcPar.Adjust.Activate();
                }
            }
        }
    }
}
