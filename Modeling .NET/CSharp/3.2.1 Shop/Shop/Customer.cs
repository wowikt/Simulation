using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
{
    /// <summary>
    /// Класс, имитирующий клиента в банке
    /// </summary>
    class Customer : Process
    {
        public int BuysCount;

        /// <summary>
        /// Алгоритм работы клиента
        /// </summary>
        protected override void Execute()
        {
            ShopSimulation ssPar = Parent as ShopSimulation;
            BuysCount = ShopSimulation.RandCust.NextInt(Param.MinBuysCount, Param.MaxBuysCount);
            ssPar.InShopStat.Start(SimTime());
            Hold(ShopSimulation.RandCust.Uniform(Param.MinShoppingTime, Param.MaxShoppingTime));
            ssPar.InShopStat.Finish(SimTime());
            // Сообщить кассиру о прибытии
            ssPar.Cash.Activate();
            // Встать в очередь ожидания
            Wait(ssPar.Queue);
            // После окончания обслуживания завершить работу
            GoFinished();
        }
    }
}
