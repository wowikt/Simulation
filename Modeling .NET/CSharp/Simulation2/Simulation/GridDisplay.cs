using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Simulation
{
    /// <summary>
    /// Класс GridDisplay содержит методы расширения для отображения статистики и гистограмм в табличных сетках
    /// </summary>
    public static class GridDisplay
    {
        /// <summary>
        /// Подготавливает табличную сетку для отображения гистограммы. 
        /// Создает и настраивает необходимые столбцы.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения гистограммы</param>
        public static void InitForHist(this DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("dgvcMean", "Значений");
            dgv.Columns[0].DefaultCellStyle.Format = "0";
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[0].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcDeviation", "Процент");
            dgv.Columns[1].DefaultCellStyle.Format = "0.00%";
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[1].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMin", "Накопление");
            dgv.Columns[2].DefaultCellStyle.Format = "0.00%";
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[2].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMax", "Гистограмма");
            dgv.Columns[3].DefaultCellStyle.Format = "0";
            dgv.Columns[3].Width = 600;
        }

        /// <summary>
        /// Подготавливает табличную сетку для отображения точечной статистики. 
        /// Создает и настраивает необходимые столбцы.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения статистики</param>
        public static void InitForStat(this DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("dgvcMean", "Среднее");
            dgv.Columns[0].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[0].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcDeviation", "Отклонение");
            dgv.Columns[1].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[1].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMin", "Минимум");
            dgv.Columns[2].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[2].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMax", "Максимум");
            dgv.Columns[3].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[3].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcCount", "Количество");
            dgv.Columns[4].DefaultCellStyle.Format = "0";
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[4].Resizable = DataGridViewTriState.True;
        }

        /// <summary>
        /// Подготавливает табличную сетку для отображения интервальной статистики. 
        /// Создает и настраивает необходимые столбцы.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения статистики</param>
        public static void InitForIntervalStat(this DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("dgvcMean", "Среднее");
            dgv.Columns[0].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[0].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcDeviation", "Отклонение");
            dgv.Columns[1].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[1].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMin", "Минимум");
            dgv.Columns[2].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[2].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMax", "Максимум");
            dgv.Columns[3].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[3].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcInterval", "Интервал");
            dgv.Columns[4].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[4].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcCurrent", "Сейчас");
            dgv.Columns[4].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[4].Resizable = DataGridViewTriState.True;
        }

        /// <summary>
        /// Подготавливает табличную сетку для отображения статистики действия. 
        /// Создает и настраивает необходимые столбцы.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения статистики</param>
        public static void InitForActionStat(this DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("dgvcMean", "Среднее");
            dgv.Columns[0].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[0].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcDeviation", "Отклонение");
            dgv.Columns[1].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[1].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMax", "Максимум");
            dgv.Columns[2].DefaultCellStyle.Format = "0";
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[2].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcCurrent", "Сейчас");
            dgv.Columns[3].DefaultCellStyle.Format = "0";
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[3].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcFinished", "Выполнено");
            dgv.Columns[4].DefaultCellStyle.Format = "0";
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[4].Resizable = DataGridViewTriState.True;
        }

        /// <summary>
        /// Подготавливает табличную сетку для отображения статистики обслуживающего действия. 
        /// Создает и настраивает необходимые столбцы.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения статистики</param>
        public static void InitForServiceStat(this DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("dgvcCount", "Количество");
            dgv.Columns[0].DefaultCellStyle.Format = "0";
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[0].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMean", "Среднее");
            dgv.Columns[1].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[1].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcDeviation", "Отклонение");
            dgv.Columns[2].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[2].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcCurrent", "Сейчас");
            dgv.Columns[3].DefaultCellStyle.Format = "0";
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[3].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMeanBlock", "Ср. блок / простой");
            dgv.Columns[4].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[4].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMaxIdle", "Макс. простой");
            dgv.Columns[5].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[5].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMaxBusy", "Макс. работа");
            dgv.Columns[6].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[6].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcFinished", "Выполнено");
            dgv.Columns[7].DefaultCellStyle.Format = "0";
            dgv.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[7].Resizable = DataGridViewTriState.True;
        }

        /// <summary>
        /// Подготавливает табличную сетку для отображения статистики очереди (списка). 
        /// Создает и настраивает необходимые столбцы.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения статистики</param>
        public static void InitForQueueStat(this DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("dgvcMean", "Среднее");
            dgv.Columns[0].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[0].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcDeviation", "Отклонение");
            dgv.Columns[1].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[1].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMax", "Максимум");
            dgv.Columns[2].DefaultCellStyle.Format = "0";
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[2].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcCurrent", "Сейчас");
            dgv.Columns[3].DefaultCellStyle.Format = "0";
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[3].Resizable = DataGridViewTriState.True;
            dgv.Columns.Add("dgvcMeanTime", "Ср. время");
            dgv.Columns[4].DefaultCellStyle.Format = "0.0000";
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[4].Resizable = DataGridViewTriState.True;
        }

        /// <summary>
        /// Отображает в табличной сетке результаты сбора точечной статистики.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения</param>
        /// <param name="stat">Объекты статистики, которые следует отобразить</param>
        public static void ShowStat(this DataGridView dgv, params Statistics[] stat)
        {
            int N = stat.Length;
            if (dgv.Rows.Count != N)
            {
                dgv.Rows.Clear();
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows.Add(new object[] { stat[i].Mean(), stat[i].Deviation(), stat[i].Min, stat[i].Max, stat[i].Count });
                    dgv.Rows[i].HeaderCell.Value = stat[i].Header;
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows[i].Cells[0].Value = stat[i].Mean();
                    dgv.Rows[i].Cells[1].Value = stat[i].Deviation();
                    dgv.Rows[i].Cells[2].Value = stat[i].Min;
                    dgv.Rows[i].Cells[3].Value = stat[i].Max;
                    dgv.Rows[i].Cells[4].Value = stat[i].Count;
                }
            }
        }

        /// <summary>
        /// Отображает в табличной сетке результаты сбора интервальной статистики.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения</param>
        /// <param name="stat">Объекты статистики, которые следует отобразить</param>
        public static void ShowStat(this DataGridView dgv, params IntervalStatistics[] stat)
        {
            int N = stat.Length;
            if (dgv.Rows.Count != N)
            {
                dgv.Rows.Clear();
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows.Add(new object[] { stat[i].Mean(), stat[i].Deviation(), stat[i].Min, stat[i].Max, stat[i].TotalTime, stat[i].LastX });
                    dgv.Rows[i].HeaderCell.Value = stat[i].Header;
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows[i].Cells[0].Value = stat[i].Mean();
                    dgv.Rows[i].Cells[1].Value = stat[i].Deviation();
                    dgv.Rows[i].Cells[2].Value = stat[i].Min;
                    dgv.Rows[i].Cells[3].Value = stat[i].Max;
                    dgv.Rows[i].Cells[4].Value = stat[i].TotalTime;
                    dgv.Rows[i].Cells[5].Value = stat[i].LastX;
                }
            }
        }

        /// <summary>
        /// Отображает в табличной сетке результаты сбора статистики действия.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения</param>
        /// <param name="stat">Объекты статистики, которые следует отобразить</param>
        public static void ShowStat(this DataGridView dgv, params ActionStatistics[] stat)
        {
            int N = stat.Length;
            if (dgv.Rows.Count != N)
            {
                dgv.Rows.Clear();
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows.Add(new object[] { stat[i].Mean(), stat[i].Deviation(), stat[i].Max, stat[i].Running, stat[i].Finished });
                    dgv.Rows[i].HeaderCell.Value = stat[i].Header;
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows[i].Cells[0].Value = stat[i].Mean();
                    dgv.Rows[i].Cells[1].Value = stat[i].Deviation();
                    dgv.Rows[i].Cells[2].Value = stat[i].Max;
                    dgv.Rows[i].Cells[3].Value = stat[i].Running;
                    dgv.Rows[i].Cells[4].Value = stat[i].Finished;
                }
            }
        }

        /// <summary>
        /// Отображает в табличной сетке результаты сбора статистики обслуживающего действия.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения</param>
        /// <param name="stat">Объекты статистики, которые следует отобразить</param>
        public static void ShowStat(this DataGridView dgv, params ServiceStatistics[] stat)
        {
            int N = stat.Length;
            if (dgv.Rows.Count != N)
            {
                dgv.Rows.Clear();
                for (int i = 0; i < N; i++)
                {
                    if (stat[i].Devices > 1)
                        dgv.Rows.Add(new object[] { stat[i].Devices, stat[i].Mean(), stat[i].Deviation(), 
                            stat[i].Running, stat[i].MeanBlockage(), stat[i].Devices - stat[i].MinBusy, stat[i].MaxBusy, stat[i].Finished });
                    else
                        dgv.Rows.Add(new object[] { stat[i].Devices, stat[i].Mean(), stat[i].Deviation(), 
                            stat[i].Running, stat[i].MeanBlockage(), stat[i].MaxIdleTime, stat[i].MaxBusyTime, stat[i].Finished });
                    dgv.Rows[i].HeaderCell.Value = stat[i].Header;
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows[i].Cells[0].Value = stat[i].Devices;
                    dgv.Rows[i].Cells[1].Value = stat[i].Mean();
                    dgv.Rows[i].Cells[2].Value = stat[i].Deviation();
                    dgv.Rows[i].Cells[3].Value = stat[i].Running;
                    dgv.Rows[i].Cells[4].Value = stat[i].MeanBlockage();
                    if (stat[i].Devices > 1)
                    {
                        dgv.Rows[i].Cells[5].Value = stat[i].Devices - stat[i].MinBusy;
                        dgv.Rows[i].Cells[6].Value = stat[i].MaxBusy;
                    }
                    else
                    {
                        dgv.Rows[i].Cells[5].Value = stat[i].MaxIdleTime;
                        dgv.Rows[i].Cells[6].Value = stat[i].MaxBusyTime;
                    }
                    dgv.Rows[i].Cells[7].Value = stat[i].Finished;
                }
            }
        }

        /// <summary>
        /// Отображает в табличной сетке результаты сбора статистики по очередям.
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения</param>
        /// <param name="stat">Очереди, статистику по которым которые следует отобразить</param>
        public static void ShowStat(this DataGridView dgv, params Queue[] stat)
        {
            int N = stat.Length;
            if (dgv.Rows.Count != N)
            {
                dgv.Rows.Clear();
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows.Add(new object[] { stat[i].LengthStat.Mean(), stat[i].LengthStat.Deviation(), 
                        stat[i].LengthStat.Max, stat[i].LengthStat.LastX, stat[i].TimeStat.Mean() });
                    dgv.Rows[i].HeaderCell.Value = stat[i].Header;
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows[i].Cells[0].Value = stat[i].LengthStat.Mean();
                    dgv.Rows[i].Cells[1].Value = stat[i].LengthStat.Deviation();
                    dgv.Rows[i].Cells[2].Value = stat[i].LengthStat.Max;
                    dgv.Rows[i].Cells[3].Value = stat[i].LengthStat.LastX;
                    dgv.Rows[i].Cells[4].Value = stat[i].TimeStat.Mean();
                }
            }
        }

        /// <summary>
        /// Отображает гистограмму в табличной сетке. Работает совместно с событием DrawGridViewCellPainting
        /// </summary>
        /// <param name="dgv">Табличная сетка для отображения статистики</param>
        /// <param name="hist">Гистограмма, содержимое которой следует отобразить</param>
        public static void ShowHist(this DataGridView dgv, Histogram hist)
        {
            int N = hist.IntervalCount + 2;
            if (dgv.Rows.Count != N)
            {
                dgv.Rows.Clear();
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows.Add(new object[] { hist.Count(i), hist.Percent(i), hist.CumulativePercent(i) });
                    if (i == 0)
                    {
                        dgv.Rows[i].HeaderCell.Value = "-INF - " + hist.UpperBound(i).ToString("0.00");
                    }
                    else if (i == N - 1)
                    {
                        dgv.Rows[i].HeaderCell.Value = hist.LowerBound(i).ToString("0.00") + " - +INF";
                    }
                    else
                    {
                        dgv.Rows[i].HeaderCell.Value = hist.LowerBound(i).ToString("0.00") + " - " + hist.UpperBound(i).ToString("0.00");
                    }
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    dgv.Rows[i].Cells[0].Value = hist.Count(i);
                    dgv.Rows[i].Cells[1].Value = hist.Percent(i);
                    dgv.Rows[i].Cells[2].Value = hist.CumulativePercent(i);
                }
            }
            dgv.Refresh();
        }

        /// <summary>
        /// Отображает графическое изображение элемента данных гистограммы в ячейке табличной сетки. 
        /// Вызывается из процедуры события DrawGridViewCellPainting
        /// </summary>
        /// <param name="dgv">Табличная сетка, в которой отображается гистограмма</param>
        /// <param name="e">Аргументы события DrawGridViewCellPainting</param>
        /// <param name="hist">Отображаемая гистограмма</param>
        public static void DrawCell(this DataGridView dgv, DataGridViewCellPaintingEventArgs e, Histogram hist)
        {
            int N = hist.IntervalCount + 2;
            if (e.ColumnIndex != 3 || e.RowIndex < 0)
            {
                return;
            }
            Rectangle CellRect = e.CellBounds;
            Graphics g = e.Graphics;
            e.PaintBackground(e.CellBounds, false);
            g.DrawLine(Pens.DarkGray, CellRect.Left + CellRect.Width / 4, CellRect.Top, 
                CellRect.Left + CellRect.Width / 4, CellRect.Bottom);
            g.DrawLine(Pens.DarkGray, CellRect.Left + CellRect.Width / 2, CellRect.Top, 
                CellRect.Left + CellRect.Width / 2, CellRect.Bottom);
            g.DrawLine(Pens.DarkGray, CellRect.Left + CellRect.Width * 3 / 4, CellRect.Top, 
                CellRect.Left + CellRect.Width * 3 / 4, CellRect.Bottom);
            if (hist.Percent(e.RowIndex) > 0)
            {
                Rectangle HistRect = new Rectangle(CellRect.Left, CellRect.Top + CellRect.Height / 6,
                    (int)(CellRect.Width * hist.Percent(e.RowIndex)), CellRect.Height * 2 / 3 + 1);
                g.FillRectangle(Brushes.DarkGray, HistRect.Left + 2, HistRect.Top + 2, HistRect.Width + 1,
                    HistRect.Height + 1);
                g.FillRectangle(Brushes.Red, HistRect);
                g.DrawRectangle(Pens.DarkBlue, HistRect);
            }
            int X = (int)(CellRect.Left + CellRect.Width * hist.CumulativePercent(e.RowIndex));
            int Y = CellRect.Top + CellRect.Height / 2;
            g.FillEllipse(Brushes.Blue, X - 4, Y - 4, 8, 8);
            Pen BluePen = new Pen(Color.Blue, 3);
            if (e.RowIndex > 0)
            {
                if (e.RowIndex == dgv.FirstDisplayedCell.RowIndex)
                {
                    double PrevX = CellRect.Left + CellRect.Width * hist.CumulativePercent(e.RowIndex - 1);
                    int MidX = (int)(X + PrevX) / 2;
                    int MidY = CellRect.Top;
                    g.DrawLine(BluePen, X, Y, MidX, MidY);
                }
                else
                {
                    int PrevX = (int)(CellRect.Left + CellRect.Width * hist.CumulativePercent(e.RowIndex - 1));
                    int PrevY = CellRect.Top - CellRect.Height / 2;
                    g.DrawLine(BluePen, X, Y, PrevX, PrevY);
                }
            }
            if (e.RowIndex < N - 1)
            {
                if (e.RowIndex == dgv.FirstDisplayedCell.RowIndex + dgv.DisplayedRowCount(true) - 1)
                {
                    double NextX = CellRect.Left + CellRect.Width * hist.CumulativePercent(e.RowIndex + 1);
                    int MidX = (int)((X + NextX) / 2);
                    int MidY = CellRect.Bottom;
                    g.DrawLine(BluePen, X, Y, MidX, MidY);
                }
                else
                {
                    int NextX = (int)(CellRect.Left + CellRect.Width * hist.CumulativePercent(e.RowIndex + 1));
                    int NextY = CellRect.Bottom + CellRect.Height / 2;
                    g.DrawLine(BluePen, X, Y, NextX, NextY);
                }
            }
            e.Handled = true;
        }
    }
}
