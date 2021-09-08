using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Интерфейс IAction является общей базой для объектов 
    /// действий - Action, Service, ServiceSelector
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Проверяет, может ли объект начать исполнение действия
        /// </summary>
        /// <param name="comp">Компонент, над которым предполагается 
        /// исполнение действия</param>
        /// <returns>true, если можно исполнять действие</returns>
        bool CanStart(IComponent comp);

        /// <summary>
        /// Запускает исполнение действия
        /// </summary>
        /// <param name="comp">Компонент, над которым исполняется действие</param>
        /// <returns>true, есил действие успешно запущено</returns>
        bool Start(IComponent comp);
    }
}
