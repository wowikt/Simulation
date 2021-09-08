using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс ActionStatistics собирает статистику по действиям
    /// </summary>
    public class ActionStatistics
    {
        /// <summary>
        /// Конструктор. Создает объект статистики действия 
        /// в текущий момент имитационного времени.
        /// </summary>
        public ActionStatistics()
        {
            Running = 0;
            LastTime = Global.SimTime();
        }

        /// <summary>
        /// Конструктор. Создает объект статистики действия 
        /// в текущий момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        public ActionStatistics(int initX)
        {
            Running = initX;
            LastTime = Global.SimTime();
        }

        /// <summary>
        /// Конструктор. Создает объект статистики действия 
        /// в заданный момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        /// <param name="initTime">Момент времени, когда создается объект</param>
        public ActionStatistics(int initX, double initTime)
        {
            Running = initX;
            LastTime = initTime;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики действия 
        /// в заданный момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        /// <param name="initTime">Момент времени, когда создается объект</param>
        /// <param name="aHeader">Заголовок для вывода статистики</param>
        public ActionStatistics(int initX, double initTime, string aHeader)
        {
            Running = initX;
            LastTime = initTime;
            Header = aHeader;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики действия 
        /// в текущий момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        /// <param name="aHeader">Заголовок для вывода статистики</param>
        public ActionStatistics(int initX, string aHeader)
        {
            Running = initX;
            LastTime = Global.SimTime();
            Header = aHeader;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики действия 
        /// в текущий момент имитационного времени.
        /// </summary>
        /// <param name="aHeader">Заголовок для вывода статистики</param>
        public ActionStatistics(string aHeader)
        {
            Running = 0;
            LastTime = Global.SimTime();
            Header = aHeader;
        }

        /// <summary>
        /// Заголовок для вывода статистики на экран
        /// </summary>
        public string Header;

        /// <summary>
        /// Момент последнего изменения величины
        /// </summary>
        private double LastTime;

        /// <summary>
        /// Интеграл наблюдаемой величины по времени
        /// </summary>
        private double SumX;

        /// <summary>
        /// Интеграл квадрата наблюдаемой величины по времени
        /// </summary>
        private double SumX_2;

        /// <summary>
        /// Количество завершенный действий
        /// </summary>
        public int Finished
        {
            get;
            private set;
        }

        /// <summary>
        /// Максимальное значение среди накопленных
        /// </summary>
        public int Max
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество исполняемых действий
        /// </summary>
        public int Running
        {
            get;
            private set;
        }

        /// <summary>
        /// Общее время наблюдения со сбором статистики
        /// </summary>
        public double TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Очистка статистики, подготовка к новому сбору данных 
        /// в текущий момент имитационного времени
        /// </summary>
        public void ClearStat()
        {
            ClearStat(Global.SimTime());
        }

        /// <summary>
        /// Очистка статистики, подготовка к новому сбору данных 
        /// в заданный момент имитационного времени
        /// </summary>
        /// <param name="newTime"></param>
        public void ClearStat(double newTime)
        {
            SumX = SumX_2 = TotalTime = Finished = Max = 0;
            LastTime = newTime;
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
            if (TotalTime == 0)
                return 0;
            else
                return SumX_2 / TotalTime - Mean() * Mean();
        }

        /// <summary>
        /// Отметить окончание действия в текущий момент времени
        /// </summary>
        public void Finish()
        {
            Finish(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// Отметить окончание действия в заданный момент времени
        /// </summary>
        /// <param name="newTime">Время окончания действия</param>
        public void Finish(double newTime)
        {
            if (newTime > LastTime)
            {
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
            Running--;
            Finished++;
        }

        /// <summary>
        /// Возвращает среднее арифметическое по накопленным данным
        /// </summary>
        /// <returns>Среднее арифметическое</returns>
        public double Mean()
        {
            if (TotalTime == 0)
                return 0;
            else
                return SumX / TotalTime;
        }

        /// <summary>
        /// Отметить приостановку действия в текущий момент времени
        /// </summary>
        public void Preempt()
        {
            Preempt(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// Отметить приостановку действия в заданный момент времени
        /// </summary>
        /// <param name="newTime">Время приостановки действия</param>
        public void Preempt(double newTime)
        {
            if (newTime > LastTime)
            {
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
            Running--;
        }

        /// <summary>
        /// Отметить возобновление действия в текущий момент времени
        /// </summary>
        public void Resume()
        {
            Resume(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// Отметить возобновление действия в заданный момент времени
        /// </summary>
        /// <param name="newTime">Время возобновления действия</param>
        public void Resume(double newTime)
        {
            Start(newTime);
        }

        /// <summary>
        /// Отметить начало действия в текущий момент времени
        /// </summary>
        public void Start()
        {
            Start(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// Отметить начало действия в заданный момент времени
        /// </summary>
        /// <param name="newTime">Время начала действия</param>
        public void Start(double newTime)
        {
            if (newTime > LastTime)
            {
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
            Running++;
        }

        /// <summary>
        /// Коррекция статистики к текущему имитационному времени.
        /// Учитывается интервал времени, прошедший с момента 
        /// последнего изменения или коррекции.
        /// </summary>
        public void StopStat()
        {
            StopStat(Global.SimTime());
        }

        /// <summary>
        /// Коррекция статистики к заданному имитационному времени.
        /// Учитывается интервал времени, прошедший с момента 
        /// последнего изменения или коррекции.
        /// </summary>
        /// <param name="newTime">Имитационное время 
        /// момента коррекции статистики</param>
        public void StopStat(double newTime)
        {
            if (newTime > LastTime)
            {
                // Время прошло, величина не изменилась
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
        }

        /// <summary>
        /// Преобразует содержимое статистики в текст для отображения на экране
        /// </summary>
        /// <returns>Преобразованное содержимое</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("Среднее = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("Максимум = {0,2}\n", Max);
            Result.AppendFormat("Сейчас выполняется {0,2} действий, завершено {1,2}", 
                Running, Finished);
            return Result.ToString();
        }
    }
}
