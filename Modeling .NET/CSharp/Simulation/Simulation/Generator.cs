using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс Generator&lt;T&gt; представляет компонент, который регулярно создает
    /// компоненты имитации
    /// </summary>
    /// <typeparam name="T">Тип создаваемых компонентов. 
    /// Должен иметь конструктор без параметров</typeparam>
    public class Generator<T> : Node
        where T : Component
    {
    }
}
