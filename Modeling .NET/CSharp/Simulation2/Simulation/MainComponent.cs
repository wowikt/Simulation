using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс MainComponent создает главный объект имитации как компонент,
    /// работающий на основе событийного подхода
    /// </summary>
    public class MainComponent : SchedulableComponent, IMainSimulation
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainComponent()
        {
            PrevSim = Global.CurrSim;
            Global.CurrSim = this;
            (this as IMainSimulation).CurrentSimTime = 0;
            Global.GlobalSimTime = 0;
            Parent = this;
            Calendar = new Queue(Global.CalendarOrder, 0, "Календарь");
            Finished = new Queue();
            Running = new Queue();
            Event = new ComponentEventNotice(0, this);
            Event.InsertFirst(Calendar);
            OnFinishEvent = FinishEvent;
            Global.CurrSim = PrevSim;
        }

        /// <summary>
        /// Календарь событий
        /// </summary>
        public Queue Calendar
        {
            get;
            set;
        }

        /// <summary>
        /// Сборщик завершенных процессов
        /// </summary>
        public Collector Collect
        {
            get;
            internal set;
        }

        private IMainSimulation PrevSim;

        /// <summary>
        /// Текущее имитационное время
        /// </summary>
        double IMainSimulation.CurrentSimTime
        {
            get;
            set;
        }

        /// <summary>
        /// Список завершенных процессов для периодического удаления
        /// </summary>
        public Queue Finished
        {
            get;
            internal set;
        }

        /// <summary>
        /// Список свободных процессов
        /// </summary>
        public Queue Running
        {
            get;
            internal set;
        }

        /// <summary>
        /// Признак завершения имитации
        /// </summary>
        public bool Terminated
        {
            get
            {
                return TerminatedState;
            }
        }

        internal static StopSim Stopped = new StopSim();

        internal bool TerminatedState;

        /// <summary>
        /// Ссылка на обработчик события завершения имитации
        /// </summary>
        internal protected EventProc OnFinishEvent;

        /// <summary>
        /// Процесс-визуализатор
        /// </summary>
        internal Visualizer VisProc;

        /// <summary>
        /// Шаг визуализации
        /// </summary>
        public double VisualInterval
        {
            get
            {
                if (VisProc == null)
                {
                    return 0;
                }
                else
                {
                    return VisProc.Interval;
                }
            }
            set
            {
                if (VisProc == null)
                {
                    VisProc = new Visualizer(value);
                    VisProc.Parent = this;
                    VisProc.Activate();
                }
                else
                {
                    VisProc.Interval = value;
                }
            }
        }

        /// <summary>
        /// Очистка статистики
        /// </summary>
        public virtual void ClearStat()
        {
            Calendar.ClearStat();
        }

        /// <summary>
        /// Вывод на консоль календаря событий. Используется с целью диагностики при отладке программ
        /// </summary>
        public void DumpEventQueue()
        {
            SchedulableEventNotice en = Calendar.First as SchedulableEventNotice;
            while (en != null)
            {
                Console.WriteLine(en);
                en = en.Next as SchedulableEventNotice;
            }
        }

        /// <summary>
        /// Завершение имитации
        /// </summary>
        public override void Finish()
        {
            Finished.Finish();
            Running.Finish();
            Calendar.Finish();
            base.Finish();
        }

        /// <summary>
        /// Создание и настройка всех объектов имитации. Должна переопределяться 
        /// в производных классах, создающих внутреннюю инфраструктуру.
        /// В данном классе ничего не делает
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// Исполняемый метод процесса имитации. Никогда не должен вызываться прямо или переопределяться
        /// </summary>
        internal override object Run()
        {
            if (OnNextScheduledEvent == OnStartEvent)
            {
                Global.CurrSim = this;
                Collect = new Collector();
                Init();
                StartingTime = SimTime();
                if (VisProc != null)
                {
                    VisProc.Activate();
                }
                OnNextScheduledEvent();
                if (OnNextScheduledEvent == OnStartEvent)
                {
                    OnNextScheduledEvent = OnFinishEvent;
                }
                return new ContinueSim();
            }
            else if (OnNextScheduledEvent == OnFinishEvent)
            {
                OnNextScheduledEvent();
                TerminatedState = true;
                StopStat();
                return new StopSim();
            }
            else
            {
                OnNextScheduledEvent();
                return new ContinueSim();
            }
        }

        /// <summary>
        /// Возвращает текущее имитационное время данной имитации
        /// </summary>
        /// <returns>Имитационное время</returns>
        public override double SimTime()
        {
            return (this as IMainSimulation).CurrentSimTime;
        }

        /// <summary>
        /// Запуск процесса имитации. Фактически, запускается диспетчер, управляющий имитацией
        /// </summary>
        public void Start()
        {
            object res;
            int i = 0;
            PrevSim = Global.CurrSim;
            Global.CurrSim = this;
            do
            {
                // Получить очередную запись календаря событий
                SchedulableEventNotice en = Calendar.First as SchedulableEventNotice;
                // Исполнить событие
                res = en.RunEvent();
                i++;
            }
            while (!(res is StopSim));
            // Если результат исполнения - пустой указатель, приостановить исполнение
            Global.CurrSim = PrevSim;
        }

        /// <summary>
        /// Коррекция статистики к текущему моменту
        /// </summary>
        public virtual void StopStat()
        {
            Calendar.StopStat();
        }
    }
}
