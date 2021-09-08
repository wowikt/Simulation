using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
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
            // Создать имитацию
            ShopSimulation bs = new ShopSimulation();
            // Запустить ее
            bs.Start();
            Console.WriteLine("Имитация завершена в момент {0,6:0.000}", bs.SimTime());
            Console.WriteLine();
            Console.WriteLine(bs.CashStat);
            Console.WriteLine();
            Console.WriteLine(bs.TimeStat);
            Console.WriteLine();
            Console.WriteLine(bs.InShopStat);
            Console.WriteLine();
            Console.WriteLine(bs.Queue.Statistics());
            Console.WriteLine();
            Console.WriteLine(bs.Calendar.Statistics());
            Console.WriteLine();
            Console.WriteLine(bs.TimeHist);
            // Удалить имитацию
            bs.Finish();
            Console.WriteLine("Готово");
            Console.ReadLine();
        }
    }
}
