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
            FlowLineSim.RandPiece = new SimRandom();
            FlowLineSim.RandWorker1 = new SimRandom();
            FlowLineSim.RandWorker2 = new SimRandom();
            // СОздать имитацию и запустить ее
            FlowLineSim flSim = new FlowLineSim();
            flSim.Start();
            // Вывести статистику
            Console.WriteLine("Имитация завершена в {0}", flSim.SimTime());
            Console.WriteLine();
            Console.WriteLine(flSim.TimeInSystemStat);
            Console.WriteLine();
            Console.WriteLine(flSim.BalksStat);
            Console.WriteLine();
            Console.WriteLine(flSim.Worker1.ServiceStat);
            Console.WriteLine();
            Console.WriteLine(flSim.Worker2.ServiceStat);
            Console.WriteLine();
            Console.WriteLine(flSim.Worker1.Queue.Statistics());
            Console.WriteLine();
            Console.WriteLine(flSim.Worker2.Queue.Statistics());
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
