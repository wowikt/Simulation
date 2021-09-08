using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace TVControl
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Программа имитации контроля телевизоров
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
            TVControlSim.RandAdjuster = new Simulation.Random();
            TVControlSim.RandInspector = new Simulation.Random();
            TVControlSim.RandTVSet = new Simulation.Random();
            // Создать имитацию и запустить ее
            TVControlSim tvc = new TVControlSim();
            tvc.Start();
            // Вывести результаты
            Console.WriteLine("Имитация завершена в {0}", tvc.SimTime());
            Console.WriteLine();
            Console.WriteLine(tvc.TimeInSystemStat);
            Console.WriteLine();
            Console.WriteLine(tvc.InspectorsStat);
            Console.WriteLine();
            Console.WriteLine(tvc.AdjustmentStat);
            Console.WriteLine();
            Console.WriteLine(tvc.InspectionQueue.Statistics());
            Console.WriteLine();
            Console.WriteLine(tvc.AdjustmentQueue.Statistics());
            Console.WriteLine();
            Console.WriteLine(tvc.Calendar.Statistics());
            Console.ReadLine();
            // Завершить имитацию
            tvc.Finish();
        }
    }
}
