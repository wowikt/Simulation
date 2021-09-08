using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace TVControl
{
    /// <summary>
    /// Параметры имитации контроля телевизоров
    /// </summary>
    public static class Params
    {
        /// <summary>
        /// Минимальный интервал прибытия телевизоров
        /// </summary>
        public static double MinTVSetInterval = 3.5;

        /// <summary>
        /// Максимальный интервал прибытия телевизоров
        /// </summary>
        public static double MaxTVSetInterval = 7.5;

        /// <summary>
        /// Минимальное время проверки
        /// </summary>
        public static double MinInspectionTime = 6;

        /// <summary>
        /// Максимальное время проверки
        /// </summary>
        public static double MaxInspectionTime = 12;

        /// <summary>
        /// Вероятность того, что телевизор исправен и не нуждается в настройке
        /// </summary>
        public static double NoAdjustmentProb = 0.85;

        /// <summary>
        /// Минимальное время настройки
        /// </summary>
        public static double MinAdjustmentTime = 20;

        /// <summary>
        /// Максимальное время настройки
        /// </summary>
        public static double MaxAdjustmentTime = 40;

        /// <summary>
        /// Количество проверяющих
        /// </summary>
        public static int InspectorCount = 2;

        /// <summary>
        /// Время выполнения имитации
        /// </summary>
        public static double SimulationTime = 480;
    }
}
