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

    /// <summary>
    /// ����� <c>TimeBetStatistics</c> �������� �������� ���������� �� ���������� ������� ����� ���������
    /// </summary>
    public class TimeBetStatistics : Statistics
    {
        /// <summary>
        /// ����������� �� ���������.
        /// </summary>
        TimeBetStatistics()
        {
            Count = -1;
            LastTime = -1;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="AHeader">��������� ��� ������ �� �����</param>
        TimeBetStatistics(string AHeader)
            : base(AHeader)
        {
            Count = -1;
            LastTime = -1;
        }

        private double LastTime;

        /// <summary>
        /// ��������� ������� ������ ������������� �������, ��������������� �������� ��������
        /// </summary>
        public void AddData()
        {
            AddData(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// ��������� ����� �������� ��� �������� ����� ��������� ��������� � ��������� ����������� ���������
        /// </summary>
        /// <param name="newTime"></param>
        public override void AddData(double newTime)
        {
            if (Count < 0)
            {
                LastTime = newTime;
                Count++;
            }
            else
            {
                double dt = newTime - LastTime;
                base.AddData(dt);
                LastTime = newTime;
            }
        }

        /// <summary>
        /// ���������� ���������� � �������� ���������
        /// </summary>
        public override void ClearStat()
        {
            base.ClearStat();
            Count = -1;
            LastTime = -1;
        }
    }

    /// <summary>
    /// ����� <c>IntervalStatistics</c> �������� ������������ ���������� �� ���������, ��������� ���������� �� �������
    /// </summary>
    public class IntervalStatistics
    {
        /// <summary>
        /// �����������. ������� ������ ������������ ���������� � ������� ������ ������������� �������.
        /// </summary>
        /// <param name="initX">��������� �������� ����������� ��������</param>
        public IntervalStatistics(double initX)
        {
            LastX = initX;
            LastTime = Global.SimTime();
        }

        /// <summary>
        /// �����������. ������� ������ ������������ ���������� � �������� ������ ������������� �������.
        /// </summary>
        /// <param name="initX">��������� �������� ����������� ��������</param>
        /// <param name="initTime">������ �������, ����� ��������� ������</param>
        public IntervalStatistics(double initX, double initTime)
        {
            LastX = initX;
            LastTime = initTime;
        }

        /// <summary>
        /// ��������� ��� ������ ���������� �� �����
        /// </summary>
        public string Header;

        /// <summary>
        /// ������ ���������� ��������� ��������
        /// </summary>
        private double LastTime;

        /// <summary>
        /// �������� �������� � ������� ���������� ���������
        /// </summary>
        internal double LastX;

        /// <summary>
        /// �������� ����������� �������� �� �������
        /// </summary>
        private double SumX;

        /// <summary>
        /// �������� �������� ����������� �������� �� �������
        /// </summary>
        private double SumX_2;

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
        /// ����� ����� ���������� �� ������ ����������
        /// </summary>
        public double TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// ���������� ������ �������� ����������� �������� � ���������� � ������� ������ ������������� �������.
        /// ���������� ����������� ���������� �� ���������� ������� � ���������� ��������� �� ��������.
        /// ��������� �������� ������������ ��� ����� � �������.
        /// </summary>
        /// <param name="newX">����� �������� ����������� ��������</param>
        public void AddData(double newX)
        {
            AddData(newX, Global.SimTime());
        }

        /// <summary>
        /// ���������� ������ �������� ����������� �������� � ���������� � ��������� ������� �������, ����� ��� ����������.
        /// ���������� ����������� ���������� �� ���������� ������� � ���������� ��������� �� ��������.
        /// ��������� �������� ������������ ��� ����� � �������.
        /// </summary>
        /// <param name="newX">����� �������� ����������� ��������</param>
        /// <param name="newTime">������������ ����� � ������ ���������</param>
        public void AddData(double newX, double newTime)
        {
            // ���������� ������� � ������� ���������� ����������
            double dt = newTime - LastTime;
            if (dt < 0)
                // ��������� ��������
                //   *** (����� ����, ������������ ����������?)
                dt = 0;
            // ���� � ������� ���������� ��������� ������ ��������� �����
            if (dt > 0)
            {
                // ���� ��� ������ ��������� ��������, ���������������� ����������
                if (TotalTime == 0)
                    Min = Max = LastX;
                // ����� - ��������������� ������� ��������
                else if (LastX < Min)
                    Min = LastX;
                else if (LastX > Max)
                    Max = LastX;
                // ������ ���������� �������� �� ������ LastX � ������� ������� dt
                SumX += dt * LastX;
                SumX_2 += dt * LastX * LastX;
                TotalTime += dt;
            }
            // ��������� ����� ���������
            LastTime = newTime;
            LastX = newX;
        }

        /// <summary>
        /// ������� ����������, ���������� � ������ ����� ������ � ������� ������ ������������� �������
        /// </summary>
        public void ClearStat()
        {
            ClearStat(Global.SimTime());
        }

        /// <summary>
        /// ������� ����������, ���������� � ������ ����� ������ � �������� ������ ������������� �������
        /// </summary>
        /// <param name="newTime"></param>
        public void ClearStat(double newTime)
        {
            SumX = SumX_2 = Min = Max = TotalTime = 0;
            LastTime = newTime;
            // �������� LastX �� ����������
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
            if (TotalTime == 0)
                return 0;
            else
                return SumX_2 / TotalTime - Mean() * Mean();
        }

        /// <summary>
        /// ���������� ������� �������������� �� ����������� ������
        /// </summary>
        /// <returns>������� ��������������</returns>
        public double Mean()
        {
            if (TotalTime == 0)
                return 0;
            else
                return SumX / TotalTime;
        }

        /// <summary>
        /// ��������� ���������� � �������� ������������� �������.
        /// ����������� �������� �������, ��������� � ������� ���������� ��������� ��� ���������.
        /// </summary>
        public void StopStat()
        {
            StopStat(Global.SimTime());
        }

        /// <summary>
        /// ��������� ���������� � ��������� ������������� �������.
        /// ����������� �������� �������, ��������� � ������� ���������� ��������� ��� ���������.
        /// </summary>
        /// <param name="newTime">������������ ����� ������� ��������� ����������</param>
        public void StopStat(double newTime)
        {
            // ����� ������, �������� �� ����������
            AddData(LastX, newTime);
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
            Result.AppendFormat("����� {0,6:0.000} ������ �������, ������� �������� = {1,6:0.000}", TotalTime, LastX);
            return Result.ToString();
        }
    }

    /// <summary>
    /// ����� ActionStatistics �������� ���������� �� ���������
    /// </summary>
    public class ActionStatistics
    {
        /// <summary>
        /// �����������. ������� ������ ���������� �������� � ������� ������ ������������� �������.
        /// </summary>
        public ActionStatistics()
        {
            Running = 0;
            LastTime = Global.SimTime();
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������� � ������� ������ ������������� �������.
        /// </summary>
        /// <param name="initX">��������� �������� ����������� ��������</param>
        public ActionStatistics(int initX)
        {
            Running = initX;
            LastTime = Global.SimTime();
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������� � �������� ������ ������������� �������.
        /// </summary>
        /// <param name="initX">��������� �������� ����������� ��������</param>
        /// <param name="initTime">������ �������, ����� ��������� ������</param>
        public ActionStatistics(int initX, double initTime)
        {
            Running = initX;
            LastTime = initTime;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������� � �������� ������ ������������� �������.
        /// </summary>
        /// <param name="initX">��������� �������� ����������� ��������</param>
        /// <param name="initTime">������ �������, ����� ��������� ������</param>
        /// <param name="aHeader">��������� ��� ������ ����������</param>
        public ActionStatistics(int initX, double initTime, string aHeader)
        {
            Running = initX;
            LastTime = initTime;
            Header = aHeader;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������� � ������� ������ ������������� �������.
        /// </summary>
        /// <param name="initX">��������� �������� ����������� ��������</param>
        /// <param name="aHeader">��������� ��� ������ ����������</param>
        public ActionStatistics(int initX, string aHeader)
        {
            Running = initX;
            LastTime = Global.SimTime();
            Header = aHeader;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������� � ������� ������ ������������� �������.
        /// </summary>
        /// <param name="aHeader">��������� ��� ������ ����������</param>
        public ActionStatistics(string aHeader)
        {
            Running = 0;
            LastTime = Global.SimTime();
            Header = aHeader;
        }

        /// <summary>
        /// ��������� ��� ������ ���������� �� �����
        /// </summary>
        public string Header;

        /// <summary>
        /// ������ ���������� ��������� ��������
        /// </summary>
        private double LastTime;

        /// <summary>
        /// �������� ����������� �������� �� �������
        /// </summary>
        private double SumX;

        /// <summary>
        /// �������� �������� ����������� �������� �� �������
        /// </summary>
        private double SumX_2;

        /// <summary>
        /// ���������� ����������� ��������
        /// </summary>
        public int Finished
        {
            get;
            private set;
        }

        /// <summary>
        /// ������������ �������� ����� �����������
        /// </summary>
        public int Max
        {
            get;
            private set;
        }

        /// <summary>
        /// ���������� ����������� ��������
        /// </summary>
        public int Running 
        {
            get;
            private set;
        }

        /// <summary>
        /// ����� ����� ���������� �� ������ ����������
        /// </summary>
        public double TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// ������� ����������, ���������� � ������ ����� ������ � ������� ������ ������������� �������
        /// </summary>
        public void ClearStat()
        {
            ClearStat(Global.SimTime());
        }

        /// <summary>
        /// ������� ����������, ���������� � ������ ����� ������ � �������� ������ ������������� �������
        /// </summary>
        /// <param name="newTime"></param>
        public void ClearStat(double newTime)
        {
            SumX = SumX_2 = TotalTime = Finished = Max = 0;
            LastTime = newTime;
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
            if (TotalTime == 0)
                return 0;
            else
                return SumX_2 / TotalTime - Mean() * Mean();
        }

        /// <summary>
        /// �������� ��������� �������� � ������� ������ �������
        /// </summary>
        public void Finish()
        {
            Finish(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ��������� �������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ��������� ��������</param>
        public void Finish(double newTime)
        {
            if (newTime > LastTime)
            {
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
            Running--;
            Finished++;
        }

        /// <summary>
        /// ���������� ������� �������������� �� ����������� ������
        /// </summary>
        /// <returns>������� ��������������</returns>
        public double Mean()
        {
            if (TotalTime == 0)
                return 0;
            else
                return SumX / TotalTime;
        }

        /// <summary>
        /// �������� ������������ �������� � ������� ������ �������
        /// </summary>
        public void Preempt()
        {
            Preempt(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ������������ �������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ������������ ��������</param>
        public void Preempt(double newTime)
        {
            if (newTime > LastTime)
            {
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
            Running--;
        }

        /// <summary>
        /// �������� ������������� �������� � ������� ������ �������
        /// </summary>
        public void Resume()
        {
            Resume(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ������������� �������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ������������� ��������</param>
        public void Resume(double newTime)
        {
            Start(newTime);
        }

        /// <summary>
        /// �������� ������ �������� � ������� ������ �������
        /// </summary>
        public void Start()
        {
            Start(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ������ �������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ������ ��������</param>
        public void Start(double newTime)
        {
            if (newTime > LastTime)
            {
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
            Running++;
        }

        /// <summary>
        /// ��������� ���������� � �������� ������������� �������.
        /// ����������� �������� �������, ��������� � ������� ���������� ��������� ��� ���������.
        /// </summary>
        public void StopStat()
        {
            StopStat(Global.SimTime());
        }

        /// <summary>
        /// ��������� ���������� � ��������� ������������� �������.
        /// ����������� �������� �������, ��������� � ������� ���������� ��������� ��� ���������.
        /// </summary>
        /// <param name="newTime">������������ ����� ������� ��������� ����������</param>
        public void StopStat(double newTime)
        {
            if (newTime > LastTime)
            {
                // ����� ������, �������� �� ����������
                double dt = newTime - LastTime;
                LastTime = newTime;
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                if (Running > Max)
                {
                    Max = Running;
                }
            }
        }

        /// <summary>
        /// ����������� ���������� ���������� � ����� ��� ����������� �� ������
        /// </summary>
        /// <returns>��������������� ����������</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("������� = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("�������� = {0,2}\n", Max);
            Result.AppendFormat("������ ����������� {0,2} ��������, ��������� {1,2}", Running, Finished);
            return Result.ToString();
        }
    }

    /// <summary>
    /// ����� ServiceStatistics �������� ���������� �� ������������� ���������
    /// </summary>
    public class ServiceStatistics
    {
        /// <summary>
        /// �����������. ������� ������ ���������� �������������� �������� ��� ������ ���������� � ������� ������ ������������� �������.
        /// </summary>
        public ServiceStatistics()
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = 1;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������������� �������� � ������� ������ ������������� �������.
        /// </summary>
        /// <param name="initDevices">��������� ���������� ������������� ���������</param>
        public ServiceStatistics(int initDevices)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = initDevices;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������������� �������� � �������� ������ ������������� �������.
        /// </summary>
        /// <param name="initDevices">��������� ���������� ������������� ���������</param>
        /// <param name="initUtil">��������� ���������� ������� ���������</param>
        /// <param name="initTime">������ �������, ����� ��������� ������</param>
        public ServiceStatistics(int initDevices, int initUtil, double initTime)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = initTime;
            MinBusy = Devices = initDevices;
            Running = initUtil;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������������� �������� � �������� ������ ������������� �������.
        /// </summary>
        /// <param name="initDevices">��������� ���������� ������������� ���������</param>
        /// <param name="initUtil">��������� ���������� ������� ���������</param>
        /// <param name="initTime">������ �������, ����� ��������� ������</param>
        /// <param name="aHeader">��������� ��� ����������� ���������� �� ������</param>
        public ServiceStatistics(int initDevices, int initUtil, double initTime, string aHeader)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = initTime;
            MinBusy = Devices = initDevices;
            Running = initUtil;
            Header = aHeader;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������������� �������� � ������� ������ ������������� �������.
        /// </summary>
        /// <param name="initDevices">��������� ���������� ������������� ���������</param>
        /// <param name="aHeader">��������� ��� ����������� ���������� �� ������</param>
        public ServiceStatistics(int initDevices, string aHeader)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = initDevices;
            Header = aHeader;
        }

        /// <summary>
        /// �����������. ������� ������ ���������� �������������� �������� ��� ������ ���������� � ������� ������ ������������� �������.
        /// </summary>
        /// <param name="aHeader">��������� ��� ����������� ���������� �� ������</param>
        public ServiceStatistics(string aHeader)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Running = MaxBusy = Finished = 0;
            LastTime = LastBusyStart = LastIdleStart = Global.SimTime();
            MinBusy = Devices = 1;
            Header = aHeader;
        }

        /// <summary>
        /// ��������� ��� ������ ���������� �� �����
        /// </summary>
        public string Header;

        /// <summary>
        /// ������� ���������� ��������������� ���������
        /// </summary>
        private int LastBlockage;

        /// <summary>
        /// ��������� ����� ���������� � ��������������� ���������
        /// </summary>
        private double LastBlockTime;

        /// <summary>
        /// ������ ������ �������� ���������
        /// </summary>
        private double LastBusyStart;

        /// <summary>
        /// ��������� ����� ���������� � ������� ���������
        /// </summary>
        private double LastBusyTime;

        /// <summary>
        /// ������ ������ ���������� ���������
        /// </summary>
        private double LastIdleStart;

        /// <summary>
        /// ��������� ����� ���������� � ��������� ���������
        /// </summary>
        private double LastIdleTime;

        /// <summary>
        /// ������ ���������� ��������� ��������
        /// </summary>
        private double LastTime;

        /// <summary>
        /// ��������� ����� ���������� � ��������������� ���������
        /// </summary>
        private double SumBlockage;

        /// <summary>
        /// �������� ����������� �������� �� �������
        /// </summary>
        private double SumX;

        /// <summary>
        /// �������� �������� ����������� �������� �� �������
        /// </summary>
        private double SumX_2;

        /// <summary>
        /// ���������� ��������� ���������
        /// </summary>
        public int Devices
        {
            get;
            private set;
        }

        /// <summary>
        /// ���������� ����������� ��������
        /// </summary>
        public int Finished
        {
            get;
            private set;
        }

        /// <summary>
        /// ������������ ���������� ������������ ���������
        /// </summary>
        public int MaxBusy
        {
            get;
            private set;
        }

        /// <summary>
        /// ������������ ����� ����������� ���������
        /// </summary>
        public double MaxBusyTime
        {
            get;
            private set;
        }

        /// <summary>
        /// ������������ ����� ���������� ���������
        /// </summary>
        public double MaxIdleTime
        {
            get;
            private set;
        }

        /// <summary>
        /// ����������� ���������� ������� ���������
        /// </summary>
        public int MinBusy
        {
            get;
            private set;
        }

        /// <summary>
        /// ���������� ������� ���������
        /// </summary>
        public int Running
        {
            get;
            private set;
        }

        /// <summary>
        /// ����� ����� ���������� �� ������ ����������
        /// </summary>
        public double TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// ������� ����������, ���������� � ������ ����� ������ � ������� ������ ������������� �������
        /// </summary>
        public void ClearStat()
        {
            ClearStat(Global.SimTime());
        }

        /// <summary>
        /// ������� ����������, ���������� � ������ ����� ������ � �������� ������ ������������� �������
        /// </summary>
        /// <param name="newTime"></param>
        public void ClearStat(double newTime)
        {
            SumX = SumX_2 = SumBlockage = TotalTime = MaxBusyTime = MaxIdleTime = LastBlockTime = LastBusyTime = LastIdleTime = 0;
            Finished = 0;
            MaxBusy = MinBusy = Running;
            LastTime = LastBusyStart = LastIdleStart = newTime;
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
            if (TotalTime == 0)
                return 0;
            else
                return SumX_2 / TotalTime - Mean() * Mean();
        }

        /// <summary>
        /// �������� ��������� �������� � ������� ������ �������
        /// </summary>
        public void Finish()
        {
            Finish(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ��������� �������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ��������� ��������</param>
        public void Finish(double newTime)
        {
            double dt = newTime - LastTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                LastTime = newTime;
                if (Running > MaxBusy)
                {
                    MaxBusy = Running;
                }
                else if (Running < MinBusy)
                {
                    MinBusy = Running;
                }
            }
            Running--;
            Finished++;
            if (Running == 0)
            {
                dt = newTime - LastBusyStart;
                if (LastIdleTime == 0)
                {
                    LastBusyTime += dt;
                }
                else
                {
                    LastBusyTime = dt;
                }
                if (LastBusyTime > MaxBusyTime)
                {
                    MaxBusyTime = LastBusyTime;
                }
                LastIdleStart = newTime;
            }
        }

        /// <summary>
        /// �������� ��������� ���������� � ������� ������ �������
        /// </summary>
        public void FinishBlock()
        {
            FinishBlock(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ��������� ��������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ��������� ����������</param>
        public void FinishBlock(double newTime)
        {
            double dt = newTime - LastBlockTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumBlockage += dt * LastBlockage;
                LastBlockTime = newTime;
            }
            LastBlockage--;
        }

        /// <summary>
        /// ���������� ������� �������������� ���������� ������������ ���������
        /// </summary>
        /// <returns>������� ��������������</returns>
        public double Mean()
        {
            if (TotalTime == 0)
                return 0;
            else
                return SumX / TotalTime;
        }

        /// <summary>
        /// ���������� ������� �������������� ���������� ��������������� ���������
        /// </summary>
        /// <returns>������� ��������������</returns>
        public double MeanBlockage()
        {
            if (TotalTime == 0)
                return 0;
            else
                return SumBlockage / TotalTime;
        }

        /// <summary>
        /// �������� ������ �������� � ������� ������ �������
        /// </summary>
        public void Start()
        {
            Start(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ������ �������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ������ ��������</param>
        public void Start(double newTime)
        {
            double dt = newTime - LastTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                LastTime = newTime;
                if (Running > MaxBusy)
                {
                    MaxBusy = Running;
                }
                else if (Running < MinBusy)
                {
                    MinBusy = Running;
                }
            }
            Running++;
            if (Running == 1)
            {
                dt = newTime - LastIdleStart;
                if (LastBusyTime == 0)
                {
                    LastIdleTime += dt;
                }
                else
                {
                    LastIdleTime = dt;
                }
                if (LastIdleTime > MaxIdleTime)
                {
                    MaxIdleTime = LastIdleTime;
                }
                LastBusyStart = newTime;
            }
        }

        /// <summary>
        /// �������� ������ ���������� � ������� ������ �������
        /// </summary>
        public void StartBlock()
        {
            StartBlock(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// �������� ������ ���������� � �������� ������ �������
        /// </summary>
        /// <param name="newTime">����� ������ ��������</param>
        public void StartBlock(double newTime)
        {
            double dt = newTime - LastBlockTime;
            if (dt < 0)
            {
                dt = 0;
            }
            if (dt > 0)
            {
                SumBlockage += dt * LastBlockage;
                LastBlockTime = newTime;
            }
            LastBlockage++;
        }

        /// <summary>
        /// ��������� ���������� � �������� ������������� �������.
        /// ����������� �������� �������, ��������� � ������� ���������� ��������� ��� ���������.
        /// </summary>
        public void StopStat()
        {
            StopStat(Global.SimTime());
        }

        /// <summary>
        /// ��������� ���������� � ��������� ������������� �������.
        /// ����������� �������� �������, ��������� � ������� ���������� ��������� ��� ���������.
        /// </summary>
        /// <param name="newTime">������������ ����� ������� ��������� ����������</param>
        public void StopStat(double newTime)
        {
            double dt = newTime - LastTime;
            if (dt < 0)
            {
                return;
            }
            if (dt > 0)
            {
                SumX += Running * dt;
                SumX_2 += Running * Running * dt;
                TotalTime += dt;
                LastTime = newTime;
                if (Running > MaxBusy)
                {
                    MaxBusy = Running;
                }
                else if (Running < MinBusy)
                {
                    MinBusy = Running;
                }
                dt = newTime - LastBlockTime;
                SumBlockage += dt * LastBlockage;
                LastBlockTime = newTime;
            }
            if (Running == 0)
            {
                dt = newTime - LastIdleStart;
                if (LastBusyTime == 0)
                {
                    LastIdleTime += dt;
                }
                else
                {
                    LastIdleTime = dt;
                }
                if (LastIdleTime > MaxIdleTime)
                {
                    MaxIdleTime = LastIdleTime;
                }
                LastBusyTime = 0;
                LastIdleStart = newTime;
            }
            else
            {
                dt = newTime - LastBusyStart;
                if (LastIdleTime == 0)
                {
                    LastBusyTime += dt;
                }
                else
                {
                    LastBusyTime = dt;
                }
                if (LastBusyTime > MaxBusyTime)
                {
                    MaxBusyTime = LastBusyTime;
                }
                LastIdleTime = 0;
                LastBusyStart = newTime;
            }
        }

        /// <summary>
        /// ����������� ���������� ���������� � ����� ��� ����������� �� ������
        /// </summary>
        /// <returns>��������������� ����������</returns>
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder(Header + "\n");
            Result.AppendFormat("���������� ��������� = {0,2}\n", Devices);
            Result.AppendFormat("��������� = {0,6:0.000} +/- {1,6:0.000}\n", Mean(), Deviation());
            Result.AppendFormat("������ ����������� {0,2} ��������, ��������� {1,2}\n", Running, Finished);
            Result.AppendFormat("������� ���������� = {0,6:0.000}\n", MeanBlockage());
            if (Devices == 1)
            {
                Result.AppendFormat("������������ ����� ������� = {0,6:0.000}, ������������ ����� ������ = {1,6:0.000}", 
                    MaxIdleTime, MaxBusyTime);
            }
            else
            {
                Result.AppendFormat("������������ ����� ��������� ��������� = {0,6:0.000}, ������������ ����� ������� ��������� = {1,6:0.000}", 
                    Devices - MinBusy, MaxBusy);
            }
            return Result.ToString();
        }
    }

    /// <summary>
    /// ����� <c>HistogramBase</c> �������� ����� �������� ��� �������� � ������������ ����������
    /// </summary>
    public abstract class HistogramBase
    {
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ALow">������ ������� ������� ��������� ���������</param>
        /// <param name="AStep">��� ������� ��������� ���������</param>
        /// <param name="AIntervalCount">���������� �������� ����������</param>
        public HistogramBase(double ALow, double AStep, int AIntervalCount)
        {
            Low = ALow;
            Step = AStep;
            IntervalCount = AIntervalCount;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ALow">������ ������� ������� ��������� ���������</param>
        /// <param name="AStep">��� ������� ��������� ���������</param>
        /// <param name="AIntervalCount">���������� �������� ����������</param>
        /// <param name="AHeader">��������� ��� ������ �� �������</param>
        public HistogramBase(double ALow, double AStep, int AIntervalCount, string AHeader)
        {
            Low = ALow;
            Step = AStep;
            IntervalCount = AIntervalCount;
            Header = AHeader;
        }

        /// <summary>
        /// ��������� ����������� ��� ������
        /// </summary>
        public string Header;

        /// <summary>
        /// ���������� �������� ���������.
        /// <para>����� ���������� ���������� ����������� �� 2 ������, ��� ��� ��� �������� ��� ���
        /// ��������������� ��������� � ������ �������</para>
        /// </summary>
        public int IntervalCount
        {
            get;
            protected set;
        }

        /// <summary>
        /// ������ ������� ������� ��������� ���������
        /// </summary>
        public double Low
        {
            get;
            protected set;
        }

        /// <summary>
        /// ��� ������� ��������� ���������
        /// </summary>
        public double Step
        {
            get;
            protected set;
        }

        /// <summary>
        /// ������� �����������
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// ���������� ���� �� ������ ���������� ��������, �������� � ��������� �� ������ �� ����������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>���� ���������� ��������</returns>
        public abstract double CumulativePercent(int index);

        /// <summary>
        /// ���������� ���� �� ������ ���������� ��������, �������� � ��������� ��������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>���� ���������� ��������</returns>
        public abstract double Percent(int index);

        /// <summary>
        /// ���������� ������ ���������, � ������� �������� ��������� ��������.
        /// <para>��������� ����� 0, ���� �������� ������ ������ ������� ������ ��������� ��������� (<c>Low</c>).</para>
        /// <para>��������� ����� <c>IntervalCount + 1</c>, ���� �������� ������ ��� ����� ������� ������� ���������� ��������� ���������</para>
        /// </summary>
        /// <param name="val">��������, ������ ��� �������� ��������� ����������</param>
        /// <returns>������ ���������</returns>
        public int IntervalIndex(double val)
        {
            if (val >= Low)
            {
                int iStep = (int)((val - Low) / Step) + 1;
                if (iStep > IntervalCount + 1)
                    return IntervalCount + 1;
                else
                    return iStep;
            }
            else
                return 0;
        }

        /// <summary>
        /// ���������� ����� (������) ������� ���������� ���������.
        /// <para>���� ������ ��������� �����������, ���������� �������� "����� �������������".</para>
        /// <para>���� ������ ��������� ��������� ����������� ���������, ���������� ����� ������� ������� ���������������� ���������</para>
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>����� ������� ���������</returns>
        public double LowerBound(int index)
        {
            if (index <= 0)
                return double.NegativeInfinity;
            else if (index >= IntervalCount + 1)
                return Low + Step * IntervalCount;
            else
                return Low + Step * (index - 1);
        }

        /// <summary>
        /// ���������� ������ (�������) ������� ���������� ���������.
        /// <para>���� ������ ��������� �����������, ���������� ������ ������� ������ ���������������� ���������</para>
        /// <para>���� ������ ��������� ��������� ����������� ���������, ���������� �������� "���� �������������".</para>
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>������ ������� ���������</returns>
        public double UpperBound(int index)
        {
            if (index <= 0)
                return Low;
            else if (index >= IntervalCount + 1)
                return double.PositiveInfinity;
            else
                return Low + Step * index;
        }
    }

    /// <summary>
    /// ����� <c>Histogram</c> �������� ������ �� ���������� ��������, ���������� � �������� ���������
    /// </summary>
    public class Histogram : HistogramBase
    {
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ALow">������ ������� ������� ��������� ���������</param>
        /// <param name="AStep">��� ������� ��������� ���������</param>
        /// <param name="AIntervalCount">���������� �������� ����������</param>
        public Histogram(double ALow, double AStep, int AIntervalCount)
            : base(ALow, AStep, AIntervalCount)
        {
            Data = new int[IntervalCount + 2];
            TotalCount = 0;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ALow">������ ������� ������� ��������� ���������</param>
        /// <param name="AStep">��� ������� ��������� ���������</param>
        /// <param name="AIntervalCount">���������� �������� ����������</param>
        /// <param name="AHeader">��������� ��� ������ �� �����</param>
        public Histogram(double ALow, double AStep, int AIntervalCount, string AHeader)
            : base(ALow, AStep, AIntervalCount, AHeader)
        {
            Data = new int[IntervalCount + 2];
            TotalCount = 0;
        }

        /// <summary>
        /// ������, � ������� ���������� ���������� ��������, ���������� � ������ ��������
        /// </summary>
        private int[] Data;

        /// <summary>
        /// ����� ���������� ���������� ��������
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// ��������� � ����������� ����� ��������. ����������� �� 1 ������� �������, ��������������� ����������� ��������, 
        /// � ����� ���������� ��������
        /// </summary>
        /// <param name="newData">����������� ��������</param>
        public void AddData(double newData)
        {
            Data[IntervalIndex(newData)]++;
            TotalCount++;
        }

        /// <summary>
        /// ������� �����������, ��������� ������� �������� �� ��� �������� ������� � ������� ������� ��������
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i <= IntervalCount + 1; i++)
                Data[i] = 0;
            TotalCount = 0;
        }

        /// <summary>
        /// ���������� ���������� ��������, �������� � ��������� ��������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>���������� ��������</returns>
        public int Count(int index)
        {
            if (index <= 0)
                return Data[0];
            else if (index >= IntervalCount + 1)
                return Data[IntervalCount + 1];
            else
                return Data[index];
        }

        /// <summary>
        /// ����� ���������� ��������, �������� � ��������� �� �������� ������ �� ����������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>����� ���������� ��������</returns>
        public int CumulativeCount(int index)
        {
            int Sum = 0;
            for (int i = 0; i <= index && i <= IntervalCount + 1; i++)
                Sum += Data[i];
            return Sum;
        }

        /// <summary>
        /// ���������� ���� �� ������ ���������� ��������, �������� � ��������� �� ������ �� ����������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>���� ���������� ��������</returns>
        public override double CumulativePercent(int index)
        {
            if (TotalCount == 0)
                return 0;
            else
                return (double)CumulativeCount(index) / TotalCount;
        }

        /// <summary>
        /// ���������� ���� �� ������ ���������� ��������, �������� � ��������� ��������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>���� ���������� ��������</returns>
        public override double Percent(int index)
        {
            if (TotalCount == 0)
                return 0;
            else
                return (double)Count(index) / TotalCount;
        }

        /// <summary>
        /// ���������� ���������� ����������� � ��������� ���� ��� ����������� �� ������
        /// </summary>
        /// <returns>��������� ��� �����������</returns>
        public override string ToString()
        {
            string Output = Header + "\n";

            // ������� ��������������� ��������
            int CumCount = (int)Math.Round(CumulativePercent(0) * 40) - 1;
            int PercentCount = (int)Math.Round(Percent(0) * 40);
            StringBuilder Graph = new StringBuilder();
            if (CumCount > 0)
                Graph.Append(' ', CumCount);
            if (CumCount >= 0)
                Graph.Append('o');
            for (int j = 0; j < PercentCount; j++)
                Graph[j] = '*';
            Output += string.Format(" -INF - {0,5:0.00} : {1,4} ({2,5:0.00}%), {3,6:0.00}% {4}\n", 
                UpperBound(0), Count(0), Percent(0) * 100, CumulativePercent(0) * 100, Graph);

            // �������� ���������
            for (int i = 1; i <= IntervalCount; i++)
            {
                CumCount = (int)Math.Round(CumulativePercent(i) * 40) - 1;
                PercentCount = (int)Math.Round(Percent(i) * 40);
                Graph = new StringBuilder();
                if (CumCount > 0)
                    Graph.Append(' ', CumCount);
                if (CumCount >= 0)
                    Graph.Append('o');
                for (int j = 0; j < PercentCount; j++)
                    Graph[j] = '*';
                Output += string.Format("{0,5:0.00} - {1,5:0.00} : {2,4} ({3,5:0.00}%), {4,6:0.00}% {5}\n", 
                    LowerBound(i), UpperBound(i), Count(i), Percent(i) * 100, CumulativePercent(i) * 100, Graph);
            }

            // ��������� ��������������� ��������
            CumCount = (int)Math.Round(CumulativePercent(IntervalCount + 1) * 40) - 1;
            PercentCount = (int)Math.Round(Percent(IntervalCount + 1) * 40);
            Graph = new StringBuilder();
            if (CumCount > 0)
                Graph.Append(' ', CumCount);
            if (CumCount >= 0)
                Graph.Append('o');
            for (int j = 0; j < PercentCount; j++)
                Graph[j] = '*';
            Output += string.Format("{0,5:0.00} -  +INF : {1,4} ({2,5:0.00}%), {3,6:0.00}% {4}", 
                LowerBound(IntervalCount + 1), Count(IntervalCount + 1), Percent(IntervalCount + 1) * 100, 
                CumulativePercent(IntervalCount + 1) * 100, Graph);

            return Output;
        }
    }

    /// <summary>
    /// ����� <c>IntervalHistogram</c> �������� ������ �� ����������� �������, � ������� ������� �������� ���������� � �������� ����������
    /// </summary>
    public class IntervalHistogram : HistogramBase
    {
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ALow">������ ������� ������� ��������� ���������</param>
        /// <param name="AStep">��� ������� ��������� ���������</param>
        /// <param name="AIntervalCount">���������� �������� ����������</param>
        public IntervalHistogram(double ALow, double AStep, int AIntervalCount)
            : base(ALow, AStep, AIntervalCount)
        {
            Data = new double[IntervalCount + 2];
            TotalTime = 0;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ALow">������ ������� ������� ��������� ���������</param>
        /// <param name="AStep">��� ������� ��������� ���������</param>
        /// <param name="AIntervalCount">���������� �������� ����������</param>
        /// <param name="AHeader">��������� ��� ������ �� �����</param>
        public IntervalHistogram(double ALow, double AStep, int AIntervalCount, string AHeader)
            : base(ALow, AStep, AIntervalCount, AHeader)
        {
            Data = new double[IntervalCount + 2];
            TotalTime = 0;
        }

        /// <summary>
        /// ������, � ������� ���������� �����, � ������� �������� �������� �������� ���������� � ������ ���������
        /// </summary>
        private double[] Data;

        /// <summary>
        /// ��������� ����� ���������� ��������
        /// </summary>
        public double TotalTime
        {
            get;
            private set;
        }

        /// <summary>
        /// ��������� � ����������� ����� ��������. ����������� �� <c>dTime</c> ������� �������, ��������������� ����������� ��������, 
        /// � ����� ���������� ��������
        /// </summary>
        /// <param name="newData">����������� ��������</param>
        /// <param name="dTime">�����, � ������� �������� �������� ��������� ��������� ��������</param>
        public void AddData(double newData, double dTime)
        {
            Data[IntervalIndex(newData)] += dTime;
            TotalTime += dTime;
        }

        /// <summary>
        /// ������� �����������, ��������� ������� �������� �� ��� �������� ������� � ������� ����� �����
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i <= IntervalCount + 1; i++)
                Data[i] = 0;
            TotalTime = 0;
        }

        /// <summary>
        /// ���������� ���� �� ������ �������, � ������� �������� �������� �������� � ��������� �� ������ �� ����������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>���� �������</returns>
        public override double CumulativePercent(int index)
        {
            if (TotalTime == 0)
                return 0;
            else
                return CumulativeTime(index) / TotalTime;
        }

        /// <summary>
        /// ����� ���������� �������, � ������� �������� �������� �������� � ��������� �� �������� ������ �� ����������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>����� ���������� �������</returns>
        public double CumulativeTime(int index)
        {
            double Sum = 0;
            for (int i = 0; i <= index && i <= IntervalCount + 1; i++)
                Sum += Data[i];
            return Sum;
        }

        /// <summary>
        /// ���������� ���� �� ������ �������, � ������� �������� �������� �������� � ��������� ��������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>���� �������</returns>
        public override double Percent(int index)
        {
            if (TotalTime == 0)
                return 0;
            else
                return Time(index) / TotalTime;
        }

        /// <summary>
        /// ���������� ��������� �����, � ������� �������� �������� ���������� � ��������� ���������
        /// </summary>
        /// <param name="index">������ ���������</param>
        /// <returns>��������� �����</returns>
        public double Time(int index)
        {
            if (index <= 0)
                return Data[0];
            else if (index >= IntervalCount + 1)
                return Data[IntervalCount + 1];
            else
                return Data[index];
        }

        /// <summary>
        /// ���������� ���������� ����������� � ��������� ���� ��� ����������� �� ������
        /// </summary>
        /// <returns>��������� ��� �����������</returns>
        public override string ToString()
        {
            string Output = Header + "\n";

            // ������� ��������������� ��������
            int CumCount = (int)Math.Round(CumulativePercent(0) * 40) - 1;
            int PercentCount = (int)Math.Round(Percent(0) * 40);
            StringBuilder Graph = new StringBuilder();
            if (CumCount > 0)
                Graph.Append(' ', CumCount);
            if (CumCount >= 0)
                Graph.Append('o');
            for (int j = 0; j < PercentCount; j++)
                Graph[j] = '*';
            Output += string.Format(" -INF - {0,5:0.00} : {1,5:0.0} ({2,5:0.00}%), {3,6:0.00}% {4}\n", 
                UpperBound(0), Time(0), Percent(0) * 100, CumulativePercent(0) * 100, Graph);

            // �������� ���������
            for (int i = 1; i <= IntervalCount; i++)
            {
                CumCount = (int)Math.Round(CumulativePercent(i) * 40) - 1;
                PercentCount = (int)Math.Round(Percent(i) * 40);
                Graph = new StringBuilder();
                if (CumCount > 0)
                    Graph.Append(' ', CumCount);
                if (CumCount >= 0)
                    Graph.Append('o');
                for (int j = 0; j < PercentCount; j++)
                    Graph[j] = '*';
                Output += string.Format("{0,5:0.00} - {1,5:0.00} : {2,5:0.0} ({3,5:0.00}%), {4,6:0.00}% {5}\n", 
                    LowerBound(i), UpperBound(i), Time(i), Percent(i) * 100, CumulativePercent(i) * 100, Graph);
            }

            // ��������� ��������������� ��������
            CumCount = (int)Math.Round(CumulativePercent(IntervalCount + 1) * 40) - 1;
            PercentCount = (int)Math.Round(Percent(IntervalCount + 1) * 40);
            Graph = new StringBuilder();
            if (CumCount > 0)
                Graph.Append(' ', CumCount);
            if (CumCount >= 0)
                Graph.Append('o');
            for (int j = 0; j < PercentCount; j++)
                Graph[j] = '*';
            Output += string.Format("{0,5:0.00} -  +INF : {1,5:0.0} ({2,5:0.00}%), {3,6:0.00}% {4}", 
                LowerBound(IntervalCount + 1), Time(IntervalCount + 1), Percent(IntervalCount + 1) * 100, 
                CumulativePercent(IntervalCount + 1) * 100, Graph);
            return Output;
        }
    }
}
