using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
{
    /// <summary>
    /// Класс, имитирующий покупателя магазина
    /// </summary>
    internal class Customer : Component
    {
        /// <summary>
        /// Количество покупок
        /// </summary>
        internal int BuysCount;

        /// <summary>
        /// Начало работы клиента
        /// </summary>
        public override void StartEvent()
        {
            ShopSimulation ssPar = Parent as ShopSimulation;
            // Вычислить количество покупок
            BuysCount = ShopSimulation.RandCust.NextInt(Param.MinBuysCount, Param.MaxBuysCount);
            // Начать выбор покупок
            ssPar.InShopStat.Start(SimTime());
            // Запланировать окончание выбора покупок
            ReactivateDelay(ShopSimulation.RandCust.Uniform(Param.MinShoppingTime, 
                Param.MaxShoppingTime), ActionFinishedEvent);
        }

        /// <summary>
        /// Событие оконачния выбора покупок
        /// </summary>
        public void ActionFinishedEvent()
        {
            ShopSimulation ssPar = Parent as ShopSimulation;
            // Закончить выбор покупок
            ssPar.InShopStat.Finish(SimTime());
            // Сообщить кассиру о прибытии
            ssPar.Cash.Activate();
            // Встать в очередь ожидания
            Wait(ssPar.Queue);
        }
    }
}
