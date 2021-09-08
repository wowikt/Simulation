using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс <c>Statistics</c> собирает точечную статистику по независимым значениям
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Statistics()
        {
            ClearStat();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="AHeader">Заголовок для вывода на экран</param>
        public Statistics(string AHeader)
        {
            ClearStat();
            Header = AHeader;
        }

        /// <summary>
        /// Заголовок для вывода статистики на экран
        /// </summary>
        public string Header;

        /// <summary>
        /// Сумма величин
        /// </summary>
        protected double SumX;

        /// <summary>
        /// Сумма квадратов величин
        /// </summary>
        protected double SumX_2;

        /// <summary>
        /// Количество накопленных значений
        /// </summary>
        public int Count
        {
            get;
            protected set;
        }

        /// <summary>
        /// Максимальное значение среди накопленных
        /// </summary>
        public double Max
        {
            get;
            private set;
        }

        /// <summary>
        /// Минимальное значение среди накопленных
        /// </summary>
        public double Min
        {
            get;
            private set;
        }

        /// <summary>
        /// Добавление нового значения к статистике
        /// </summary>
        /// <param name="newX">Добавляемое значение</param>
        public virtual void AddData(double newX)
        {
            // Если это первый элемент данных, взять его в качестве минимального и максимального
            if (Count == 0)
                Min = Max = newX;
            // Иначе учесть, если он является новым максимальным либо минимальным
            else if (newX > Max)
                Max = newX;
            else if (newX < Min)
                Min = newX;
            SumX += newX;
            SumX_2 += newX * newX;
            Count++;
        }

        /// <summary>
        /// Очистка статистики, подготовка к новому сбору данных
        /// </summary>
        public virtual void ClearStat()
        {
            SumX = SumX_2 = Min = Max = 0;
            Count = 0;
        }

        /// <summary>
        /// Возвращает стандартное отклонение накопленных значений
        /// </summary>
        /// <returns>Стандартное отклонение</returns>
        public double Deviation()
        {
            return Math.Sqrt(Disperse());
        }

        /// <summary>
        /// Возвращает дисперсию накопленных значений
        /// </summary>
        /// <returns>Дисперсия</returns>
        public double Disperse()
        {
            if (Count <= 1)
                return 0;
            else
                return (SumX_2 - Count * Mean() * Mean()) / (Count - 1);
        }

        /// <summary>
        /// Возвращает среднее арифметическое по накопленным данным
        /// </summary>
        /// <returns>Среднее арифметическое</returns>
        public double Mean()
        {
            if (Count <= 0)
                return 0;
            else
                return SumX / Count;
        }

        /// <summary>
        /// Преобразует содержимое статистики в текст для отображения на экране
        /// </summary>
        /// <returns>Преобразованное содержимое</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("Среднее = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("Минимум = {0,6:0.000}, максимум = {1,6:0.000}\n", Min, Max);
            Result.AppendFormat("Всего {0,4} значений", Count);
            return Result.ToString();
        }
    }
}
