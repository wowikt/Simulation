using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс Collector определяет компонент-сборщик завершенных процессов.
    /// Завершаемый процесс в конце своей работы должен встать в список
    /// завершенных процессов, выполнив метод GoFinished().
    /// Какой-либо из процессов имитации должен периодически вызывать метод
    /// ClearFinished(), который активирует данный объект.
    /// Одноименный метод класса Component не активирует данный компонент,
    /// а очищает список завершенных процессов непосредственно
    /// </summary>
    public class Collector : Component
    {
        /// <summary>
        /// Событийный метод очистки списка завершенных процессов
        /// </summary>
        public override void StartEvent()
        {
            ClearFinished();
        }
    }
}
