using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс <c>IntervalHistogram</c> собирает данные по промежуткам времени, в течение которые значение находилось в заданных интервалах
    /// </summary>
    public class IntervalHistogram : HistogramBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ALow">Нижняя граница первого конечного интервала</param>
        /// <param name="AStep">Шаг каждого конечного интервала</param>
        /// <param name="AIntervalCount">Количество конечных интервалов</param>
        public IntervalHistogram(double ALow, double AStep, int AIntervalCount)
            : base(ALow, AStep, AIntervalCount)
        {
            Data = new double[IntervalCount + 2];
            TotalTime = 0;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ALow">Нижняя граница первого конечного интервала</param>
        /// <param name="AStep">Шаг каждого конечного интервала</param>
        /// <param name="AIntervalCount">Количество конечных интервалов</param>
        /// <param name="AHeader">Заголовок при выводе на экран</param>
        public IntervalHistogram(double ALow, double AStep, int AIntervalCount, string AHeader)
            : base(ALow, AStep, AIntervalCount, AHeader)
        {
            Data = new double[IntervalCount + 2];
            TotalTime = 0;
        }

        /// <summary>
        /// Массив, в котором содержится время, в течение которого значение величины находилось в каждом интервале
        /// </summary>
        private double[] Data;

        /// <summary>
        /// Суммарное время записанных значений
        /// </summary>
        public double TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Добавляет к гистограмме новое значение. Увеличивает на <c>dTime</c> элемент массива, соответствующий добавляемой величине, 
        /// и общее количество значений
        /// </summary>
        /// <param name="newData">Добавляемое значение</param>
        /// <param name="dTime">Время, в течение которого величина принимала указанное значение</param>
        public void AddData(double newData, double dTime)
        {
            Data[IntervalIndex(newData)] += dTime;
            TotalTime += dTime;
        }

        /// <summary>
        /// Очищает гистограмму, записывая нулевые значения во все элементы массива и обнуляя общее время
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i <= IntervalCount + 1; i++)
                Data[i] = 0;
            TotalTime = 0;
        }

        /// <summary>
        /// Возвращает долю от общего времени, в течение которого значение попадало в интервалы от левого до указанного
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля времени</returns>
        public override double CumulativePercent(int index)
        {
            if (TotalTime == 0)
                return 0;
            else
                return CumulativeTime(index) / TotalTime;
        }

        /// <summary>
        /// Общий промежуток времени, в течение которого значение попадало в интервалы от крайнего левого до указанного
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Общий промежуток времени</returns>
        public double CumulativeTime(int index)
        {
            double Sum = 0;
            for (int i = 0; i <= index && i <= IntervalCount + 1; i++)
                Sum += Data[i];
            return Sum;
        }

        /// <summary>
        /// Возвращает долю от общего времени, в течение которого значение попадало в указанный интервал
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля времени</returns>
        public override double Percent(int index)
        {
            if (TotalTime == 0)
                return 0;
            else
                return Time(index) / TotalTime;
        }

        /// <summary>
        /// Возвращает суммарное время, в течение которого значение находилось в указанном интервале
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Суммарное время</returns>
        public double Time(int index)
        {
            if (index <= 0)
                return Data[0];
            else if (index >= IntervalCount + 1)
                return Data[IntervalCount + 1];
            else
                return Data[index];
        }

        /// <summary>
        /// Отображает содержимое гистограммы в текстовом виде для отображения на экране
        /// </summary>
        /// <returns>Текстовый вид гистограммы</returns>
        public override string ToString()
        {
            string Output = Header + "\n";

            // Нулевой полубесконечный интервал
            int CumCount = (int)Math.Round(CumulativePercent(0) * 40) - 1;
            int PercentCount = (int)Math.Round(Percent(0) * 40);
            StringBuilder Graph = new StringBuilder();
            if (CumCount > 0)
                Graph.Append(' ', CumCount);
            if (CumCount >= 0)
                Graph.Append('o');
            for (int j = 0; j < PercentCount; j++)
                Graph[j] = '*';
            Output += string.Format(" -INF - {0,5:0.00} : {1,5:0.0} ({2,5:0.00}%), {3,6:0.00}% {4}\n",
                UpperBound(0), Time(0), Percent(0) * 100, CumulativePercent(0) * 100, Graph);

            // Конечные интервалы
            for (int i = 1; i <= IntervalCount; i++)
            {
                CumCount = (int)Math.Round(CumulativePercent(i) * 40) - 1;
                PercentCount = (int)Math.Round(Percent(i) * 40);
                Graph = new StringBuilder();
                if (CumCount > 0)
                    Graph.Append(' ', CumCount);
                if (CumCount >= 0)
                    Graph.Append('o');
                for (int j = 0; j < PercentCount; j++)
                    Graph[j] = '*';
                Output += string.Format("{0,5:0.00} - {1,5:0.00} : {2,5:0.0} ({3,5:0.00}%), {4,6:0.00}% {5}\n",
                    LowerBound(i), UpperBound(i), Time(i), Percent(i) * 100, CumulativePercent(i) * 100, Graph);
            }

            // Последний полубесконечный интервал
            CumCount = (int)Math.Round(CumulativePercent(IntervalCount + 1) * 40) - 1;
            PercentCount = (int)Math.Round(Percent(IntervalCount + 1) * 40);
            Graph = new StringBuilder();
            if (CumCount > 0)
                Graph.Append(' ', CumCount);
            if (CumCount >= 0)
                Graph.Append('o');
            for (int j = 0; j < PercentCount; j++)
                Graph[j] = '*';
            Output += string.Format("{0,5:0.00} -  +INF : {1,5:0.0} ({2,5:0.00}%), {3,6:0.00}% {4}",
                LowerBound(IntervalCount + 1), Time(IntervalCount + 1), Percent(IntervalCount + 1) * 100,
                CumulativePercent(IntervalCount + 1) * 100, Graph);
            return Output;
        }
    }
}
