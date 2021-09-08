using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Интерфейс ILinkage - базовый для построения связных списков и многих объектов моделирования
    /// </summary>
    public interface ILinkage
    {
        /// <summary>
        /// Ссылка на заголовочную ячейку списка
        /// </summary>
        List HeaderNode
        {
            get;
        }

        /// <summary>
        /// Только для чтения. Ссылка на предыдущий узел, 
        /// если он является внутренней ячейкой списка.
        /// В противном случае - null.
        /// </summary>
        ILink Prev
        {
            get;
        }

        /// <summary>
        /// Только для чтения. Ссылка на следующий узел, 
        /// если он является внутренней ячейкой списка.
        /// В пртивном случае - null.
        /// </summary>
        ILink Next
        {
            get;
        }

        /// <summary>
        /// Заготовка для метода завершения работы объекта.
        /// </summary>
        void Finish();

        /// <summary>
        /// Получение ссылки на следующий узел списка независимо от того, является он внутренней или заголовочной ячейкой
        /// </summary>
        /// <returns>Ссылка на следующий узел</returns>
        ILinkage GetNext();

        /// <summary>
        /// Получение ссылки на предыдущий узел списка независимо от того, является он внутренней или заголовочной ячейкой
        /// </summary>
        /// <returns>Ссылка на предыдущий узел</returns>
        ILinkage GetPrev();

        /// <summary>
        /// Установка ссылки на следующий узел списка
        /// </summary>
        /// <param name="newNext">Новая ссылка на следующий узел</param>
        void SetNext(ILinkage newNext);

        /// <summary>
        /// Установка ссылки на предыдущий узел списка
        /// </summary>
        /// <param name="newPrev">Новая ссылка на предыдущий узел</param>
        void SetPrev(ILinkage newPrev);
    }
}
