using System;
using System.Collections.Generic;
using System.Text;
using Simulation;

namespace Queens
{
    /// <summary>
    /// Сопрограмма, организующая работу по расстановке ферзей
    /// </summary>
    class QueensRun : Coroutine
    {
        /// <summary>
        /// Основной метод
        /// </summary>
        protected override void Execute()
        {
            base.Execute();
            // Создать ферзей
            for (byte i = 0; i < Board.QueenCount; i++)
                Board.Queens[i] = new Queen(i);
            // Запустить первого
            Board.Queens[0].SwitchTo();
            // Отобразить количество полученных расстановок
            Console.WriteLine("Найдено расстановок: {0}", Board.BoardCount);
            // Удалить ферзей
            for (int i = 0; i < Board.QueenCount; i++)
                Board.Queens[i].Finish();
        }
    }
}
