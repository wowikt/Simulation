using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
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
            FPrev = FNext = HeaderNode = null;
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
        public List HeaderNode
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
            return HeaderNode;
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
        /// <param name="lst">������ �� ������������ ������ ������, � ������� ����������� ������� ����</param>
        public void Insert(List lst)
        {
            lst.Insert(this);
        }

        /// <summary>
        /// ������� ���� � ������ � �������������� ��������� ������� ���������
        /// </summary>
        /// <param name="lst">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        /// <param name="cmp">������� ���������, ������� ������� ��������������� ������� ����</param>
        public void Insert(List lst, CompareFunction cmp)
        {
            lst.Insert(this, cmp);
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
            HeaderNode = l.HeaderNode;
            HeaderNode.Size++;
            HeaderNode.LengthStat.AddData(HeaderNode.Size, InsertTime);
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
            HeaderNode = l.HeaderNode;
            HeaderNode.Size++;
            HeaderNode.LengthStat.AddData(HeaderNode.Size, InsertTime);
        }

        /// <summary>
        /// ������� ���� � ������ ������� ������
        /// </summary>
        /// <param name="lst">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        public void InsertFirst(List lst)
        {
            // ������� �� ������ ����� - ��� ������� ����� ������������ ������
            lst.InsertFirst(this);
        }

        /// <summary>
        /// ������� ���� � ��������� ������� ������
        /// </summary>
        /// <param name="lst">������ �� ������������ ������ ������, � ������� ����������� ����</param>
        public void InsertLast(List lst)
        {
            // ������� �� ��������� ����� - ��� ������� ����� ������������ �������
            lst.InsertLast(this);
        }

        /// <summary>
        /// ���������� ���� �� ������, � ������� �� ���������
        /// </summary>
        public void Remove()
        {
            if (FNext != null)
            {
                HeaderNode.Size--;
                FNext.SetPrev(FPrev);
                FPrev.SetNext(FNext);
                FNext = null;
                FPrev = null;
                HeaderNode.TimeStat.AddData(Global.SimTime() - InsertTime);
                HeaderNode.LengthStat.AddData(HeaderNode.Size, Global.SimTime());
                List head = HeaderNode;
                HeaderNode = null;
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
}
