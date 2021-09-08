using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Simulation;
using Shop;

namespace ShopVisual
{
    public partial class ShopVisual : Form
    {
        public ShopVisual()
        {
            InitializeComponent();
            ShopSimulation.RandCust = new Simulation.Random();
            ShopSimulation.RandService = new Simulation.Random();
            dgvCash.InitForServiceStat();
            dgvInShop.InitForActionStat();
            dgvQueue.InitForQueueStat();
            dgvStat.InitForStat();
            dgvHist.InitForHist();
        }

        internal static ShopSimulation Shop;

        private int LastFinished;

        internal static double VisualInterval = 1;

        private void tmrShop_Tick(object sender, EventArgs e)
        {
            lblInShop.Text = new string('*', Shop.InShopStat.Running);
            lblQueue.Text = new string('*', Shop.Queue.Size);
            lblCash.Text = (Shop.CashStat.Running == 0) ? "" : "(*)";
            lblServiced.Text = Shop.CashStat.Finished.ToString();
            lblSimTime.Text = Shop.SimTime().ToString("0.0");
            dgvStat.ShowStat(Shop.TimeStat);
            dgvInShop.ShowStat(Shop.InShopStat);
            dgvCash.ShowStat(Shop.CashStat);
            dgvQueue.ShowStat(Shop.Queue, Shop.Calendar);
            if (Shop.CashStat.Finished != LastFinished)
            {
                LastFinished = Shop.CashStat.Finished;
                dgvHist.ShowHist(Shop.TimeHist);
            }
            if (Shop.Terminated)
            {
                tmrShop.Enabled = false;
            }
            else
            {
                Shop.Start();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tmrShop.Enabled)
            {
                tmrShop.Enabled = false;
            }
            else
            {
                tmrShop.Enabled = true;
                if (Shop != null)
                {
                    Shop.Finish();
                }
                Shop = new ShopSimulation();
                Shop.VisualInterval = VisualInterval;
                Shop.Start();
            }
        }

        private void dgvHist_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (Shop != null)
            {
                dgvHist.DrawCell(e, Shop.TimeHist);
            }
        }

        private void dgvHist_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                dgvHist.Refresh();
            }
        }
    }
}
