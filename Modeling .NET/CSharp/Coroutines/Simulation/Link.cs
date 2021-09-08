using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс Linkage - базовый для построения связных списков и многих объектов моделирования
    /// </summary>
    public class Linkage
    {
        /// <summary>
        /// Поле связи. Ссылка на предыдущий узел списка
        /// </summary>
        internal Linkage FPrev;

        /// <summary>
        /// Поле связи. Ссылка на следующий узел списка
        /// </summary>
        internal Linkage FNext;

        /// <summary>
        /// Конструктор. Устанавливает пустые поля связи
        /// </summary>
        public Linkage()
        {
            FPrev = null;
            FNext = null;
        }

        /// <summary>
        /// Только для чтения. Ссылка на предыдущий узел, 
        /// если он является внутренней ячейкой списка.
        /// В противном случае - null.
        /// </summary>
        public Link Prev
        {
            get
            {
                if (FPrev is Link)
                    return FPrev as Link;
                else
                    return null;
            }
        }

        /// <summary>
        /// Только для чтения. Ссылка на следующий узел, 
        /// если он является внутренней ячейкой списка.
        /// В пртивном случае - null.
        /// </summary>
        public Link Next
        {
            get
            {
                if (FNext is Link)
                    return FNext as Link;
                else
                    return null;
            }
        }

        /// <summary>
        /// Заготовка для метода завершения работы объекта.
        /// В данном классе ничего не делает
        /// </summary>
        public virtual void Finish()
        {
        }
    }

    /// <summary>
    /// Делегат для определения функции сравнения, определяющей место вставки узла в список.
    /// Вставляемый узел помещается в список перед первым узлом, для которого функция дает результат true.
    /// </summary>
    /// <param name="a">Ссылка на вставляемый узел</param>
    /// <param name="b">Ссылка на сравниваемый узел списка</param>
    /// <returns>Результат сравнения</returns>
    public delegate bool CompareFunction(Link a, Link b);

    /// <summary>
    /// Класс Link - базовый класс внутреннего узла списка
    /// </summary>
    public class Link : Linkage
    {
        /// <summary>
        /// Имитационное время вставки узла в список.
        /// Используется для сбора статистики по времени нахождения узлов в списке.
        /// </summary>
        internal double InsertTime;

        /// <summary>
        /// Возвращает ссылку на заголовочную ячейку списка, в котором находится узел
        /// </summary>
        /// <returns>Ссылка на заголовочную ячейку</returns>
        protected internal List GetHeader()
        {
            if (Prev == null)
                return FPrev as List;
            else if (Next == null)
                return FNext as List;
            else
            {
                Link lnk = Prev;
                while (lnk.Prev != null)
                    lnk = lnk.Prev;
                return lnk.FPrev as List;
            }
        }

        /// <summary>
        /// Вставка узла в список после указанного
        /// </summary>
        /// <param name="l">Узел, после которого следует вставлять текущий</param>
        public void InsertAfter(Linkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FPrev = l;
            FNext = l.FNext;
            FNext.FPrev = this;
            l.FNext = this;
            List lst = GetHeader();
            lst.LengthStat.AddData(lst.Size, InsertTime);
        }

        /// <summary>
        /// Вставка узла в список перед указанным
        /// </summary>
        /// <param name="l">Узел, перед которым следует вставлять текущий</param>
        public void InsertBefore(Linkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FNext = l;
            FPrev = l.FPrev;
            FPrev.FNext = this;
            l.FPrev = this;
            List lst = GetHeader();
            lst.LengthStat.AddData(lst.Size, InsertTime);
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
                Link lnk = l.First;
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
            Link lnk = l.First;
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
        /// Исключение узла из списка, в котором он находится
        /// </summary>
        public void Remove()
        {
            if (FNext != null)
            {
                List lst = GetHeader();
                FNext.FPrev = FPrev;
                FPrev.FNext = FNext;
                FNext = null;
                FPrev = null;
                lst.TimeStat.AddData(Global.SimTime() - InsertTime);
                lst.LengthStat.AddData(lst.Size, Global.SimTime());
            }
        }

        /// <summary>
        /// Завершение работы узла. Узел исключается из списка.
        /// В переопределенном методе производного класса 
        /// ПОСЛЕДНИМ оператором должен быть base.Finish();
        /// </summary>
        public override void Finish()
        {
            Remove();
        }
    }

    /// <summary>
    /// Класс List - список узлов. Непосредственно сам объект является заголовочной ячейкой списка.
    /// </summary>
    public class List : Linkage
    {
        /// <summary>
        /// Конструктор по умолчанию. 
        /// Список создается с привязкой к моменту имитационного времени, соответствующему текущему процессу имитации.
        /// Функция сравнения не задается.
        /// </summary>
        public List()
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к заданному моменту имитационного времени.
        /// Функция сравнения не задается.
        /// </summary>
        /// <param name="simTime">Имитационное время, соответствующее созданию списка</param>
        public List(double simTime)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, соответствующему текущему процессу имитации.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравненияжж</param>
        public List(CompareFunction order)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к заданному моменту имитационного времени.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравненияжж</param>
        /// <param name="simTime">Имитационное время, соответствующее созданию списка</param>
        public List(CompareFunction order, double simTime)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// Конструктор с указанием заголовка. 
        /// Список создается с привязкой к моменту имитационного времени, соответствующему текущему процессу имитации.
        /// Функция сравнения не задается.
        /// </summary>
       /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к заданному моменту имитационного времени.
        /// Функция сравнения не задается.
        /// </summary>
        /// <param name="simTime">Имитационное время, соответствующее созданию списка</param>
       /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к моменту имитационного времени, соответствующему текущему процессу имитации.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравненияжж</param>
       /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(CompareFunction order, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с привязкой к заданному моменту имитационного времени.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравненияжж</param>
        /// <param name="simTime">Имитационное время, соответствующее созданию списка</param>
       /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public List(CompareFunction order, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// Делегат функции сравнения, определяющий упорядоченность списка
        /// </summary>
        protected internal CompareFunction CompFunc = null;

        /// <summary>
        /// Статистика по времени нахождения узлов в очереди
        /// </summary>
        public Statistics TimeStat;

        /// <summary>
        /// Статистика по длине списка
        /// </summary>
        public IntervalStatistics LengthStat;

        /// <summary>
        /// Заголовок при выводе статистики списка
        /// </summary>
        public string Header;

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
        /// Удаление списка
        /// </summary>
        public override void Finish()
        {
            // Очистить список
            Clear();
            base.Finish();
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
        /// Ссылка на первый узел списка.
        /// </summary>
        public Link First
        {
            get
            {
                return Next;
            }
        }

        /// <summary>
        /// Ссылка на последний узел списка.
        /// </summary>
        public Link Last
        {
            get
            {
                return Prev;
            }
        }

        /// <summary>
        /// Количество узлов списка
        /// </summary>
        public int Size
        {
            get
            {
                int i = 0;
                Link lnk = Next;
                while (lnk != null)
                {
                    i++;
                    lnk = lnk.Next;
                }
                return i;
            }
        }
    }
}
