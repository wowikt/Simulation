using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// ����� Process - ������� ����� ��������, ������������ ��� ���������� ��������
    /// </summary>
    public class Process : Coroutine, ISchedulable, IComponent
    {
        /// <summary>
        /// �����������. ������������� ������ �� ������������ ��������.
        /// </summary>
        public Process()
        {
            // �������� ������ �� ������������ ������� ��������
            Parent = Global.CurrSim;
            // ���� ������������ ������� ���������� � ������ ������� - �� ������� ��������,
            //   ������ � ������ ��������� ���������
            if (Parent != null && !(this is SimProc))
            {
                StartRunning();
            }
            OnActionStartingEvent = ActionStartingEvent;
            OnActionFinishedEvent = ActionFinishedEvent;
            OnEnteredNode = EnteredEvent;
            OnNodeEnterFailed = NodeEnterFailedEvent;
            OnReleased = ReleasedEvent;
            // ����������� � ������� ���
            Event = null;
            //Global.CurrProc = this;
        }

        /// <summary>
        /// ����������� � �������, ��������� � ���������
        /// </summary>
        public SchedulableEventNotice Event
        {
            get;
            set;
        }

        /// <summary>
        /// ������ �� ����� ��������� ������� ������ �������������� ��������
        /// </summary>
        public ActionEventProc OnActionStartingEvent
        {
            get;
            internal set;
        }

        /// <summary>
        /// ������ �� ����� ��������� ������� ������ �������������� ��������
        /// </summary>
        public ActionEventProc OnActionFinishedEvent
        {
            get;
            internal set;
        }

        /// <summary>
        /// ���������� �����, ���������� ����� �������� ������� ������ � ������
        /// </summary>
        public NodeEventProc OnEnteredNode
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// ���������� �����, ���������� ����� ��������� ������� ������� ������
        /// � ������ �� ������� ������������ ������
        /// </summary>
        public NodeEventProc OnNodeEnterFailed
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// ���������� �����, ���������� ����� ���������� ������ �� ������
        /// </summary>
        public NodeEventProc OnReleased
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// ������� ��������, � ������ ������� ����������� ������ �������
        /// </summary>
        public IMainSimulation Parent
        {
            get;
            set;
        }

        /// <summary>
        /// ������ ������, � ������� ���������� ��������� � ������ ���� ������-�������
        /// ����� ��������� �������. �� ������ ��������� �������� �� 0 �� 
        /// ������������� ������� ������, � ��������� ������ ����������� ����������.
        /// ���� ������� ����� ���� ������, ��� �������� �� ����������� �� ��������.
        /// </summary>
        public int QueueIndex
        {
            get;
            set;
        }

        /// <summary>
        /// ������������ ����� ������ ������ ��������
        /// </summary>
        public double StartingTime
        {
            get;
            set;
        }

        /// <summary>
        /// ������������ �����, ���������� �� ���������� �������������� ��������.
        /// ������������, ���� ������� �������� ���� ����������� ������ ���������.
        /// </summary>
        public double TimeLeft
        {
            get;
            set;
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
                    return (Event as SchedulableEventNotice).EventTime;
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
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
        /// </summary>
        public void Activate()
        {
            ActivateDelay(0);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ��������� ������
        /// </summary>
        /// <param name="en">������ ����������� � �������, ����� ������� 
        /// ������� ��������� ������ ������ �������</param>
        internal void ActivateAfter(SchedulableEventNotice en)
        {
            if (!Idle)
            {
                return;
            }
            Event = new ProcessEventNotice(en.EventTime, this);
            Event.InsertAfter(en);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ������ ���������� ��������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, 
        /// ����������� ����������</para>
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
                throw new ESimulationException(
                    "������ ��������� ActivateAfter(p) ����� ���������� ��������");
            }
            ActivateAfter(p.Event);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� � ��������� �����
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
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
        /// ������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ��������� �������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, 
        /// ����������� ����������</para>
        /// </summary>
        /// <param name="en">������ ����������� � �������, ����� ������� 
        /// ������� ��������� ������ ������ �������</param>
        internal void ActivateBefore(SchedulableEventNotice en)
        {
            if (!Idle)
            {
                return;
            }
            Event = new ProcessEventNotice(en.EventTime, this);
            Event.InsertBefore(en);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ������� ���������� ��������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
        /// <para>���� �������-�������� ��������� � ��������� ��� ����������� ���������, 
        /// ����������� ����������</para>
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
                throw new ESimulationException(
                    "������ ��������� ActivateAfter(p) ����� ���������� ��������");
            }
            ActivateBefore(p.Event);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� � ��������� ��������� �������
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
        /// </summary>
        /// <param name="dt">�������� ������������ �������� ������������� �������</param>
        public void ActivateDelay(double dt)
        {
            ActivateAt(SimTime() + dt);
        }

        /// <summary>
        /// ������� ������ ����������� �� ��������� �������� � ��������� ����� 
        /// � ����������� �� ��������� � ���������, ��������������� ����� �� �� �� �����
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
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
        /// ������� ������ ����������� �� ��������� �������� � ��������� ��������� ������� 
        /// � ����������� �� ��������� � ���������, ��������������� ����� �� �� �� �����
        /// <para>���� ������� ��������� � �������� ��� ���������������� ���������, 
        /// ������ �� ������</para>
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
        /// � ���������������� ������ ������������ ������ 
        /// ����������� ������ ���������� ���������.
        /// </summary>
        public override void Finish()
        {
            SchedulableEventNotice en = Event;
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
        /// <para>����� �������, �� ����������� ���� ����� ������� Finish(), 
        /// ������ ��������� ���� ������ ����������� ����� ������</para>
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
        /// <para>����� �������������� ��� �������� ���������� �������� ��������, 
        /// ������� ������ ����������� � ��������� ��������</para>
        /// <para>� ������� �� ������������, ����������� 
        /// � ��������� ������ ������� ��������</para>
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// ��������� ������� � ��������� ���������, ������ ��� 
        /// ������ ����������� � ������� �� ���������
        /// <para>���� ������� �������� �������, ���������� ���������� 
        /// ���������� �� ������� ��������</para>
        /// </summary>
        public void Passivate()
        {
            if (Idle)
            {
                return;
            }
            SchedulableEventNotice pen = Event;
            (Event as ProcessEventNotice).Proc = null;
            Event = null;
            pen.Finish();
            if (Global.CurrProc == this)
            {
                RunNextProc();
            }
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� ��������, 
        /// � ��� ����� �����������������, ��������������� ����� ��������.
        /// <para>���� ������� ��������� � �������� ��������� 
        /// (�� ����, �������� �������), ������ �� ������</para>
        /// </summary>
        public void Reactivate()
        {
            ReactivateDelay(0);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ��������� ������
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, 
        /// ����������� ����������</para>
        /// </summary>
        /// <param name="en">������, ����� ������� ������� ������������ �������</param>
        internal void ReactivateAfter(SchedulableEventNotice en)
        {
            if (Idle)
            {
                ActivateAfter(en);
                return;
            }
            if (Event.IsFirst)
            {
                (Event as SchedulableEventNotice).EventTime = en.EventTime;
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
        /// ���������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ������ ���������� ��������
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, 
        /// ����������� ����������</para>
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
                throw new ESimulationException(
                    "������ ��������� ReactivateAfter(p) ����� ���������� ��������");
            }
            ReactivateAfter(p.Event);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� �� ��������� �����
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
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
        /// ���������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ��������� �������
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, 
        /// ����������� ����������</para>
        /// </summary>
        /// <param name="en">������, ����� ������� ������� ������������ �������</param>
        internal void ReactivateBefore(SchedulableEventNotice en)
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
        /// ���������� ������ ����������� �� ��������� �������� 
        /// ��������������� ����� ������� ���������� ��������
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// <para>���� �������-�������� ��������� � ��������� ���������, 
        /// ����������� ����������</para>
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
                throw new ESimulationException(
                    "������ ��������� ReactivateBefore(p) ����� ��������� ���������");
            }
            ReactivateBefore(p.Event);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� 
        /// � ��������� ��������� ������� ������������ �������� ��������.
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
        /// <para>���� ������� �������� ��������, �� ������������������</para>
        /// </summary>
        /// <param name="dt">������������ ����� ��������� ��������</param>
        public void ReactivateDelay(double dt)
        {
            ReactivateAt(SimTime() + dt);
        }

        /// <summary>
        /// ���������� ������ ����������� �� ��������� �������� 
        /// �� ��������� ����� � �����������.
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
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
        /// ���������� ������ ����������� �� ��������� �������� 
        /// � ��������� ��������� ������� ������������ �������� �������� � �����������.
        /// <para>���� ������� ��������� � ��������� ���������, 
        /// ������� ����� ������ �����������</para>
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
            SchedulableEventNotice en = Event;
            (Event as ProcessEventNotice).Proc = null;
            Event = null;
            en.Finish();
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
        /// ����������� ����� ��������� �������������� ��������.
        /// � ������ ������ ������ �� ������.
        /// </summary>
        protected internal virtual void ActionStartingEvent(IAction act)
        {
        }

        /// <summary>
        /// ����������� ����� ��������� �������������� ��������.
        /// � ������ ������ ������ �� ������.
        /// </summary>
        protected internal virtual void ActionFinishedEvent(IAction act)
        {
        }

        /// <summary>
        /// ���������� �������� � ������� � ������� ��� � ����� ��������
        /// <para>���� ������� ����� ������������ ������, ������� 
        /// �� �������� � �������</para>
        /// </summary>
        /// <param name="q">������� ��������</param>
        /// <returns><para>false, ���� ������� �� ��������� � �������.</para> 
        /// <para>true, ���� ������� ������� ��������� 
        /// � ����������� ����� ��������</para></returns>
        public bool Wait(List q)
        {
            if (q.MaxSize == 0 || q.Size < q.MaxSize)
            {
                Insert(q);
                Passivate();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ��������� ������������� ��������
        /// </summary>
        /// <param name="serv">������������� ��������</param>
        /// <param name="dt">����������� ����������������� ��������</param>
        /// <returns>true, ���� �������� ���� ������� ���������. 
        /// false, ���� ��������� �� ������� ��������� � ������� 
        /// �� ������� �� �������������.</returns>
        public bool DoService(Service serv, double dt)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            TimeLeft = dt;
            serv.Agents.ActivateFirst();
            Wait(serv.Queue);
            return true;
        }

        /// <summary>
        /// ��������� ������������� ��������.
        /// ����������������� �������� ������������ ���������, ����������� 
        /// � ������� ��������. ���� ������� �����������, ����������� ����������.
        /// </summary>
        /// <param name="serv">������������� ��������</param>
        /// <returns>true, ���� �������� ���� ������� ���������. 
        /// false, ���� ��������� �� ������� ��������� � ������� 
        /// �� ������� �� �������������.</returns>
        protected bool DoService(Service serv)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            if (serv.Duration == null)
            {
                throw new ESimulationException(
                    "DoService(serv): � ������� �������� �� ����� �������, " + 
                    "����������� ������������ ������������");
            }
            TimeLeft = serv.Duration();
            serv.Agents.ActivateFirst();
            Wait(serv.Queue);
            return true;
        }

        /// <summary>
        /// ����������� ���������� ����� ��� ������� ������� � �������.
        /// � ������ ������ ������ �� ������
        /// </summary>
        /// <param name="node">����, � ������� ��� ������� ������</param>
        protected internal virtual void EnteredEvent(Node node)
        {
        }

        /// <summary>
        /// ����������� ���������� ����� ��� ������� ������� ������� � �������.
        /// � ������ ������ ������ �� ������
        /// </summary>
        /// <param name="node">�������, � ������� ��������������� ������� 
        /// ������� �������</param>
        protected internal virtual void NodeEnterFailedEvent(Node node)
        {
        }

        /// <summary>
        /// ����������� ���������� ����� ��� ������� ���������� �� �������.
        /// � ������ ������ ������ �� ������
        /// </summary>
        /// <param name="node">�������, �� ������� ��� �������� ������</param>
        protected internal virtual void ReleasedEvent(Node node)
        {
        }
    }
}
