using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// ��������� ILinkage - ������� ��� ���������� ������� ������� � ������ �������� �������������
    /// </summary>
    public interface ILinkage
    {
        /// <summary>
        /// ������ �� ������������ ������ ������
        /// </summary>
        List Header
        {
            get;
        }

        /// <summary>
        /// ������ ��� ������. ������ �� ���������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � ��������� ������ - null.
        /// </summary>
        ILink Prev
        {
            get;
        }

        /// <summary>
        /// ������ ��� ������. ������ �� ��������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � �������� ������ - null.
        /// </summary>
        ILink Next
        {
            get;
        }

        /// <summary>
        /// ��������� ��� ������ ���������� ������ �������.
        /// </summary>
        void Finish();

        /// <summary>
        /// ��������� ������ �� ��������� ���� ������ ���������� �� ����, �������� �� ���������� ��� ������������ �������
        /// </summary>
        /// <returns>������ �� ��������� ����</returns>
        ILinkage GetNext();

        /// <summary>
        /// ��������� ������ �� ���������� ���� ������ ���������� �� ����, �������� �� ���������� ��� ������������ �������
        /// </summary>
        /// <returns>������ �� ���������� ����</returns>
        ILinkage GetPrev();

        /// <summary>
        /// ��������� ������ �� ��������� ���� ������
        /// </summary>
        /// <param name="newNext">����� ������ �� ��������� ����</param>
        void SetNext(ILinkage newNext);

        /// <summary>
        /// ��������� ������ �� ���������� ���� ������
        /// </summary>
        /// <param name="newPrev">����� ������ �� ���������� ����</param>
        void SetPrev(ILinkage newPrev);
    }

    /// <summary>
    /// ������� ��� ����������� ������� ���������, ������������ ����� ������� ���� � ������.
    /// ����������� ���� ���������� � ������ ����� ������ �����, ��� �������� ������� ���� ��������� true.
    /// </summary>
    /// <param name="a">������ �� ����������� ����</param>
    /// <param name="b">������ �� ������������ ���� ������</param>
    /// <returns>��������� ���������</returns>
    public delegate bool CompareFunction(ILink a, ILink b);

    /// <summary>
    /// ��������� ILink �������� �����������, ������������ ��� ����� ��������, ������� ����� ���� ��������� � ������
    /// </summary>
    public interface ILink : ILinkage
    {
        /// <summary>
        /// ������������ ����� ������� ���� � ������.
        /// ������������ ��� ����� ���������� �� ������� ���������� ����� � ������.
        /// </summary>
        double InsertTime
        {
            get;
            //set;
        }

        /// <summary>
        /// ��������, �������� �� ������ ������ � ������
        /// </summary>
        bool IsFirst
        {
            get;
        }

        /// <summary>
        /// ��������, �������� �� ������ ��������� � ������
        /// </summary>
        bool IsLast
        {
            get;
        }

        /// <summary>
        /// ���������� ������ �� ������������ ������ ������, � ������� ��������� ����
        /// </summary>
        /// <returns>������ �� ������������ ������</returns>
        List GetHeader();

        /// <summary>
        /// ������� ���� � ������. 
        /// ���� ��� ���� ������ ����������� ������� ���������, ������� ������������ � �� ��������������.
        /// � ��������� ������ ���� ����������� � ������ ���������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ������� ����</param>
        void Insert(List l);

        /// <summary>
        /// ������� ���� � ������ � �������������� ��������� ������� ���������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        /// <param name="cmp">������� ���������, ������� ������� ��������������� ������� ����</param>
        void Insert(List l, CompareFunction cmp);

        /// <summary>
        /// ������� ���� � ������ ����� ����������
        /// </summary>
        /// <param name="l">����, ����� �������� ������� ��������� �������</param>
        void InsertAfter(ILinkage l);

        /// <summary>
        /// ������� ���� � ������ ����� ���������
        /// </summary>
        /// <param name="l">����, ����� ������� ������� ��������� �������</param>
        void InsertBefore(ILinkage l);

        /// <summary>
        /// ������� ���� � ������ ������� ������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        void InsertFirst(List l);

        /// <summary>
        /// ������� ���� � ��������� ������� ������
        /// </summary>
        /// <param name="l">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        void InsertLast(List l);

        /// <summary>
        /// ���������� ���� �� ������, � ������� �� ���������
        /// </summary>
        void Remove();
    }

    /// <summary>
    /// ����� Link - ������� ����� ����������� ���� (������) ������
    /// </summary>
    public class Link : ILink
    {
        /// <summary>
        /// ����������� �� ���������. ��������� ������, �� ���������� �� � ���� ������
        /// </summary>
        public Link()
        {
            FPrev = FNext = Header = null;
        }

        /// <summary>
        /// ���� �����. ������ �� ���������� ���� ������
        /// </summary>
        internal ILinkage FPrev;

        /// <summary>
        /// ���� �����. ������ �� ��������� ���� ������
        /// </summary>
        internal ILinkage FNext;

        /// <summary>
        /// ������ �� ������������ ������ ������
        /// </summary>
        public List Header
        {
            get;
            internal set;
        }

        /// <summary>
        /// ������������ ����� ������� ���� � ������.
        /// ������������ ��� ����� ���������� �� ������� ���������� ����� � ������.
        /// </summary>
        public double InsertTime
        {
            get;
            internal set;
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
        /// ������ ��� ������. ������ �� ���������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � ��������� ������ - null.
        /// </summary>
        public ILink Prev
        {
            get
            {
                if (FPrev is ILink)
                    return FPrev as ILink;
                else
                    return null;
            }
        }

        /// <summary>
        /// ������ ��� ������. ������ �� ��������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � �������� ������ - null.
        /// </summary>
        public ILink Next
        {
            get
            {
                if (FNext is ILink)
                    return FNext as ILink;
                else
                    return null;
            }
        }

        /// <summary>
        /// ���������� ������ ����. ���� ����������� �� ������.
        /// � ���������������� ������ ������������ ������ 
        /// ��������� ���������� ������ ���� base.Finish();
        /// </summary>
        public virtual void Finish()
        {
            Remove();
        }

        /// <summary>
        /// ���������� ������ �� ������������ ������ ������, � ������� ��������� ����
        /// </summary>
        /// <returns>������ �� ������������ ������</returns>
        public List GetHeader()
        {
            return Header;
        }

        /// <summary>
        /// ��������� ������ �� ��������� ���� ������ ���������� �� ����, �������� �� ���������� ��� ������������ �������
        /// </summary>
        /// <returns>������ �� ��������� ����</returns>
        public ILinkage GetNext()
        {
            return FNext;
        }

        /// <summary>
        /// ��������� ������ �� ���������� ���� ������ ���������� �� ����, �������� �� ���������� ��� ������������ �������
        /// </summary>
        /// <returns>������ �� ���������� ����</returns>
        public ILinkage GetPrev()
        {
            return FPrev;
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
                ILink lnk = l.First;
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
            ILink lnk = l.First;
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
        /// ������� ���� � ������ ����� ����������
        /// </summary>
        /// <param name="l">����, ����� �������� ������� ��������� �������</param>
        public void InsertAfter(ILinkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FPrev = l;
            FNext = l.GetNext();
            FNext.SetPrev(this);
            l.SetNext(this);
            Header = l.Header;
            Header.Size++;
            Header.LengthStat.AddData(Header.Size, InsertTime);
        }

        /// <summary>
        /// ������� ���� � ������ ����� ���������
        /// </summary>
        /// <param name="l">����, ����� ������� ������� ��������� �������</param>
        public void InsertBefore(ILinkage l)
        {
            Remove();
            InsertTime = Global.SimTime();
            FNext = l;
            FPrev = l.GetPrev();
            FPrev.SetNext(this);
            l.SetPrev(this);
            Header = l.Header;
            Header.Size++;
            Header.LengthStat.AddData(Header.Size, InsertTime);
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
        /// ���������� ���� �� ������, � ������� �� ���������
        /// </summary>
        public void Remove()
        {
            if (FNext != null)
            {
                Header.Size--;
                FNext.SetPrev(FPrev);
                FPrev.SetNext(FNext);
                FNext = null;
                FPrev = null;
                Header.TimeStat.AddData(Global.SimTime() - InsertTime);
                Header.LengthStat.AddData(Header.Size, Global.SimTime());
                Header = null;
            }
        }

        /// <summary>
        /// ��������� ������ �� ��������� ���� ������
        /// </summary>
        /// <param name="newNext">����� ������ �� ��������� ����</param>
        public void SetNext(ILinkage newNext)
        {
            FNext = newNext;
        }

        /// <summary>
        /// ��������� ������ �� ���������� ���� ������
        /// </summary>
        /// <param name="newPrev">����� ������ �� ���������� ����</param>
        public void SetPrev(ILinkage newPrev)
        {
            FPrev = newPrev;
        }
    }

    /// <summary>
    /// ����� List - ������ �����. ��������������� ��� ������ �������� ������������ ������� ������.
    /// </summary>
    public class List : ILinkage
    {
        /// <summary>
        /// ����������� �� ���������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������.
        /// ������� ��������� �� ��������.
        /// ������������ ����� �� �����������.
        /// </summary>
        public List()
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������.
        /// ������� ��������� �������� ����������.
        /// ������������ ����� �� �����������.
        /// </summary>
        /// <param name="order">������� ������� ���������</param>
        public List(CompareFunction order)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������, � �������� ������������ ������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� ���������</param>
        /// <param name="max">������������ ������ �������</param>
        public List(CompareFunction order, int max)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ��������� ������� 
        /// ������������� ������� � �������� ������������ ������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� ���������</param>
        /// <param name="max">������������ ������ �������</param>
        /// <param name="simTime">������������ �����, ��������������� �������� ������</param>
        public List(CompareFunction order, int max, double simTime)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ��������� ������� ������������� ������� 
        /// � �������� ������������ ������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� ���������</param>
        /// <param name="max">������������ ������ �������</param>
        /// <param name="simTime">������������ �����, ��������������� �������� ������</param>
        /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(CompareFunction order, int max, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������, � �������� ������������ ������.
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="order">������� ������� ���������</param>
        /// <param name="max">������������ ������ �������</param>
        /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(CompareFunction order, int max, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������.
        /// ������� ��������� �������� ����������.
        /// ������������ ����� �� �����������.
        /// </summary>
        /// <param name="order">������� ������� ���������</param>
        /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(CompareFunction order, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = order;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
        }

        /// <summary>
        /// �����������. 
        /// ������ ��������� � ��������� � ��������� ������� ������������� ������� 
        /// � �������� ������������ ������.
        /// ������� ��������� �� ��������.
        /// </summary>
        /// <param name="max">������������ ������ �������</param>
        /// <param name="simTime">������������ �����, ��������������� �������� ������</param>
        /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(int max, double simTime, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0, simTime);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// ����������� �� ���������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������, � �������� ������������ ������.
        /// ������� ��������� �� ��������.
        /// </summary>
        /// <param name="max">������������ ������ �������</param>
        public List(int max)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// ����������� � ��������� ���������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������, � �������� ������������ ������.
        /// ������� ��������� �� ��������.
        /// </summary>
        /// <param name="max">������������ ������ �������</param>
        /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(int max, string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
            MaxSize = max;
        }

        /// <summary>
        /// ����������� � ��������� ���������. 
        /// ������ ��������� � ��������� � ������� ������������� �������, 
        /// ���������������� �������� �������� ��������.
        /// ������� ��������� �� ��������.
        /// ������������ ����� �� �����������.
        /// </summary>
        /// <param name="aHeader">��������� ������ ��� ������ ����������</param>
        public List(string aHeader)
        {
            FNext = FPrev = this;
            CompFunc = null;
            LengthStat = new IntervalStatistics(0);
            TimeStat = new Statistics();
            StatHeader = aHeader;
            Header = this;
        }

        /// <summary>
        /// ������� ������� ���������, ������������ ��������������� ������
        /// </summary>
        protected internal CompareFunction CompFunc = null;

        /// <summary>
        /// ���� �����. ������ �� ���������� ���� ������
        /// </summary>
        internal ILinkage FPrev;

        /// <summary>
        /// ���� �����. ������ �� ��������� ���� ������
        /// </summary>
        internal ILinkage FNext;

        /// <summary>
        /// ���������� �� ����� ������
        /// </summary>
        public IntervalStatistics LengthStat;

        /// <summary>
        /// ����������� ��������� ������ �������. ������ ������� List � Link 
        /// �� ��������� ���, ������ ��� �������� ����� ����������� �������� �������
        /// � ����������� ��� ������ � ���������. 
        /// ��������, ������ 0, �������� ���������� ����������� �� �����.
        /// </summary>
        public int MaxSize;

        /// <summary>
        /// ��������� ��� ������ ���������� ������
        /// </summary>
        public string StatHeader;

        /// <summary>
        /// ���������� �� ������� ���������� ����� � �������
        /// </summary>
        public Statistics TimeStat;

        /// <summary>
        /// ������ �� ������ ���� ������.
        /// </summary>
        public ILink First
        {
            get
            {
                return Next;
            }
        }

        /// <summary>
        /// ������ �� ������������ ������ ������
        /// </summary>
        public List Header
        {
            get;
            internal set;
        }

        /// <summary>
        /// ������ �� ��������� ���� ������.
        /// </summary>
        public ILink Last
        {
            get
            {
                return Prev;
            }
        }

        /// <summary>
        /// ������ ��� ������. ������ �� ��������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � �������� ������ - null.
        /// </summary>
        public ILink Next
        {
            get
            {
                if (FNext is ILink)
                    return FNext as ILink;
                else
                    return null;
            }
        }

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
        /// ������ ��� ������. ������ �� ���������� ����, 
        /// ���� �� �������� ���������� ������� ������.
        /// � ��������� ������ - null.
        /// </summary>
        public ILink Prev
        {
            get
            {
                if (FPrev is ILink)
                    return FPrev as ILink;
                else
                    return null;
            }
        }

        /// <summary>
        /// ���������� ����� ������
        /// </summary>
        public int Size
        {
            get;
            internal set;
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
        /// �������� ������ �� �������
        /// </summary>
        /// <returns>true, ���� ������ ����. false, ���� � ��� ���� ���� �� ���� ����.</returns>
        public bool Empty()
        {
            return FNext == this;
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        public virtual void Finish()
        {
            // �������� ������
            Clear();
            FNext = null;
            FPrev = null;
        }

        /// <summary>
        /// ��������� ������ �� ��������� ���� ������ ���������� �� ����, �������� �� ���������� ��� ������������ �������
        /// </summary>
        /// <returns>������ �� ��������� ����</returns>
        public ILinkage GetNext()
        {
            return FNext;
        }

        /// <summary>
        /// ��������� ������ �� ���������� ���� ������ ���������� �� ����, �������� �� ���������� ��� ������������ �������
        /// </summary>
        /// <returns>������ �� ���������� ����</returns>
        public ILinkage GetPrev()
        {
            return FPrev;
        }

        /// <summary>
        /// ��������� ������ �� ��������� ���� ������
        /// </summary>
        /// <param name="newNext">����� ������ �� ��������� ����</param>
        public void SetNext(ILinkage newNext)
        {
            FNext = newNext;
        }

        /// <summary>
        /// ��������� ������ �� ���������� ���� ������
        /// </summary>
        /// <param name="newPrev">����� ������ �� ���������� ����</param>
        public void SetPrev(ILinkage newPrev)
        {
            FPrev = newPrev;
        }

        /// <summary>
        /// ����������� ���������� �� ������������� ������
        /// </summary>
        /// <returns>���������� � ���� ������</returns>
        public string Statistics()
        {
            StringBuilder Result = new StringBuilder(StatHeader + "\n");
            Result.AppendFormat("������� ����� = {0,6:0.000} +/- {1,6:0.000}\n", LengthStat.Mean(), LengthStat.Deviation());
            Result.AppendFormat("������������ ����� = {0,2}, ������ = {1,2}\n", LengthStat.Max, Size);
            Result.AppendFormat("������� ����� �������� = {0,6:0.000}", TimeStat.Mean());
            return Result.ToString();
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
    }
}
