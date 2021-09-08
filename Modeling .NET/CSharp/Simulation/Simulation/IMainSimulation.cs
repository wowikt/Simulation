using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Интерфейс IMainSimulation определяет основные свойства и методы процессов и компонентов имитации
    /// </summary>
    public interface IMainSimulation : ISchedulable
    {
        /// <summary>
        /// Календарь событий имитации
        /// </summary>
        List Calendar
        {
            get;
            set;
        }

        /// <summary>
        /// Текущее имитационное время
        /// </summary>
        double CurrentSimTime
        {
            get;
            set;
        }

        /// <summary>
        /// Шаг времени визуализации
        /// </summary>
        double VisualInterval
        {
            get;
            set;
        }

        /// <summary>
        /// Список завершенных процессов и иных объектов, для которых требуется
        /// явное удаление посредством метода Finish()
        /// </summary>
        List Finished
        {
            get;
        }

        /// <summary>
        /// Список свободных процессов и иных объектов, для которых требуется
        /// явное удаление посредством метода Finish()
        /// </summary>
        List Running
        {
            get;
        }

        /// <summary>
        /// Признак завершения имитации
        /// </summary>
        bool Terminated
        {
            get;
        }

        /// <summary>
        /// Компонент, удаляющий завершенные процессы
        /// </summary>
        Collector Collect
        {
            get;
        }

        /// <summary>
        /// Очистка всех объектов статистики имитации
        /// </summary>
        void ClearStat();

        /// <summary>
        /// Удаление объекта имитации
        /// </summary>
        void Finish();

        /// <summary>
        /// Возвращает текущее имитационное время
        /// </summary>
        /// <returns>Текущее имитационное время</returns>
        double SimTime();

        /// <summary>
        /// Запуск или возобновление прогона имитации
        /// </summary>
        void Start();

        /// <summary>
        /// Коррекция всех объектов статистики к текущему имитационному времени
        /// </summary>
        void StopStat();
    }
}
