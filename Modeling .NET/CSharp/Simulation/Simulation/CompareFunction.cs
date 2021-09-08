using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Делегат для определения функции сравнения, определяющей место вставки узла в список.
    /// Вставляемый узел помещается в список перед первым узлом, для которого функция дает результат true.
    /// </summary>
    /// <param name="a">Ссылка на вставляемый узел</param>
    /// <param name="b">Ссылка на сравниваемый узел списка</param>
    /// <returns>Результат сравнения</returns>
    public delegate bool CompareFunction(ILink a, ILink b);
}
