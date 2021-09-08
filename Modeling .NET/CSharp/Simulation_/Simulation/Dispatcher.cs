using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fibers;

namespace Simulation
{
    /// <summary>
    /// Класс Dispatcher - диспетчер для управления исполнением сопрограмм
    /// <para>Сопрограммы, работающие под управлением диспетчесра, должны передавать управление
    /// друг другу посредством метода SwitchTo()</para>
    /// <para>Вызов SwitchTo(null) приостанавливает или завершает работу диспетчера и всех сопрограмм, которыми он управляет</para>
    /// </summary>
    internal class Dispatcher : Fiber
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="first">Указывает, с какой сопрограммы должно начаться исполнение</param>
        public Dispatcher(Coroutine first)
        {
            NextProc = first;
        }

        /// <summary>
        /// Очередная сопрограма для исполнения
        /// </summary>
        internal Coroutine NextProc;

        /// <summary>
        /// Указатель на сопрограмму, которая вызвала диспетчера (null, если был вызов из главного потока программы)
        /// </summary>
        private Coroutine PrevProc;

        /// <summary>
        /// Основной цикл работы диспетчера сопрограмм
        /// </summary>
        protected override void Run()
        {
            // Запомнить указатель на вызвавшую сопрограмму
            PrevProc = Global.CurrProc;
            while (true)
            {
                while (true)
                {
                    // Запомнить указатель на очередную сопрограмму
                    Global.CurrProc = NextProc;
                    // Исполнить следующую сопрограмму
                    object res = NextProc.Resume();
                    // Если результат исполнения - null, приостановить выполнение
                    if (res == null)
                    {
                        break;
                    }
                    NextProc = res as Coroutine;
                }
                // Передать исполнение вызвавшей сопрограмме
                Global.CurrProc = PrevProc;
                Yield(null);
            }
        }
    }
}
