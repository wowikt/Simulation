using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Интерфейс ILink содержит определения, обязательные для любых объектов, которые могут быть вставлены в список
    /// </summary>
    public interface ILink : ILinkage
    {
        /// <summary>
        /// Имитационное время вставки узла в список.
        /// Используется для сбора статистики по времени нахождения узлов в списке.
        /// </summary>
        double InsertTime
        {
            get;
            //set;
        }

        /// <summary>
        /// Проверка, является ли ячейка первой в списке
        /// </summary>
        bool IsFirst
        {
            get;
        }

        /// <summary>
        /// Проверка, является ли ячейка последней в списке
        /// </summary>
        bool IsLast
        {
            get;
        }

        /// <summary>
        /// Возвращает ссылку на заголовочную ячейку списка, в котором находится узел
        /// </summary>
        /// <returns>Ссылка на заголовочную ячейку</returns>
        List GetHeader();

        /// <summary>
        /// Вставка узла в список. 
        /// Если для узла задана собственная функция сравнения, вставка производится с ее использованием.
        /// В противном случае узел вставляется в список последним
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется текущий узел</param>
        void Insert(List l);

        /// <summary>
        /// Вставка узла в список с использованием указанной функции сравнения
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        /// <param name="cmp">Функция сравнения, которую следует использоватьпри вставке узла</param>
        void Insert(List l, CompareFunction cmp);

        /// <summary>
        /// Вставка узла в список после указанного
        /// </summary>
        /// <param name="l">Узел, после которого следует вставлять текущий</param>
        void InsertAfter(ILinkage l);

        /// <summary>
        /// Вставка узла в список перед указанным
        /// </summary>
        /// <param name="l">Узел, перед которым следует вставлять текущий</param>
        void InsertBefore(ILinkage l);

        /// <summary>
        /// Вставка узла в первую позицию списка
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        void InsertFirst(List l);

        /// <summary>
        /// Вставка узла в последнюю позицию списка
        /// </summary>
        /// <param name="l">Ссылка на заголовочную ячейку списка, в который вставляется узел</param>
        void InsertLast(List l);

        /// <summary>
        /// Исключение узла из списка, в котором он находится
        /// </summary>
        void Remove();
    }
}
