using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Делегат, описывающий событийные методы компонентов с параметром-обслуживающим действием
    /// </summary>
    public delegate void ServiceEventProc(IService queue);
}
