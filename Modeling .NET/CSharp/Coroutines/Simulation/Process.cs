using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// ����� ������������ �������, ��������� � �������������� ��������
    /// </summary>
    internal class ProcessEventNotice : EventNotice
    {
        /// <summary>
        /// �����������. ���������� �������� ���������� � ���� �������
        /// </summary>
        /// <param name="time">����� ����������� �������</param>
        /// <param name="proc">�������, ������������ ��� ����������� �������</param>
        public ProcessEventNotice(double time, Process proc) : base(time)
        {
            Proc = proc;
        }

        /// <summary>
        /// �������, ������� ������������ ��� ����������� �������
        /// </summary>
        internal Process Proc;

        /// <summary>
        /// ��������� �������, ���������� � ���������
        /// </summary>
        public override void RunEvent()
        {
            Global.CurrSim.CurrentSimTime = EventTime;
            Proc.SwitchTo();
        }
    }

    /// <summary>
    /// ����� Process - ������� ����� ��������, ������������ ��� ���������� ��������
    /// </summary>
    public class Process : Coroutine
    {
        /// <summary>
        /// �����������. ������������� ������ �� ������������ ��������.
        /// </summary>
        public Process()
        {
            // �������� ������ �� ������������ ������� ��������
            // ������ ������ �� �������� ����� ���� ������ � �������� �������� ��������
            if (Owner == null || Owner is SimProc)
                Parent = Owner as SimProc;
            else
                // ���� ������ �������� � �� ��������, �� ��� ������� �������
                Parent = (Owner as Process).Parent;
            // ����������� � ������� ���
            Event = null;
        }

        /// <summary>
        /// ������� ��������, � ������ ������� ����������� ������ �������
        /// </summary>
        protected internal SimProc Parent;

        /// <summary>
        /// ������������ ����� ������ ������ ��������
        /// </summary>
        public double StartingTime;

        /// <summary>
        /// ������������ �����, ���������� �� ���������� �������������� ��������.
        /// ������������, ���� ������� �������� ���� ����������� ������ ���������.
        /// </summary>
        public double TimeLeft;

        /// <summary>
        /// ����������� � �������, ��������� � ���������
        /// </summary>
        internal EventNotice Event;

        /// <summary>
        /// ���������� ���������� ������ Run() � ��������� ��������
        /// </summary>
        protected override void Execute()
        {
            Global.CurrProc = this;
            Detach();
            Init();
            StartRunning();
            StartingTime = SimTime();
            Run();
            TerminatedState = true;
            Passivate();
            while (true)
                Detach();
        }

        /// <summary>
        /// ������������� ��������
        /// <para>����� �������������� ��� �������� ���������� �������� ��������, ������� ������ ����������� � ��������� ��������</para>
        /// <para>� ������� �� ������������, ����������� � ��������� ������ ������� ��������</para>
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// ���������� ������ ��������. ������� ����������� � ������� �� ���������.
        /// � ���������������� ������ ������������ ������ ����������� ������ ���������� ���������.
        /// </summary>
        public override void Finish()
        {
            if (Event != null)
                Event.Finish();
            base.Finish();
        }

        /// <summary>
        /// �������� �������� ������ ��������. ������ ���� ������������� � ����������� ������.
        /// � ������ ������ ������ �� ������.
        /// </summary>
        protected override void Run()
        {

        }

        private void RunNextProc()
        {
            // ������� � ������� ������� � ���������
            (Parent.Calendar.First as EventNotice).RunEvent();
            //Parent.CurrentSimTime = (Parent.Calendar.First as EventNotice).EventTime;
            //(Parent.Calendar.First as ProcessEventNotice).Proc.SwitchTo();
        }

        /// <summary>
        /// ����������� ������� ������������ �����. 
        /// ���������� � ������������� ������� ��������.
        /// </summary>
        /// <returns>������� ������������ �����</returns>
        public virtual double SimTime()
        {
            return Parent.SimTime();
        }

        /// <summary>
        /// ����� ����������� ������� ������� ��������. 
        /// ���� ������� ���, ������� ������������� �������� (-1e300).
        /// </summary>
        public double EventTime
        {
            get
            {
                if (Idle)
                    return -1e300;
                else
                    return Event.EventTime;
            }
        }

        /// <summary>
        /// ���������, ��������� �� ������� � ��������� ��� ����������� ���������
        /// </summary>
        public bool Idle
        {
            get
            {
                return Event == null;
            }
        }

        /// <summary>
        /// �������� ������ ����������� �� ��������� �������� ��������������� ����� ��������.
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// </summary>
        public void Activate()
        {
            ActivateAfter(Parent.Calendar.First as EventNotice);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� � ��������� �����
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// </summary>
        /// <param name="t">������������ ����� ��������� ��������</param>
        public void ActivateAt(double t)
        {
            if (!Idle)
            {
                return;                
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            Event = new ProcessEventNotice(t, this);
            Event.Insert(Parent.Calendar);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� � ��������� ��������� �������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// </summary>
        /// <param name="dt">�������� ������������ �������� ������������� �������</param>
        public void ActivateDelay(double dt)
        {
            ActivateAt(SimTime() + dt);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� � ��������� ����� � ����������� �� ��������� � ���������,
        /// ��������������� ����� �� �� �� �����
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// </summary>
        /// <param name="t">������������ ����� ��������� ��������</param>
        public void ActivatePriorAt(double t)
        {
            if (!Idle)
            {
                return;                
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            Event = new ProcessEventNotice(t, this);
            Event.InsertPrior(Parent.Calendar);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� � ��������� ��������� ������� � ����������� �� ��������� � ���������,
        /// ��������������� ����� �� �� �� �����
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// </summary>
        /// <param name="dt">�������� ������������ �������� ������������� �������</param>
        public void ActivatePriorDelay(double dt)
        {
            ActivatePriorAt(SimTime() + dt);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� ��������������� ����� ������� ���������� ��������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� ������� ������� ������������ ������</param>
        public void ActivateBefore(Process p)
        {
            if (!Idle)
            {
                return;                
            }
            if (p.Idle)
            {
                throw new ESimulationException("������ ��������� ActivateAfter(p) ����� ���������� ��������");
            }
            ActivateBefore(p.Event);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� ��������������� ����� ��������� �������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="en">������ ����������� � �������, ����� ������� ������� ��������� ������ ������ �������</param>
        internal void ActivateBefore(EventNotice en)
        {
            if (!Idle)
            {
                return;                
            }
            Event = new ProcessEventNotice(en.EventTime, this);
            Event.InsertBefore(en);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� ��������������� ����� ������ ���������� ��������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� �������� ������� ������������ ������</param>
        public void ActivateAfter(Process p)
        {
            if (!Idle)
            {
                return;                
            }
            if (p.Idle)
            {
                throw new ESimulationException("������ ��������� ActivateAfter(p) ����� ���������� ��������");
            }
            ActivateAfter(p.Event);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� ��������������� ����� ��������� ������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="en">������ ����������� � �������, ����� ������� ������� ��������� ������ ������ �������</param>
        internal void ActivateAfter(EventNotice en)
        {
            if (!Idle)
            {
                return;                
            }
            Event = new ProcessEventNotice(en.EventTime, this);
            Event.InsertAfter(en);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� ��������, � ��� ����� �����������������, ��������������� ����� ��������.
        /// <para>���� ������� ��������� � �������� ��������� (�� ����, �������� �������), ������ �� ������</para>
        /// </summary>
        public void Reactivate()
        {
            ReactivateAfter(Parent.Calendar.First as EventNotice);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� �� ��������� �����
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// </summary>
        /// <param name="t">������������ ����� ��������� ��������</param>
        public void ReactivateAt(double t)
        {
            if (Idle)
            {
                ActivateAt(t);
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            if (Event.IsFirst)
            {
                Event.SetTime(t);
                RunNextProc();
            }
            else
            {
                Event.SetTime(t);
            }
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� � ��������� ��������� ������� ������������ �������� ��������.
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// </summary>
        /// <param name="dt">������������ ����� ��������� ��������</param>
        public void ReactivateDelay(double dt)
        {
            ReactivateAt(SimTime() + dt);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� �� ��������� ����� � �����������.
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// </summary>
        /// <param name="t">������������ ����� ��������� ��������</param>
        public void ReactivatePriorAt(double t)
        {
            if (Idle)
            {
                ActivatePriorAt(t);
                return;
            }
            if (t < SimTime())
            {
                t = SimTime();
            }
            if (Event.IsFirst)
            {
                Event.SetTimePrior(t);
                RunNextProc();
            }
            else
            {
                Event.SetTimePrior(t);
            }
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� � ��������� ��������� ������� ������������ �������� �������� � �����������.
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// </summary>
        /// <param name="dt">������������ ����� ��������� ��������</param>
        public void ReactivatePriorDelay(double dt)
        {
            ReactivatePriorAt(SimTime() + dt);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� ��������������� ����� ������� ���������� ��������
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� ������� ������� ������������ ������</param>
        public void ReactivateBefore(Process p)
        {
            if (Idle)
            {
                ActivateBefore(p);
                return;
            }
            if (p.Idle)
            {
                throw new ESimulationException("������ ��������� ReactivateBefore(p) ����� ���������� ��������");
            }
            ReactivateBefore(p.Event);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� ��������������� ����� ��������� �������
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="en">������, ����� ������� ������� ������������ �������</param>
        internal void ReactivateBefore(EventNotice en)
        {
            if (Idle)
            {
                ActivateBefore(en);
                return;
            }
            if (Event.IsFirst)
            {
                Event.EventTime = en.EventTime;
                Event.InsertBefore(en);
                RunNextProc();
            }
            else
            {
                Event.EventTime = en.EventTime;
                Event.InsertBefore(en);
            }
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� ��������������� ����� ������ ���������� ��������
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� ������� ������� ������������ ������</param>
        public void ReactivateAfter(Process p)
        {
            if (Idle)
            {
                ActivateAfter(p);
                return;
            }
            if (p.Idle)
            {
                throw new ESimulationException("������ ��������� ReactivateAfter(p) ����� ���������� ��������");
            }
            ReactivateAfter(p.Event);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� ��������������� ����� ��������� ������
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="en">������, ����� ������� ������� ������������ �������</param>
        internal void ReactivateAfter(EventNotice en)
        {
            if (Idle)
            {
                ActivateAfter(en);
                return;
            }
            if (Event.IsFirst)
            {
                Event.EventTime = en.EventTime;
                Event.InsertAfter(en);
                RunNextProc();
            }
            else
            {
                Event.EventTime = en.EventTime;
                Event.InsertAfter(en);
            }
        }

        /// <summary>
        /// ��������� ������� � ��������� ���������, ������ ��� ������ ����������� � ������� �� ���������
        /// <para>���� ������� �������� �������, ���������� ���������� ���������� �� ������� ��������</para>
        /// </summary>
        public void Passivate()
        {
            if (Idle)
            {
                return;
            }
            Event.Finish();
            Event = null;
            if (Global.CurrProc == this)
            {
                RunNextProc();
            }
        }

        /// <summary>
        /// ���������������� ������� �� �������� ���������� �������
        /// </summary>
        /// <param name="dt">����� ������������ ��������</param>
        public void Hold(double dt)
        {
            ReactivateDelay(dt);
        }

        /// <summary>
        /// ��������� �������� � ������ ���������. ������������ ����� ��� ���������� �������� �� ������� ��������
        /// </summary>
        public void StartRunning()
        {
            Insert(Parent.Running);
        }

        /// <summary>
        /// ��������� �������� � ������ ����������� ��� ������������ ��������������� ��������.
        /// <para>����� �������, �� ������������ ���� ����� ������� Finish(), ������ ��������� ���� ������ ����������� ����� ������</para>
        /// </summary>
        public void GoFinished()
        {
            Insert(Parent.Finished);
        }

        /// <summary>
        /// �������� ����������� ���������. ������������ ��� ����������� ���������� ������� � ���������
        /// </summary>
        public void ClearFinished()
        {
            Parent.Finished.Clear();
        }
    }

    /// <summary>
    /// ����� Simulation - ������� ������� ��������
    /// </summary>
    public class SimProc : Process
    {
        /// <summary>
        /// �����������.
        /// </summary>
        public SimProc()
        {
            CurrentSimTime = 0;
            Parent = this;
            Calendar = new List(CalendarOrder, 0, "���������");
            Finished = new List();
            Running = new List();
            Event = new ProcessEventNotice(0, this);
            Event.InsertFirst(Calendar);
        }

        private static bool CalendarOrder(Link a, Link b)
        {
            return (a as EventNotice).EventTime < (b as EventNotice).EventTime;
        }

        /// <summary>
        /// ��������� �������
        /// </summary>
        public List Calendar;

        /// <summary>
        /// ������� ������������ �����
        /// </summary>
        internal double CurrentSimTime;

        internal List Finished;

        internal List Running;

        /// <summary>
        /// ���������� ������� ������������ ����� ������ ��������
        /// </summary>
        /// <returns>������������ �����</returns>
        public override double SimTime()
        {
            return CurrentSimTime;
        }

        /// <summary>
        /// ���������� ��������
        /// </summary>
        public override void Finish()
        {
            Finished.Finish();
            Running.Finish();
            base.Finish();
        }

        /// <summary>
        /// ����������� ����� �������� ��������. ������� �� ������ ���������� ����� ��� ����������������
        /// </summary>
        protected override void Execute()
        {
            Global.CurrProc = this;
            Global.CurrSim = this;
            Init();
            Detach();
            StartingTime = SimTime();
            Run();
            TerminatedState = true;
            while (true)
                Detach();
        }

        /// <summary>
        /// ����� �� ������� ��������� �������. ������������ � ����� ����������� ��� ������� ��������
        /// </summary>
        public void DumpEventQueue()
        {
            EventNotice en = Calendar.First as EventNotice;
            while (en != null)
            {
                if (en is ProcessEventNotice)
                {
                    Console.WriteLine((en as ProcessEventNotice).Proc);
                    en = en.Next as EventNotice;
                }
            }
        }
    }
}
