using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
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
            Parent = Global.CurrSim;
            // ���� ������������ ������� ���������� (�� ����, ������ ������� - �� ������� ��������),
            //   ������ � ������ ��������� ���������
            if (Parent != null && Parent != this)
            {
                StartRunning();
            }
            OnServiceStartingEvent = ServiceStartingEvent;
            // ����������� � ������� ���
            (this as ISchedulable).Event = null;
            //Global.CurrProc = this;
        }

        /// <summary>
        /// ����������� � �������, ��������� � ���������
        /// </summary>
        EventNotice ISchedulable.Event
        {
            get;
            set;
        }

        /// <summary>
        /// ������ �� ����� ��������� ������� ������ �������������� ��������
        /// </summary>
        public EventProc OnServiceStartingEvent
        {
            get;
            internal set;
        }

        /// <summary>
        /// ������� ��������, � ������ ������� ����������� ������ �������
        /// </summary>
        protected internal IMainSimulation Parent;

        /// <summary>
        /// ������������ ����� ������ ������ ��������
        /// </summary>
        public double StartingTime;

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
                    return ((this as ISchedulable).Event as EventNotice).EventTime;
            }
        }

        /// <summary>
        /// ���������, ��������� �� ������� � ��������� ��� ����������� ���������
        /// </summary>
        public bool Idle
        {
            get
            {
                return (this as ISchedulable).Event == null;
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
            (this as ISchedulable).Event = new ProcessEventNotice(en.EventTime, this);
            (this as ISchedulable).Event.InsertAfter(en);
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
            (this as ISchedulable).Event = new ProcessEventNotice(t, this);
            (this as ISchedulable).Event.Insert(Parent.Calendar);
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
            (this as ISchedulable).Event = new ProcessEventNotice(en.EventTime, this);
            (this as ISchedulable).Event.InsertBefore(en);
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
            (this as ISchedulable).Event = new ProcessEventNotice(t, this);
            (this as ISchedulable).Event.InsertPrior(Parent.Calendar);
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
            EventNotice en = (this as ISchedulable).Event;
            if ((this as ISchedulable).Event != null)
            {
                ((this as ISchedulable).Event as ProcessEventNotice).Proc = null;
                (this as ISchedulable).Event = null;
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
            EventNotice pen = (this as ISchedulable).Event;
            ((this as ISchedulable).Event as ProcessEventNotice).Proc = null;
            (this as ISchedulable).Event = null;
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                ((this as ISchedulable).Event as EventNotice).EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertAfter(en);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertAfter(en);
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                (this as ISchedulable).Event.SetTime(t);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.SetTime(t);
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                (this as ISchedulable).Event.EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertBefore(en);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.EventTime = en.EventTime;
                (this as ISchedulable).Event.InsertBefore(en);
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
            if ((this as ISchedulable).Event.IsFirst)
            {
                (this as ISchedulable).Event.SetTimePrior(t);
                RunNextProc();
            }
            else
            {
                (this as ISchedulable).Event.SetTimePrior(t);
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
            EventNotice en = (this as ISchedulable).Event;
            ((this as ISchedulable).Event as ProcessEventNotice).Proc = null;
            (this as ISchedulable).Event = null;
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
        /// ����������� ����� ��������� �������������� ��������.
        /// � ������ ������ ������ �� ������.
        /// </summary>
        public virtual void ServiceStartingEvent()
        {
        }

        /// <summary>
        /// ���������� �������� � ������� � ������� ��� � ����� ��������
        /// <para>���� ������� ����� ������������ ������, ������� 
        /// �� �������� � �������</para>
        /// </summary>
        /// <param name="q">������� ��������</param>
        /// <returns><para>false, ���� ������� �� ��������� � �������.</para> 
        /// <para>true, ���� ������� ������� ��������� � ����������� ����� ��������</para></returns>
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
        /// <returns>true, ���� ��������� ��� ������� ��������� � ������� 
        /// �������� ��������. false, ���� ��������� �� ������� ��������� � ������� 
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
        /// ��������� ������������� ��������
        /// </summary>
        /// <param name="serv">������������� ��������</param>
        /// <param name="dt">����������� ����������������� ��������</param>
        /// <param name="start">�����, ����������� � ������ ������ ������������</param>
        /// <returns>true, ���� ��������� ��� ������� ��������� � ������� 
        /// �������� ��������. false, ���� ��������� �� ������� ��������� � ������� 
        /// �� ������� �� �������������.</returns>
        public bool DoService(Service serv, double dt, EventProc start)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            TimeLeft = dt;
            OnServiceStartingEvent = start;
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
        /// <returns>true, ���� ��������� ��� ������� ��������� � ������� 
        /// �������� ��������. false, ���� ��������� �� ������� ��������� � ������� 
        /// �� ������� �� �������������.</returns>
        public bool DoService(Service serv)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            if (serv.Duration == null)
            {
                throw new ESimulationException(
                    "DoService(): � ������� �������� �� ����� �������, ����������� ������������ ������������");
            }
            TimeLeft = serv.Duration();
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
        /// <param name="start">�����, ����������� � ������ ������ ������������</param>
        /// <returns>true, ���� ��������� ��� ������� ��������� � ������� 
        /// �������� ��������. false, ���� ��������� �� ������� ��������� � ������� 
        /// �� ������� �� �������������.</returns>
        public bool DoService(Service serv, EventProc start)
        {
            if (serv.Queue.MaxSize > 0 && serv.Queue.Size >= serv.Queue.MaxSize)
            {
                return false;
            }
            if (serv.Duration == null)
            {
                throw new ESimulationException(
                    "DoService(): � ������� �������� �� ����� �������, ����������� ������������ ������������");
            }
            TimeLeft = serv.Duration();
            OnServiceStartingEvent = start;
            serv.Agents.ActivateFirst();
            Wait(serv.Queue);
            return true;
        }
    }
}
