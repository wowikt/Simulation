using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace TVControl
{
    using SimRandom = Simulation.Random;

    /// <summary>
    /// Класс TVControlSim имитирует процесс контроля качества телевизоров
    /// </summary>
    public class TVControlSim : SimComponent
    {
        /// <summary>
        /// Очередь на настройку
        /// </summary>
        public List AdjustmentQueue;

        /// <summary>
        /// Очередь на проверку
        /// </summary>
        public List InspectionQueue;

        /// <summary>
        /// Статистика по времени пребывания в системе
        /// </summary>
        public Statistics TimeInSystemStat;

        /// <summary>
        /// Статистика по загруженности проверяющих
        /// </summary>
        public ServiceStatistics InspectorsStat;

        /// <summary>
        /// Статистика по загруженности настройщика
        /// </summary>
        public ServiceStatistics AdjustmentStat;

        /// <summary>
        /// Генератор случайных чисел, управляющий прибытием телевизоров на контроль
        /// </summary>
        public static SimRandom RandTVSet;

        /// <summary>
        /// Генератор случайных чисел, управляющий продолжительностью проверки
        /// </summary>
        public static SimRandom RandInspector;

        /// <summary>
        /// Генератор случайных чисел, управляющий продолжительностью настройки
        /// </summary>
        public static SimRandom RandAdjuster;

        /// <summary>
        /// Массив проверяющих
        /// </summary>
        internal Inspector[] Inspect;

        /// <summary>
        /// Настройщик
        /// </summary>
        internal Adjuster Adjust;

        /// <summary>
        /// Генератор телевизоров
        /// </summary>
        internal TVSetGenerator Generator;

        /// <summary>
        /// Завершение имитации
        /// </summary>
        public override void Finish()
        {
            // Завершить настройщика и проверяющих
            Adjust.Finish();
            for (int i = 0; i < Params.InspectorCount; i++)
            {
                Inspect[i].Finish();
            }
            // Удалить очереди
            InspectionQueue.Finish();
            AdjustmentQueue.Finish();
            base.Finish();
        }

        /// <summary>
        /// Создание содержимого имитации
        /// </summary>
        protected override void Init()
        {
            // Создание очередей
            InspectionQueue = new List("Очередь на проверку");
            AdjustmentQueue = new List("Очередь на настройку");
            // Содание проверяющих и настройщика
            Inspect = new Inspector[Params.InspectorCount];
            for (int i = 0; i < Params.InspectorCount; i++)
            {
                Inspect[i] = new Inspector();
            }
            Adjust = new Adjuster();
            Generator = new TVSetGenerator();
            // Создание объектов статистики
            TimeInSystemStat = new Statistics("Время пребывания в системе");
            InspectorsStat = new ServiceStatistics(Params.InspectorCount, "Загруженность проверяющих");
            AdjustmentStat = new ServiceStatistics("Загруженность настройщика");
        }

        /// <summary>
        /// Событие начала имитации
        /// </summary>
        public override void StartEvent()
        {
            // Запустить генератор
            Generator.Activate();
            // Ожидать окончания имитации
            ReactivateDelay(Params.SimulationTime);
        }

        /// <summary>
        /// Коррекция статистики к текущему времени
        /// </summary>
        public override void StopStat()
        {
            base.StopStat();
            InspectionQueue.StopStat(SimTime());
            AdjustmentQueue.StopStat(SimTime());
            InspectorsStat.StopStat(SimTime());
            AdjustmentStat.StopStat(SimTime());
        }
    }
}
