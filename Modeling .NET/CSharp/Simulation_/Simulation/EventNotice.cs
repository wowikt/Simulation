using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс EventNotice - ячейка календаря событий
    /// </summary>
    public abstract class EventNotice : Link
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

        /// <summary>
        /// Вставка в календарь событий до всех уведомлений с тем же значением времени
        /// </summary>
        /// <param name="l">Календарь событий</param>
        internal void InsertPrior(List l)
        {
            Insert(l, PriorCompFunc);
        }

        /// <summary>
        /// Функция сравнения для вставки с приоритетом уведомления о событиях в календарь
        /// </summary>
        /// <param name="a">Ссылка на вставляемое уведомление</param>
        /// <param name="b">Ссылка на очередное уведомление в списке</param>
        /// <returns>true, если ячейка a может быть вставлена перед ячейкой b</returns>
        private static bool PriorCompFunc(ILink a, ILink b)
        {
            return (a as EventNotice).EventTime <= (b as EventNotice).EventTime;
        }

        /// <summary>
        /// Обработка очередного события
        /// </summary>
        public abstract object RunEvent();

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
    }
}
