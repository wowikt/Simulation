using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс ESimulationException - класс исключения имитации
    /// </summary>
    public class ESimulationException : Exception
    {
        /// <summary>
        /// Конструктор со строковым параметром
        /// </summary>
        /// <param name="message">Параметр - сообщение о причине исключения</param>
        public ESimulationException(string message)
            : base(message)
        {
        }
    }
}
