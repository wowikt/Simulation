using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Делегат, описывающий событийные методы компонентов с параметром-очередью
    /// </summary>
    public delegate void QueueEventProc(List queue);
}
