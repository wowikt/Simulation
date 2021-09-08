using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс Queue - список узлов. Непосредственно сам объект является заголовочной ячейкой списка.
    /// </summary>
    public class Queue : ILinkage
    {
        /// <summary>
        /// Конструктор по умолчанию. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации.
        /// Функция сравнения не задается.
        /// Максимальная длина не установлена.
        /// </summary>
        public Queue()
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            HeaderNode = this;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации.
        /// Функция сравнения задается параметром.
        /// Максимальная длина не установлена.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        public Queue(CompareFunction order)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            HeaderNode = this;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации, и заданной максимальной длиной.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="max">Максимальный размер очереди</param>
        public Queue(CompareFunction order, int max)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            HeaderNode = this;
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
        public Queue(CompareFunction order, int max, double simTime)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            HeaderNode = this;
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
        public Queue(CompareFunction order, int max, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = aHeader;
            HeaderNode = this;
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
        public Queue(CompareFunction order, int max, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
            HeaderNode = this;
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
        public Queue(CompareFunction order, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
            HeaderNode = this;
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
        public Queue(int max, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = aHeader;
            HeaderNode = this;
            MaxSize = max;
        }

        /// <summary>
        /// Конструктор по умолчанию. 
        /// Список создается с привязкой к моменту имитационного времени, 
        /// соответствующему текущему процессу имитации, и заданной максимальной длиной.
        /// Функция сравнения не задается.
        /// </summary>
        /// <param name="max">Максимальный размер очереди</param>
        public Queue(int max)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            HeaderNode = this;
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
        public Queue(int max, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
            HeaderNode = this;
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
        public Queue(string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
            HeaderNode = this;
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
        /// Статистика по длине списка
        /// </summary>
        public IntervalStatistics LengthStat;

        /// <summary>
        /// Максимально возможный размер очереди. Методы классов Queue и Link 
        /// не учитывают его, однако это значение может учитываться методами классов
        /// и компонентов при работе с очередями. 
        /// Значение, равное 0, означает отсутствие ограничения на длину.
        /// </summary>
        public int MaxSize;

        /// <summary>
        /// Заголовок при выводе статистики списка
        /// </summary>
        public string Header;

        /// <summary>
        /// Статистика по времени нахождения узлов в очереди
        /// </summary>
        public Statistics TimeStat;

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
        public Queue HeaderNode
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
        /// Вставка в список нового узла
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        /// <returns>true, если узел был вставлен. 
        /// false, если попытка вставки оказалась неудачной по причине переполнения списка</returns>
        public void Insert(ILink inserted)
        {
            // Если функция сравнения в списке не указана, поместить на последнее место
            if (CompFunc == null)
                InsertLast(inserted);
            else
            {
                // Найти место вставки
                ILink lnk = First;
                while (lnk != null)
                {
                    if (CompFunc(inserted, lnk))
                        break;
                    lnk = lnk.Next;
                }
                // Если список закончен, вставить в конец
                if (lnk == null)
                    InsertLast(inserted);
                // Иначе вставить перед найденной ячейкой
                else
                    inserted.InsertBefore(lnk);
            }
        }

        /// <summary>
        /// Вставка в список нового узла с указанной функцией сравнения
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        /// <param name="cmp">Функция сравнения, используемая при вставке</param>
        /// <returns>true, если узел был вставлен. 
        /// false, если попытка вставки оказалась неудачной по причине переполнения списка</returns>
        public void Insert(ILink inserted, CompareFunction cmp)
        {
            // Найти место вставки
            ILink lnk = First;
            while (lnk != null)
            {
                if (cmp(inserted, lnk))
                    break;
                lnk = lnk.Next;
            }
            // Если список закончен, вставить в конец
            if (lnk == null)
                InsertLast(inserted);
            // Иначе вставить перед найденной ячейкой
            else
                inserted.InsertBefore(lnk);
        }

        /// <summary>
        /// Вставка нового узла в начало списка
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        /// <returns>true, если узел был вставлен. 
        /// false, если попытка вставки оказалась неудачной по причине переполнения списка</returns>
        public void InsertFirst(ILink inserted)
        {
            // Вставка на первое место - это вставка ПОСЛЕ заголовочной ячейки
            inserted.InsertAfter(this);
        }

        /// <summary>
        /// Вставка нового узла в конец списка
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        /// <returns>true, если узел был вставлен. 
        /// false, если попытка вставки оказалась неудачной по причине переполнения списка</returns>
        public void InsertLast(ILink inserted)
        {
            // Вставка на первое место - это вставка ПОСЛЕ заголовочной ячейки
            inserted.InsertBefore(this);
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
            StringBuilder Result = new StringBuilder(Header + "\n");
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

    /// <summary>
    /// Типизированный класс для представления списков, элементами которых 
    /// могут являться объекты только определенного типа или производных от него
    /// </summary>
    /// <typeparam name="T">Тип элементов списка. Должен реализовать интерфейс ILink.
    /// Фактически, как правило, будут указываться классы, производные 
    /// от Component и Process.</typeparam>
    public class Queue<T> : Queue
        where T : class, ILink
    {

        /// <summary>
        /// Делегат для определения функции сравнения, определяющей место вставки узла в список.
        /// Вставляемый узел помещается в список перед первым узлом, для которого функция дает результат true.
        /// </summary>
        /// <param name="a">Ссылка на вставляемый узел</param>
        /// <param name="b">Ссылка на сравниваемый узел списка</param>
        /// <returns>Результат сравнения</returns>
        public new delegate bool CompareFunction(T a, T b);

        /// <summary>
        /// Делегат функции сравнения, определяющий упорядоченность списка
        /// </summary>
        protected internal new CompareFunction CompFunc = null;

        /// <summary>
        /// Ссылка на первый узел списка.
        /// </summary>
        public new T First
        {
            get
            {
                return Next;
            }
        }

        /// <summary>
        /// Ссылка на последний узел списка.
        /// </summary>
        public new T Last
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
        public new T Next
        {
            get
            {
                if (FNext is T)
                    return FNext as T;
                else
                    return null;
            }
        }

        /// <summary>
        /// Установка делегата функции сравнения. 
        /// Возможна только для пустого списка, для которого эта функция еще не была задана.
        /// Если любое из этих условий нарушается, не выполняется никаких действий.
        /// </summary>
        public new CompareFunction OrderFunc
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
        public new T Prev
        {
            get
            {
                if (FPrev is T)
                    return FPrev as T;
                else
                    return null;
            }
        }

        /// <summary>
        /// Вставка в список нового узла
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        public void Insert(T inserted)
        {
            // Если функция сравнения в списке не указана, поместить на последнее место
            if (CompFunc == null)
                InsertLast(inserted);
            else
            {
                // Найти место вставки
                T lnk = First;
                while (lnk != null)
                {
                    if (CompFunc(inserted, lnk))
                        break;
                    lnk = lnk.Next as T;
                }
                // Если список закончен, вставить в конец
                if (lnk == null)
                    InsertLast(inserted);
                // Иначе вставить перед найденной ячейкой
                else
                    inserted.InsertBefore(lnk);
            }
        }

        /// <summary>
        /// Вставка в список нового узла с указанной функцией сравнения
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        /// <param name="cmp">Функция сравнения, используемая при вставке</param>
        public void Insert(T inserted, CompareFunction cmp)
        {
            // Найти место вставки
            T lnk = First;
            while (lnk != null)
            {
                if (cmp(inserted, lnk))
                    break;
                lnk = lnk.Next as T;
            }
            // Если список закончен, вставить в конец
            if (lnk == null)
                InsertLast(inserted);
            // Иначе вставить перед найденной ячейкой
            else
                inserted.InsertBefore(lnk);
        }

        /// <summary>
        /// Вставка нового узла в начало списка
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        public void InsertFirst(T inserted)
        {
            // Вставка на первое место - это вставка ПОСЛЕ заголовочной ячейки
            inserted.InsertAfter(this);
        }

        /// <summary>
        /// Вставка нового узла в конец списка
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        public void InsertLast(T inserted)
        {
            // Вставка на первое место - это вставка ПОСЛЕ заголовочной ячейки
            inserted.InsertBefore(this);
        }

        /// <summary>
        /// Вставка в список нового узла без контроля типа. Всегда порождает исключение
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        public new void Insert(ILink inserted)
        {
            throw new ESimulationException(
                "Queue<T>.Insert(): нельзя вставлять в типизированный список " + 
                "элемент неподходящего типа");
        }

        /// <summary>
        /// Вставка в список нового узла с указанной функцией сравнения 
        /// без контроля типа. Всегда порождает исключение
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        /// <param name="cmp">Функция сравнения, используемая при вставке</param>
        public new void Insert(ILink inserted, Queue.CompareFunction cmp)
        {
            throw new ESimulationException(
                "Queue<T>.Insert(): нельзя вставлять в типизированный список " + 
                "элемент неподходящего типа");
        }

        /// <summary>
        /// Вставка нового узла в начало списка
        /// без контроля типа. Всегда порождает исключение
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        public new void InsertFirst(ILink inserted)
        {
            throw new ESimulationException(
                "Queue<T>.Insert(): нельзя вставлять в типизированный список " + 
                "элемент неподходящего типа");
        }

        /// <summary>
        /// Вставка нового узла в конец списка
        /// без контроля типа. Всегда порождает исключение
        /// </summary>
        /// <param name="inserted">Вставляемый узел</param>
        public new void InsertLast(ILink inserted)
        {
            throw new ESimulationException(
                "Queue<T>.Insert(): нельзя вставлять в типизированный список " + 
                "элемент неподходящего типа");
        }
    }
}
