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

namespace ShopMultiVisual
{
    using SimRandom = Simulation.Random;

    public partial class ShopMultiVisual : Form
    {
        public ShopMultiVisual()
        {
            InitializeComponent();
            ShopSimulation.RandCust = new SimRandom();
            ShopSimulation.RandService = new SimRandom();
            Program.CashUsageStat = new Statistics("Занятость кассира");
            Program.TimeStat = new Statistics("Среднее время пребывания в системе");
            Program.InShopStat = new Statistics("Среднее количество покупателей в торговом зале");
            Program.InShopMaxStat = new Statistics("Максимальное количество покупателей в торговом зале");
            Program.MaxQueueLenStat = new Statistics("Максимальная длина очереди");
            Program.WaitStat = new Statistics("Среднее время ожидания в очереди");
            Program.TimeHist = new Histogram(Program.HistMin, Program.HistStep, 
                Program.HistStepCount, "Среднее время пребывания в системе");
            dgvHist.InitForHist();
            dgvStat.InitForStat();
        }

        internal int StepNum;

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tmrShop.Enabled)
            {
                tmrShop.Enabled = false;
            }
            else
            {
                tmrShop.Enabled = true;
                StepNum = 0;
                Program.CashUsageStat.ClearStat();
                Program.TimeStat.ClearStat();
                Program.InShopStat.ClearStat();
                Program.InShopMaxStat.ClearStat();
                Program.MaxQueueLenStat.ClearStat();
                Program.WaitStat.ClearStat();
                Program.TimeHist.Clear();
            }
        }

        private void tmrShop_Tick(object sender, EventArgs e)
        {
            // Создать имитацию
            ShopSimulation ShopSim = new ShopSimulation();
            // Запустить ее
            ShopSim.Start();
            // Собрать статистику
            Program.CashUsageStat.AddData(ShopSim.CashStat.Mean());
            Program.TimeStat.AddData(ShopSim.TimeStat.Mean());
            Program.TimeHist.AddData(ShopSim.TimeStat.Mean());
            Program.InShopStat.AddData(ShopSim.InShopStat.Mean());
            Program.InShopMaxStat.AddData(ShopSim.InShopStat.Max);
            Program.MaxQueueLenStat.AddData(ShopSim.Queue.LengthStat.Max);
            Program.WaitStat.AddData(ShopSim.Queue.TimeStat.Mean());
            // Удалить имитацию
            ShopSim.Finish();
            dgvStat.ShowStat(Program.CashUsageStat, Program.TimeStat, Program.InShopStat, 
                Program.InShopMaxStat, Program.MaxQueueLenStat, Program.WaitStat);
            dgvHist.ShowHist(Program.TimeHist);
            if (++StepNum == Program.RunCount)
            {
                tmrShop.Enabled = false;
            }
            lblRunCount.Text = StepNum.ToString();
        }

        private void dgvHist_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            dgvHist.DrawCell(e, Program.TimeHist);
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
