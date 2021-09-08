using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace TVControl
{
    /// <summary>
    /// Класс TVSet представляет проверяемый телевизор
    /// </summary>
    internal class TVSet : Link
    {
        /// <summary>
        /// Время появления телевизора в системе
        /// </summary>
        internal double StartingTime;
    }
}
