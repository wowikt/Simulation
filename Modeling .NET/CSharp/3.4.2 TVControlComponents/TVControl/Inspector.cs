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
    internal class Inspector : Component
    {
        /// <summary>
        /// Текущий проверяемый телевизор
        /// </summary>
        private TVSet tv;

        /// <summary>
        /// Событие начала проверки
        /// </summary>
        public override void StartEvent()
        {
            TVControlSim tvcPar = Parent as TVControlSim;
            // ОЖидать появления телевизоров
            if (tvcPar.InspectionQueue.Empty())
            {
                return;
            }
            // Извлечь первый телевизор из очереди
            tv = tvcPar.InspectionQueue.First as TVSet;
            tv.Insert(tvcPar.Running);
            // Начать проверку
            tvcPar.InspectorsStat.Start(SimTime());
            ReactivateDelay(TVControlSim.RandInspector.Uniform(Params.MinInspectionTime, 
                Params.MaxInspectionTime), ServiceFinishedEvent);
        }

        /// <summary>
        /// Событие окончания проверки
        /// </summary>
        public override void ServiceFinishedEvent()
        {
            TVControlSim tvcPar = Parent as TVControlSim;
            // Закончить проверку
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
            // Перейти к следующему телевизору
            tv = null;
            Reactivate(StartEvent);
        }
    }
}
