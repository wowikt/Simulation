using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// ����� Linkage - ������� ��� ���������� ������� ������� � ������ �������� �������������
    /// </summary>
    public class Linkage
    {
        /// <summary>
        /// ���� �����. ������ �� ���������� ���� ������
        /// </summary>
        internal Linkage FPrev;

        /// <summary>
        /// ���� �����. ������ �� ��������� ���� ������
        /// </summary>
        internal Linkage FNext;

        /// <summary>
        /// �����������. ������������� ������ ���� �����
        /// </summary>
        public Linkage()
        {
            FPrev = null;
            FNext = null;
        }

        /// <summary>
        /// ������ ��� ������. ������ �� ���������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � ��������� ������ - null.
        /// </summary>
        public Link Prev
        {
            get
            {
                if (FPrev is Link)
                    return FPrev as Link;
                else
                    return null;
            }
        }

        /// <summary>
        /// ������ ��� ������. ������ �� ��������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � �������� ������ - null.
        /// </summary>
        public Link Next
        {
            get
            {
                if (FNext is Link)
                    return FNext as Link;
                else
                    return null;
            }
        }

        /// <summary>
        /// ��������� ��� ������ ���������� ������ �������.
        /// � ������ ������ ������ �� ������
        /// </summary>
        public virtual void Finish()
        {
        }
    }

    /// <summary>
    /// ������� ��� ����������� ������� ���������, ������������ ����� ������� ���� � ������.
    /// ����������� ���� ���������� � ������ ����� ������ �����, ��� �������� ������� ���� ��������� true.
    /// </summary>
    /// <param name="a">������ �� ����������� ����</param>
    /// <param name="b">������ �� ������������ ���� ������</param>
    /// <returns>��������� ���������</returns>
    public delegate bool CompareFunction(Link a, Link b);

    /// <summary>
    /// ����� Link - ������� ����� ����������� ���� ������
    /// </summary>
    public class Link : Linkage
    {
        /// <summary>
        /// ������������ ����� ������� ���� � ������.
        /// ������������ ��� ����� ���������� �� ������� ���������� ����� � ������.
        /// </summary>
        internal double InsertTime;

        /// <summary>
        /// ���������� ������ �� ������������ ������ ������, � ������� ��������� ����
        /// </summary>
        /// <returns>������ �� ������������ ������</returns>
        protected internal List GetHeader()
        {
            if (Prev == null)
                return FPrev as List;
            else if (Next == null)
                return FNext as List;
            else
            {
                Link lnk = Prev;
                while (lnk.Prev != null)
                    lnk = lnk.Prev;
                return lnk.FPrev as List;
            }
        }

        /// <summary>
        /// ������� ���� � ������ ����� ����������
        /// </summary>
        /// <param name="l">����, ����� �������� ������� ��������� �������</param>
        public void InsertAfter(Linkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FPrev = l;
            FNext = l.FNext;
            FNext.FPrev = this;
            l.FNext = this;
            List lst = GetHeader();
            lst.LengthStat.AddData(lst.Size, InsertTime);
        }

        /// <summary>
        /// ������� ���� � ������ ����� ���������
        /// </summary>
        /// <param name="l">����, ����� ������� ������� ��������� �������</param>
        public void InsertBefore(Linkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FNext = l;
            FPrev = l.FPrev;
            FPrev.FNext = this;
            l.FPrev = this;
            List lst = GetHeader();
            lst.LengthStat.AddData(lst.Size, InsertTime);
        }

        /// <summary>
        /// ������� ���� � ������. 
        /// ���� ��� ���� ������ ����������� ������� ���������, ������� ������������ � �� ��������������.
        /// � ��������� ������ ���� ����������� � ������ ���������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ������� ����</param>
        public void Insert(List l)
        {
            // ���� ������� ��������� � ������ �� �������, ��������� �� ��������� �����
            if (l.CompFunc == null)
                InsertLast(l);
            else
            {
                // ����� ����� �������
                Link lnk = l.First;
                while (lnk != null)
                {
                    if (l.CompFunc(this, lnk))
                        break;
                    lnk = lnk.Next;
                }
                // ���� ������ ��������, �������� � �����
                if (lnk == null)
                    InsertLast(l);
                // ����� �������� ����� ��������� �������
                else
                    InsertBefore(lnk);
            }
        }

        /// <summary>
        /// ������� ���� � ������ � �������������� ��������� ������� ���������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        /// <param name="cmp">������� ���������, ������� ������� ��������������� ������� ����</param>
        public void Insert(List l, CompareFunction cmp)
        {
            // ����� ����� �������
            Link lnk = l.First;
            while (lnk != null)
            {
                if (cmp(this, lnk))
                    break;
                lnk = lnk.Next;
            }
            // ���� ������ ��������, �������� � �����
            if (lnk == null)
                InsertLast(l);
            // ����� �������� ����� ��������� �������
            else
                InsertBefore(lnk);
        }

        /// <summary>
        /// ������� ���� � ������ ������� ������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        public void InsertFirst(List l)
        {
            // ������� �� ������ ����� - ��� ������� ����� ������������ ������
            InsertAfter(l);
        }

        /// <summary>
        /// ������� ���� � ��������� ������� ������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        public void InsertLast(List l)
        {
            // ������� �� ��������� ����� - ��� ������� ����� ������������ �������
            InsertBefore(l);
        }

        /// <summary>
        /// ��������, �������� �� ������ ������ � ������
        /// </summary>
        public bool IsFirst
        {
            get
            {
                return (Prev == null);
            }
        }

        /// <summary>
        /// ��������, �������� �� ������ ��������� � ������
        /// </summary>
        public bool IsLast
        {
            get
            {
                return (Next == null);
            }
        }

        /// <summary>
        /// ���������� ���� �� ������, � ������� �� ���������
        /// </summary>
        public void Remove()
        {
            if (FNext != null)
            {
                List lst = GetHeader();
                FNext.FPrev = FPrev;
                FPrev.FNext = FNext;
                FNext = null;
                FPrev = null;
                lst.TimeStat.AddData(Global.SimTime() - InsertTime);
                lst.LengthStat.AddData(lst.Size, Global.SimTime());
            }
        }

        /// <summary>
        /// ���������� ������ ����. ���� ����������� �� ������.
        /// � ���������������� ������ ������������ ������ 
        /// ��������� ���������� ������ ���� base.Finish();
        /// </summary>
        public override void Finish()
        {
            Remove();
        }
    }

    /// <summary>
    /// ����� List - ������ �����. ��������������� ��� ������ �������� ������������ ������� ������.
    /// </summary>
    public class List : Linkage
    {
        /// <summary>
        /// ����������� �� ���������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, ���������������� �������� �������� ��������.
        /// ������� ��������� �� ��������.
        /// </summary>
        public List()
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ��������� ������� ������������� �������.
        /// ������� ��������� �� ��������.
        /// </summary>
        /// <param name="simTime">������������ �����, ��������������� �������� ������</param>
        public List(double simTime)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, ���������������� �������� �������� ��������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� �����������</param>
        public List(CompareFunction order)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ��������� ������� ������������� �������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� �����������</param>
        /// <param name="simTime">������������ �����, ��������������� �������� ������</param>
        public List(CompareFunction order, double simTime)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
        }

        /// <summary>
        /// ����������� � ��������� ���������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, ���������������� �������� �������� ��������.
        /// ������� ��������� �� ��������.
        /// </summary>
       /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ��������� ������� ������������� �������.
        /// ������� ��������� �� ��������.
        /// </summary>
        /// <param name="simTime">������������ �����, ��������������� �������� ������</param>
       /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, ���������������� �������� �������� ��������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� �����������</param>
       /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(CompareFunction order, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ��������� ������� ������������� �������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� �����������</param>
        /// <param name="simTime">������������ �����, ��������������� �������� ������</param>
       /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(CompareFunction order, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = aHeader;
        }

        /// <summary>
        /// ������� ������� ���������, ������������ ��������������� ������
        /// </summary>
        protected internal CompareFunction CompFunc = null;

        /// <summary>
        /// ���������� �� ������� ���������� ����� � �������
        /// </summary>
        public Statistics TimeStat;

        /// <summary>
        /// ���������� �� ����� ������
        /// </summary>
        public IntervalStatistics LengthStat;

        /// <summary>
        /// ��������� ��� ������ ���������� ������
        /// </summary>
        public string Header;

        /// <summary>
        /// ��������� �������� ������� ���������. 
        /// �������� ������ ��� ������� ������, ��� �������� ��� ������� ��� �� ���� ������.
        /// ���� ����� �� ���� ������� ����������, �� ����������� ������� ��������.
        /// </summary>
        public CompareFunction OrderFunc
        {
            set
            {
                if (CompFunc == null && Empty())
                {
                    CompFunc = value;
                }
            }
        }

        /// <summary>
        /// ������� ������ � ����������� ���� �������� � ���� �����
        /// </summary>
        public void Clear()
        {
            while (!Empty())
                First.Finish();
        }

        /// <summary>
        /// ������� ��������� ������ � ��������� � �������� ������������� �������
        /// </summary>
        public void ClearStat()
        {
            ClearStat(Global.SimTime());
        }

        /// <summary>
        /// ������� ��������� ������ � ��������� � ��������� ������������� �������
        /// </summary>
        /// <param name="simTime">������������ �����, ����� ����������� ������� ���������</param>
        public void ClearStat(double simTime)
        {
            TimeStat.ClearStat();
            LengthStat.ClearStat(simTime);
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        public override void Finish()
        {
            // �������� ������
            Clear();
            base.Finish();
        }

        /// <summary>
        /// �������� ������ �� �������
        /// </summary>
        /// <returns>true, ���� ������ ����. false, ���� � ��� ���� ���� �� ���� ����.</returns>
        public bool Empty()
        {
            return FNext == this;
        }

        /// <summary>
        /// ��������� ��������� ������ � �������� ������������� �������
        /// </summary>
        public void StopStat()
        {
            StopStat(Global.SimTime());
        }

        /// <summary>
        /// ��������� ��������� ������ � ��������� ������������� �������
        /// </summary>
        /// <param name="simTime">������������ �����, � �������� �������������� ����������</param>
        public void StopStat(double simTime)
        {
            LengthStat.StopStat(simTime);
        }

        /// <summary>
        /// ����������� ���������� �� ������������� ������
        /// </summary>
        /// <returns>���������� � ���� ������</returns>
        public string Statistics()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("������� ����� = {0,6:0.000} +/- {1,6:0.000}\n", LengthStat.Mean(), LengthStat.Deviation());
            Result.AppendFormat("������������ ����� = {0,2}, ������ = {1,2}\n", LengthStat.Max, Size);
            Result.AppendFormat("������� ����� �������� = {0,6:0.000}", TimeStat.Mean());
            return Result.ToString();
        }

        /// <summary>
        /// ������ �� ������ ���� ������.
        /// </summary>
        public Link First
        {
            get
            {
                return Next;
            }
        }

        /// <summary>
        /// ������ �� ��������� ���� ������.
        /// </summary>
        public Link Last
        {
            get
            {
                return Prev;
            }
        }

        /// <summary>
        /// ���������� ����� ������
        /// </summary>
        public int Size
        {
            get
            {
                int i = 0;
                Link lnk = Next;
                while (lnk != null)
                {
                    i++;
                    lnk = lnk.Next;
                }
                return i;
            }
        }
    }
}
