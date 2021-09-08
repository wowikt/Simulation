using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Simulation;

namespace Coroutines
{
    /// <summary>
    /// Основной класс приложения
    /// </summary>
    class Program
    {
        /// <summary>
        /// Главный метод
        /// </summary>
        /// <param name="args">Параметры командной строки не используются</param>
        static void Main(string[] args)
        {
            // Создать три объекта-сопрограммы
            MyProc corA = new MyProc("A");
            MyProc corB = new MyProc("B");
            MyProc corC = new MyProc("C");
            // Задать взаимный порядок переключения
            corA.NextProc = corB;
            corB.NextProc = corC;
            corC.NextProc = corA;
            // Все готово к запуску
            Console.WriteLine("Готово. Нажми Enter.");
            Console.ReadLine();
            Global.RunDispatcher(corA);
            //// Запустить первую сопрограмму
            //corA.SwitchTo();
            // Удалить объекты
            corA.Finish();
            corB.Finish();
            corC.Finish();
            // Работа окончена
            Console.WriteLine("Выполнено.");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Клас сопрограммы
    /// </summary>
    class MyProc : Coroutine
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="aName">Отображаемое имя сопрогграммы</param>
        public MyProc(string aName)
        {
            Name = aName;
        }

        /// <summary>
        /// Основной метод исполнения
        /// </summary>
        protected override void Execute()
        {
            base.Execute();
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Сопрограмма {0}: {1}", Name, i);
                Yield(NextProc);
            }
            Yield(null);
        }

        /// <summary>
        /// Отображемое имя
        /// </summary>
        public string Name;

        /// <summary>
        /// Ссылка на следующую по очереди сопрограмму
        /// </summary>
        public Coroutine NextProc;
    }
}
