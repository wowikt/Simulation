using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Fibers;

namespace Simulation
{
    /// <summary>
    /// Класс <c>Coroutine</c> - базовый класс для построения сопрограмм
    /// </summary>
    public class Coroutine : Fiber, ILink
    {
        /// <summary>
        /// Конструктор. 
        /// <para>Создает всю необходимую инфраструктуру и обеспечивает исполнение начального фрагмента алгоритма</para>
        /// </summary>
        public Coroutine()
        {
            Owner = Global.CurrProc;
            //Resume();
        }

        /// <summary>
        /// Поле связи. Ссылка на следующий узел списка
        /// </summary>
        internal ILinkage FNext;

        /// <summary>
        /// Поле связи. Ссылка на предыдущий узел списка
        /// </summary>
        internal ILinkage FPrev;

        /// <summary>
        /// Ссылка на заголовочную ячейку списка
        /// </summary>
        public List Header
        {
            get;
            internal set;
        }

        /// <summary>
        /// Сопрограмма-владелец текущей или null, если владельцем является главный поток
        /// </summary>
        internal Coroutine Owner;

        /// <summary>
        /// Поле, указывающее на завершенное состояние сопрограммы. 
        /// Доступно для чтения посредством свойства <c>Terminated</c>.
        /// </summary>
        protected bool TerminatedState;

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
        /// Указывает, завершена ли работа сопрограммы
        /// </summary>
        public bool Terminated
        {
            get
            {
                return TerminatedState;
            }
        }

        /// <summary>
        /// Переключение к сопрограмме-владельцу
        /// </summary>
        protected void Detach()
        {
            if (Global.CurrProc == this)
            {
                Yield(Owner);
            }
        }

        /// <summary>
        /// Основной алгоритм работы сопрограммы. 
        /// <para>Должен переопределяться в производных классах.
        /// В данном классе ничего не делает.</para>
        /// </summary>
        protected virtual void Execute()
        {
        }

        /// <summary>
        /// Завершение работы узла. Узел исключается из списка.
        /// В переопределенном методе производного класса 
        /// ПОСЛЕДНИМ оператором должен быть base.Finish();
        /// </summary>
        public virtual void Finish()
        {
            Remove();
            Dispose();
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
            Header = GetPrev().Header;
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
            Header = GetNext().Header;
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
        /// <para>Метод, обеспечивающий исполнение потока сопрограммы. Организует исполнение метода Run() в контесте сопрограммы.</para>
        /// <para>Никогда не должен переопределяться.</para>
        /// </summary>
        protected override void Run()
        {
            Global.CurrProc = this;
            //Detach();
            Execute();
            TerminatedState = true;
            while (true)
                Detach();
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
        /// Переключение к данной сопрограмме
        /// </summary>
        public void SwitchTo()
        {
            Global.CurrProc.Yield(this);
        }

        /// <summary>
        /// Переключение к заданной сопрограмме
        /// </summary>
        /// <param name="cor">Активируемая сопрограмма</param>
        protected void SwitchTo(Coroutine cor)
        {
            Global.CurrProc.Yield(cor);
        }
    }
}
