using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Simulation;
using TVControl;

namespace TVControlVisual
{
    public partial class TVControlForm : Form
    {
        public TVControlForm()
        {
            InitializeComponent();
            dgvQueue.InitForQueueStat();
            dgvService.InitForServiceStat();
            dgvTime.InitForStat();
            TVControlSim.RandAdjuster = new Simulation.Random();
            TVControlSim.RandInspector = new Simulation.Random();
            TVControlSim.RandTVSet = new Simulation.Random();
        }

        internal TVControlSim tvc;

        internal static double VisTimeStep = 0.5;

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tmrTVControl.Enabled)
            {
                tmrTVControl.Enabled = false;
            }
            else
            {
                tmrTVControl.Enabled = true;
                if (tvc != null)
                {
                    tvc.Finish();
                }
                tvc = new TVControlSim();
                tvc.VisualInterval = VisTimeStep;
                tvc.Start();
            }
        }

        private void tmrTVControl_Tick(object sender, EventArgs e)
        {
            lblSimTime.Text = tvc.SimTime().ToString("0.0");
            lblInspectionQueue.Text = new string('*', tvc.InspectionQueue.Size);
            lblAdjustmentQueue.Text = new string('*', tvc.AdjustmentQueue.Size);
            lblInspection.Text = (tvc.InspectorsStat.Running > 0) ? 
                ("(" + new string('*', tvc.InspectorsStat.Running) + ")") : "";
            lblAdjustment.Text = (tvc.AdjustmentStat.Running > 0) ? "(*)" : "";
            lblInspectionUsage.Width = (int)(tvc.InspectorsStat.Mean() / Params.InspectorCount * lblInspectionBack.Width);
            lblAdjustmentUsage.Width = (int)(tvc.AdjustmentStat.Mean() * lblAdjustmentBack.Width);
            dgvTime.ShowStat(tvc.TimeInSystemStat);
            dgvService.ShowStat(tvc.InspectorsStat, tvc.AdjustmentStat);
            dgvQueue.ShowStat(tvc.InspectionQueue, tvc.AdjustmentQueue, tvc.Calendar);
            if (tvc.Terminated)
            {
                tmrTVControl.Enabled = false;
            }
            else
            {
                tvc.Start();
            }
        }
    }
}
