using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс SimProc - главный процесс имитации
    /// </summary>
    public class SimProc : Process, IMainSimulation
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SimProc()
        {
            PrevSim = Global.CurrSim;
            Global.CurrSim = this;
            (this as IMainSimulation).CurrentSimTime = 0;
            Global.GlobalSimTime = 0;
            Parent = this;
            Calendar = new List(Global.CalendarOrder, 0, "Календарь");
            Finished = new List();
            Running = new List();
            (this as ISchedulable).Event = new ProcessEventNotice(0, this);
            (this as ISchedulable).Event.InsertFirst(Calendar);
            Global.CurrSim = PrevSim;
        }

        /// <summary>
        /// Календарь событий
        /// </summary>
        public List Calendar
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
        public List Finished
        {
            get;
            internal set;
        }

        /// <summary>
        /// Список свободных процессов
        /// </summary>
        public List Running
        {
            get;
            internal set;
        }

        internal static StopSim Stopped = new StopSim();

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
            EventNotice en = Calendar.First as EventNotice;
            while (en != null)
            {
                Console.WriteLine(en);
                en = en.Next as EventNotice;
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
        /// Исполняемый метод процесса имитации. Никогда не должен вызываться прямо или переопределяться
        /// </summary>
        protected override void Run()
        {
            Collect = new Collector();
            Init();
            StartingTime = SimTime();
            if (VisProc != null)
            {
                VisProc.Activate();
            }
            Execute();
            TerminatedState = true;
            StopStat();
            Yield(new StopSim());
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
                EventNotice en = Calendar.First as EventNotice;
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
