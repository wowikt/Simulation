namespace FlowLineVisual
{
    partial class FlowLineForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.lblQueue1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWorker1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblQueue2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblWorker2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblServiced = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblBalks = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblSimTime = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.tabStat = new System.Windows.Forms.TabControl();
            this.tpgStat = new System.Windows.Forms.TabPage();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.dgvService = new System.Windows.Forms.DataGridView();
            this.dgvTime = new System.Windows.Forms.DataGridView();
            this.tpgHist = new System.Windows.Forms.TabPage();
            this.dgvHist = new System.Windows.Forms.DataGridView();
            this.tmrFlowLine = new System.Windows.Forms.Timer(this.components);
            this.tabStat.SuspendLayout();
            this.tpgStat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTime)).BeginInit();
            this.tpgHist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHist)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Очередь 1";
            // 
            // lblQueue1
            // 
            this.lblQueue1.Location = new System.Drawing.Point(12, 34);
            this.lblQueue1.Name = "lblQueue1";
            this.lblQueue1.Size = new System.Drawing.Size(116, 25);
            this.lblQueue1.TabIndex = 0;
            this.lblQueue1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Рабочий 1";
            // 
            // lblWorker1
            // 
            this.lblWorker1.Location = new System.Drawing.Point(134, 34);
            this.lblWorker1.Name = "lblWorker1";
            this.lblWorker1.Size = new System.Drawing.Size(115, 25);
            this.lblWorker1.TabIndex = 0;
            this.lblWorker1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Очередь 2";
            // 
            // lblQueue2
            // 
            this.lblQueue2.Location = new System.Drawing.Point(255, 34);
            this.lblQueue2.Name = "lblQueue2";
            this.lblQueue2.Size = new System.Drawing.Size(116, 25);
            this.lblQueue2.TabIndex = 0;
            this.lblQueue2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(378, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "Рабочий 2";
            // 
            // lblWorker2
            // 
            this.lblWorker2.Location = new System.Drawing.Point(378, 34);
            this.lblWorker2.Name = "lblWorker2";
            this.lblWorker2.Size = new System.Drawing.Size(115, 25);
            this.lblWorker2.TabIndex = 0;
            this.lblWorker2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(499, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 25);
            this.label8.TabIndex = 0;
            this.label8.Text = "Обслужено";
            // 
            // lblServiced
            // 
            this.lblServiced.Location = new System.Drawing.Point(499, 34);
            this.lblServiced.Name = "lblServiced";
            this.lblServiced.Size = new System.Drawing.Size(125, 25);
            this.lblServiced.TabIndex = 0;
            this.lblServiced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(630, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(154, 25);
            this.label10.TabIndex = 0;
            this.label10.Text = "Не обслужено";
            // 
            // lblBalks
            // 
            this.lblBalks.Location = new System.Drawing.Point(630, 34);
            this.lblBalks.Name = "lblBalks";
            this.lblBalks.Size = new System.Drawing.Size(154, 25);
            this.lblBalks.TabIndex = 0;
            this.lblBalks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(499, 59);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 25);
            this.label12.TabIndex = 0;
            this.label12.Text = "Время";
            // 
            // lblSimTime
            // 
            this.lblSimTime.Location = new System.Drawing.Point(581, 59);
            this.lblSimTime.Name = "lblSimTime";
            this.lblSimTime.Size = new System.Drawing.Size(74, 25);
            this.lblSimTime.TabIndex = 0;
            this.lblSimTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(383, 62);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 42);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Пуск";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tabStat
            // 
            this.tabStat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabStat.Controls.Add(this.tpgStat);
            this.tabStat.Controls.Add(this.tpgHist);
            this.tabStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabStat.Location = new System.Drawing.Point(12, 110);
            this.tabStat.Name = "tabStat";
            this.tabStat.SelectedIndex = 0;
            this.tabStat.Size = new System.Drawing.Size(901, 418);
            this.tabStat.TabIndex = 2;
            // 
            // tpgStat
            // 
            this.tpgStat.Controls.Add(this.dgvQueue);
            this.tpgStat.Controls.Add(this.dgvService);
            this.tpgStat.Controls.Add(this.dgvTime);
            this.tpgStat.Location = new System.Drawing.Point(4, 25);
            this.tpgStat.Name = "tpgStat";
            this.tpgStat.Padding = new System.Windows.Forms.Padding(3);
            this.tpgStat.Size = new System.Drawing.Size(893, 389);
            this.tpgStat.TabIndex = 0;
            this.tpgStat.Text = "Статистика";
            this.tpgStat.UseVisualStyleBackColor = true;
            // 
            // dgvQueue
            // 
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            this.dgvQueue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueue.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQueue.Location = new System.Drawing.Point(6, 220);
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvQueue.Size = new System.Drawing.Size(881, 115);
            this.dgvQueue.TabIndex = 2;
            // 
            // dgvService
            // 
            this.dgvService.AllowUserToAddRows = false;
            this.dgvService.AllowUserToDeleteRows = false;
            this.dgvService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvService.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvService.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvService.Location = new System.Drawing.Point(6, 103);
            this.dgvService.Name = "dgvService";
            this.dgvService.ReadOnly = true;
            this.dgvService.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvService.Size = new System.Drawing.Size(881, 111);
            this.dgvService.TabIndex = 1;
            // 
            // dgvTime
            // 
            this.dgvTime.AllowUserToAddRows = false;
            this.dgvTime.AllowUserToDeleteRows = false;
            this.dgvTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTime.Location = new System.Drawing.Point(6, 6);
            this.dgvTime.Name = "dgvTime";
            this.dgvTime.ReadOnly = true;
            this.dgvTime.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvTime.Size = new System.Drawing.Size(881, 91);
            this.dgvTime.TabIndex = 0;
            // 
            // tpgHist
            // 
            this.tpgHist.Controls.Add(this.dgvHist);
            this.tpgHist.Location = new System.Drawing.Point(4, 25);
            this.tpgHist.Name = "tpgHist";
            this.tpgHist.Padding = new System.Windows.Forms.Padding(3);
            this.tpgHist.Size = new System.Drawing.Size(893, 389);
            this.tpgHist.TabIndex = 1;
            this.tpgHist.Text = "Гистограмма";
            this.tpgHist.UseVisualStyleBackColor = true;
            // 
            // dgvHist
            // 
            this.dgvHist.AllowUserToAddRows = false;
            this.dgvHist.AllowUserToDeleteRows = false;
            this.dgvHist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHist.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvHist.Location = new System.Drawing.Point(6, 6);
            this.dgvHist.Name = "dgvHist";
            this.dgvHist.ReadOnly = true;
            this.dgvHist.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvHist.Size = new System.Drawing.Size(881, 377);
            this.dgvHist.TabIndex = 3;
            this.dgvHist.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvHist_CellPainting);
            // 
            // tmrFlowLine
            // 
            this.tmrFlowLine.Tick += new System.EventHandler(this.tmrFlowLine_Tick);
            // 
            // FlowLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 540);
            this.Controls.Add(this.tabStat);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblSimTime);
            this.Controls.Add(this.lblBalks);
            this.Controls.Add(this.lblServiced);
            this.Controls.Add(this.lblWorker2);
            this.Controls.Add(this.lblQueue2);
            this.Controls.Add(this.lblWorker1);
            this.Controls.Add(this.lblQueue1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "FlowLineForm";
            this.Text = "Поточное производство";
            this.tabStat.ResumeLayout(false);
            this.tpgStat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTime)).EndInit();
            this.tpgHist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHist)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblQueue1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWorker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblQueue2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblWorker2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblServiced;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblBalks;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblSimTime;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TabControl tabStat;
        private System.Windows.Forms.TabPage tpgStat;
        private System.Windows.Forms.DataGridView dgvTime;
        private System.Windows.Forms.TabPage tpgHist;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridView dgvService;
        private System.Windows.Forms.DataGridView dgvHist;
        private System.Windows.Forms.Timer tmrFlowLine;
    }
}

