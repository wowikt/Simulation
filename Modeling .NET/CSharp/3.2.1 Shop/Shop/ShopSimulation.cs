using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace Shop
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Имитация обслуживания покупателей в магазине
    /// </summary>
    public class ShopSimulation : SimProc
    {
        /// <summary>
        /// Кассир
        /// </summary>
        internal Cashman Cash;

        /// <summary>
        /// Статистика по занятости кассира
        /// </summary>
        public ServiceStatistics CashStat;

        /// <summary>
        /// Генератор покупателей
        /// </summary>
        internal Generator CustGen;

        /// <summary>
        /// Статистика по числу покупателей в торговом зале
        /// </summary>
        public ActionStatistics InShopStat;

        /// <summary>
        /// Точечная гистограмма по времени пребывания в магазине
        /// </summary>
        public Histogram TimeHist;

        /// <summary>
        /// Точечная статистика по времени пребывания в магазине
        /// </summary>
        public Statistics TimeStat;

        /// <summary>
        /// Очередь ожидания
        /// </summary>
        public List Queue;

        /// <summary>
        /// Генератор случайных чисел, управляющий созданием клиентов
        /// </summary>
        public static SimRandom RandCust;

        /// <summary>
        /// Генератор случайных чисел, управляющий работой кассира
        /// </summary>
        public static SimRandom RandService;

        /// <summary>
        /// Алгоритм имитации
        /// </summary>
        protected override void  Execute()
        {
            // Запустить генератор клиентов
            CustGen.Activate();
            // Ждать окончания обслуживания
            Hold(Param.SimulationTime);
        }

        /// <summary>
        /// Создание объектов имитации
        /// </summary>
        protected override void Init()
        {
            base.Init();
            Cash = new Cashman();
            CustGen = new Generator();
            Queue = new List("Очередь ожидания");
            TimeStat = new Statistics("Время нахождения клиентов в системе");
            TimeHist = new Histogram(Param.HistMin, Param.HistStep, Param.HistStepCount, "Время нахождения клиентов в системе");
            CashStat = new ServiceStatistics("Занятость кассира");
            InShopStat = new ActionStatistics("Количество покупателей в торговом зале");
        }

        /// <summary>
        /// Удаление имитации
        /// </summary>
        public override void Finish()
        {
            // Требуется удалить все процессы
            Queue.Finish();
            Cash.Finish();
            CustGen.Finish();
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
            InShopStat.StopStat();
        }
    }
}
