using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Simulation
{
    /// <summary>
    /// ����� <c>Coroutine</c> - ������� ����� ��� ���������� ����������
    /// </summary>
    public class Coroutine : Link
    {
        /// <summary>
        /// �����������. 
        /// <para>������� ��� ����������� �������������� � ������������ ���������� ���������� ��������� ���������</para>
        /// </summary>
        public Coroutine()
        {
            Owner = Global.CurrProc;
            MyThread = new Thread(Execute);
            MySem = new Semaphore(0, 1);
            MyThread.Start();
            if (Owner == null)
                Global.GlSem.WaitOne();
            else
                Owner.MySem.WaitOne();
        }

        private Thread MyThread;

        /// <summary>
        /// ����, ����������� �� ����������� ��������� �����������. 
        /// �������� ��� ������ ����������� �������� <c>Terminated</c>.
        /// </summary>
        protected bool TerminatedState;

        /// <summary>
        /// �������, ����������� ������� �����������
        /// </summary>
        internal Semaphore MySem;

        /// <summary>
        /// �����������-�������� ������� ��� null, ���� ���������� �������� ������� �����
        /// </summary>
        internal Coroutine Owner;

        /// <summary>
        /// <para>�����, �������������� ���������� ������ �����������. ���������� ���������� ������ Run() � �������� �����������.</para>
        /// <para>������� �� ������ ����������������.</para>
        /// </summary>
        protected virtual void Execute()
        {
            Global.CurrProc = this;
            Detach();
            Run();
            TerminatedState = true;
            while (true)
                Detach();
        }

        /// <summary>
        /// �������� �������� ������ �����������. 
        /// <para>������ ���������������� � ����������� �������.
        /// � ������ ������ ������ �� ������.</para>
        /// </summary>
        protected virtual void Run()
        {
        }

        /// <summary>
        /// ������������ � ������ �����������
        /// </summary>
        public void SwitchTo()
        {
            Global.SwitchTo(this);
        }

        /// <summary>
        /// ������������ � �������� �����������
        /// </summary>
        /// <param name="cor">������������ �����������</param>
        protected void SwitchTo(Coroutine cor)
        {
            Global.SwitchTo(cor);
        }

        /// <summary>
        /// ������������ � �����������-���������
        /// </summary>
        protected void Detach()
        {
            Global.Detach();
        }

        /// <summary>
        /// ���������� ������ �����������. 
        /// <para>��� �����������, ��������� � ���������, 
        /// ����������� ������ ����������� � ������� ����� ������.
        /// � ��������� ������ ���������� ������ ��� ����������� ���������
        /// ����� ����������� �����������.</para>
        /// </summary>
        public override void Finish()
        {
            MyThread.Abort();
            base.Finish();
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
    }
}
