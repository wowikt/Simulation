﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Simulation;

namespace BankVisual
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
            Application.Run(new BankVisual());
        }

        internal static SimRandom RandClient;
        internal static SimRandom RandCashman;
    }
}
