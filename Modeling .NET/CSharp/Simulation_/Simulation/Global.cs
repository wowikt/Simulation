using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Simulation
{
    /// <summary>
    /// Класс Global содержит основные глобальные переменные и методы, управляющие процессом имитации.
    /// Выполняет переключение потоков в режиме сопрограмм.
    /// </summary>
    public class Global
    {
        /// <summary>
        /// Периодичность срабатывания очистки завершенных объектов
        /// </summary>
        static public double CleanTimeStep = 1;

        /// <summary>
        /// Ссылка на текущую исполняемую сопрограмму
        /// </summary>
        static internal Coroutine CurrProc = null;

        /// <summary>
        /// Текущая исполняемая имитация
        /// </summary>
        static internal IMainSimulation CurrSim = null;

        /// <summary>
        /// Глобальный диспетчер, управляющий работой сопрограмм
        /// </summary>
        static internal Dispatcher Dispatch;

        static internal double GlobalSimTime;

        /// <summary>
        /// Возобновляет приостановленное исполнение сопрограмм под управлением глобального диспетчера.
        /// <para>Если глобального диспетчера нет, порождается исключение</para>
        /// </summary>
        static public void ResumeDispatcher()
        {
            if (Dispatch != null)
            {
                Dispatch.Resume();
            }
            else
                throw new ESimulationException("ResumeDispatcher(): попытка возобновления несуществующего диспетчера сопрограмм");
        }

        /// <summary>
        /// Создает глобальный диспетчер, если его нет, и запускает исполнение сопрограмм под его управлением
        /// <para>Если глобальный диспетчер существует, запускает исполнение сопрограмм под его управлением</para>
        /// </summary>
        /// <param name="first">Сопрограмма, с которой начинается исполнение</param>
        static public void RunDispatcher(Coroutine first)
        {
            if (Dispatch == null)
            {
                Dispatch = new Dispatcher(first);
            }
            else
            {
                Dispatch.NextProc = first;
            }
            Dispatch.Resume(); 
        }

        /// <summary>
        /// Возвращает текущее имитационное время, соответствующее активной исполняемой имитации
        /// </summary>
        /// <returns>Текущее имитационное время</returns>
        static public double SimTime()
        {
            if (CurrSim == null)
                return GlobalSimTime;
            else
                return CurrSim.SimTime();
        }

        /// <summary>
        /// Стандартная функция сравнения для календаря событий. 
        /// Используется при вставке уведомлений о событиях в календарь без приоритета
        /// </summary>
        /// <param name="a">Ссылка на вставляемое уведомление</param>
        /// <param name="b">Ссылка на очередное уведомление списка</param>
        /// <returns>Результат сравнения</returns>
        internal static bool CalendarOrder(ILink a, ILink b)
        {
            return (a as EventNotice).EventTime < (b as EventNotice).EventTime;
        }
    }
}
