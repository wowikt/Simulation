using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Bank
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Имитация обслуживания клиентов в банке
    /// </summary>
    public class BankSimulation : SimProc
    {
        /// <summary>
        /// Точечная гистограмма по времени пребывания в банке
        /// </summary>
        public Histogram InBankHist;

        /// <summary>
        /// Количество клиентов, не попавших в банк
        /// </summary>
        public int NotServiced;

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
        /// Генератор случайных чисел, управляющий созданием клиентов
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
        /// Кассир
        /// </summary>
        internal Cashman[] Cash;

        /// <summary>
        /// Генератор клиентов
        /// </summary>
        internal ClientGenerator Generator;

        /// <summary>
        /// Алгоритм имитации
        /// </summary>
        protected override void  Execute()
        {
            // Запустить генератор клиентов
            Generator.Activate();
            Cash.ActivateAll();
            Queue.ActivateAll();
            // Ждать окончания обслуживания
            Passivate();
            StopStat();
        }

        /// <summary>
        /// Создание объектов имитации
        /// </summary>
        protected override void Init()
        {
            base.Init();
            Queue = new List(Param.MaxQueueLength, "Очередь ожидания");
            Cash = new Cashman[Param.CashCount];
            for (int i = 0; i < Param.CashCount; i++)
            {
                Cash[i] = new Cashman(new Client(true));
            }
            for (int i = 0; i < Param.StartClientNum; i++)
            {
                (new Client(true)).Insert(Queue);
            }
            Generator = new ClientGenerator();
            InBankStat = new Statistics("Время нахождения клиентов в системе");
            InBankHist = new Histogram(Param.HistMin, Param.HistStep, Param.HistStepCount, "Время нахождения клиентов в системе");
            CashStat = new ServiceStatistics(Param.CashCount, "Занятость кассира");
            NotWaited = 0;
            //VisualInterval = 10;
        }

        /// <summary>
        /// Удаление имитации
        /// </summary>
        public override void Finish()
        {
            // Требуется удалить все процессы
            Queue.Finish();
            for (int i = 0; i < Param.CashCount; i++)
            {
                Cash[i].Finish();
            }
            Generator.Finish();
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
        }
    }
}
