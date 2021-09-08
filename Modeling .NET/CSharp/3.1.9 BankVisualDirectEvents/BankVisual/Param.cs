using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankVisual
{
    /// <summary>
    /// Параметры работы имитации
    /// </summary>
    class Param
    {
        /// <summary>
        /// Нижняя граница гистограммы
        /// </summary>
        static public double HistMin = 2;

        /// <summary>
        /// Размер шага гистограммы
        /// </summary>
        static public double HistStep = 1;

        /// <summary>
        /// Количество шагов гистограммы
        /// </summary>
        static public int HistStepCount = 20;

        /// <summary>
        /// Максимальное время обслуживания на кассе
        /// </summary>
        static public double MaxCashTime = 6;

        /// <summary>
        /// Количество клиентов для обслуживания
        /// </summary>
        static public int MaxClientCount = 100;

        /// <summary>
        /// Средний интервал между прибытием клиентов
        /// </summary>
        static public double MeanClientInterval = 5;

        /// <summary>
        /// Минимальное время обслуживания на кассе
        /// </summary>
        static public double MinCashTime = 2;

        static public double VisTimeStep = 0.5;
    }
}
