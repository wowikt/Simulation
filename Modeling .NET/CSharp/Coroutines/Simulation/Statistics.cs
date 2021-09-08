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
        /// Сумма величин
        /// </summary>
        protected double SumX;

        /// <summary>
        /// Сумма квадратов величин
        /// </summary>
        protected double SumX_2;

        /// <summary>
        /// Заголовок для вывода статистики на экран
        /// </summary>
        public string Header;

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
        /// Возвращает стандартное отклонение накопленных значений
        /// </summary>
        /// <returns>Стандартное отклонение</returns>
        public double Deviation()
        {
            return Math.Sqrt(Disperse());
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
        /// Максимальное значение среди накопленных
        /// </summary>
        public double Max
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество накопленных значений
        /// </summary>
        public int Count
        {
            get;
            protected set;
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

    /// <summary>
    /// Класс <c>TimeBetStatistics</c> собирает точечную статистику по интервалам времени между событиями
    /// </summary>
    public class TimeBetStatistics : Statistics
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        TimeBetStatistics()
        {
            Count = -1;
            LastTime = -1;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="AHeader">Заголовок для вывода на экран</param>
        TimeBetStatistics(string AHeader)
            : base(AHeader)
        {
            Count = -1;
            LastTime = -1;
        }

        private double LastTime;

        /// <summary>
        /// Добавляет текущий момент имитационного времени, соответствующий активной имитации
        /// </summary>
        public void AddData()
        {
        }

        /// <summary>
        /// Добавляет новое значение как разность между значением параметра и последним добавленным значением
        /// </summary>
        /// <param name="newTime"></param>
        public override void AddData(double newTime)
        {
            if (Count < 0)
            {
                LastTime = newTime;
                Count++;
            }
            else
            {
                double dt = newTime - LastTime;
                base.AddData(dt);
                LastTime = newTime;
            }
        }

        /// <summary>
        /// Приведение статистики в исходное состояние
        /// </summary>
        public override void ClearStat()
        {
            base.ClearStat();
            Count = -1;
            LastTime = -1;
        }
    }

    /// <summary>
    /// Класс <c>IntervalStatistics</c> собирает интервальную статистику по значениям, дисретно меняющимся во времени
    /// </summary>
    public class IntervalStatistics
    {
        /// <summary>
        /// Конструктор. Создает объект интервальной статистики в текущий момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        public IntervalStatistics(double initX)
        {
            LastX = initX;
            LastTime = Global.SimTime();
        }

        /// <summary>
        /// Конструктор. Создает объект интервальной статистики в заданный момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        /// <param name="initTime">Момент времени, когда создается объект</param>
        public IntervalStatistics(double initX, double initTime)
        {
            LastX = initX;
            LastTime = initTime;
        }

        private double SumX;
        private double SumX_2;
        private double LastTime;
        private double LastX;

        /// <summary>
        /// Заголовок для вывода статистики на экран
        /// </summary>
        public string Header;

        /// <summary>
        /// Добавление нового значения наблюдаемой величины к статистике с указанием момента времени, когда оно изменяется.
        /// Фактически учитывается статистика за промежуток времени с последнего изменения до текущего.
        /// Указанное значение запоминается для учета в будущем.
        /// </summary>
        /// <param name="newX">Новое значение наблюдаемой величины</param>
        /// <param name="newTime">Имитационное время в момент изменения</param>
        public void AddData(double newX, double newTime)
        {
            // Промежуток времени с момента последнего обновления
            double dt = newTime - LastTime;
            if (dt < 0)
                // Ошибочное значение
                //   *** (может быть, генерировать исключение?)
                dt = 0;
            // Если с момента последнего изменения прошло ненулевое время
            if (dt > 0)
            {
                // Если это первое изменение величины, инициализировать статистику
                if (TotalTime == 0)
                    Min = Max = LastX;
                // Иначе - скорректировать крайние значения
                else if (LastX < Min)
                    Min = LastX;
                else if (LastX > Max)
                    Max = LastX;
                // Учесть нахождение величины на уровне LastX в течение времени dt
                SumX += dt * LastX;
                SumX_2 += dt * LastX * LastX;
                TotalTime += dt;
            }
            // Запомнить новое состояние
            LastTime = newTime;
            LastX = newX;
        }

        /// <summary>
        /// Добавление нового значения наблюдаемой величины к статистике в текущий момент имитационного времени.
        /// Фактически учитывается статистика за промежуток времени с последнего изменения до текущего.
        /// Указанное значение запоминается для учета в будущем.
        /// </summary>
        /// <param name="newX">Новое значение наблюдаемой величины</param>
        public void AddData(double newX)
        {
            AddData(newX, Global.SimTime());
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
            SumX = SumX_2 = Min = Max = TotalTime = 0;
            LastTime = newTime;
            // Величина LastX не изменяется
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
            // Время прошло, величина не изменилась
            AddData(LastX, newTime);
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
        /// Возвращает стандартное отклонение накопленных значений
        /// </summary>
        /// <returns>Стандартное отклонение</returns>
        public double Deviation()
        {
            return Math.Sqrt(Disperse());
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
        /// Максимальное значение среди накопленных
        /// </summary>
        public double Max
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
        /// Преобразует содержимое статистики в текст для отображения на экране
        /// </summary>
        /// <returns>Преобразованное содержимое</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("Среднее = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("Минимум = {0,6:0.000}, максимум = {1,6:0.000}\n", Min, Max);
            Result.AppendFormat("Всего {0,6:0.000} единиц времени, текущее значение = {1,6:0.000}", TotalTime, LastX);
            return Result.ToString();
        }
    }

    /// <summary>
    /// Класс ActionStatistics собирает статистику по действиям
    /// </summary>
    public class ActionStatistics
    {
        /// <summary>
        /// Конструктор. Создает объект статистики действия в текущий момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        public ActionStatistics(int initX)
        {
            Running = initX;
            LastTime = Global.SimTime();
        }

        /// <summary>
        /// Конструктор. Создает объект статистики действия в заданный момент имитационного времени.
        /// </summary>
        /// <param name="initX">Начальное значение наблюдаемой величины</param>
        /// <param name="initTime">Момент времени, когда создается объект</param>
        public ActionStatistics(int initX, double initTime)
        {
            Running = initX;
            LastTime = initTime;
        }

        private double SumX;
        private double SumX_2;
        private double LastTime;

        /// <summary>
        /// Заголовок для вывода статистики на экран
        /// </summary>
        public string Header;

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
        /// Отметить начало действия в текущий момент времени
        /// </summary>
        public void Start()
        {
            Start(Global.CurrSim.SimTime());
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
        /// Отметить окончание действия в текущий момент времени
        /// </summary>
        public void Finish()
        {
            Finish(Global.CurrSim.SimTime());
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
        /// Отметить приостановку действия в текущий момент времени
        /// </summary>
        public void Preempt()
        {
            Preempt(Global.CurrSim.SimTime());
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
        /// Отметить возобновление действия в текущий момент времени
        /// </summary>
        public void Resume()
        {
            Resume(Global.CurrSim.SimTime());
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
            SumX = SumX_2 = TotalTime = Max = 0;
            LastTime = newTime;
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
        /// Возвращает стандартное отклонение накопленных значений
        /// </summary>
        /// <returns>Стандартное отклонение</returns>
        public double Deviation()
        {
            return Math.Sqrt(Disperse());
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
        /// Общее время наблюдения со сбором статистики
        /// </summary>
        public double TotalTime
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
        /// Количество исполняемых действий
        /// </summary>
        public int Running 
        {
            get;
            private set;
        }

        /// <summary>
        /// Преобразует содержимое статистики в текст для отображения на экране
        /// </summary>
        /// <returns>Преобразованное содержимое</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("Среднее = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("Максимум = {1,6:0.000}\n", Max);
            Result.AppendFormat("Сейчас выполняется {0,2} действий, завершено {1,2}", Running, Finished);
            return Result.ToString();
        }
    }

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
        /// Заголовок гистограммы при выводе
        /// </summary>
        public string Header;

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

        /// <summary>
        /// Очищает гистограмму
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Возвращает долю от общего количества значений, попавших в указанный интервал
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля количества значений</returns>
        public abstract double Percent(int index);

        /// <summary>
        /// Возвращает долю от общего количества значений, попавших в интервалы от левого до указанного
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля количества значений</returns>
        public abstract double CumulativePercent(int index);
    }

    /// <summary>
    /// Класс <c>Histogram</c> собирает данные по количеству значений, попадающих в заданные интервалы
    /// </summary>
    public class Histogram : HistogramBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ALow">Нижняя граница первого конечного интервала</param>
        /// <param name="AStep">Шаг каждого конечного интервала</param>
        /// <param name="AIntervalCount">Количество конечных интервалов</param>
        public Histogram(double ALow, double AStep, int AIntervalCount)
            : base(ALow, AStep, AIntervalCount)
        {
            Data = new int[IntervalCount + 2];
            TotalCount = 0;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ALow">Нижняя граница первого конечного интервала</param>
        /// <param name="AStep">Шаг каждого конечного интервала</param>
        /// <param name="AIntervalCount">Количество конечных интервалов</param>
        /// <param name="AHeader">Заголовок при выводе на экран</param>
        public Histogram(double ALow, double AStep, int AIntervalCount, string AHeader)
            : base(ALow, AStep, AIntervalCount, AHeader)
        {
            Data = new int[IntervalCount + 2];
            TotalCount = 0;
        }

        /// <summary>
        /// Массив, в котором содержится количество значений, попадающее в каждый интервал
        /// </summary>
        private int[] Data;

        /// <summary>
        /// Общее количество записанных значений
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Добавляет к гистограмме новое значение. Увеличивает на 1 элемент массива, соответствующий добавляемой величине, 
        /// и общее количество значений
        /// </summary>
        /// <param name="newData">Добавляемое значение</param>
        public void AddData(double newData)
        {
            Data[IntervalIndex(newData)]++;
            TotalCount++;
        }

        /// <summary>
        /// Очищает гистограмму, записывая нулевые значения во все элементы массива и обнуляя счетчик значений
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i <= IntervalCount + 1; i++)
                Data[i] = 0;
            TotalCount = 0;
        }

        /// <summary>
        /// Возвращает количество значений, попавших в указанный интервал
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Количество значений</returns>
        public int Count(int index)
        {
            if (index <= 0)
                return Data[0];
            else if (index >= IntervalCount + 1)
                return Data[IntervalCount + 1];
            else
                return Data[index];
        }

        /// <summary>
        /// Общее количество значений, попавших в интервалы от крайнего левого до указанного
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Общее количество значений</returns>
        public int CumulativeCount(int index)
        {
            int Sum = 0;
            for (int i = 0; i <= index && i <= IntervalCount + 1; i++)
                Sum += Data[i];
            return Sum;
        }

        /// <summary>
        /// Возвращает долю от общего количества значений, попавших в указанный интервал
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля количества значений</returns>
        public override double Percent(int index)
        {
            if (TotalCount == 0)
                return 0;
            else
                return (double)Count(index) / TotalCount;
        }

        /// <summary>
        /// Возвращает долю от общего количества значений, попавших в интервалы от левого до указанного
        /// </summary>
        /// <param name="index">Индекс интервала</param>
        /// <returns>Доля количества значений</returns>
        public override double CumulativePercent(int index)
        {
            if (TotalCount == 0)
                return 0;
            else
                return (double)CumulativeCount(index) / TotalCount;
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
            Output += string.Format(" -INF - {0,5:0.00} : {1,4} ({2,5:0.00}%), {3,6:0.00}% {4}\n", UpperBound(0), Count(0), Percent(0) * 100, CumulativePercent(0) * 100, Graph);

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
                Output += string.Format("{0,5:0.00} - {1,5:0.00} : {2,4} ({3,5:0.00}%), {4,6:0.00}% {5}\n", LowerBound(i), UpperBound(i), Count(i), Percent(i) * 100, CumulativePercent(i) * 100, Graph);
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
            Output += string.Format("{0,5:0.00} -  +INF : {1,4} ({2,5:0.00}%), {3,6:0.00}% {4}", LowerBound(IntervalCount + 1), Count(IntervalCount + 1),
                Percent(IntervalCount + 1) * 100, CumulativePercent(IntervalCount + 1) * 100, Graph);

            return Output;
        }
    }

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
            Output += string.Format(" -INF - {0,5:0.00} : {1,5:0.0} ({2,5:0.00}%), {3,6:0.00}% {4}\n", UpperBound(0), Time(0), Percent(0) * 100, CumulativePercent(0) * 100, Graph);

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
                Output += string.Format("{0,5:0.00} - {1,5:0.00} : {2,5:0.0} ({3,5:0.00}%), {4,6:0.00}% {5}\n", LowerBound(i), UpperBound(i), Time(i), Percent(i) * 100, CumulativePercent(i) * 100, Graph);
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
            Output += string.Format("{0,5:0.00} -  +INF : {1,5:0.0} ({2,5:0.00}%), {3,6:0.00}% {4}", LowerBound(IntervalCount + 1), Time(IntervalCount + 1),
                Percent(IntervalCount + 1) * 100, CumulativePercent(IntervalCount + 1) * 100, Graph);
            return Output;
        }
    }
}
