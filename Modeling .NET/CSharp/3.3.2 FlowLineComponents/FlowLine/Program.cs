using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowLine
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Программа имитации поточной линии
    /// </summary>
    class Program
    {
        /// <summary>
        /// Главный метод
        /// </summary>
        /// <param name="args">Параметры командной строки (не используются)</param>
        static void Main(string[] args)
        {
            // Создать генераторы случайных чисел
            FlowLine.RandPiece = new SimRandom();
            FlowLine.RandWorker1 = new SimRandom();
            FlowLine.RandWorker2 = new SimRandom();
            // СОздать имитацию и запустить ее
            FlowLine flSim = new FlowLine();
            flSim.Start();
            // Вывести статистику
            Console.WriteLine("Имитация завершена в {0}", flSim.SimTime());
            Console.WriteLine();
            Console.WriteLine(flSim.TimeInSystemStat);
            Console.WriteLine();
            Console.WriteLine(flSim.BalksStat);
            Console.WriteLine();
            Console.WriteLine(flSim.Worker1Stat);
            Console.WriteLine();
            Console.WriteLine(flSim.Worker2Stat);
            Console.WriteLine();
            Console.WriteLine(flSim.Queue1.Statistics());
            Console.WriteLine();
            Console.WriteLine(flSim.Queue2.Statistics());
            Console.WriteLine();
            Console.WriteLine(flSim.Calendar.Statistics());
            Console.WriteLine();
            Console.WriteLine(flSim.TimeHist);
            Console.ReadLine();
            // Удалить имитацию
            flSim.Finish();
        }
    }
}
