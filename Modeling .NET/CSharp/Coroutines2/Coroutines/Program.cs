using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Simulation;

namespace Coroutines
{
    /// <summary>
    /// �������� ����� ����������
    /// </summary>
    class Program
    {
        /// <summary>
        /// ������� �����
        /// </summary>
        /// <param name="args">��������� ��������� ������ �� ������������</param>
        static void Main(string[] args)
        {
            // ������� ��� �������-�����������
            MyProc corA = new MyProc("A");
            MyProc corB = new MyProc("B");
            MyProc corC = new MyProc("C");
            // ������ �������� ������� ������������
            corA.NextProc = corB;
            corB.NextProc = corC;
            corC.NextProc = corA;
            // ��� ������ � �������
            Console.WriteLine("������. ����� Enter.");
            Console.ReadLine();
            Global.RunDispatcher(corA);
            //// ��������� ������ �����������
            //corA.SwitchTo();
            // ������� �������
            corA.Finish();
            corB.Finish();
            corC.Finish();
            // ������ ��������
            Console.WriteLine("���������.");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// ���� �����������
    /// </summary>
    class MyProc : Coroutine
    {
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="aName">������������ ��� ������������</param>
        public MyProc(string aName)
        {
            Name = aName;
        }

        /// <summary>
        /// �������� ����� ����������
        /// </summary>
        protected override void Execute()
        {
            base.Execute();
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("����������� {0}: {1}", Name, i);
                Yield(NextProc);
            }
            Yield(null);
        }

        /// <summary>
        /// ����������� ���
        /// </summary>
        public string Name;

        /// <summary>
        /// ������ �� ��������� �� ������� �����������
        /// </summary>
        public Coroutine NextProc;
    }
}
