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
    /// Одноименный метод класса SchedulableComponent не активирует данный компонент,
    /// а очищает список завершенных процессов непосредственно
    /// </summary>
    public class Collector : SchedulableComponent
    {
        /// <summary>
        /// Событийный метод очистки списка завершенных процессов
        /// </summary>
        protected internal override void StartEvent()
        {
            ClearFinished();
        }
    }
}
