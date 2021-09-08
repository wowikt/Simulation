using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
{
    /// <summary>
    /// Процесс, имитирущий работу кассира в банке
    /// </summary>
    class Cashman : Process
    {
        /// <summary>
        /// Алгоритм работы
        /// </summary>
        protected override void Execute()
        {
            ShopSimulation ssPar = Parent as ShopSimulation;
            while (true)
            {
                // Ожидать появления покупателя в очереди
                while (ssPar.Queue.Empty())
                {
                    Passivate();
                }
                // Извлечь первого покупателя из очереди
                Customer cust = ssPar.Queue.First as Customer;
                cust.StartRunning();
                // Выполнить обслуживание
                ssPar.CashStat.Start(SimTime());
                Hold(ShopSimulation.RandService.Erlang(Param.MeanTimePerBuy, cust.BuysCount));
                ssPar.CashStat.Finish(SimTime());
                // Вычислить время пребывания в системе и учесть его в статистике и гистограмме
                double inBankTime = SimTime() - cust.StartingTime;
                ssPar.TimeHist.AddData(inBankTime);
                ssPar.TimeStat.AddData(inBankTime);
                // Активировать покупателя для завершения им работы
                cust.Activate();
            }
        }
    }
}
