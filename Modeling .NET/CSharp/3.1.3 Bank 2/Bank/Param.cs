using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    /// <summary>
    /// Параметры работы имитации
    /// </summary>
    public static class Param
    {
        /// <summary>
        /// Количество касс
        /// </summary>
        static public int CashCount = 2;

        /// <summary>
        /// Момент прибытия первого клиента (не считая тех, которые с самого начала находятся в системе)
        /// </summary>
        static public double FirstClientArrival = 5;

        /// <summary>
        /// Нижняя граница гистограммы
        /// </summary>
        static public double HistMin = 2;

        /// <summary>
        /// Размер шага гистограммы
        /// </summary>
        static public double HistStep = 2;

        /// <summary>
        /// Количество шагов гистограммы
        /// </summary>
        static public int HistStepCount = 20;

        /// <summary>
        /// Максимальное время обслуживания на кассе
        /// </summary>
        static public double MaxCashTime = 12;

        /// <summary>
        /// Количество клиентов для обслуживания
        /// </summary>
        static public int MaxClientCount = 100;

        /// <summary>
        /// Максимальная длина очереди
        /// </summary>
        static public int MaxQueueLength = 10;

        /// <summary>
        /// Средний интервал между прибытием клиентов
        /// </summary>
        static public double MeanClientInterval = 5;

        /// <summary>
        /// Минимальное время обслуживания на кассе
        /// </summary>
        static public double MinCashTime = 6;

        /// <summary>
        /// Количество клиентов, находящихся в очереди в момент запуска системы
        /// </summary>
        static public int StartClientNum = 2;
    }
}
