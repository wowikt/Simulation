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

        public override void Finish()
        {
            if (Proc != null)
            {
                if (Proc is SimProc)
                {
                    Proc.Event = null;
                    Proc = null;
                }
                else if (Proc.Event == this)
                {
                    Proc.Event = null;
                    Proc.Finish();
                    Proc.Dispose();
                    Proc = null;
                }
            }
            base.Finish();
        }

        /// <summary>
        /// ��������� �������, ���������� � ���������
        /// </summary>
        public override object RunEvent()
        {
            Global.CurrSim.CurrentSimTime = EventTime;
            Global.CurrProc = Proc;
            return Proc.Resume();
        }

        /// <summary>
        /// ����������� ����������� ����������� � ������� � ��������� ����
        /// </summary>
        /// <returns>����� �������� � ����� ���������������� �������</returns>
        public override string ToString()
        {
            return "����������� � ������� �������� " + Proc.ToString() + " � " + EventTime.ToString();
        }
    }

    /// <summary>
    /// ����� Process - ������� ����� ��������, ������������ ��� ���������� ��������
    /// </summary>
    public class Process : Coroutine, ISchedulable
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
            // ���� ������������ ������� ���������� (�� ����, ������ ������� - �� ������� ��������),
            //   ������ � ������ ��������� ���������
            if (Parent != null && Parent != this)
            {
                StartRunning();
            }
            // ����������� � ������� ���
            Event = null;
            //Global.CurrProc = this;
        }

        /// <summary>
        /// ����������� � �������, ��������� � ���������
        /// </summary>
        public EventNotice Event
        {
            get;
            internal set;
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
                    return (Event as EventNotice).EventTime;
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
            ActivateDelay(0);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� ��������������� ����� ��������� ������
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
        /// ������� ������ ����������� �� ��������� �������� ��������������� ����� ������ ���������� ��������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� �������� ������� ������������ ������</param>
        public void ActivateAfter(ISchedulable p)
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
        /// ������� ������ ����������� �� ��������� �������� ��������������� ����� ������� ���������� ��������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� ������� ������� ������������ ������</param>
        public void ActivateBefore(ISchedulable p)
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
        /// �������� ����������� ���������. 
        /// ������������ ��� ����������� ���������� ������� � ���������
        /// </summary>
        public void ClearFinished()
        {
            Parent.Collect.Activate();
        }

        /// <summary>
        /// �������� �������� ������ ��������. ������ ���� ������������� � ����������� ������.
        /// � ������ ������ ������ �� ������.
        /// </summary>
        protected override void Execute()
        {
        }

        /// <summary>
        /// ���������� ������ ��������. ������� ����������� � ������� �� ���������.
        /// � ���������������� ������ ������������ ������ ����������� ������ ���������� ���������.
        /// </summary>
        public override void Finish()
        {
            EventNotice en = Event;
            if (Event != null)
            {
                (Event as ProcessEventNotice).Proc = null;
                Event = null;
                en.Finish();
            }
            base.Finish();
        }

        /// <summary>
        /// ��������� �������� � ������ ����������� ��� ������������ ��������������� ��������.
        /// <para>����� �������, �� ����������� ���� ����� ������� Finish(), ������ ��������� ���� ������ ����������� ����� ������</para>
        /// </summary>
        public void GoFinished()
        {
            Insert(Parent.Finished);
        }

        /// <summary>
        /// ������������ ��� �������, ��������������� �� ������� �����, � ������������ �������
        /// <para>������������ ������ Hold(0)</para>
        /// </summary>
        public void Hold()
        {
            Hold(0);
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
        /// ������������� ��������
        /// <para>����� �������������� ��� �������� ���������� �������� ��������, ������� ������ ����������� � ��������� ��������</para>
        /// <para>� ������� �� ������������, ����������� � ��������� ������ ������� ��������</para>
        /// </summary>
        protected virtual void Init()
        {
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
            EventNotice pen = Event;
            (Event as ProcessEventNotice).Proc = null;
            Event = null;
            pen.Finish();
            if (Global.CurrProc == this)
            {
                RunNextProc();
            }
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� ��������, � ��� ����� �����������������, ��������������� ����� ��������.
        /// <para>���� ������� ��������� � �������� ��������� (�� ����, �������� �������), ������ �� ������</para>
        /// </summary>
        public void Reactivate()
        {
            ReactivateDelay(0);
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
                (Event as EventNotice).EventTime = en.EventTime;
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
        /// ���������� ������ ����������� �� ��������� �������� ��������������� ����� ������ ���������� ��������
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� ������� ������� ������������ ������</param>
        public void ReactivateAfter(ISchedulable p)
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
        /// ���������� ������ ����������� �� ��������� �������� ��������������� ����� ������� ���������� ��������
        /// <para>���� ������� ��������� � ��������� ���������, ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, ����������� ����������</para>
        /// </summary>
        /// <param name="p">�������, ����� ������� ������� ������������ ������</param>
        public void ReactivateBefore(ISchedulable p)
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
        /// ���������� ���������� ������ Execute() � ��������� ��������
        /// </summary>
        protected override void Run()
        {
            Global.CurrProc = this;
            Init();
            StartingTime = SimTime();
            Execute();
            TerminatedState = true;
            EventNotice en = Event;
            (Event as ProcessEventNotice).Proc = null;
            Event = null;
            en.Finish();
            //Remove();
        }

        /// <summary>
        /// ������� � ���������� ������� � ���������
        /// </summary>
        private void RunNextProc()
        {
            Yield(new ContinueSim());
        }

        /// <summary>
        /// ���������� ������� ������������ �����. 
        /// ���������� � ������������� ������� ��������.
        /// </summary>
        /// <returns>������� ������������ �����</returns>
        public virtual double SimTime()
        {
            return Parent.SimTime();
        }

        /// <summary>
        /// ��������� �������� � ������ ���������. ������������ ����� 
        /// ��� ���������� �������� �� ������� �������� 
        /// (��� ���� �� ��� ����� �������� � ������ ��������� ���������)
        /// </summary>
        public void StartRunning()
        {
            Insert(Parent.Running);
        }

        /// <summary>
        /// ���������� �������� � ������� � ������� ��� � ����� ��������
        /// </summary>
        /// <param name="q">������� ��������</param>
        public void Wait(List q)
        {
            Insert(q);
            Passivate();
        }
    }

    /// <summary>
    /// ��������� �����, ��������������� ��� �������� �� ����������� ������� ����������
    /// � ���, ��� �������� ������ ���� ��������������
    /// </summary>
    internal class StopSim
    {
    }

    /// <summary>
    /// ��������� �����, ��������������� ��� �������� �� ����������� ������� ����������
    /// � ���, ��� �������� ������ ������������
    /// </summary>
    internal class ContinueSim
    {
    }

    /// <summary>
    /// ����� SimProc - ������� ������� ��������
    /// </summary>
    public class SimProc : Process
    {
        /// <summary>
        /// �����������.
        /// </summary>
        public SimProc()
        {
            Global.CurrSim = this;
            CurrentSimTime = 0;
            Parent = this;
            Calendar = new List(CalendarOrder, 0, "���������");
            Finished = new List();
            Running = new List();
            Event = new ProcessEventNotice(0, this);
            Event.InsertFirst(Calendar);
        }

        /// <summary>
        /// ��������� �������
        /// </summary>
        public List Calendar;

        /// <summary>
        /// ������� ����������� ���������
        /// </summary>
        internal Collector Collect;

        /// <summary>
        /// ������� ������������ �����
        /// </summary>
        public double CurrentSimTime;

        /// <summary>
        /// ���������, ����������� ������� ������ ��������
        /// </summary>
        internal SimulationDispatcher Dispatcher;

        /// <summary>
        /// ������ ����������� ��������� ��� �������������� ��������
        /// </summary>
        public List Finished;

        /// <summary>
        /// ������ ��������� ���������
        /// </summary>
        public List Running;

        internal static StopSim Stopped = new StopSim();

        /// <summary>
        /// �������-������������
        /// </summary>
        internal Visualizer VisProc;

        /// <summary>
        /// ��� ������������
        /// </summary>
        public double VisualInterval
        {
            get
            {
                if (VisProc == null)
                {
                    return 0;
                }
                else
                {
                    return VisProc.Interval;
                }
            }
            set
            {
                if (VisProc == null)
                {
                    VisProc = new Visualizer(value);
                }
                else
                {
                    VisProc.Interval = value;
                }
            }
        }

        /// <summary>
        /// ����������� ������� ��������� ��� ��������� �������. 
        /// ������������ ��� ������� ����������� � �������� � ��������� ��� ����������
        /// </summary>
        /// <param name="a">������ �� ����������� �����������</param>
        /// <param name="b">������ �� ��������� ����������� ������</param>
        /// <returns>��������� ���������</returns>
        private static bool CalendarOrder(ILink a, ILink b)
        {
            return (a as EventNotice).EventTime < (b as EventNotice).EventTime;
        }

        /// <summary>
        /// ������� ����������
        /// </summary>
        public virtual void ClearStat()
        {
            Calendar.ClearStat();
        }

        /// <summary>
        /// ����� �� ������� ��������� �������. ������������ � ����� ����������� ��� ������� ��������
        /// </summary>
        public void DumpEventQueue()
        {
            EventNotice en = Calendar.First as EventNotice;
            while (en != null)
            {
                Console.WriteLine(en);
                en = en.Next as EventNotice;
            }
        }

        /// <summary>
        /// ���������� ��������
        /// </summary>
        public override void Finish()
        {
            Finished.Finish();
            Running.Finish();
            Calendar.Finish();
            base.Finish();
        }

        /// <summary>
        /// ����������� ����� �������� ��������. ������� �� ������ ���������� ����� ��� ����������������
        /// </summary>
        protected override void Run()
        {
            Global.CurrProc = this;
            Global.CurrSim = this;
            Collect = new Collector();
            Init();
            StartingTime = SimTime();
            Collect.Activate();
            if (VisProc != null)
            {
                VisProc.Activate();
            }
            Execute();
            TerminatedState = true;
            StopStat();
            GC.Collect();
            Yield(new StopSim());
            //while (true)
                //Yield(Dispatcher);
        }

        /// <summary>
        /// ���������� ������� ������������ ����� ������ ��������
        /// </summary>
        /// <returns>������������ �����</returns>
        public override double SimTime()
        {
            return CurrentSimTime;
        }

        /// <summary>
        /// ������ �������� ��������. ����������, ����������� ���������, ����������� ���������
        /// </summary>
        public void Start()
        {
            object res;
            int i = 0;
            Global.CurrSim = this;
            Global.CurrProc = this;
            do
            {
                // �������� ��������� ������ ��������� �������
                EventNotice en = Calendar.First as EventNotice;
                // ��������� �������
                res = en.RunEvent();
                i++;
            }
            while (!(res is StopSim));
            // ���� ��������� ���������� - ������ ���������, ������������� ����������
            Global.CurrProc = null;
        }

        /// <summary>
        /// ��������� ���������� � �������� �������
        /// </summary>
        public virtual void StopStat()
        {
            Calendar.StopStat();
        }
    }
}
