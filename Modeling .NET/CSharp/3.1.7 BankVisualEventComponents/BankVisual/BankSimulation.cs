﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace BankVisual
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Имитация обслуживания клиентов в банке
    /// </summary>
    public class BankSimulation : SimComponent
    {
        /// <summary>
        /// Точечная гистограмма по времени пребывания в банке
        /// </summary>
        public Histogram InBankHist;

        /// <summary>
        /// Количество клиентов, обслуженных без ожидания в очереди
        /// </summary>
        public int NotWaited;

        /// <summary>
        /// Очередь ожидания
        /// </summary>
        public List Queue;

        /// <summary>
        /// Статистика по занятости кассира
        /// </summary>
        public ServiceStatistics CashStat;

        /// <summary>
        /// Генератор случайных чисел, управляющий работой клиентов
        /// </summary>
        public static SimRandom RandClient;

        /// <summary>
        /// Генератор случайных чисел, управляющий работой кассира
        /// </summary>
        public static SimRandom RandCashman;

        /// <summary>
        /// Точечная статистика по времени пребывания в банке
        /// </summary>
        public Statistics InBankStat;

        /// <summary>
        /// Клиент, обслуживаемый в данный момент
        /// </summary>
        internal Client CurrentClient;

        /// <summary>
        /// Алгоритм имитации
        /// </summary>
        public override void  StartEvent()
        {
            (new ArrivalEvent()).Activate();
        }

        /// <summary>
        /// Создание объектов имитации
        /// </summary>
        protected override void Init()
        {
            base.Init();
            Queue = new List("Очередь ожидания");
            InBankStat = new Statistics("Время нахождения клиентов в системе");
            InBankHist = new Histogram(Param.HistMin, Param.HistStep, Param.HistStepCount, "Время нахождения клиентов в системе");
            CashStat = new ServiceStatistics("Занятость кассира");
            NotWaited = 0;
            ArrivalEvent.ArrivedCount = 0;
        }

        /// <summary>
        /// Удаление имитации
        /// </summary>
        public override void Finish()
        {
            // Требуется удалить все процессы
            Queue.Finish();
            // Остальные объекты имитации удалять не требуется
            base.Finish();
        }

        /// <summary>
        /// Коррекция статистики
        /// </summary>
        public override void StopStat()
        {
            base.StopStat();
            CashStat.StopStat();
            Queue.StopStat();
        }
    }
}
