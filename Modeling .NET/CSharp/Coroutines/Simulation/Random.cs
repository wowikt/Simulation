using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    using SysRandom = System.Random;
    /// <summary>
    /// Класс Random предоставляет генератор случайных чисел с возможностью получения последовательностей с различным распределением
    /// </summary>
    public class Random
    {
        private SysRandom Rnd;
        private bool HasNextNormal;
        private double NextNormal;

        /// <summary>
        /// Конструктор. Инициализирует объект автоматически в зависимости от текущего системного времени
        /// </summary>
        public Random()
        {
            Rnd = new SysRandom();
        }

        /// <summary>
        /// Конструктор. Инициализирует объект заданным начальным значением
        /// </summary>
        /// <param name="seed">Исходное значение</param>
        public Random(int seed)
        {
            Rnd = new SysRandom(seed);
        }

        /// <summary>
        /// Возвращает очередное псевдослучайное вещественное значение
        /// </summary>
        /// <returns>Равномерно распределенное вещественное значение в интервале [0, 1)</returns>
        public double NextFloat()
        {
            return Rnd.NextDouble();
        }

        /// <summary>
        /// Возвращает очередное псеводслучайное целочисленное значение 
        /// </summary>
        /// <returns>Равномерно распределенное целочисленное значение в интервале от 0 до 2^31 - 1</returns>
        public int NextInt()
        {
            return Rnd.Next();
        }

        /// <summary>
        /// Возвращает очередное псеводслучайное целочисленное значение,
        /// ограниченное сверху
        /// </summary>
        /// <param name="max">Верхняя граница псевдослучайного значения</param>
        /// <returns>Расномерно распределенное целочисленное значение в интервале от 0 до max - 1</returns>
        public int NextInt(int max)
        {
            return Rnd.Next(max);
        }

        /// <summary>
        /// Возвращает очередное псеводслучайное целочисленное значение,
        /// ограниченное с двух сторон
        /// </summary>
        /// <param name="min">Нижняя граница псевдослучайного значения</param>
        /// <param name="max">Верхняя граница псеводслучайного значения</param>
        /// <returns>Расномерно распределенное целочисленное значение в интервале от min до max - 1</returns>
        public int NextInt(int min, int max)
        {
            return Rnd.Next(min, max);
        }

        /// <summary>
        /// Возвращает true с заданной вероятностью
        /// </summary>
        /// <param name="prob">Вероятность появления результата true</param>
        /// <returns>Логическое значение, равное true с вероятностью prob, и false с вероятностью 1 - prob</returns>
        public bool Draw(double prob)
        {
            return Rnd.NextDouble() < prob;
        }

        /// <summary>
        /// Возвращает очередное равномерно распределенное псеводслучайное вещественное значение
        /// </summary>
        /// <param name="min">Нижняя граница</param>
        /// <param name="max">Верхняя граница</param>
        /// <returns>Значение в диапазоне [min, max)</returns>
        public double Uniform(double min, double max)
        {
            return Rnd.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Возвращает целочисленное значение, распределенное с вероятностями для каждого возможного значения, задаваемыми в массиве.
        /// Параметр-массив должен быть упорядочен по возрастанию.
        /// Например, если массив содержит значения (0.2, 0.6, 0.7, 0.9), то вероятности появления результатов будут следующими:
        /// 0 - 0,2, 1 - 0,4, 2 - 0,1, 3 - 0,2, 4 - 0,1.
        /// </summary>
        /// <param name="table">Массив вероятностей</param>
        /// <returns>Целочисленное значение в интервале от 0 до table.Length</returns>
        public int TableIndex(double[] table)
        {
            if (table.Length == 0)
                return 0;
            double Rand = Rnd.NextDouble();
            // Двоичный поиск диапазона, в который попадает случайное значение.
            // Исходный массив должен быть упорядочен.
            // Если это не так, алгоритм неработоспособен.
            // Проверка упорядоченнсти не проводится с целью повышения эффективности.
            // ***(а может, сделать?)
            int L = 0;
            int R = table.Length - 1;
            while (L < R)
            {
                int M = (L + R) / 2;
                if (Rand > table[M])
                    L = M + 1;
                else
                    R = M;
            }
            if (Rand < table[L])
                return L;
            // Значение, равное граничному, принадлежит следующему диапазону
            // Значение, превышающее граничное, может появиться только в последнем диапазоне
            else
                return L + 1;
        }

        /// <summary>
        /// Возвращает нормально распределенное вещественное значение с заданными
        /// значениями математического ожидания и стандартного отклонения
        /// </summary>
        /// <param name="mean">Математическое ожидание</param>
        /// <param name="deviation">Стандартное отклонение</param>
        /// <returns>Нормально распределенное вещественное значение</returns>
        public double Normal(double mean, double deviation)
        {
            if (HasNextNormal)
            {
                HasNextNormal = false;
                return (NextNormal * deviation + mean);
            }
            else
            {
                double Rnd1, Rnd2, W;
                HasNextNormal = true;
                do
                {
                    Rnd1 = 2 * NextFloat() - 1;
                    Rnd2 = 2 * NextFloat() - 1;
                    W = Rnd1 * Rnd1 + Rnd2 * Rnd2;
                }
                while (W > 1);
                NextNormal = Rnd2 * Math.Sqrt(-2 * Math.Log(W) / W);
                return (Rnd1 * Math.Sqrt(-2 * Math.Log(W) / W) * deviation + mean);
            }
        }

        /// <summary>
        /// Возвращает экспоненциально распределенную вещественную величину
        /// с заданным математическим ожиданием
        /// </summary>
        /// <param name="mean">Математическое ожидание (для данного распределения оно равно стандартному отклонению)</param>
        /// <returns>Экспоненциально распределенная величина</returns>
        public double Exponential(double mean)
        {
            // Вместо NextFloat() берется 1 - NextFloat(), так как NextFloat() может быть равно 0
            return -Math.Log(1 - NextFloat()) * mean;
        }

        /// <summary>
        /// Возвращает вещественную величину, распределенную в соответствии с законом Эрланга.
        /// Она равна сумме count величин, распределенных экспоненциально с математическим ожиданием mean каждая.
        /// </summary>
        /// <param name="mean">Математическое ожидание отдельного слагаемого</param>
        /// <param name="count">Количество слагаемых</param>
        /// <returns>Величина, распределенная в соответствии с законом Эрланга</returns>
        public double Erlang(double mean, int count)
        {
            double Res = 1;
            for (int i = 0; i < count; i++)
                // Вместо NextFloat() берется 1 - NextFloat()
                Res *= 1 - NextFloat();
            return -mean * Math.Log(Res);
        }

        /// <summary>
        /// Возвращает целочисленное значение, распределенное в соответствии с законом Пуассона
        /// </summary>
        /// <param name="mean">Математическое ожидание</param>
        /// <returns>Целочисленное значение, распределенно в соответствии с законом Пуассона</returns>
        public int Poisson(double mean)
        {
            double Border = Math.Exp(-mean);
            double Prod = NextFloat();
            int i;
            for (i = 0; Prod >= Border; i++)
                Prod *= NextFloat();
            return i;
        }

        /// <summary>
        /// Возвращает вещественную величину, распределенную треугольно
        /// </summary>
        /// <param name="min">Нижняя граница</param>
        /// <param name="moda">Мода, то есть значение, для которого плотность вероятности распределения максимальна</param>
        /// <param name="max">Верхняя граница</param>
        /// <returns>Треугольно распределенная величина</returns>
        public double Triangular(double min, double moda, double max)
        {
            double Rand = NextFloat();
            if (Rand <= (moda - min) / (max - min))
                return min + Math.Sqrt((moda - min) * (max - min) * Rand);
            else
                return max - Math.Sqrt((max - moda) * (max - min) * (1 - Rand));
        }
    }
}
