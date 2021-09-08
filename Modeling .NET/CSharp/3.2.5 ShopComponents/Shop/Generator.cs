using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
{
    /// <summary>
    /// Генератор покупателей магазина
    /// </summary>
    internal class Generator : Component
    {
        /// <summary>
        /// Алгоритм работы генератора
        /// </summary>
        public override void StartEvent()
        {
            // Создать покупателя и запустить его
            (new Customer()).Activate();
            // Ожидать перед созданием следующего
            ReactivateDelay(ShopSimulation.RandCust.Exponential(Param.MeanCustInterval));
        }
    }
}
