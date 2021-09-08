using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс Component используется как базовый для построения объектов имитации,
    /// поведение которых не планируется в календаре. Если необходимо определить
    /// компонент с необходимостью планирования событий, следует воспользоваться
    /// базовым классом SchedulableComponent, который является производным от данного.
    /// </summary>
    public class Component : Link, IComponent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Component()
        {
            Parent = Global.CurrSim;
            OnActionFinishedEvent = ActionFinishedEvent;
            OnActionStartingEvent = ActionStartingEvent;
            OnEnteredNode = EnteredEvent;
            OnNodeEnterFailed = NodeEnterFailedEvent;
            OnReleased = ReleasedEvent;
            StartingTime = Global.SimTime();
        }

        /// <summary>
        /// Ссылка на родительскую имитацию
        /// </summary>
        public IMainSimulation Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Событийный метод, вызываемый после успешной вставки ячейки в список
        /// </summary>
        public NodeEventProc OnEnteredNode
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Событийный метод, вызываемый после неудачной попытки вставки ячейки
        /// в список по причине переполнения списка
        /// </summary>
        public NodeEventProc OnNodeEnterFailed
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Событийный метод, вызываемый после исключения ячейки из списка
        /// </summary>
        public NodeEventProc OnReleased
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Ссылка на событийную процедуру для события окончания обслуживания
        /// </summary>
        public ActionEventProc OnActionFinishedEvent
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Ссылка на метод обработки события начала обслуживающего действия
        /// </summary>
        public ActionEventProc OnActionStartingEvent
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Индекс списка, в который помещается компонент в случае если объект-очередь
        /// имеет несколько списков. Он должен принимать значение от 0 до 
        /// максимального индекса списка, в противном случае порождается исключение.
        /// Если очередь имеет один список, это свойство не принимается во внимание.
        /// </summary>
        public int QueueIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Имитационное время, оставшееся до выполнения незавершенного действия.
        /// Используется, если текущее действие было перехвачено другим процессом.
        /// </summary>
        public double TimeLeft
        {
            get;
            set;
        }

        /// <summary>
        /// Время создания объекта. Устанавливается автоматически в конструкторе. 
        /// При необходимости впоследствии может быть изменено.
        /// </summary>
        public double StartingTime
        {
            get;
            set;
        }

        /// <summary>
        /// Включение компонента в список свободных. 
        /// Используется также для извлечения компонента из очереди ожидания
        /// <para>Представлен для совместимости с классом Process 
        /// с целью более простого перехода от процессов к компонентам. 
        /// Реально в этом методе нет необходимости, поскольку в .NET 
        /// объекты, на которые нет ссылок, удаляются автоматически при сборке мусора</para>
        /// <para>Если в программе не используются процессы, а есть только компоненты, 
        /// этот метод, а также ClearFinished() и GoFinished(), не нужны</para>
        /// </summary>
        public void StartRunning()
        {
            Insert(Parent.Running);
        }

        /// <summary>
        /// Событийный метод окончания обслуживания. 
        /// <para>При планировании обслуживающего действия ссылка на этот метод 
        /// помечается как очередной, если не указан иной метод.</para>
        /// <para>Если в жизненном цикле компонента имеется только одно 
        /// обслуживающее действие, следует переопределить только этот метод.</para>
        /// <para>Если алгоритм работы компонента подразумевает несколько 
        /// обслуживающих действий, для окончания каждого из них должен быть 
        /// предусмотрен метод с аналогичной сигнатурой</para>
        /// </summary>
        protected internal virtual void ActionFinishedEvent(IAction act)
        {
        }

        /// <summary>
        /// Событие начала обслуживания.
        /// <para>Возникает после извлечения компонента из очереди, но до того, 
        /// как в статистике обслуживания будет зафиксировано начало действия.</para>
        /// </summary>
        protected internal virtual void ActionStartingEvent(IAction act)
        {
        }

        /// <summary>
        /// Стандартный событийный метод для события вставки в очередь.
        /// В данном классе ничего не делает
        /// </summary>
        /// <param name="node">Узел в который был помещен объект</param>
        protected internal virtual void EnteredEvent(Node node)
        {
        }

        /// <summary>
        /// Стандартный событийный метод для события неудачи вставки в очередь.
        /// В данном классе ничего не делает
        /// </summary>
        /// <param name="node">Узел, в который предпринималась попытка 
        /// помещения объекта</param>
        protected internal virtual void NodeEnterFailedEvent(Node node)
        {
        }

        /// <summary>
        /// Стандартный событийный метод для события исключения из очереди.
        /// В данном классе ничего не делает
        /// </summary>
        /// <param name="node">Узел, из которой был извлечен объект</param>
        protected internal virtual void ReleasedEvent(Node node)
        {
        }
    }
}
