using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс ServiceStatistics собирает статистику по обслуживающим действиям
    /// </summary>
    public class ServiceStatistics
    {
        /// <summary>
        /// Конструктор. Создает объект статистики обслуживающего действия для одного устройства в текущий момент имитационного времени.
        /// </summary>
        public ServiceStatistics()
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = 1;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики обслуживающего действия в текущий момент имитационного времени.
        /// </summary>
        /// <param name="initDevices">Начальное количество обслуживающих устройств</param>
        public ServiceStatistics(int initDevices)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = initDevices;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики обслуживающего действия в заданный момент имитационного времени.
        /// </summary>
        /// <param name="initDevices">Начальное количество обслуживающих устройств</param>
        /// <param name="initUtil">Начальное количество занятых устройств</param>
        /// <param name="initTime">Момент времени, когда создается объект</param>
        public ServiceStatistics(int initDevices, int initUtil, double initTime)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = initTime;
            MinBusy = Devices = initDevices;
            Running = initUtil;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики обслуживающего действия в заданный момент имитационного времени.
        /// </summary>
        /// <param name="initDevices">Начальное количество обслуживающих устройств</param>
        /// <param name="initUtil">Начальное количество занятых устройств</param>
        /// <param name="initTime">Момент времени, когда создается объект</param>
        /// <param name="aHeader">Заголовок при отображении статистики на экране</param>
        public ServiceStatistics(int initDevices, int initUtil, double initTime, string aHeader)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = initTime;
            MinBusy = Devices = initDevices;
            Running = initUtil;
            Header = aHeader;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики обслуживающего действия в текущий момент имитационного времени.
        /// </summary>
        /// <param name="initDevices">Начальное количество обслуживающих устройств</param>
        /// <param name="aHeader">Заголовок при отображении статистики на экране</param>
        public ServiceStatistics(int initDevices, string aHeader)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = initDevices;
            Header = aHeader;
        }

        /// <summary>
        /// Конструктор. Создает объект статистики обслуживающего действия для одного устройства в текущий момент имитационного времени.
        /// </summary>
        /// <param name="aHeader">Заголовок при отображении статистики на экране</param>
        public ServiceStatistics(string aHeader)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = 1;
            Header = aHeader;
        }

        /// <summary>
        /// Заголовок для вывода статистики на экран
        /// </summary>
        public string Header;

        /// <summary>
        /// Последнее время нахождения в заблокированном состоянии
        /// </summary>
        private double LastBlockTime;

        /// <summary>
        /// Момент начала занятого состояния
        /// </summary>
        private double LastBusyStart;

        /// <summary>
        /// Последнее время нахождения в занятом состоянии
        /// </summary>
        private double LastBusyTime;

        /// <summary>
        /// Момент начала свободного состояния
        /// </summary>
        private double LastIdleStart;

        /// <summary>
        /// Последнее время нахождения в свободном состоянии
        /// </summary>
        internal double LastIdleTime;

        /// <summary>
        /// Момент последнего изменения величины
        /// </summary>
        private double LastTime;

        /// <summary>
        /// Суммарное время нахождения в заблокированном состоянии
        /// </summary>
        private double SumBlockage;

        /// <summary>
        /// Интеграл наблюдаемой величины по времени
        /// </summary>
        private double SumX;

        /// <summary>
        /// Интеграл квадрата наблюдаемой величины по времени
        /// </summary>
        private double SumX_2;

        /// <summary>
        /// Текущее количество заблокированных устройств
        /// </summary>
        public int Blocked
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество доступных устройств
        /// </summary>
        public int Devices
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество завершенный действий
        /// </summary>
        public int Finished
        {
            get;
            private set;
        }

        /// <summary>
        /// Максимальное количество используемых устройств
        /// </summary>
        public int MaxBusy
        {
            get;
            private set;
        }

        /// <summary>
        /// Максимальное время непрерывной занятости
        /// </summary>
        public double MaxBusyTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Максимальное время свободного состояния
        /// </summary>
        public double MaxIdleTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Минимальное количество занятых устройств
        /// </summary>
        public int MinBusy
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество занятых устройств
        /// </summary>
        public int Running
        {
            get;
            private set;
        }

        /// <summary>
        /// Суммарное время занятости обрабатывающих устройств
        /// (одновременная работа нескольких устройств не принимается во внимание)
        /// </summary>
        public double TotalBusyTime
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
        /// Очистка статистики, подготовка к новому сбору данных в текущий момент имитационного времени
        /// </summary>
        public void ClearStat()
        {
            ClearStat(Global.SimTime());
        }

        /// <summary>
        /// Очистка статистики, подготовка к новому сбору данных в заданный момент имитационного времени
        /// </summary>
        /// <param name="newTime"></param>
        public void ClearStat(double newTime)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = 
                MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = TotalBusyTime = 0;
            Finished = 0;
            MaxBusy = MinBusy = Running;
            LastTime = LastBusyStart = LastIdleStart = newTime;
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
            double dt = newTime - LastTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                TotalBusyTime += dt;
                LastTime = newTime;
                if (Running > MaxBusy)
                {
                    MaxBusy = Running;
                }
                else if (Running < MinBusy)
                {
                    MinBusy = Running;
                }
            }
            Running--;
            Finished++;
            if (Running == 0)
            {
                dt = newTime - LastBusyStart;
                if (LastIdleTime == 0)
                {
                    LastBusyTime += dt;
                }
                else
                {
                    LastBusyTime = dt;
                }
                if (LastBusyTime > MaxBusyTime)
                {
                    MaxBusyTime = LastBusyTime;
                }
                LastIdleStart = newTime;
            }
        }

        /// <summary>
        /// Отметить окончание блокировки в текущий момент времени
        /// </summary>
        public void FinishBlock()
        {
            FinishBlock(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// Отметить окончание блокирвки в заданный момент времени
        /// </summary>
        /// <param name="newTime">Время окончания блокировки</param>
        public void FinishBlock(double newTime)
        {
            double dt = newTime - LastBlockTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumBlockage += dt * Blocked;
                LastBlockTime = newTime;
            }
            Blocked--;
        }

        /// <summary>
        /// Возвращает среднее арифметическое количество используемых устройств
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
        /// Возвращает среднее арифметическое количество заблокированных устройств
        /// </summary>
        /// <returns>Среднее арифметическое</returns>
        public double MeanBlockage()
        {
            if (TotalTime == 0)
                return 0;
            else
                return SumBlockage / TotalTime;
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
            double dt = newTime - LastTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                LastTime = newTime;
                if (Running > MaxBusy)
                {
                    MaxBusy = Running;
                }
                else if (Running < MinBusy)
                {
                    MinBusy = Running;
                }
            }
            Running++;
            if (Running == 1)
            {
                dt = newTime - LastIdleStart;
                if (LastBusyTime == 0)
                {
                    LastIdleTime += dt;
                }
                else
                {
                    LastIdleTime = dt;
                }
                if (LastIdleTime > MaxIdleTime)
                {
                    MaxIdleTime = LastIdleTime;
                }
                LastBusyStart = newTime;
            }
            else
            {
                TotalBusyTime += dt;
            }
        }

        /// <summary>
        /// Отметить начало блокировки в текущий момент времени
        /// </summary>
        public void StartBlock()
        {
            StartBlock(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// Отметить начало блокировки в заданный момент времени
        /// </summary>
        /// <param name="newTime">Время начала действия</param>
        public void StartBlock(double newTime)
        {
            double dt = newTime - LastBlockTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumBlockage += dt * Blocked;
                LastBlockTime = newTime;
            }
            Blocked++;
        }

        /// <summary>
        /// Коррекция статистики к текущему имитационному времени.
        /// Учитывается интервал времени, прошедший с момента последнего изменения или коррекции.
        /// </summary>
        public void StopStat()
        {
            StopStat(Global.SimTime());
        }

        /// <summary>
        /// Коррекция статистики к заданному имитационному времени.
        /// Учитывается интервал времени, прошедший с момента последнего изменения или коррекции.
        /// </summary>
        /// <param name="newTime">Имитационное время момента коррекции статистики</param>
        public void StopStat(double newTime)
        {
            double dt = newTime - LastTime;
            if (dt < 0)
            {
                return;
            }
            if (dt > 0)
            {
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                LastTime = newTime;
                if (Running > MaxBusy)
                {
                    MaxBusy = Running;
                }
                else if (Running < MinBusy)
                {
                    MinBusy = Running;
                }
                dt = newTime - LastBlockTime;
                SumBlockage += dt * Blocked;
                LastBlockTime = newTime;
            }
            if (Running == 0)
            {
                dt = newTime - LastIdleStart;
                if (LastBusyTime == 0)
                {
                    LastIdleTime += dt;
                }
                else
                {
                    LastIdleTime = dt;
                }
                if (LastIdleTime > MaxIdleTime)
                {
                    MaxIdleTime = LastIdleTime;
                }
                LastBusyTime = 0;
                LastIdleStart = newTime;
            }
            else
            {
                TotalBusyTime += dt;
                dt = newTime - LastBusyStart;
                if (LastIdleTime == 0)
                {
                    LastBusyTime += dt;
                }
                else
                {
                    LastBusyTime = dt;
                }
                if (LastBusyTime > MaxBusyTime)
                {
                    MaxBusyTime = LastBusyTime;
                }
                LastIdleTime = 0;
                LastBusyStart = newTime;
            }
        }

        /// <summary>
        /// Преобразует содержимое статистики в текст для отображения на экране
        /// </summary>
        /// <returns>Преобразованное содержимое</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("Количество устройств = {0,2}\n", Devices);
            Result.AppendFormat("Занятость = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("Сейчас выполняется {0,2} действий, завершено {1,2}\n", Running, Finished);
            Result.AppendFormat("Средняя блокировка = {0,6:0.000}\n", MeanBlockage());
            if (Devices == 1)
            {
                Result.AppendFormat("Максимальное время простоя = {0,6:0.000}, максимальное время работы = {1,6:0.000}",
                    MaxIdleTime, MaxBusyTime);
            }
            else
            {
                Result.AppendFormat("Максимальное число свободных устройств = {0,2:0.}\n", Devices - MinBusy);
                Result.AppendFormat("Максимальное число занятых устройств = {0,2:0.}", MaxBusy);
            }
            return Result.ToString();
        }
    }
}
