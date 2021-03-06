using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace BankVisual
{
    /// <summary>
    /// Имитация обслуживания клиентов в банке
    /// </summary>
    class BankSimulation : SimComponent
    {
        /// <summary>
        /// Статистика по занятости кассира
        /// </summary>
        internal ServiceStatistics CashStat;

        /// <summary>
        /// Клиент, обслуживаемый в данный момент
        /// </summary>
        internal Client CurrentClient;

        /// <summary>
        /// Точечная гистограмма по времени пребывания в банке
        /// </summary>
        internal Histogram InBankHist;

        /// <summary>
        /// Точечная статистика по времени пребывания в банке
        /// </summary>
        internal Statistics InBankStat;

        /// <summary>
        /// Количество клиентов, обслуженных без ожидания в очереди
        /// </summary>
        internal int NotWaited;

        /// <summary>
        /// Очередь ожидания
        /// </summary>
        internal List Queue;

        /// <summary>
        /// Алгоритм имитации
        /// </summary>
        public override void  StartEvent()
        {
            (new Client()).Activate();
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
            VisualInterval = Param.VisTimeStep;
            Client.ArrivedCount = 0;
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
