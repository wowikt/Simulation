using System;
using System.Collections.Generic;
using System.Text;
using Simulation;

namespace Queens
{
    /// <summary>
    /// �����������, ����������� ������������ ������ �����
    /// </summary>
    class Queen : Coroutine
    {
        /// <summary>
        /// ����� ���������
        /// </summary>
        public byte Col;

        /// <summary>
        /// ����� �����������
        /// </summary>
        public byte Row;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="aCol">����� ��������� ��� �����</param>
        public Queen(byte aCol)
        {
            Col = aCol;
            Row = 0;
        }

        /// <summary>
        /// �������� ������ ����� ��� �����
        /// </summary>
        protected override void Execute()
        {
            base.Execute();
            for (; ; )
            {
                // ���� ��������� ���� ��������
                if (Board.IsFree(Col, Row))
                {
                    // ������ ���
                    Board.MakeOccupied(Col, Row);
                    // ���� ����� �� ���������
                    if (Col < Board.QueenCount - 1)
                        // ������������ ���������� ��� ������ �����
                        Board.Queens[Col + 1].SwitchTo();
                    else
                        // � ��������� ������ ���������� �����������
                        Board.Remember();
                    // ���������� ����
                    Board.MakeFree(Col, Row);
                }
                // ����������� ��������� ����
                Row++;
                // ���� ��� ��������� �� ���������
                if (Row == Board.QueenCount)
                {
                    // ������ ������
                    Row = 0;
                    // ���� ��� ������ �����
                    if (Col == 0)
                        // ��������� ������
                        return;
                    else
                        // ����� ������������ ����������� ��� ������ ������� ��������
                        Board.Queens[Col - 1].SwitchTo();
                }
            }
        }
    }
}
