using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
{
    /// <summary>
    /// Компонент, имитирущий работу кассира в банке
    /// </summary>
    internal class Cashman : Component
    {
        /// <summary>
        /// Текущий обслуживаемый покупатель
        /// </summary>
        internal Customer Cust;

        /// <summary>
        /// Начало работы с клиентом
        /// </summary>
        public override void StartEvent()
        {
            ShopSimulation ssPar = Parent as ShopSimulation;
            // Если очередь пуста, ничего не делать
            if (ssPar.Queue.Empty())
            {
                return;
            }
            // Извлечь первого покупателя из очереди
            Cust = ssPar.Queue.First as Customer;
            Cust.StartRunning();
            // Начать обслуживание
            ssPar.CashStat.Start(SimTime());
            // Запланировать событие окончания обслуживания
            ReactivateDelay(ShopSimulation.RandService.Erlang(Param.MeanTimePerBuy, 
                Cust.BuysCount), OnServiceFinishedEvent);
        }

        /// <summary>
        /// Событие окончания обслуживания
        /// </summary>
        public override void ServiceFinishedEvent()
        {
            ShopSimulation ssPar = Parent as ShopSimulation;
            // Закончить обслуживание
            ssPar.CashStat.Finish(SimTime());
            // Вычислить время пребывания в системе и учесть его в статистике и гистограмме
            double inBankTime = SimTime() - Cust.StartingTime;
            ssPar.TimeHist.AddData(inBankTime);
            ssPar.TimeStat.AddData(inBankTime);
            // Перейти к обслуживанию следующего клиента
            Reactivate(OnStartEvent);
        }
    }
}
