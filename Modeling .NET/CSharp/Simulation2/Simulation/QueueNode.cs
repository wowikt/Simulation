
namespace Simulation
{
    /// <summary>
    /// Класс QueueNode определяет очередь, автоматически направляющую компоненты на обслуживание.
    /// Фактически, является оболочкой вокруг списка
    /// </summary>
    public class QueueNode : Node
    {
        /// <summary>
        /// Конструктор. Список создается без ограничения длины.
        /// </summary>
        public QueueNode()
        {
            TheQueue = new Queue();
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с заданной максимальной длиной.
        /// </summary>
        /// <param name="max">Максимальный размер очереди</param>
        public QueueNode(int max)
        {
            TheQueue = new Queue(max);
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с заданной максимальной длиной.
        /// </summary>
        /// <param name="max">Максимальный размер очереди</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public QueueNode(int max, string aHeader)
        {
            TheQueue = new Queue(max, aHeader);
        }

        /// <summary>
        /// Конструктор. 
        /// </summary>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public QueueNode(string aHeader)
        {
            TheQueue = new Queue(aHeader);
        }

        /// <summary>
        /// Конструктор. 
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        public QueueNode(Queue.CompareFunction order)
        {
            TheQueue = new Queue(order);
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с заданной максимальной длиной.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="max">Максимальный размер очереди</param>
        public QueueNode(Queue.CompareFunction order, int max)
        {
            TheQueue = new Queue(order, max);
        }

        /// <summary>
        /// Конструктор. 
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public QueueNode(Queue.CompareFunction order, string aHeader)
        {
            TheQueue = new Queue(order, aHeader);
        }

        /// <summary>
        /// Конструктор. 
        /// Список создается с заданной максимальной длиной.
        /// Функция сравнения задается параметром.
        /// </summary>
        /// <param name="order">Делегат функции сравнения</param>
        /// <param name="max">Максимальный размер очереди</param>
        /// <param name="aHeader">Заголовок списка при выводе статистики</param>
        public QueueNode(Queue.CompareFunction order, int max, string aHeader)
        {
            TheQueue = new Queue(order, max, aHeader);
        }

        /// <summary>
        /// Список, образующий очередь
        /// </summary>
        private Queue TheQueue;

        /// <summary>
        /// Указывает, следует ли блокировать предыдущее действие при
        /// переполнении очереди
        /// </summary>
        public bool BlockOnOverflow;

        /// <summary>
        /// Проверяет, может ли узел принять указанный компонент. В случае 
        /// утвердительного ответа между этим вызовом и последующим вызовом 
        /// AcceptComponent() не должно быть других обращений к этому узлу.
        /// </summary>
        /// <param name="comp">Компонент, возможность принятия которого проверяется</param>
        /// <returns>true, если компонент может быть принят узлом</returns>
        public override bool CanAccept(IComponent comp)
        {
            return !BlockOnOverflow || TheQueue.MaxSize == 0 || TheQueue.Size < TheQueue.MaxSize;
        }

        /// <summary>
        /// Помещение компонента в узел. В случае неудачи помещения компонента следует 
        /// предусмотреть корректную обработку его дальнейшего поведения - например,
        /// удалить его или задать событийный метод для дальнейшей обработки
        /// </summary>
        /// <param name="comp">Компонент, который требуется поместить в узел</param>
        /// <returns>true, если компонент был успешно помещен в узел</returns>
        public override bool AcceptComponent(IComponent comp)
        {
            // Если очередь переполнена
            if (TheQueue.MaxSize > 0 && TheQueue.Size >= TheQueue.MaxSize)
            {
                if (comp.OnNodeEnterFailed != null)
                {
                    comp.OnNodeEnterFailed(this);
                }
                if (BlockOnOverflow)
                {
                    // При переполнении следует заблокировать предыдущее действие
                    return false;
                }
                else
                {
                    return true;
                }
            }
            // Поместить компонент в очередь
            TheQueue.Insert(comp);
            if (comp.OnEnteredNode != null)
            {
                comp.OnEnteredNode(this);
            }
            return true;
        }

        /// <summary>
        /// Проверяет, меожет ли узел выдать очередной компонент для обработки
        /// </summary>
        /// <returns>true, если узел может выдать компонент</returns>
        public override bool CanRelease()
        {
            return !TheQueue.Empty();
        }

        /// <summary>
        /// Запрашивает у узла очередной компонент для обработки
        /// </summary>
        /// <returns>Ссылка на компонент, извлеченный из узла. Гарантируется, что в узле 
        /// не остается ссылок на этот компонент. Если в узле нет очередного компонента, 
        /// результат равен null.</returns>
        public override IComponent QueryRelease()
        {
            if (TheQueue.Empty())
            {
                return null;
            }
            else
            {
                IComponent comp = TheQueue.First as IComponent;
                comp.StartRunning();
                if (comp.OnReleased != null)
                {
                    comp.OnReleased(this);
                }
                return comp;
            }
        }
    }
}
