using System;
using System.Collections.Generic;
using System.Text;
using Simulation;

namespace Queens
{
    /// <summary>
    /// �����������, ������������ ������ �� ����������� ������
    /// </summary>
    class QueensRun : Coroutine
    {
        /// <summary>
        /// �������� �����
        /// </summary>
        protected override void Execute()
        {
            base.Execute();
            // ������� ������
            for (byte i = 0; i < Board.QueenCount; i++)
                Board.Queens[i] = new Queen(i);
            // ��������� �������
            Board.Queens[0].SwitchTo();
            // ���������� ���������� ���������� �����������
            Console.WriteLine("������� �����������: {0}", Board.BoardCount);
            // ������� ������
            for (int i = 0; i < Board.QueenCount; i++)
                Board.Queens[i].Finish();
        }
    }
}
