using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace TVControl
{
    /// <summary>
    /// Класс Adjuster представляет работу настройщика в имитации контроля телевизоров
    /// </summary>
    internal class Adjuster : Process
    {
        /// <summary>
        /// Алгоритм работы настройщика
        /// </summary>
        protected override void Execute()
        {
            TVControlSim tvcPar = Parent as TVControlSim;
            while (true)
            {
                // Ожидать появления телевизоров в очереди
                while (tvcPar.AdjustmentQueue.Empty())
                {
                    Passivate();
                }
                // Извлечь первый телевизор из очереди
                TVSet tv = tvcPar.AdjustmentQueue.First as TVSet;
                tv.Insert(tvcPar.Running);
                // Выполнить настройку
                tvcPar.AdjustmentStat.Start(SimTime());
                Hold(TVControlSim.RandTVSet.Uniform(Params.MinAdjustmentTime, 
                    Params.MaxAdjustmentTime));
                tvcPar.AdjustmentStat.Finish(SimTime());
                // Поместить настроенный телевизор в очередь на проверку
                tv.Insert(tvcPar.InspectionQueue);
                // Активировать свободного проверяющего
                tvcPar.Inspect.ActivateFirst();
            }
        }
    }
}
