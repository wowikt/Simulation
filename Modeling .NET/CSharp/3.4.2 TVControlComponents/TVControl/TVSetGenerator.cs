using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace TVControl
{
    /// <summary>
    /// Класс TVSetGenerator обеспечивает поступление проверяемых телевизоров в систему
    /// </summary>
    class TVSetGenerator : Component
    {
        /// <summary>
        /// Основное событие генератора
        /// </summary>
        public override void StartEvent()
        {
            TVControlSim tvcPar = Parent as TVControlSim;
            // Создать новый телевизор
            TVSet tv = new TVSet();
            tv.StartingTime = SimTime();
            // Поместить его в очередь на проверку
            tv.Insert(tvcPar.InspectionQueue);
            // Активизировать первого свободного проверяющего
            tvcPar.Inspect.ActivateFirst();
            // Ожидать перед поступлением следующего телевизора
            ReactivateDelay(TVControlSim.RandTVSet.Uniform(Params.MinTVSetInterval, 
                Params.MaxTVSetInterval));
        }
    }
}
