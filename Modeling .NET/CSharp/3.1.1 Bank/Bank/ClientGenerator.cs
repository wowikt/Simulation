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
    class ClientGenerator : Process
    {
        /// <summary>
        /// Алгоритм работы генератора
        /// </summary>
        protected override void Execute()
        {
            // Количество клиентов задано заранее
            for (int i = 0; i < Param.MaxClientCount; i++)
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
