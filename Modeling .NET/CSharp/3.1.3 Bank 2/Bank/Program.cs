using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Bank
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
            BankSimulation.RandClient = new SimRandom();
            BankSimulation.RandCashman = new SimRandom();
            // Создать имитацию
            BankSimulation bs = new BankSimulation();
            // Запустить ее
            bs.Start();
            Console.WriteLine("Имитация завершена в момент {0,6:0.000}", bs.SimTime());
            Console.WriteLine();
            Console.WriteLine(bs.InBankStat);
            Console.WriteLine();
            Console.WriteLine(bs.CashStat);
            Console.WriteLine();
            Console.WriteLine(bs.Queue.Statistics());
            Console.WriteLine();
            Console.WriteLine(bs.Calendar.Statistics());
            Console.WriteLine();
            Console.WriteLine("Обслужено без ожидания: {0}", bs.NotWaited);
            Console.WriteLine("Не обслужено: {0}", bs.NotServiced);
            Console.WriteLine();
            Console.WriteLine(bs.InBankHist);
            // Удалить имитацию
            bs.Finish();
            Console.WriteLine("Готово");
            Console.ReadLine();
        }
    }
}
