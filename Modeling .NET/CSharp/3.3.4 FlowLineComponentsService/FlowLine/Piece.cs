using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace FlowLine
{
    /// <summary>
    /// Класс Piece моделирует цикл существования изделия
    /// </summary>
    internal class Piece : SchedulableComponent
    {
        /// <summary>
        /// Событие прибытия изделия
        /// </summary>
        protected override void StartEvent()
        {
            FlowLineSim flPar = Parent as FlowLineSim;
            // Запланировать прибытие следующего изделия
            (new Piece()).ActivateDelay(FlowLineSim.RandPiece.Exponential(Params.PieceMeanInterval));
            // Попытаться выполнить первое действие
            if (!DoService(flPar.Worker1, FinishEvent))
            {
                // Если попытка неудачна, зафиксировать статистику по отказам
                flPar.BalksStat.AddData(SimTime());
            }
        }

        protected override void ActionFinishedEvent(IAction act)
        {
            FlowLineSim flPar = Parent as FlowLineSim;
            if (act == flPar.Worker1)
            {
                // Выполнить второе действие
                DoService(flPar.Worker2);
            }
            else if (act == flPar.Worker2)
            {
                // Собрать статистику по времени пребывания изделия в системе
                flPar.TimeInSystemStat.AddData(SimTime() - StartingTime);
                flPar.TimeHist.AddData(SimTime() - StartingTime);
            }
        }
    }
}
