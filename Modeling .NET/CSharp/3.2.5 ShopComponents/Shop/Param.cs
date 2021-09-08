using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop
{
    /// <summary>
    /// Параметры работы имитации
    /// </summary>
    public static class Param
    {
        /// <summary>
        /// Нижняя граница гистограммы
        /// </summary>
        public static double HistMin = 2;

        /// <summary>
        /// Размер шага гистограммы
        /// </summary>
        public static double HistStep = 2;

        /// <summary>
        /// Количество шагов гистограммы
        /// </summary>
        public static int HistStepCount = 20;

        /// <summary>
        /// Максимальное число покупок
        /// </summary>
        public static int MaxBuysCount = 16;

        /// <summary>
        /// Максимальное время выбора покупок
        /// </summary>
        public static double MaxShoppingTime = 12;

        /// <summary>
        /// Средний интервал между прибытием покупателей
        /// </summary>
        public static double MeanCustInterval = 2;

        /// <summary>
        /// Среднее время обслуживания одной покупки на кассе
        /// </summary>
        public static double MeanTimePerBuy = 0.2;

        /// <summary>
        /// Минимальное число покупок
        /// </summary>
        public static int MinBuysCount = 2;

        /// <summary>
        /// Минимальное время выбора покупок
        /// </summary>
        public static double MinShoppingTime = 2;

        /// <summary>
        /// Время имитации
        /// </summary>
        public static double SimulationTime = 480;
    }
}
