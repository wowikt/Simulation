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
    internal class Piece : Component
    {
        /// <summary>
        /// Событие прибытия изделия
        /// </summary>
        public override void StartEvent()
        {
            FlowLine flPar = Parent as FlowLine;
            // Запланировать прибытие следующего изделия
            (new Piece()).ActivateDelay(FlowLine.RandPiece.Exponential(Params.PieceMeanInterval));
            // Если в очереди к первому рабочему месту есть место
            if (flPar.Queue1.Size < Params.Queue1MaxSize)
            {
                // Запустить обслуживание
                flPar.Worker1.Activate();
                // Встать в очередь
                Wait(flPar.Queue1);
            }
            else
            {
                // Иначе - зафиксировать статистику по отказам
                flPar.BalksStat.AddData(SimTime());
            }
        }
    }
}
