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
    internal class Client : Process
    {
        public Client()
        {
            Inserted = false;
        }

        public Client(bool inserted)
        {
            Inserted = inserted;
        }

        internal bool Inserted;

        /// <summary>
        /// Алгоритм работы клиента
        /// </summary>
        protected override void Execute()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            // Если клиент был внедрен в систему принудительно
            if (Inserted)
            {
                // Сразу ожидать обслуживания
                Passivate();
            }
            else if (bsPar.Queue.Size < Param.MaxQueueLength)
            {
                // Активировать кассиров
                bsPar.Cash.ActivateFirst();
                // Ждать обслуживания
                Wait(bsPar.Queue);
            }
            else
            {
                // Очередь заполнена - увеличить счетчик отказов
                bsPar.NotServiced++;
            }
            // Встать в очередь завершенных процессов
            GoFinished();
        }
    }
}
