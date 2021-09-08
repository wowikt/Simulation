using System;
using System.Collections.Generic;
using System.Text;

namespace Queens
{
    /// <summary>
    /// ���������� �����������, ������������ ��������� �����
    /// </summary>
    class Board
    {
        /// <summary>
        /// ���������� ������ � ������ �����
        /// </summary>
        public const int QueenCount = 4;

        /// <summary>
        /// ������, ����������� ��������� ������������
        /// </summary>
        static bool[] Rows = new bool[QueenCount];

        /// <summary>
        /// ������, ����������� ��������� ���������� �� ������ ������� ���� � ������ �������
        /// </summary>
        static bool[] DiagsUp = new bool[2 * QueenCount - 1];

        /// <summary>
        /// ������, ����������� ��������� ���������� �� ������ �������� ���� � ������ ������
        /// </summary>
        static bool[] DiagsDown = new bool[2 * QueenCount - 1];

        /// <summary>
        /// ������ ����������, ����������� ����������� ������
        /// </summary>
        static public Queen[] Queens = new Queen[QueenCount];

        /// <summary>
        /// ���������� ��������� �����������
        /// </summary>
        static public int BoardCount = 0;

        /// <summary>
        /// ��������, �������� �� ���� �������� �����
        /// </summary>
        /// <param name="Col">����� ���������</param>
        /// <param name="Row">����� �����������</param>
        /// <returns>true, ���� ���� ��������</returns>
        static public bool IsFree(byte Col, byte Row)
        {
            return !Rows[Row] && !DiagsUp[Col - Row + QueenCount - 1] && !DiagsDown[Col + Row];
        }

        /// <summary>
        /// ������ ���� ��������� �����
        /// </summary>
        /// <param name="Col">����� ���������</param>
        /// <param name="Row">����� �����������</param>
        static public void MakeOccupied(byte Col, byte Row)
        {
            Rows[Row] = true;
            DiagsUp[Col - Row + QueenCount - 1] = true;
            DiagsDown[Col + Row] = true;
        }

        /// <summary>
        /// ���������� ���� ��������� �����
        /// </summary>
        /// <param name="Col">����� ���������</param>
        /// <param name="Row">����� �����������</param>
        static public void MakeFree(byte Col, byte Row)
        {
            Rows[Row] = false;
            DiagsUp[Col - Row + QueenCount - 1] = false;
            DiagsDown[Col + Row] = false;
        }

        /// <summary>
        /// ���������� ������� �����������
        /// </summary>
        static public void Remember()
        {
            for (int i = 0; i < QueenCount; i++)
                Console.Write("{0} ", Queens[i].Row);
            Console.WriteLine();
            BoardCount++;
        }
    }
}
