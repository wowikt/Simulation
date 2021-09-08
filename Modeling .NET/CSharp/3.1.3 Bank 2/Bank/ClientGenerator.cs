using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Bank
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
            Hold(Param.FirstClientArrival);
            // Количество клиентов задано заранее
            while (true)
            {
                ClearFinished();
                // Создать клиента и запустить его
                (new Client()).Activate();
                // ОЖидать перед созданием следующего
                Hold(BankSimulation.RandClient.Exponential(Param.MeanClientInterval));
            }
        }
    }
}
