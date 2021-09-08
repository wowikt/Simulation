using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace FlowLine
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Класс FlowLineSim - имитация поточного производства
    /// </summary>
    public class FlowLineSim : SimComponent
    {
        /// <summary>
        /// Статистика по промежуткам времени между отказами в обслуживании
        /// </summary>
        public TimeBetStatistics BalksStat;

        /// <summary>
        /// Статистика по времени пребывания обслуженных изделий в системе
        /// </summary>
        public Statistics TimeInSystemStat;

        /// <summary>
        /// Гистограмма по времени пребывания обслуженных изделий в системе
        /// </summary>
        public Histogram TimeHist;

        /// <summary>
        /// Генератор случайных чисел, управляющий поступлением изделий на обработку
        /// </summary>
        public static SimRandom RandPiece;

        /// <summary>
        /// Генератор случайных чисел, управляющий продолжительностью операции 
        /// на первом рабочем месте
        /// </summary>
        public static SimRandom RandWorker1;

        /// <summary>
        /// Генератор случайных чисел, управляющий продолжительностью операции 
        /// на втором рабочем месте
        /// </summary>
        public static SimRandom RandWorker2;

        /// <summary>
        /// Процесс, имитирующий выполнение первой операции
        /// </summary>
        internal Service Worker1;

        /// <summary>
        /// Процесс, имитирующий выполнение второй операции
        /// </summary>
        internal Service Worker2;

        /// <summary>
        /// Завершение работы имитации
        /// </summary>
        public override void Finish()
        {
            Worker1.Finish();
            Worker2.Finish();
            base.Finish();
        }

        /// <summary>
        /// Создание объектов имитации
        /// </summary>
        protected override void Init()
        {
            base.Init();
            BalksStat = new TimeBetStatistics("Интервалы времени между отказами в обслуживании");
            TimeInSystemStat = new Statistics("Время в системе");
            TimeHist = new Histogram(Params.HistMin, Params.HistStep, Params.HistStepCount, "Время в системе");
            Worker1 = new Service(1, Params.Queue1MaxSize, 
                () => FlowLineSim.RandWorker1.Exponential(Params.Worker1MeanTime), "Первое рабочее место");
            Worker2 = new Service(1, Params.Queue2MaxSize, 
                () => FlowLineSim.RandWorker2.Exponential(Params.Worker2MeanTime), "Второе рабочее место");
            Worker1.NextService = Worker2;
        }

        /// <summary>
        /// Начало работы
        /// </summary>
        protected override void StartEvent()
        {
            // Создать первое изделие
            (new Piece()).Activate();
            // ОЖидать окончания имитации
            ReactivateDelay(Params.SimulationTime);
        }

        /// <summary>
        /// Коррекция статистики
        /// </summary>
        public override void StopStat()
        {
            base.StopStat();
            Worker1.StopStat();
            Worker2.StopStat();
        }
    }
}
