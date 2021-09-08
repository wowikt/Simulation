using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Интерфейс ILinkage - базовый для построения связных списков и многих объектов моделирования
    /// </summary>
    public interface ILinkage
    {
        /// <summary>
        /// Ссылка на заголовочную ячейку списка
        /// </summary>
        List Header
        {
            get;
        }

        /// <summary>
        /// Только для чтения. Ссылка на предыдущий узел, 
        /// если он является внутренней ячейкой списка.
        /// В противном случае - null.
        /// </summary>
        ILink Prev
        {
            get;
        }

        /// <summary>
        /// Только для чтения. Ссылка на следующий узел, 
        /// если он является внутренней ячейкой списка.
        /// В пртивном случае - null.
        /// </summary>
        ILink Next
        {
            get;
        }

        /// <summary>
        /// Заготовка для метода завершения работы объекта.
        /// </summary>
        void Finish();

        /// <summary>
        /// Получение ссылки на следующий узел списка независимо от того, является он внутренней или заголовочной ячейкой
        /// </summary>
        /// <returns>Ссылка на следующий узел</returns>
        ILinkage GetNext();

        /// <summary>
        /// Получение ссылки на предыдущий узел списка независимо от того, является он внутренней или заголовочной ячейкой
        /// </summary>
        /// <returns>Ссылка на предыдущий узел</returns>
        ILinkage GetPrev();

        /// <summary>
        /// Установка ссылки на следующий узел списка
        /// </summary>
        /// <param name="newNext">Новая ссылка на следующий узел</param>
        void SetNext(ILinkage newNext);

        /// <summary>
        /// Установка ссылки на предыдущий узел списка
        /// </summary>
        /// <param name="newPrev">Новая ссылка на предыдущий узел</param>
        void SetPrev(ILinkage newPrev);
    }

    /// <summary>
    /// Делегат для определения функции сравнения, определяющей место вставки узла в список.
    /// Вставляемый узел помещается в список перед первым узлом, для которого функция дает результат true.
    /// </summary>
    /// <param name="a">Ссылка на вставляемый узел</param>
    /// <param name="b">Ссылка на сравниваемый узел списка</param>
    /// <returns>Результат сравнения</returns>
    public delegate bool CompareFunction(ILink a, ILink b);

    /// <summary>
    /// Интерфейс ILink содержит определения, обязательные для любых объектов, которые могут быть вставлены в список
    /// </summary>
    public interface ILink : ILinkage
    {
        /// <summary>
        /// Имитационное время вставки узла в список.
        /// Используется для сбора статистики по времени нахождения узлов в списке.
        /// </summary>
        double InsertTime
        {
            get;
            //set;
        }

        /// <summary>
        /// Проверка, является ли ячейка первой в списке
        /// </summary>
        bool IsFirst
        {
            get;
        }

        /// <summary>
        /// Проверка, является ли ячейка последней в списке
        /// </summary>
        bool IsLast
        {
            get;
        }

        /// <summary>
        /// Возвращает ссылку на заголовочную ячейку списка, в котором находится узел
        /// </summary>
        /// <returns>Ссылка на заголовочную ячейку</returns>
        List GetHeader();

        /// <summary>
        /// Вставка узла в список. 
        /// Если для узла задана собственная функция сравнения, вставка производится с ее использованием.
        /// В противном случае узел вставляется в список последним
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется текущий узел</param>
        void Insert(List l);

        /// <summary>
        /// Вставка узла в список с использованием указанной функции сравнения
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        /// <param name="cmp">Функция сравнения, которую следует использоватьпри вставке узла</param>
        void Insert(List l, CompareFunction cmp);

        /// <summary>
        /// Вставка узла в список после указанного
        /// </summary>
        /// <param name="l">Узел, после которого следует вставлять текущий</param>
        void InsertAfter(ILinkage l);

        /// <summary>
        /// Вставка узла в список перед указанным
        /// </summary>
        /// <param name="l">Узел, перед которым следует вставлять текущий</param>
        void InsertBefore(ILinkage l);

        /// <summary>
        /// Вставка узла в первую позицию списка
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        void InsertFirst(List l);

        /// <summary>
        /// Вставка узла в последнюю позицию списка
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        void InsertLast(List l);

        /// <summary>
        /// Исключение узла из списка, в котором он находится
        /// </summary>
        void Remove();
    }

    /// <summary>
    /// Класс Link - базовый класс внутреннего узла (ячейки) списка
    /// </summary>
    public class Link : ILink
    {
        /// <summary>
        /// Конструктор по умолчанию. Создается ячейка, не включенная ни в один список
        /// </summary>
        public Link()
        {
            FPrev = FNext = Header = null;
        }

        /// <summary>
        /// Поле связи. Ссылка на предыдущий узел списка
        /// </summary>
        internal ILinkage FPrev;

        /// <summary>
        /// Поле связи. Ссылка на следующий узел списка
        /// </summary>
        internal ILinkage FNext;

        /// <summary>
        /// Ссылка на заголовочную ячейку списка
        /// </summary>
        public List Header
        {
            get;
            internal set;
        }

        /// <summary>
        /// Имитационное время вставки узла в список.
        /// Используется для сбора статистики по времени нахождения узлов в списке.
        /// </summary>
        public double InsertTime
        {
            get;
            internal set;
        }

        /// <summary>
        /// Проверка, является ли ячейка первой в списке
        /// </summary>
        public bool IsFirst
        {
            get
            {
                return (Prev == null);
            }
        }

        /// <summary>
        /// Проверка, является ли ячейка последней в списке
        /// </summary>
        public bool IsLast
        {
            get
            {
                return (Next == null);
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
        /// Завершение работы узла. Узел исключается из списка.
        /// В переопределенном методе производного класса 
        /// ПОСЛЕДНИМ оператором должен быть base.Finish();
        /// </summary>
        public virtual void Finish()
        {
            Remove();
        }

        /// <summary>
        /// Возвращает ссылку на заголовочную ячейку списка, в котором находится узел
        /// </summary>
        /// <returns>Ссылка на заголовочную ячейку</returns>
        public List GetHeader()
        {
            return Header;
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
        /// Вставка узла в список. 
        /// Если для узла задана собственная функция сравнения, вставка производится с ее использованием.
        /// В противном случае узел вставляется в список последним
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется текущий узел</param>
        public void Insert(List l)
        {
            // Если функция сравнения в списке не указана, поместить на последнее место
            if (l.CompFunc == null)
                InsertLast(l);
            else
            {
                // Найти место вставки
                ILink lnk = l.First;
                while (lnk != null)
                {
                    if (l.CompFunc(this, lnk))
                        break;
                    lnk = lnk.Next;
                }
                // Если список закончен, вставить в конец
                if (lnk == null)
                    InsertLast(l);
                // Иначе вставить перед найденной ячейкой
                else
                    InsertBefore(lnk);
            }
        }

        /// <summary>
        /// Вставка узла в список с использованием указанной функции сравнения
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        /// <param name="cmp">Функция сравнения, которую следует использоватьпри вставке узла</param>
        public void Insert(List l, CompareFunction cmp)
        {
            // Найти место вставки
            ILink lnk = l.First;
            while (lnk != null)
            {
                if (cmp(this, lnk))
                    break;
                lnk = lnk.Next;
            }
            // Если список закончен, вставить в конец
            if (lnk == null)
                InsertLast(l);
            // Иначе вставить перед найденной ячейкой
            else
                InsertBefore(lnk);
        }

        /// <summary>
        /// Вставка узла в список после указанного
        /// </summary>
        /// <param name="l">Узел, после которого следует вставлять текущий</param>
        public void InsertAfter(ILinkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FPrev = l;
            FNext = l.GetNext();
            FNext.SetPrev(this);
            l.SetNext(this);
            Header = l.Header;
            Header.Size++;
            Header.LengthStat.AddData(Header.Size, InsertTime);
        }

        /// <summary>
        /// Вставка узла в список перед указанным
        /// </summary>
        /// <param name="l">Узел, перед которым следует вставлять текущий</param>
        public void InsertBefore(ILinkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FNext = l;
            FPrev = l.GetPrev();
            FPrev.SetNext(this);
            l.SetPrev(this);
            Header = l.Header;
            Header.Size++;
            Header.LengthStat.AddData(Header.Size, InsertTime);
        }

        /// <summary>
        /// Вставка узла в первую позицию списка
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        public void InsertFirst(List l)
        {
            // Вставка на первое место - это вставка ПОСЛЕ заголовочной ячейки
            InsertAfter(l);
        }

        /// <summary>
        /// Вставка узла в последнюю позицию списка
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        public void InsertLast(List l)
        {
            // Вставка на последнее место - это вставка ПЕРЕД заголовочной ячейкой
            InsertBefore(l);
        }

        /// <summary>
        /// Исключение узла из списка, в котором он находится
        /// </summary>
        public void Remove()
        {
            if (FNext != null)
            {
                Header.Size--;
                FNext.SetPrev(FPrev);
                FPrev.SetNext(FNext);
                FNext = null;
                FPrev = null;
                Header.TimeStat.AddData(Global.SimTime() - InsertTime);
                Header.LengthStat.AddData(Header.Size, Global.SimTime());
                Header = null;
            }
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
    }

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
