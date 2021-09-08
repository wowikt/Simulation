using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Simulation
{
    /// <summary>
    /// Класс <c>Coroutine</c> - базовый класс для построения сопрограмм
    /// </summary>
    public class Coroutine : Link
    {
        /// <summary>
        /// Конструктор. 
        /// <para>Создает всю необходимую инфраструктуру и обеспечивает исполнение начального фрагмента алгоритма</para>
        /// </summary>
        public Coroutine()
        {
            Owner = Global.CurrProc;
            MyThread = new Thread(Execute);
            MySem = new Semaphore(0, 1);
            MyThread.Start();
            if (Owner == null)
                Global.GlSem.WaitOne();
            else
                Owner.MySem.WaitOne();
        }

        private Thread MyThread;

        /// <summary>
        /// Поле, указывающее на завершенное состояние сопрограммы. 
        /// Доступно для чтения посредством свойства <c>Terminated</c>.
        /// </summary>
        protected bool TerminatedState;

        /// <summary>
        /// Семафор, управляющий работой сопрограммы
        /// </summary>
        internal Semaphore MySem;

        /// <summary>
        /// Сопрограмма-владелец текущей или null, если владельцем является главный поток
        /// </summary>
        internal Coroutine Owner;

        /// <summary>
        /// <para>Метод, обеспечивающий исполнение потока сопрограммы. Организует исполнение метода Run() в контесте сопрограммы.</para>
        /// <para>Никогда не должен переопределяться.</para>
        /// </summary>
        protected virtual void Execute()
        {
            Global.CurrProc = this;
            Detach();
            Run();
            TerminatedState = true;
            while (true)
                Detach();
        }

        /// <summary>
        /// Основной алгоритм работы сопрограммы. 
        /// <para>Должен переопределяться в производных классах.
        /// В данном классе ничего не делает.</para>
        /// </summary>
        protected virtual void Run()
        {
        }

        /// <summary>
        /// Переключение к данной сопрограмме
        /// </summary>
        public void SwitchTo()
        {
            Global.SwitchTo(this);
        }

        /// <summary>
        /// Переключение к заданной сопрограмме
        /// </summary>
        /// <param name="cor">Активируемая сопрограмма</param>
        protected void SwitchTo(Coroutine cor)
        {
            Global.SwitchTo(cor);
        }

        /// <summary>
        /// Переключение к сопрограмме-владельцу
        /// </summary>
        protected void Detach()
        {
            Global.Detach();
        }

        /// <summary>
        /// Завершение работы сопрограммы. 
        /// <para>Все сопрограммы, созданные в программе, 
        /// обязательно должны завершаться с помощью этого метода.
        /// В противном случае завершение работы или продолжение программы
        /// будут происходить некорректно.</para>
        /// </summary>
        public override void Finish()
        {
            MyThread.Abort();
            base.Finish();
        }

        /// <summary>
        /// Указывает, завершена ли работа сопрограммы
        /// </summary>
        public bool Terminated
        {
            get
            {
                return TerminatedState;
            }
        }
    }
}
