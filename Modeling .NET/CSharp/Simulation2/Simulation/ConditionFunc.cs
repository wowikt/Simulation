using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Делегат, представляющий предикат компонента
    /// </summary>
    /// <param name="comp">Компонент, для которого проверяется условие</param>
    /// <returns>Результат проверки</returns>
    public delegate bool ConditionFunc(IComponent comp);
}
