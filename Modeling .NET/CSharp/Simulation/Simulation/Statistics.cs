using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// ����� <c>Statistics</c> �������� �������� ���������� �� ����������� ���������
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// ����������� �� ���������
        /// </summary>
        public Statistics()
        {
            ClearStat();
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="AHeader">��������� ��� ������ �� �����</param>
        public Statistics(string AHeader)
        {
            ClearStat();
            Header = AHeader;
        }

        /// <summary>
        /// ��������� ��� ������ ���������� �� �����
        /// </summary>
        public string Header;

        /// <summary>
        /// ����� �������
        /// </summary>
        protected double SumX;

        /// <summary>
        /// ����� ��������� �������
        /// </summary>
        protected double SumX_2;

        /// <summary>
        /// ���������� ����������� ��������
        /// </summary>
        public int Count
        {
            get;
            protected set;
        }

        /// <summary>
        /// ������������ �������� ����� �����������
        /// </summary>
        public double Max
        {
            get;
            private set;
        }

        /// <summary>
        /// ����������� �������� ����� �����������
        /// </summary>
        public double Min
        {
            get;
            private set;
        }

        /// <summary>
        /// ���������� ������ �������� � ����������
        /// </summary>
        /// <param name="newX">����������� ��������</param>
        public virtual void AddData(double newX)
        {
            // ���� ��� ������ ������� ������, ����� ��� � �������� ������������ � �������������
            if (Count == 0)
                Min = Max = newX;
            // ����� ������, ���� �� �������� ����� ������������ ���� �����������
            else if (newX > Max)
                Max = newX;
            else if (newX < Min)
                Min = newX;
            SumX += newX;
            SumX_2 += newX * newX;
            Count++;
        }

        /// <summary>
        /// ������� ����������, ���������� � ������ ����� ������
        /// </summary>
        public virtual void ClearStat()
        {
            SumX = SumX_2 = Min = Max = 0;
            Count = 0;
        }

        /// <summary>
        /// ���������� ����������� ���������� ����������� ��������
        /// </summary>
        /// <returns>����������� ����������</returns>
        public double Deviation()
        {
            return Math.Sqrt(Disperse());
        }

        /// <summary>
        /// ���������� ��������� ����������� ��������
        /// </summary>
        /// <returns>���������</returns>
        public double Disperse()
        {
            if (Count <= 1)
                return 0;
            else
                return (SumX_2 - Count * Mean() * Mean()) / (Count - 1);
        }

        /// <summary>
        /// ���������� ������� �������������� �� ����������� ������
        /// </summary>
        /// <returns>������� ��������������</returns>
        public double Mean()
        {
            if (Count <= 0)
                return 0;
            else
                return SumX / Count;
        }

        /// <summary>
        /// ����������� ���������� ���������� � ����� ��� ����������� �� ������
        /// </summary>
        /// <returns>��������������� ����������</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("������� = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("������� = {0,6:0.000}, �������� = {1,6:0.000}\n", Min, Max);
            Result.AppendFormat("����� {0,4} ��������", Count);
            return Result.ToString();
        }
    }
}
