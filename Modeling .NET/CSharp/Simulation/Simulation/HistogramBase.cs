using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс <c>HistogramBase</c> содержит общие средства для точечных и интервальных гистограмм
    /// </summary>
    public abstract class HistogramBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ALow">Нижняя граница первого конечного интервала</param>
        /// <param name="AStep">Шаг каждого конечного интервала</param>
        /// <param name="AIntervalCount">Количество конечных интервалов</param>
        public HistogramBase(double ALow, double AStep, int AIntervalCount)
        {
            Low = ALow;
            Step = AStep;
            IntervalCount = AIntervalCount;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ALow">Нижняя граница первого конечного интервала</param>
        /// <param name="AStep">Шаг каждого конечного интервала</param>
        /// <param name="AIntervalCount">Количество конечных интервалов</param>
        /// <param name="AHeader">Заголовок для вывода на консоль</param>
        public HistogramBase(double ALow, double AStep, int AIntervalCount, string AHeader)
        {
            Low = ALow;
            Step = AStep;
            IntervalCount = AIntervalCount;
            Header = AHeader;
        }

        /// <summary>
        /// Заголовок гистограммы при выводе
        /// </summary>
        public string Header;

        /// <summary>
        /// Количество конечных интевалов.
        /// <para>Общее количество интервалов гистограммы на 2 больше, так как она включает еще два
        /// полубесконечных интервала с каждой стороны</para>
        /// </summary>
        public int IntervalCount
        {
            get;
            protected set;
        }

        /// <summary>
        /// Нижняя граница первого конечного интервала
        /// </summary>
        public double Low
        {
            get;
            protected set;
        }

        /// <summary>
        /// Шаг каждого конечного интервала
        /// </summary>
        public double Step
        {
            get;
            protected set;
        }

        /// <summary>
        /// Очищает гистограмму
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Возвращает долю от общего количества значений, попавших в интервалы от левого до указанного
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля количества значений</returns>
        public abstract double CumulativePercent(int index);

        /// <summary>
        /// Возвращает долю от общего количества значений, попавших в указанный интервал
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля количества значений</returns>
        public abstract double Percent(int index);

        /// <summary>
        /// Возвращает индекс интервала, в который попадает указанное значение.
        /// <para>Результат равен 0, если значение меньше нижней границы левого конечного интервала (<c>Low</c>).</para>
        /// <para>Результат равен <c>IntervalCount + 1</c>, если значение больше или равно верхней границе последнего конечного интервала</para>
        /// </summary>
        /// <param name="val">Значение, индекс для которого требуется определить</param>
        /// <returns>Индекс интервала</returns>
        public int IntervalIndex(double val)
        {
            if (val >= Low)
            {
                int iStep = (int)((val - Low) / Step) + 1;
                if (iStep > IntervalCount + 1)
                    return IntervalCount + 1;
                else
                    return iStep;
            }
            else
                return 0;
        }

        /// <summary>
        /// Возвращает левую (нижнюю) границу указанного интервала.
        /// <para>Если индекс интервала отрицателен, возвращает значение "минус бесконечность".</para>
        /// <para>Если индекс интервала превышает максимально возможный, возвращает левую границу правого полубесконечного интервала</para>
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Левая граница интервала</returns>
        public double LowerBound(int index)
        {
            if (index <= 0)
                return double.NegativeInfinity;
            else if (index >= IntervalCount + 1)
                return Low + Step * IntervalCount;
            else
                return Low + Step * (index - 1);
        }

        /// <summary>
        /// Возвращает правую (верхнюю) границу указанного интервала.
        /// <para>Если индекс интервала отрицателен, возвращает правую границу левого полубесконечного интервала</para>
        /// <para>Если индекс интервала превышает максимально возможный, возвращает значение "плюс бесконечность".</para>
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Правая граница интервала</returns>
        public double UpperBound(int index)
        {
            if (index <= 0)
                return Low;
            else if (index >= IntervalCount + 1)
                return double.PositiveInfinity;
            else
                return Low + Step * index;
        }
    }
}
