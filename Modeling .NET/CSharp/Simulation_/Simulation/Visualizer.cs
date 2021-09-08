using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Компонент-визуализатор. По истечении заданного интервала времени 
    /// передает управление главному потоку программы
    /// </summary>
    internal class Visualizer : Component
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="interval">Интервал срабатывания визуализатора</param>
        internal Visualizer(double interval)
        {
            Interval = interval;
        }

        /// <summary>
        /// Промежуток имитационного времени между срабатываниями визуализатора
        /// </summary>
        internal double Interval;

        /// <summary>
        /// Обработка события визуализатора.
        /// <para>Планирует новое событие с заданным интервалом времени и возвращает 
        /// объект класса StopSim, что трактуется как необходимость приостановки имитации</para>
        /// </summary>
        /// <returns>Объект класса StopSim, указывающий на необходимость 
        /// приостановки имитации</returns>
        internal override object Run()
        {
            Parent.StopStat();
            ActivateDelay(Interval);
            return new StopSim();
        }
    }
}
