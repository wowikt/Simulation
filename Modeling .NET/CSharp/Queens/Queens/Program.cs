using System;
using System.Collections.Generic;
using System.Text;
using Simulation;

namespace Queens
{
    class Program
    {
        /// <summary>
        /// �������� �����
        /// </summary>
        /// <param name="args">��������� ��������� ������ �� ������������</param>
        static void Main(string[] args)
        {
            // ������� � ��������� �������� �����������
            QueensRun QRun = new QueensRun();
            Global.RunDispatcher(QRun);
            QRun.Finish();
            Console.WriteLine("������.");
            Console.ReadLine();
        }
    }
}
