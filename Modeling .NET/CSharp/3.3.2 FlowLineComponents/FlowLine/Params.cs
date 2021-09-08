using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowLine
{
    /// <summary>
    /// Параметры имитации поточного производства
    /// </summary>
    public static class Params
    {
        /// <summary>
        /// Максимальный размер очереди к первому рабочему месту
        /// </summary>
        public static int Queue1MaxSize = 4;

        /// <summary>
        /// Максимальный размер очереди к второму рабочему месту
        /// </summary>
        public static int Queue2MaxSize = 2;

        /// <summary>
        /// Средний интервал между поступлением изделий в систему
        /// </summary>
        public static double PieceMeanInterval = 0.4;

        /// <summary>
        /// Среднее время выполнения первой операции
        /// </summary>
        public static double Worker1MeanTime = 0.25;

        /// <summary>
        /// Среднее время выполнения второй операции
        /// </summary>
        public static double Worker2MeanTime = 0.5;

        /// <summary>
        /// Нижняя граница гистограммы
        /// </summary>
        public static double HistMin = 0;

        /// <summary>
        /// Шаг гистограммы
        /// </summary>
        public static double HistStep = 0.5;

        /// <summary>
        /// Количество шагов гистограммы
        /// </summary>
        public static int HistStepCount = 20;

        /// <summary>
        /// Продолжительность имитации
        /// </summary>
        public static double SimulationTime = 300;
    }
}
