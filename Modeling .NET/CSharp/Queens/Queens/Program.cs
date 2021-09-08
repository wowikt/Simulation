using System;
using System.Collections.Generic;
using System.Text;
using Simulation;

namespace Queens
{
    class Program
    {
        /// <summary>
        /// Основной метод
        /// </summary>
        /// <param name="args">Параметры командной строки не используются</param>
        static void Main(string[] args)
        {
            // Создать и запустить основную сопрограмму
            QueensRun QRun = new QueensRun();
            Global.RunDispatcher(QRun);
            QRun.Finish();
            Console.WriteLine("Готово.");
            Console.ReadLine();
        }
    }
}
