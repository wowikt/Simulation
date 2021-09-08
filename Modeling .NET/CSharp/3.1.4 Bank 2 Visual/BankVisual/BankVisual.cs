using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Simulation;
using Bank;

namespace BankVisual
{
    public partial class BankVisual : Form
    {
        public BankVisual()
        {
            InitializeComponent();
            BankSimulation.RandCashman = new Simulation.Random();
            BankSimulation.RandClient = new Simulation.Random();
            dgvStat.InitForStat();
            dgvCash.InitForServiceStat();
            dgvQueue.InitForQueueStat();
            dgvHist.InitForHist();
        }

        internal static BankSimulation Bank;

        internal static double VisTimeStep = 1;

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tmrBank.Enabled)
            {
                tmrBank.Enabled = false;
            }
            else
            {
                tmrBank.Enabled = true;
                Bank = new BankSimulation();
                Bank.VisualInterval = VisTimeStep;
                Bank.Start();
            }
        }

        private int LastFinished;

        private void tmrBank_Tick(object sender, EventArgs e)
        {
            Bank.StopStat();
            lblQueue.Text = new string('*', Bank.Queue.Size);
            lblSimTime.Text = Bank.SimTime().ToString("0.0");
            lblServiced.Text = Bank.CashStat.Finished.ToString();
            lblNotWaited.Text = Bank.NotWaited.ToString();
            lblNotServiced.Text = Bank.NotServiced.ToString();
            lblCash.Text = (Bank.CashStat.Running == 0) ? "" : "(" + new string('*', Bank.CashStat.Running) + ")";
            prbCash.Value = (int) (Bank.CashStat.Mean() * 100 / Param.CashCount);
            dgvStat.ShowStat(Bank.InBankStat);
            dgvCash.ShowStat(Bank.CashStat);
            dgvQueue.ShowStat(Bank.Queue, Bank.Calendar);
            if (Bank.CashStat.Finished != LastFinished)
            {
                dgvHist.ShowHist(Bank.InBankHist);
                LastFinished = Bank.CashStat.Finished;
            }
            if (Bank.Terminated)
            {
                tmrBank.Enabled = false;
            }
            else
            {
                Bank.Start();
            }
        }

        private void dgvHist_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (Bank != null)
            {
                dgvHist.DrawCell(e, Bank.InBankHist);
            }
        }

        private void dgvHist_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll && e.NewValue < e.OldValue)
            {
                dgvHist.Refresh();
            }
        }

        private void BankVisual_Resize(object sender, EventArgs e)
        {
            int NewWidth = ClientSize.Width;
            int NewHeight = ClientSize.Height;
            tabStat.Width = NewWidth - tabStat.Location.X * 2;
            tabStat.Height = NewHeight - tabStat.Location.Y - tabStat.Location.X;
            tpgStat.Width = tabStat.ClientRectangle.Width;
            tpgStat.Height = tabStat.ClientRectangle.Height;
            tpgHist.Width = tabStat.ClientRectangle.Width;
            tpgHist.Height = tabStat.ClientRectangle.Height;
            dgvStat.Width = tpgStat.ClientSize.Width - dgvStat.Location.X * 2;
            dgvCash.Width = tpgStat.ClientSize.Width - dgvCash.Location.X * 2;
            dgvQueue.Width = tpgStat.ClientSize.Width - dgvQueue.Location.X * 2;
            dgvHist.Width = tpgHist.ClientSize.Width - dgvHist.Location.X * 2;
            dgvHist.Height = tpgHist.ClientSize.Height - dgvHist.Location.Y * 2;
        }
    }
}
