using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Simulation;

namespace ShopMultiVisual
{
    using SimRandom = Simulation.Random;

    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShopMultiVisual());
        }

        /// <summary>
        /// Статистика по занятости касира
        /// </summary>
        internal static Statistics CashUsageStat;

        /// <summary>
        /// Нижняя граница гистограммы
        /// </summary>
        internal static double HistMin = 8;

        /// <summary>
        /// Шаг гистограммы
        /// </summary>
        internal static double HistStep = 1;

        /// <summary>
        /// Количество шагов гистограммы
        /// </summary>
        internal static int HistStepCount = 20;

        /// <summary>
        /// Статистика по максимальному количеству покупателей в торговом зале
        /// </summary>
        internal static Statistics InShopMaxStat;

        /// <summary>
        /// Статистика по среднему количеству покупателей в торговом зале
        /// </summary>
        internal static Statistics InShopStat;

        /// <summary>
        /// Статистика по максимальной длине очереди
        /// </summary>
        internal static Statistics MaxQueueLenStat;

        /// <summary>
        /// Количество прогонов
        /// </summary>
        internal static int RunCount = 400;

        /// <summary>
        /// Гистограмма по времени пребывания в магазине
        /// </summary>
        internal static Histogram TimeHist;

        /// <summary>
        /// Статистика по времени пребывания в магазине
        /// </summary>
        internal static Statistics TimeStat;

        /// <summary>
        /// Статистика по среднему времени ожидания в очереди
        /// </summary>
        internal static Statistics WaitStat;
    }
}
