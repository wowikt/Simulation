using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;
using Shop;

namespace ShopMulti
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Программа имитации работы банка
    /// </summary>
    class Program
    {
        /// <summary>
        /// Главная программы
        /// </summary>
        /// <param name="args">Параметры командной строки (не используются)</param>
        static void Main(string[] args)
        {
            // Создать генераторы случайных чисел
            ShopSimulation.RandCust = new SimRandom();
            ShopSimulation.RandService = new SimRandom();
            CashUsageStat = new Statistics("Занятость кассира");
            TimeStat = new Statistics("Среднее время пребывания в системе");
            InShopStat = new Statistics("Среднее количество покупателей в торговом зале");
            InShopMaxStat = new Statistics("Максимальное количество покупателей в торговом зале");
            MaxQueueLenStat = new Statistics("Максимальная длина очереди");
            WaitStat = new Statistics("Среднее время ожидания в очереди");
            TimeHist = new Histogram(HistMin, HistStep, HistStepCount, "Среднее время пребывания в системе");
            for (int i = 0; i < RunCount; i++)
            {
                // Создать имитацию
                ShopSim = new ShopSimulation();
                // Запустить ее
                if (i % 5 == 0)
                {
                    Console.Write("*");
                }
                ShopSim.Start();
                // Собрать статистику
                CashUsageStat.AddData(ShopSim.CashStat.Mean());
                TimeStat.AddData(ShopSim.TimeStat.Mean());
                TimeHist.AddData(ShopSim.TimeStat.Mean());
                InShopStat.AddData(ShopSim.InShopStat.Mean());
                InShopMaxStat.AddData(ShopSim.InShopStat.Max);
                MaxQueueLenStat.AddData(ShopSim.Queue.LengthStat.Max);
                WaitStat.AddData(ShopSim.Queue.TimeStat.Mean());
                // Удалить имитацию
                ShopSim.Finish();
                //ShopSim.Dispose();
                ShopSim = null;
            }
            // Вывод статистики
            Console.WriteLine();
            Console.WriteLine(CashUsageStat);
            Console.WriteLine();
            Console.WriteLine(TimeStat);
            Console.WriteLine();
            Console.WriteLine(InShopStat);
            Console.WriteLine();
            Console.WriteLine(InShopMaxStat);
            Console.WriteLine();
            Console.WriteLine(MaxQueueLenStat);
            Console.WriteLine();
            Console.WriteLine(WaitStat);
            Console.WriteLine();
            Console.WriteLine(TimeHist);
            Console.WriteLine("Готово");
            Console.ReadLine();
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
        /// Ссылка на объект-имитацию
        /// </summary>
        internal static ShopSimulation ShopSim;

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
