using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Simulation;
using FlowLine;

namespace FlowLineVisual
{
    using FlowLineSim = FlowLine.FlowLine;

    public partial class FlowLineForm : Form
    {
        public FlowLineForm()
        {
            InitializeComponent();
            FlowLineSim.RandPiece = new Simulation.Random();
            FlowLineSim.RandWorker1 = new Simulation.Random();
            FlowLineSim.RandWorker2 = new Simulation.Random();
            dgvHist.InitForHist();
            dgvQueue.InitForQueueStat();
            dgvService.InitForServiceStat();
            dgvTime.InitForStat();
        }

        internal FlowLineSim flSim;

        internal double VisTimeStep = 0.5;

        private void tmrFlowLine_Tick(object sender, EventArgs e)
        {
            lblSimTime.Text = flSim.SimTime().ToString("0.0");
            lblBalks.Text = (flSim.BalksStat.Count + 1).ToString();
            lblQueue1.Text = new string('*', flSim.Queue1.Size);
            lblQueue2.Text = new string('*', flSim.Queue2.Size);
            lblServiced.Text = flSim.Worker2Stat.Finished.ToString();
            lblWorker1.Text = (flSim.Worker1Stat.Blocked > 0) ? "(*)||" : ((flSim.Worker1Stat.Running > 0) ? "(*)  " : "");
            lblWorker2.Text = (flSim.Worker2Stat.Running > 0) ? "(*)" : "";
            dgvTime.ShowStat(flSim.TimeInSystemStat, flSim.BalksStat);
            dgvService.ShowStat(flSim.Worker1Stat, flSim.Worker2Stat);
            dgvQueue.ShowStat(flSim.Queue1, flSim.Queue2, flSim.Calendar);
            dgvHist.ShowHist(flSim.TimeHist);
            if (flSim.Terminated)
            {
                tmrFlowLine.Enabled = false;
            }
            else
            {
                flSim.Start();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tmrFlowLine.Enabled)
            {
                tmrFlowLine.Enabled = false;
            }
            else
            {
                if (flSim != null)
                {
                    flSim.Finish();
                }
                tmrFlowLine.Enabled = true;
                flSim = new FlowLineSim();
                flSim.VisualInterval = VisTimeStep;
                flSim.Start();
            }
        }

        private void dgvHist_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (flSim != null)
            {
                dgvHist.DrawCell(e, flSim.TimeHist);
            }
        }
    }
}
