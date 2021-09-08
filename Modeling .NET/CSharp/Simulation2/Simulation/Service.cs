using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс Service обеспечивает выполнение обслуживающих действий
    /// </summary>
    public class Service : IAction
    {
        /// <summary>
        /// Конструктор по умолчанию. Создает действия с единичной мощностью.
        /// </summary>
        public Service() : this(1, 0, null, null, null)
        {
        }

        /// <summary>
        /// Конструктор по умолчанию. Создает действия с единичной мощностью.
        /// </summary>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        public Service(DurationFunc duration)
            : this(1, 0, duration, null, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        public Service(DurationFunc duration, Service next)
            : this(1, 0, duration, next, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(DurationFunc duration, Service next, string aHeader)
            : this(1, 0, duration, next, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(DurationFunc duration, string aHeader)
            : this(1, 0, duration, null, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        public Service(int count)
            : this(count, 0, null, null, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        public Service(int count, DurationFunc duration)
            : this(count, 0, duration, null, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        public Service(int count, DurationFunc duration, Service next)
            : this(count, 0, duration, next, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, DurationFunc duration, Service next, string aHeader)
            : this(count, 0, duration, next, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, DurationFunc duration, string aHeader)
            : this(count, 0, duration, null, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        public Service(int count, int maxSize)
            : this(count, maxSize, null, null, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        public Service(int count, int maxSize, DurationFunc duration)
            : this(count, maxSize, duration, null, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        public Service(int count, int maxSize, DurationFunc duration, Service next)
            : this(count, maxSize, duration, next, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, int maxSize, DurationFunc duration, Service next, string aHeader)
        {
            Capacity = count;
            Agents = new ServiceAgent[Capacity];
            for (int i = 0; i < Capacity; i++)
            {
                Agents[i] = new ServiceAgent(this);
            }
            Queue = new Queue(maxSize);
            ServiceStat = new ServiceStatistics(Capacity);
            if (next != null)
            {
                NextServ = next;
                NextServ.PrevServ = this;
            }
            if (aHeader != null)
            {
                StatHeader = aHeader;
            }
            Duration = duration;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        /// <param name="duration">Делегат, вычисляющий 
        /// продолжительность действия</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, int maxSize, DurationFunc duration, string aHeader)
            : this(count, maxSize, duration, null, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        public Service(int count, int maxSize, Service next)
            : this(count, maxSize, null, next, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, int maxSize, Service next, string aHeader)
            : this(count, maxSize, null, next, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="maxSize">Максимальный размер очереди</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, int maxSize, string aHeader)
            : this(count, maxSize, null, null, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        public Service(int count, Service next)
            : this(count, 0, null, next, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, Service next, string aHeader)
            : this(count, 0, null, next, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Мощность действия, то есть, количество 
        /// обслуживающих устройств</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(int count, string aHeader)
            : this(count, 0, null, null, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        public Service(Service next)
            : this(1, 0, null, next, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="next">Ссылка на действие, выполняющееся следующим</param>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(Service next, string aHeader)
            : this(1, 0, null, next, aHeader)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="aHeader">Заголовок действия</param>
        public Service(string aHeader)
            : this(1, 0, null, null, aHeader)
        {
        }

        /// <summary>
        /// Ссылка на следующее действие (если есть)
        /// </summary>
        public Service NextService
        {
            get
            {
                return NextServ;
            }
            set
            {
                NextServ = value;
                NextServ.PrevServ = this;
            }
        }

        /// <summary>
        /// Ссылка на предыдущее действия (если есть)
        /// </summary>
        public Service PrevService
        {
            get
            {
                return PrevServ;
            }
            set
            {
                PrevServ = value;
                PrevServ.NextServ = this;
            }
        }

        /// <summary>
        /// Очередь ожидания обслуживания
        /// </summary>
        public Queue Queue
        {
            get;
            internal set;
        }

        /// <summary>
        /// Статистика по обслуживающему действию
        /// </summary>
        public ServiceStatistics ServiceStat
        {
            get;
            internal set;
        }

        /// <summary>
        /// Массив агентов обработки
        /// </summary>
        internal ServiceAgent[] Agents;

        /// <summary>
        /// Делегат, вычисляющий длительность действия
        /// </summary>
        internal DurationFunc Duration;

        /// <summary>
        /// Мощность действия
        /// </summary>
        public int Capacity
        {
            get;
            internal set;
        }

        /// <summary>
        /// Текстовое описание действия
        /// </summary>
        public string StatHeader
        {
            get
            {
                return Head;
            }
            set
            {
                Head = value;
                Queue.Header = Head + " (очередь)";
                ServiceStat.Header = Head + " (действие)";
            }
        }

        /// <summary>
        /// Ссылка на следующее действие
        /// </summary>
        private Service NextServ;

        /// <summary>
        ///  Ссылка на предыдущее действие
        /// </summary>
        private Service PrevServ;

        /// <summary>
        ///  Заголовок действия
        /// </summary>
        private string Head;

        /// <summary>
        /// Очистка структуры объекта
        /// </summary>
        public void Finish()
        {
            Queue.Finish();
        }

        /// <summary>
        /// Очистка статистики
        /// </summary>
        public void ClearStat()
        {
            Queue.ClearStat();
            ServiceStat.ClearStat();
        }

        /// <summary>
        /// Коррекция статистики
        /// </summary>
        public void StopStat()
        {
            Queue.StopStat();
            ServiceStat.StopStat();
        }

        /// <summary>
        /// Проверяет, может ли объект начать исполнение действия
        /// </summary>
        /// <param name="comp">Компонент, над которым предполагается 
        /// исполнение действия</param>
        /// <returns>true, если можно исполнять действие</returns>
        public bool CanStart(IComponent comp)
        {
            int freeAgents = 0;
            for (int i = 0; i < ServiceStat.Devices; i++)
            {
                if (!Agents[i].Busy)
                {
                    freeAgents++;  
                }
            }
            return freeAgents > 0;
        }

        /// <summary>
        /// Запускает исполнение действия
        /// </summary>
        /// <param name="comp">Компонент, над которым исполняется действие</param>
        /// <returns>true, есил действие успешно запущено</returns>
        public bool Start(IComponent comp)
        {
            for (int i = 0; i < ServiceStat.Devices; i++)
            {
                if (!Agents[i].Busy)
                {
                    Agents[i].Activate();
                    return true;
                }
            }
            return false;
        }
    }
}
