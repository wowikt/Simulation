namespace TVControlVisual
{
    partial class TVControlForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.lblInspectionQueue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAdjustmentQueue = new System.Windows.Forms.Label();
            this.lblInspection = new System.Windows.Forms.Label();
            this.lblAdjustment = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSimTime = new System.Windows.Forms.Label();
            this.lblInspectionBack = new System.Windows.Forms.Label();
            this.lblInspectionUsage = new System.Windows.Forms.Label();
            this.lblAdjustmentBack = new System.Windows.Forms.Label();
            this.lblAdjustmentUsage = new System.Windows.Forms.Label();
            this.tabStat = new System.Windows.Forms.TabControl();
            this.tpStat = new System.Windows.Forms.TabPage();
            this.dgvTime = new System.Windows.Forms.DataGridView();
            this.dgvService = new System.Windows.Forms.DataGridView();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.tmrTVControl = new System.Windows.Forms.Timer(this.components);
            this.tabStat.SuspendLayout();
            this.tpStat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Очередь проверки";
            // 
            // lblInspectionQueue
            // 
            this.lblInspectionQueue.Location = new System.Drawing.Point(12, 34);
            this.lblInspectionQueue.Name = "lblInspectionQueue";
            this.lblInspectionQueue.Size = new System.Drawing.Size(198, 25);
            this.lblInspectionQueue.TabIndex = 0;
            this.lblInspectionQueue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Проверка";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(331, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Очередь настройки";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(545, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Настройка";
            // 
            // lblAdjustmentQueue
            // 
            this.lblAdjustmentQueue.Location = new System.Drawing.Point(331, 34);
            this.lblAdjustmentQueue.Name = "lblAdjustmentQueue";
            this.lblAdjustmentQueue.Size = new System.Drawing.Size(208, 25);
            this.lblAdjustmentQueue.TabIndex = 0;
            this.lblAdjustmentQueue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInspection
            // 
            this.lblInspection.Location = new System.Drawing.Point(216, 34);
            this.lblInspection.Name = "lblInspection";
            this.lblInspection.Size = new System.Drawing.Size(109, 25);
            this.lblInspection.TabIndex = 0;
            this.lblInspection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAdjustment
            // 
            this.lblAdjustment.Location = new System.Drawing.Point(545, 34);
            this.lblAdjustment.Name = "lblAdjustment";
            this.lblAdjustment.Size = new System.Drawing.Size(119, 25);
            this.lblAdjustment.TabIndex = 0;
            this.lblAdjustment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(17, 62);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 37);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Пуск";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(670, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Время";
            // 
            // lblSimTime
            // 
            this.lblSimTime.Location = new System.Drawing.Point(670, 34);
            this.lblSimTime.Name = "lblSimTime";
            this.lblSimTime.Size = new System.Drawing.Size(76, 25);
            this.lblSimTime.TabIndex = 0;
            this.lblSimTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInspectionBack
            // 
            this.lblInspectionBack.BackColor = System.Drawing.Color.MidnightBlue;
            this.lblInspectionBack.Location = new System.Drawing.Point(216, 59);
            this.lblInspectionBack.Name = "lblInspectionBack";
            this.lblInspectionBack.Size = new System.Drawing.Size(109, 28);
            this.lblInspectionBack.TabIndex = 2;
            // 
            // lblInspectionUsage
            // 
            this.lblInspectionUsage.BackColor = System.Drawing.Color.Red;
            this.lblInspectionUsage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInspectionUsage.Location = new System.Drawing.Point(216, 59);
            this.lblInspectionUsage.Name = "lblInspectionUsage";
            this.lblInspectionUsage.Size = new System.Drawing.Size(109, 28);
            this.lblInspectionUsage.TabIndex = 2;
            // 
            // lblAdjustmentBack
            // 
            this.lblAdjustmentBack.BackColor = System.Drawing.Color.MidnightBlue;
            this.lblAdjustmentBack.Location = new System.Drawing.Point(545, 59);
            this.lblAdjustmentBack.Name = "lblAdjustmentBack";
            this.lblAdjustmentBack.Size = new System.Drawing.Size(119, 28);
            this.lblAdjustmentBack.TabIndex = 2;
            // 
            // lblAdjustmentUsage
            // 
            this.lblAdjustmentUsage.BackColor = System.Drawing.Color.Red;
            this.lblAdjustmentUsage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAdjustmentUsage.Location = new System.Drawing.Point(545, 59);
            this.lblAdjustmentUsage.Name = "lblAdjustmentUsage";
            this.lblAdjustmentUsage.Size = new System.Drawing.Size(119, 28);
            this.lblAdjustmentUsage.TabIndex = 2;
            // 
            // tabStat
            // 
            this.tabStat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabStat.Controls.Add(this.tpStat);
            this.tabStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabStat.Location = new System.Drawing.Point(12, 105);
            this.tabStat.Name = "tabStat";
            this.tabStat.SelectedIndex = 0;
            this.tabStat.Size = new System.Drawing.Size(880, 431);
            this.tabStat.TabIndex = 3;
            // 
            // tpStat
            // 
            this.tpStat.Controls.Add(this.dgvQueue);
            this.tpStat.Controls.Add(this.dgvService);
            this.tpStat.Controls.Add(this.dgvTime);
            this.tpStat.Location = new System.Drawing.Point(4, 25);
            this.tpStat.Name = "tpStat";
            this.tpStat.Padding = new System.Windows.Forms.Padding(3);
            this.tpStat.Size = new System.Drawing.Size(872, 402);
            this.tpStat.TabIndex = 0;
            this.tpStat.Text = "Статистика";
            this.tpStat.UseVisualStyleBackColor = true;
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
            this.dgvTime.Size = new System.Drawing.Size(860, 69);
            this.dgvTime.TabIndex = 0;
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
            this.dgvService.Location = new System.Drawing.Point(6, 81);
            this.dgvService.Name = "dgvService";
            this.dgvService.ReadOnly = true;
            this.dgvService.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvService.Size = new System.Drawing.Size(860, 107);
            this.dgvService.TabIndex = 0;
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
            this.dgvQueue.Location = new System.Drawing.Point(6, 194);
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvQueue.Size = new System.Drawing.Size(860, 109);
            this.dgvQueue.TabIndex = 0;
            // 
            // tmrTVControl
            // 
            this.tmrTVControl.Tick += new System.EventHandler(this.tmrTVControl_Tick);
            // 
            // TVControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 548);
            this.Controls.Add(this.tabStat);
            this.Controls.Add(this.lblAdjustmentUsage);
            this.Controls.Add(this.lblInspectionUsage);
            this.Controls.Add(this.lblAdjustmentBack);
            this.Controls.Add(this.lblInspectionBack);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblSimTime);
            this.Controls.Add(this.lblAdjustment);
            this.Controls.Add(this.lblInspection);
            this.Controls.Add(this.lblAdjustmentQueue);
            this.Controls.Add(this.lblInspectionQueue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MinimumSize = new System.Drawing.Size(750, 500);
            this.Name = "TVControlForm";
            this.Text = "Контроль качества телевизоров";
            this.tabStat.ResumeLayout(false);
            this.tpStat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInspectionQueue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblAdjustmentQueue;
        private System.Windows.Forms.Label lblInspection;
        private System.Windows.Forms.Label lblAdjustment;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSimTime;
        private System.Windows.Forms.Label lblInspectionBack;
        private System.Windows.Forms.Label lblInspectionUsage;
        private System.Windows.Forms.Label lblAdjustmentBack;
        private System.Windows.Forms.Label lblAdjustmentUsage;
        private System.Windows.Forms.TabControl tabStat;
        private System.Windows.Forms.TabPage tpStat;
        private System.Windows.Forms.DataGridView dgvTime;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridView dgvService;
        private System.Windows.Forms.Timer tmrTVControl;
    }
}

