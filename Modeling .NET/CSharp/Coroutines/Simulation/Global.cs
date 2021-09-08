using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Simulation
{
    /// <summary>
    /// Класс Global содержит основные глобальные переменные и методы, управляющие процессом имитации.
    /// Выполняет переключение потоков в режиме сопрограмм.
    /// </summary>
    public class Global
    {
        /// <summary>
        /// Семафор, управляющий работой главного потока
        /// </summary>
        static internal Semaphore GlSem = new Semaphore(0, 1);

        /// <summary>
        /// Ссылка на текущую исполняемую сопрограмму
        /// </summary>
        static internal Coroutine CurrProc = null;

        /// <summary>
        /// Текущая исполняемая имитация
        /// </summary>
        static internal SimProc CurrSim = null;

        /// <summary>
        /// Передача управления заданной сопрограмме
        /// </summary>
        /// <param name="cor">Активируемая сопрограмма</param>
        static public void SwitchTo(Coroutine cor)
        {
            Coroutine Current = CurrProc;
            CurrProc = cor;
            if (cor is SimProc)
                CurrSim = cor as SimProc;
            else if (cor is Process)
                CurrSim = (cor as Process).Parent;
            else
                CurrSim = null;
            if (cor == null)
            {
                GlSem.Release();
                Current.MySem.WaitOne();
            }
            else if (Current == null)
            {
                cor.MySem.Release();
                GlSem.WaitOne();
            }
            else
            {
                cor.MySem.Release();
                Current.MySem.WaitOne();
            }
        }

        /// <summary>
        /// Передача управления сопрограмме - владельцу текущей исполняемой
        /// </summary>
        static public void Detach()
        {
            SwitchTo(CurrProc.Owner);
        }

        /// <summary>
        /// Возвращает текущее имитационное время, соответствующее активной исполняемой имитации
        /// </summary>
        /// <returns>Текущее имитационное время</returns>
        static public double SimTime()
        {
            if (CurrSim == null)
                return 0;
            else
                return CurrSim.SimTime();
        }
    }

    /// <summary>
    /// Класс EventNotice - ячейка календаря событий
    /// </summary>
    internal abstract class EventNotice : Link
    {
        /// <summary>
        /// Конструктор. Записывает значения параметров в поля объекта
        /// </summary>
        /// <param name="time">Время наступления события</param>
        protected EventNotice(double time)
        {
            EventTime = time;
        }

        /// <summary>
        /// Имитационное время наступления события
        /// </summary>
        internal double EventTime;

        private static bool PriorCompFunc(Link a, Link b)
        {
            return (a as EventNotice).EventTime <= (b as EventNotice).EventTime;
        }

        /// <summary>
        /// Вставка в календарь событий до всех уведомлений с тем же значением времени
        /// </summary>
        /// <param name="l">Календарь событий</param>
        internal void InsertPrior(List l)
        {
            Insert(l, PriorCompFunc);
        }

        /// <summary>
        /// Изменение времени наступления события и перестановка уведомления в календаре
        /// после всех уведомлений с равным временем наступления события
        /// </summary>
        /// <param name="newTime">Новое время наступления события</param>
        internal void SetTime(double newTime)
        {
            List lst = GetHeader();
            EventTime = newTime;
            Insert(lst);
        }

        /// <summary>
        /// Изменение времени наступления события и перестановка уведомления в календаре
        /// до всех уведомлений с равным временем наступления события
        /// </summary>
        /// <param name="newTime">Новое время наступления события</param>
        internal void SetTimePrior(double newTime)
        {
            List lst = GetHeader();
            EventTime = newTime;
            InsertPrior(lst);
        }

        /// <summary>
        /// Обработка очередного события
        /// </summary>
        public abstract void RunEvent();
    }

    /// <summary>
    /// Класс ESimulationException - класс исключения имитации
    /// </summary>
    public class ESimulationException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ESimulationException()
            : base()
        {
        }

        /// <summary>
        /// Конструктор со строковым параметром
        /// </summary>
        /// <param name="message">Параметр - сообщение о причине исключения</param>
        public ESimulationException(string message)
            : base(message)
        {
        }
    }
}
