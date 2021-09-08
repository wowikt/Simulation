using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace BankVisual
{
    /// <summary>
    /// Класс, имитирующий клиента в банке
    /// </summary>
    class Client : Link
    {
        public Client(double startTime)
        {
            StartingTime = startTime;
        }

        public double StartingTime;
    }
}
