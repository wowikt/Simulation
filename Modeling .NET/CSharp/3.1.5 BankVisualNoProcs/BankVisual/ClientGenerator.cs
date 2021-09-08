using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace BankVisual
{
    /// <summary>
    /// Генератор клиентов банка
    /// </summary>
    internal class ClientGenerator : Process
    {
        /// <summary>
        /// Алгоритм работы генератора
        /// </summary>
        protected override void Execute()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            // Количество клиентов задано заранее
            for (int i = 0; i < Param.MaxClientCount; i++)
            {
                ClearFinished();
                // Создать клиента и запустить его
                (new Client(SimTime())).Insert(bsPar.Queue);
                bsPar.Cash.Activate();
                // ОЖидать перед созданием следующего
                Hold(BankSimulation.RandClient.Exponential(Param.MeanClientInterval));
            }
        }
    }
}
