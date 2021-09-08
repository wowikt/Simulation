using System;
using System.Collections.Generic;
using System.Text;
using Simulation;

namespace Randoms
{
    using SimRandom = Simulation.Random;

    class Program
    {
        static void Main(string[] args)
        {
            // ������-��������� ��������� �����
            SimRandom rnd = new SimRandom();
            // ������� ����������
            Statistics st = new Statistics("����������");
            Histogram hist = new Histogram(0, 0.5, 20, "�������������");
            for (int i = 0; i < 100000; i++)
            {
                double rndVal = rnd.Triangular(0, 2, 10);
                st.AddData(rndVal);
                hist.AddData(rndVal);
            }

            Console.WriteLine(st);
            Console.WriteLine();
            Console.WriteLine(hist);
            Console.ReadLine();
        }
    }
}
