using System;
using System.Collections.Generic;
using System.Text;
using Simulation;

namespace Queens
{
    /// <summary>
    /// Сопрограмма, управляющая расстановкой одного ферзя
    /// </summary>
    class Queen : Coroutine
    {
        /// <summary>
        /// Номер вертикали
        /// </summary>
        public byte Col;

        /// <summary>
        /// Номер горизонтали
        /// </summary>
        public byte Row;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="aCol">Номер вертикали для ферзя</param>
        public Queen(byte aCol)
        {
            Col = aCol;
            Row = 0;
        }

        /// <summary>
        /// Алгоритм поиска места для ферзя
        /// </summary>
        protected override void Execute()
        {
            base.Execute();
            for (; ; )
            {
                // Если очередное поле свободно
                if (Board.IsFree(Col, Row))
                {
                    // Занять его
                    Board.MakeOccupied(Col, Row);
                    // Если ферзь не последний
                    if (Col < Board.QueenCount - 1)
                        // Активировать следующего для поиска места
                        Board.Queens[Col + 1].SwitchTo();
                    else
                        // В противном случае отобразить расстановку
                        Board.Remember();
                    // Освободить поле
                    Board.MakeFree(Col, Row);
                }
                // Попробовать следующее поле
                Row++;
                // Если оно последнее на вертикали
                if (Row == Board.QueenCount)
                {
                    // Начать заново
                    Row = 0;
                    // Если это первый ферзь
                    if (Col == 0)
                        // Завершить работу
                        return;
                    else
                        // Иначе активировать предыдущего для поиска другого варианта
                        Board.Queens[Col - 1].SwitchTo();
                }
            }
        }
    }
}
