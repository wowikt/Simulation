using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс List - список узлов. Непосредственно сам объект является заголовочной ячейкой списка.
    /// </summary>
    public class List : ILinkage
    {
        /// <summary>
        /// Конструктор по умолчанию. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации.
        /// Функция сравнения не задается.
        /// Максимальная длина не установлена.
        /// </summary>
        public List()
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации.
        /// Функция сравнения задается параметром.
        /// Максимальная длина не установлена.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        public List(CompareFunction order)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации, и заданной максимальной длиной.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="max">Максимальный размер очереди</param>
        public List(CompareFunction order, int max)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к заданному моменту 
        /// имитационного времени и заданной максимальной длиной.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="max">Максимальный размер очереди</param>
        /// <param name="simTime">Имитационное время, соответствующее созданию списка</param>
        public List(CompareFunction order, int max, double simTime)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к заданному моменту имитационного времени 
        /// и заданной максимальной длиной.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="max">Максимальный размер очереди</param>
        /// <param name="simTime">Имитационное время, соответствующее созданию списка</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(CompareFunction order, int max, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации, и заданной максимальной длиной.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="max">Максимальный размер очереди</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(CompareFunction order, int max, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации.
        /// Функция сравнения задается параметром.
        /// Максимальная длина не установлена.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(CompareFunction order, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к заданному моменту имитационного времени 
        /// и заданной максимальной длиной.
        /// Функция сравнения не задается.
        /// </summary>
        /// <param name="max">Максимальный размер очереди</param>
        /// <param name="simTime">Имитационное время, соответствующее созданию списка</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(int max, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор по умолчанию. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации, и заданной максимальной длиной.
        /// Функция сравнения не задается.
        /// </summary>
        /// <param name="max">Максимальный размер очереди</param>
        public List(int max)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор с указанием заголовка. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации, и заданной максимальной длиной.
        /// Функция сравнения не задается.
        /// </summary>
        /// <param name="max">Максимальный размер очереди</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(int max, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор с указанием заголовка. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации.
        /// Функция сравнения не задается.
        /// Максимальная длина не установлена.
        /// </summary>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
        }

        /// <summary>
        /// Делегат функции сравнения, определяющий упорядоченность списка
        /// </summary>
        protected internal CompareFunction CompFunc = null;

        /// <summary>
        /// Поле связи. Ссылка на предыдущий узел списка
        /// </summary>
        internal ILinkage FPrev;

        /// <summary>
        /// Поле связи. Ссылка на следующий узел списка
        /// </summary>
        internal ILinkage FNext;

        /// <summary>
        /// Статистика по длине списка
        /// </summary>
        public IntervalStatistics LengthStat;

        /// <summary>
        /// Максимально возможный размер очереди. Методы классов List и Link 
        /// не учитывают его, однако это значение может учитываться методами классов
        /// и компонентов при работе с очередями. 
        /// Значение, равное 0, означает отсутствие ограничения на длину.
        /// </summary>
        public int MaxSize;

        /// <summary>
        /// Заголовок при выводе статистики списка
        /// </summary>
        public string StatHeader;

        /// <summary>
        /// Статистика по времени нахождения узлов в очереди
        /// </summary>
        public Statistics TimeStat;

        /// <summary>
        /// Ссылка на первый узел списка.
        /// </summary>
        public ILink First
        {
            get
            {
                return Next;
            }
        }

        /// <summary>
        /// Ссылка на заголовочную ячейку списка
        /// </summary>
        public List Header
        {
            get;
            internal set;
        }

        /// <summary>
        /// Ссылка на последний узел списка.
        /// </summary>
        public ILink Last
        {
            get
            {
                return Prev;
            }
        }

        /// <summary>
        /// Только для чтения. Ссылка на следующий узел, 
        /// если он является внутренней ячейкой списка.
        /// В пртивном случае - null.
        /// </summary>
        public ILink Next
        {
            get
            {
                if (FNext is ILink)
                    return FNext as ILink;
                else
                    return null;
            }
        }

        /// <summary>
        /// Установка делегата функции сравнения. 
        /// Возможна только для пустого списка, для которого эта функция еще не была задана.
        /// Если любое из этих условий нарушается, не выполняется никаких действий.
        /// </summary>
        public CompareFunction OrderFunc
        {
            set
            {
                if (CompFunc == null && Empty())
                {
                    CompFunc = value;
                }
            }
        }

        /// <summary>
        /// Только для чтения. Ссылка на предыдущий узел, 
        /// если он является внутренней ячейкой списка.
        /// В противном случае - null.
        /// </summary>
        public ILink Prev
        {
            get
            {
                if (FPrev is ILink)
                    return FPrev as ILink;
                else
                    return null;
            }
        }

        /// <summary>
        /// Количество узлов списка
        /// </summary>
        public int Size
        {
            get;
            internal set;
        }

        /// <summary>
        /// Очистка списка с завершением всех входящих в него узлов
        /// </summary>
        public void Clear()
        {
            while (!Empty())
                First.Finish();
        }

        /// <summary>
        /// Очистка статистик списка с привязкой к текущему имитационному времени
        /// </summary>
        public void ClearStat()
        {
            ClearStat(Global.SimTime());
        }

        /// <summary>
        /// Очистка статистик списка с привязкой к заданному имитационному времени
        /// </summary>
        /// <param name="simTime">Имитационное время, когда выполняется очистка статистик</param>
        public void ClearStat(double simTime)
        {
            TimeStat.ClearStat();
            LengthStat.ClearStat(simTime);
        }

        /// <summary>
        /// Проверка списка на пустоту
        /// </summary>
        /// <returns>true, если список пуст. false, если в нем есть хотя бы один узел.</returns>
        public bool Empty()
        {
            return FNext == this;
        }

        /// <summary>
        /// Удаление списка
        /// </summary>
        public virtual void Finish()
        {
            // Очистить список
            Clear();
            FNext = null;
            FPrev = null;
        }

        /// <summary>
        /// Получение ссылки на следующий узел списка независимо от того, является он внутренней или заголовочной ячейкой
        /// </summary>
        /// <returns>Ссылка на следующий узел</returns>
        public ILinkage GetNext()
        {
            return FNext;
        }

        /// <summary>
        /// Получение ссылки на предыдущий узел списка независимо от того, является он внутренней или заголовочной ячейкой
        /// </summary>
        /// <returns>Ссылка на предыдущий узел</returns>
        public ILinkage GetPrev()
        {
            return FPrev;
        }

        /// <summary>
        /// Установка ссылки на следующий узел списка
        /// </summary>
        /// <param name="newNext">Новая ссылка на следующий узел</param>
        public void SetNext(ILinkage newNext)
        {
            FNext = newNext;
        }

        /// <summary>
        /// Установка ссылки на предыдущий узел списка
        /// </summary>
        /// <param name="newPrev">Новая ссылка на предыдущий узел</param>
        public void SetPrev(ILinkage newPrev)
        {
            FPrev = newPrev;
        }

        /// <summary>
        /// Отображение статистики по использованию списка
        /// </summary>
        /// <returns>Статистика в виде текста</returns>
        public string Statistics()
        {
            StringBuilder Result = new StringBuilder(StatHeader + "\n");
            Result.AppendFormat("Средняя длина = {0,6:0.000} +/- {1,6:0.000}\n", LengthStat.Mean(), LengthStat.Deviation());
            Result.AppendFormat("Максимальная длина = {0,2}, сейчас = {1,2}\n", LengthStat.Max, Size);
            Result.AppendFormat("Среднее время ожидания = {0,6:0.000}", TimeStat.Mean());
            return Result.ToString();
        }

        /// <summary>
        /// Коррекция статистик списка к текущему имитационному времени
        /// </summary>
        public void StopStat()
        {
            StopStat(Global.SimTime());
        }

        /// <summary>
        /// Коррекция статистик списка к заданному имитационному времени
        /// </summary>
        /// <param name="simTime">Имитационное время, к которому корректируется статистика</param>
        public void StopStat(double simTime)
        {
            LengthStat.StopStat(simTime);
        }
    }
}
