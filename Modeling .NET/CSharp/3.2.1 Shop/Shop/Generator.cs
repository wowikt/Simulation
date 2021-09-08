using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
{
    /// <summary>
    /// Генератор клиентов банка
    /// </summary>
    class Generator : Process
    {
        /// <summary>
        /// Алгоритм работы генератора
        /// </summary>
        protected override void Execute()
        {
            // Количество клиентов задано заранее
            while (true)
            {
                ClearFinished();
                // Создать клиента и запустить его
                (new Customer()).Activate();
                // ОЖидать перед созданием следующего
                Hold(ShopSimulation.RandCust.Exponential(Param.MeanCustInterval));
            }
        }
    }
}
