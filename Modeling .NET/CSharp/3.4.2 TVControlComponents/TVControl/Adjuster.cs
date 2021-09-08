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
    internal class Adjuster : Component
    {
        /// <summary>
        /// Текущий настраиваемый телевизор
        /// </summary>
        private TVSet tv;

        /// <summary>
        /// Событие начала настройки
        /// </summary>
        public override void StartEvent()
        {
            TVControlSim tvcPar = Parent as TVControlSim;
            // Ожидать появления телевизоров в очереди
            if (tvcPar.AdjustmentQueue.Empty())
            {
                return;
            }
            // Извлечь первый телевизор из очереди
            tv = tvcPar.AdjustmentQueue.First as TVSet;
            tv.Insert(tvcPar.Running);
            // Начать настройку
            tvcPar.AdjustmentStat.Start(SimTime());
            ReactivateDelay(TVControlSim.RandTVSet.Uniform(Params.MinAdjustmentTime, 
                Params.MaxAdjustmentTime), ServiceFinishedEvent);
        }

        /// <summary>
        /// Событие окончания настройки
        /// </summary>
        public override void ServiceFinishedEvent()
        {
            TVControlSim tvcPar = Parent as TVControlSim;
            // Завершить настройку
            tvcPar.AdjustmentStat.Finish(SimTime());
            // Поместить настроенный телевизор в очередь на проверку
            tv.Insert(tvcPar.InspectionQueue);
            // Активировать свободного проверяющего
            tvcPar.Inspect.ActivateFirst();
            // Перейти к настройке следующего телевизора
            tv = null;
            Reactivate(StartEvent);
        }
    }
}
