using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Fibers;

namespace Simulation
{
    /// <summary>
    /// ����� <c>Coroutine</c> - ������� ����� ��� ���������� ����������
    /// </summary>
    public class Coroutine : Fiber, ILink
    {
        /// <summary>
        /// �����������. 
        /// <para>������� ��� ����������� �������������� � ������������ ���������� ���������� ��������� ���������</para>
        /// </summary>
        public Coroutine()
        {
            Owner = Global.CurrProc;
            //Resume();
        }

        /// <summary>
        /// ���� �����. ������ �� ��������� ���� ������
        /// </summary>
        internal ILinkage FNext;

        /// <summary>
        /// ���� �����. ������ �� ���������� ���� ������
        /// </summary>
        internal ILinkage FPrev;

        /// <summary>
        /// ������ �� ������������ ������ ������
        /// </summary>
        public List Header
        {
            get;
            internal set;
        }

        /// <summary>
        /// �����������-�������� ������� ��� null, ���� ���������� �������� ������� �����
        /// </summary>
        internal Coroutine Owner;

        /// <summary>
        /// ����, ����������� �� ����������� ��������� �����������. 
        /// �������� ��� ������ ����������� �������� <c>Terminated</c>.
        /// </summary>
        protected bool TerminatedState;

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
        /// ���������, ��������� �� ������ �����������
        /// </summary>
        public bool Terminated
        {
            get
            {
                return TerminatedState;
            }
        }

        /// <summary>
        /// ������������ � �����������-���������
        /// </summary>
        protected void Detach()
        {
            if (Global.CurrProc == this)
            {
                Yield(Owner);
            }
        }

        /// <summary>
        /// �������� �������� ������ �����������. 
        /// <para>������ ���������������� � ����������� �������.
        /// � ������ ������ ������ �� ������.</para>
        /// </summary>
        protected virtual void Execute()
        {
        }

        /// <summary>
        /// ���������� ������ ����. ���� ����������� �� ������.
        /// � ���������������� ������ ������������ ������ 
        /// ��������� ���������� ������ ���� base.Finish();
        /// </summary>
        public virtual void Finish()
        {
            Remove();
            Dispose();
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
            Header = GetPrev().Header;
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
            Header = GetNext().Header;
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
        /// <para>�����, �������������� ���������� ������ �����������. ���������� ���������� ������ Run() � �������� �����������.</para>
        /// <para>������� �� ������ ����������������.</para>
        /// </summary>
        protected override void Run()
        {
            Global.CurrProc = this;
            //Detach();
            Execute();
            TerminatedState = true;
            while (true)
                Detach();
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
        /// ������������ � ������ �����������
        /// </summary>
        public void SwitchTo()
        {
            Global.CurrProc.Yield(this);
        }

        /// <summary>
        /// ������������ � �������� �����������
        /// </summary>
        /// <param name="cor">������������ �����������</param>
        protected void SwitchTo(Coroutine cor)
        {
            Global.CurrProc.Yield(cor);
        }
    }
}
