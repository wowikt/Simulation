using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Интерфейс Node соделжит методы, общие для узлов, из которых состоит 
    /// сетевая составляющая имитации
    /// </summary>
    public class Node : SchedulableComponent
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Node()
        {
            ReleaseActions = new List<IAction>();
            Probs = new List<double>();
            //Conditions = new List<ConditionFunc>();
        }

        /// <summary>
        /// Действия, исполняемые при выходе компонента из узла.
        /// Если в списке несколько действий, из них выбирается одно - либо
        /// с помощью вероятностей выбора каждого из них, либо
        /// с помощью проверки условий. Два способа выбора действия не могут 
        /// применяться в одном узле. Если при выборе по условию ни одно из действий
        /// не может быть выбрано, освобождаемый компонент удаляется из системы
        /// </summary>
        internal List<IAction> ReleaseActions;

        /// <summary>
        /// Вероятности выбора действий
        /// </summary>
        internal List<double> Probs;

        /// <summary>
        /// Проверяет, может ли узел принять указанный компонент. В случае 
        /// утвердительного ответа между этим вызовом и последующим вызовом 
        /// AcceptComponent() не должно быть других обращений к этому узлу.
        /// </summary>
        /// <param name="comp">Компонент, возможность принятия которого проверяется</param>
        /// <returns>true, если компонент может быть принят узлом</returns>
        public virtual bool CanAccept(IComponent comp)
        {
            return true;
        }

        /// <summary>
        /// Помещение компонента в узел. В случае неудачи помещения компонента следует 
        /// предусмотреть корректную обработку его дальнейшего поведения - например,
        /// удалить его или задать событийный метод для дальнейшей обработки
        /// </summary>
        /// <param name="comp">Компонент, который требуется поместить в узел</param>
        /// <returns>true, если компонент был успешно помещен в узел</returns>
        public virtual bool AcceptComponent(IComponent comp)
        {
            return true;
        }

        /// <summary>
        /// Проверяет, может ли узел выдать очередной компонент для обработки
        /// </summary>
        /// <returns>true, если узел может выдать компонент</returns>
        public virtual bool CanRelease()
        {
            return false;
        }

        /// <summary>
        /// Запрашивает у узла очередной компонент для обработки
        /// </summary>
        /// <returns>Ссылка на компонент, извлеченный из узла. Гарантируется, что в узле 
        /// не остается ссылок на этот компонент. Если в узле нет очередного компонента, 
        /// результат равен null.</returns>
        public virtual IComponent QueryRelease()
        {
            return null;
        }

        protected virtual void Release(IComponent comp)
        {
            IAction output = null;
            // Действий на выходе не указано
            if (ReleaseActions.Count == 0)
            {
                // Исключить компонент из узла
                RemoveReleasedComponent(comp);
                // Если компонент планируемый, активировать его
                if (comp is ISchedulable)
                {
                    (comp as ISchedulable).Activate();
                }
                // Остальные компоненты просто удаляются
            }
            // Если указано одно выходное действие
            else if (ReleaseActions.Count == 1)
            {
                // Исключить компонент из узла
                RemoveReleasedComponent(comp);
                // Если действие может быть начато
                if (ReleaseActions[0].CanStart(comp))
                {
                    // Запустить действие
                    ReleaseActions[0].Start(comp);
                }
                // В противном случае удалить компонент
                else if (comp is Process)
                {
                    (comp as Process).Finish();
                }
            }
            // Не указаны вероятности действий
            else if (Probs.Count == 0)
            {
                // Найти первое действие, способное принять компонент
                for (int i = 0; i < ReleaseActions.Count; i++)
                {
                    if (ReleaseActions[i].CanStart(comp))
                    {
                        output = ReleaseActions[i];
                        break;
                    }
                }
                // Если действие найдено
                if (output != null)
                {
                    
                }
            }
        }

        protected virtual void RemoveReleasedComponent(IComponent comp)
        {
        }
    }
}
