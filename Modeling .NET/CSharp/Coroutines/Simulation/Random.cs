using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    using SysRandom = System.Random;
    /// <summary>
    /// ����� Random ������������� ��������� ��������� ����� � ������������ ��������� ������������������� � ��������� ��������������
    /// </summary>
    public class Random
    {
        private SysRandom Rnd;
        private bool HasNextNormal;
        private double NextNormal;

        /// <summary>
        /// �����������. �������������� ������ ������������� � ����������� �� �������� ���������� �������
        /// </summary>
        public Random()
        {
            Rnd = new SysRandom();
        }

        /// <summary>
        /// �����������. �������������� ������ �������� ��������� ���������
        /// </summary>
        /// <param name="seed">�������� ��������</param>
        public Random(int seed)
        {
            Rnd = new SysRandom(seed);
        }

        /// <summary>
        /// ���������� ��������� ��������������� ������������ ��������
        /// </summary>
        /// <returns>���������� �������������� ������������ �������� � ��������� [0, 1)</returns>
        public double NextFloat()
        {
            return Rnd.NextDouble();
        }

        /// <summary>
        /// ���������� ��������� ��������������� ������������� �������� 
        /// </summary>
        /// <returns>���������� �������������� ������������� �������� � ��������� �� 0 �� 2^31 - 1</returns>
        public int NextInt()
        {
            return Rnd.Next();
        }

        /// <summary>
        /// ���������� ��������� ��������������� ������������� ��������,
        /// ������������ ������
        /// </summary>
        /// <param name="max">������� ������� ���������������� ��������</param>
        /// <returns>���������� �������������� ������������� �������� � ��������� �� 0 �� max - 1</returns>
        public int NextInt(int max)
        {
            return Rnd.Next(max);
        }

        /// <summary>
        /// ���������� ��������� ��������������� ������������� ��������,
        /// ������������ � ���� ������
        /// </summary>
        /// <param name="min">������ ������� ���������������� ��������</param>
        /// <param name="max">������� ������� ���������������� ��������</param>
        /// <returns>���������� �������������� ������������� �������� � ��������� �� min �� max - 1</returns>
        public int NextInt(int min, int max)
        {
            return Rnd.Next(min, max);
        }

        /// <summary>
        /// ���������� true � �������� ������������
        /// </summary>
        /// <param name="prob">����������� ��������� ���������� true</param>
        /// <returns>���������� ��������, ������ true � ������������ prob, � false � ������������ 1 - prob</returns>
        public bool Draw(double prob)
        {
            return Rnd.NextDouble() < prob;
        }

        /// <summary>
        /// ���������� ��������� ���������� �������������� ��������������� ������������ ��������
        /// </summary>
        /// <param name="min">������ �������</param>
        /// <param name="max">������� �������</param>
        /// <returns>�������� � ��������� [min, max)</returns>
        public double Uniform(double min, double max)
        {
            return Rnd.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// ���������� ������������� ��������, �������������� � ������������� ��� ������� ���������� ��������, ����������� � �������.
        /// ��������-������ ������ ���� ���������� �� �����������.
        /// ��������, ���� ������ �������� �������� (0.2, 0.6, 0.7, 0.9), �� ����������� ��������� ����������� ����� ����������:
        /// 0 - 0,2, 1 - 0,4, 2 - 0,1, 3 - 0,2, 4 - 0,1.
        /// </summary>
        /// <param name="table">������ ������������</param>
        /// <returns>������������� �������� � ��������� �� 0 �� table.Length</returns>
        public int TableIndex(double[] table)
        {
            if (table.Length == 0)
                return 0;
            double Rand = Rnd.NextDouble();
            // �������� ����� ���������, � ������� �������� ��������� ��������.
            // �������� ������ ������ ���� ����������.
            // ���� ��� �� ���, �������� ����������������.
            // �������� �������������� �� ���������� � ����� ��������� �������������.
            // ***(� �����, �������?)
            int L = 0;
            int R = table.Length - 1;
            while (L < R)
            {
                int M = (L + R) / 2;
                if (Rand > table[M])
                    L = M + 1;
                else
                    R = M;
            }
            if (Rand < table[L])
                return L;
            // ��������, ������ ����������, ����������� ���������� ���������
            // ��������, ����������� ���������, ����� ��������� ������ � ��������� ���������
            else
                return L + 1;
        }

        /// <summary>
        /// ���������� ��������� �������������� ������������ �������� � ���������
        /// ���������� ��������������� �������� � ������������ ����������
        /// </summary>
        /// <param name="mean">�������������� ��������</param>
        /// <param name="deviation">����������� ����������</param>
        /// <returns>��������� �������������� ������������ ��������</returns>
        public double Normal(double mean, double deviation)
        {
            if (HasNextNormal)
            {
                HasNextNormal = false;
                return (NextNormal * deviation + mean);
            }
            else
            {
                double Rnd1, Rnd2, W;
                HasNextNormal = true;
                do
                {
                    Rnd1 = 2 * NextFloat() - 1;
                    Rnd2 = 2 * NextFloat() - 1;
                    W = Rnd1 * Rnd1 + Rnd2 * Rnd2;
                }
                while (W > 1);
                NextNormal = Rnd2 * Math.Sqrt(-2 * Math.Log(W) / W);
                return (Rnd1 * Math.Sqrt(-2 * Math.Log(W) / W) * deviation + mean);
            }
        }

        /// <summary>
        /// ���������� ��������������� �������������� ������������ ��������
        /// � �������� �������������� ���������
        /// </summary>
        /// <param name="mean">�������������� �������� (��� ������� ������������� ��� ����� ������������ ����������)</param>
        /// <returns>��������������� �������������� ��������</returns>
        public double Exponential(double mean)
        {
            // ������ NextFloat() ������� 1 - NextFloat(), ��� ��� NextFloat() ����� ���� ����� 0
            return -Math.Log(1 - NextFloat()) * mean;
        }

        /// <summary>
        /// ���������� ������������ ��������, �������������� � ������������ � ������� �������.
        /// ��� ����� ����� count �������, �������������� ��������������� � �������������� ��������� mean ������.
        /// </summary>
        /// <param name="mean">�������������� �������� ���������� ����������</param>
        /// <param name="count">���������� ���������</param>
        /// <returns>��������, �������������� � ������������ � ������� �������</returns>
        public double Erlang(double mean, int count)
        {
            double Res = 1;
            for (int i = 0; i < count; i++)
                // ������ NextFloat() ������� 1 - NextFloat()
                Res *= 1 - NextFloat();
            return -mean * Math.Log(Res);
        }

        /// <summary>
        /// ���������� ������������� ��������, �������������� � ������������ � ������� ��������
        /// </summary>
        /// <param name="mean">�������������� ��������</param>
        /// <returns>������������� ��������, ������������� � ������������ � ������� ��������</returns>
        public int Poisson(double mean)
        {
            double Border = Math.Exp(-mean);
            double Prod = NextFloat();
            int i;
            for (i = 0; Prod >= Border; i++)
                Prod *= NextFloat();
            return i;
        }

        /// <summary>
        /// ���������� ������������ ��������, �������������� ����������
        /// </summary>
        /// <param name="min">������ �������</param>
        /// <param name="moda">����, �� ���� ��������, ��� �������� ��������� ����������� ������������� �����������</param>
        /// <param name="max">������� �������</param>
        /// <returns>���������� �������������� ��������</returns>
        public double Triangular(double min, double moda, double max)
        {
            double Rand = NextFloat();
            if (Rand <= (moda - min) / (max - min))
                return min + Math.Sqrt((moda - min) * (max - min) * Rand);
            else
                return max - Math.Sqrt((max - moda) * (max - min) * (1 - Rand));
        }
    }
}
