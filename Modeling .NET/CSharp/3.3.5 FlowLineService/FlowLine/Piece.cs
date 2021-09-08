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
    internal class Piece : Process
    {
        /// <summary>
        /// Событие прибытия изделия
        /// </summary>
        protected override void Execute()
        {
            FlowLineSim flPar = Parent as FlowLineSim;
            // Запланировать прибытие следующего изделия
            (new Piece()).ActivateDelay(FlowLineSim.RandPiece.Exponential(Params.PieceMeanInterval));
            // Попытаться выполнить первое действие
            if (!DoService(flPar.Worker1))
            {
                // Если неудачно, зафиксировать статистику по отказам
                flPar.BalksStat.AddData(SimTime());
            }
            else
            {
                // Выполнить второе действие
                DoService(flPar.Worker2);
                // Собрать статистику по времени пребывания изделия в системе
                flPar.TimeInSystemStat.AddData(SimTime() - StartingTime);
                flPar.TimeHist.AddData(SimTime() - StartingTime);
            }
        }
    }
}
