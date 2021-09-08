using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Bank
{
    /// <summary>
    /// Класс, имитирующий клиента в банке
    /// </summary>
    class Client : Process
    {
        /// <summary>
        /// Алгоритм работы клиента
        /// </summary>
        protected override void Execute()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            // Сообщить кассиру о прибытии
            bsPar.Cash.Activate();
            // Встать в очередь ожидания
            Wait(bsPar.Queue);
            // После окончания обслуживания завершить работу
            GoFinished();
        }
    }
}
