namespace BankVisual
{
    partial class BankVisual
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblQueue = new System.Windows.Forms.Label();
            this.lblCash = new System.Windows.Forms.Label();
            this.lblServiced = new System.Windows.Forms.Label();
            this.lblNotWaited = new System.Windows.Forms.Label();
            this.lblSimTime = new System.Windows.Forms.Label();
            this.tmrBank = new System.Windows.Forms.Timer(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.prbCash = new System.Windows.Forms.ProgressBar();
            this.tabStat = new System.Windows.Forms.TabControl();
            this.tpgStat = new System.Windows.Forms.TabPage();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.dgvCash = new System.Windows.Forms.DataGridView();
            this.dgvStat = new System.Windows.Forms.DataGridView();
            this.tpgHist = new System.Windows.Forms.TabPage();
            this.dgvHist = new System.Windows.Forms.DataGridView();
            this.tabStat.SuspendLayout();
            this.tpgStat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).BeginInit();
            this.tpgHist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHist)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Очередь";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Кассир";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Обслужено";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(338, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Без ожидания";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(497, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Время";
            // 
            // lblQueue
            // 
            this.lblQueue.Location = new System.Drawing.Point(13, 34);
            this.lblQueue.Name = "lblQueue";
            this.lblQueue.Size = new System.Drawing.Size(98, 25);
            this.lblQueue.TabIndex = 0;
            this.lblQueue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCash
            // 
            this.lblCash.Location = new System.Drawing.Point(117, 34);
            this.lblCash.Name = "lblCash";
            this.lblCash.Size = new System.Drawing.Size(84, 25);
            this.lblCash.TabIndex = 0;
            this.lblCash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServiced
            // 
            this.lblServiced.Location = new System.Drawing.Point(207, 34);
            this.lblServiced.Name = "lblServiced";
            this.lblServiced.Size = new System.Drawing.Size(125, 25);
            this.lblServiced.TabIndex = 0;
            this.lblServiced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNotWaited
            // 
            this.lblNotWaited.Location = new System.Drawing.Point(338, 34);
            this.lblNotWaited.Name = "lblNotWaited";
            this.lblNotWaited.Size = new System.Drawing.Size(153, 25);
            this.lblNotWaited.TabIndex = 0;
            this.lblNotWaited.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSimTime
            // 
            this.lblSimTime.Location = new System.Drawing.Point(497, 34);
            this.lblSimTime.Name = "lblSimTime";
            this.lblSimTime.Size = new System.Drawing.Size(76, 25);
            this.lblSimTime.TabIndex = 0;
            this.lblSimTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrBank
            // 
            this.tmrBank.Tick += new System.EventHandler(this.tmrBank_Tick);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(491, 62);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(82, 37);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Пуск";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(47, 147);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 25);
            this.lblMessage.TabIndex = 2;
            // 
            // prbCash
            // 
            this.prbCash.Location = new System.Drawing.Point(18, 62);
            this.prbCash.Name = "prbCash";
            this.prbCash.Size = new System.Drawing.Size(183, 23);
            this.prbCash.Step = 1;
            this.prbCash.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prbCash.TabIndex = 3;
            // 
            // tabStat
            // 
            this.tabStat.Controls.Add(this.tpgStat);
            this.tabStat.Controls.Add(this.tpgHist);
            this.tabStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabStat.Location = new System.Drawing.Point(12, 105);
            this.tabStat.Name = "tabStat";
            this.tabStat.SelectedIndex = 0;
            this.tabStat.Size = new System.Drawing.Size(945, 375);
            this.tabStat.TabIndex = 4;
            // 
            // tpgStat
            // 
            this.tpgStat.Controls.Add(this.dgvQueue);
            this.tpgStat.Controls.Add(this.dgvCash);
            this.tpgStat.Controls.Add(this.dgvStat);
            this.tpgStat.Location = new System.Drawing.Point(4, 25);
            this.tpgStat.Name = "tpgStat";
            this.tpgStat.Padding = new System.Windows.Forms.Padding(3);
            this.tpgStat.Size = new System.Drawing.Size(937, 346);
            this.tpgStat.TabIndex = 0;
            this.tpgStat.Text = "Статистика";
            this.tpgStat.UseVisualStyleBackColor = true;
            // 
            // dgvQueue
            // 
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            this.dgvQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvQueue.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueue.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQueue.Location = new System.Drawing.Point(6, 180);
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvQueue.Size = new System.Drawing.Size(925, 89);
            this.dgvQueue.TabIndex = 0;
            // 
            // dgvCash
            // 
            this.dgvCash.AllowUserToAddRows = false;
            this.dgvCash.AllowUserToDeleteRows = false;
            this.dgvCash.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvCash.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCash.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCash.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCash.Location = new System.Drawing.Point(6, 78);
            this.dgvCash.Name = "dgvCash";
            this.dgvCash.ReadOnly = true;
            this.dgvCash.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvCash.Size = new System.Drawing.Size(925, 96);
            this.dgvCash.TabIndex = 0;
            // 
            // dgvStat
            // 
            this.dgvStat.AllowUserToAddRows = false;
            this.dgvStat.AllowUserToDeleteRows = false;
            this.dgvStat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvStat.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStat.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStat.Location = new System.Drawing.Point(6, 6);
            this.dgvStat.Name = "dgvStat";
            this.dgvStat.ReadOnly = true;
            this.dgvStat.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvStat.Size = new System.Drawing.Size(925, 66);
            this.dgvStat.TabIndex = 0;
            // 
            // tpgHist
            // 
            this.tpgHist.Controls.Add(this.dgvHist);
            this.tpgHist.Location = new System.Drawing.Point(4, 25);
            this.tpgHist.Name = "tpgHist";
            this.tpgHist.Padding = new System.Windows.Forms.Padding(3);
            this.tpgHist.Size = new System.Drawing.Size(937, 346);
            this.tpgHist.TabIndex = 1;
            this.tpgHist.Text = "Гистограмма";
            this.tpgHist.UseVisualStyleBackColor = true;
            // 
            // dgvHist
            // 
            this.dgvHist.AllowUserToAddRows = false;
            this.dgvHist.AllowUserToDeleteRows = false;
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHist.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvHist.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvHist.Size = new System.Drawing.Size(925, 334);
            this.dgvHist.TabIndex = 0;
            this.dgvHist.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvHist_Scroll);
            this.dgvHist.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvHist_CellPainting);
            // 
            // BankVisual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 492);
            this.Controls.Add(this.tabStat);
            this.Controls.Add(this.prbCash);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSimTime);
            this.Controls.Add(this.lblNotWaited);
            this.Controls.Add(this.lblServiced);
            this.Controls.Add(this.lblCash);
            this.Controls.Add(this.lblQueue);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "BankVisual";
            this.Text = "Модель банка";
            this.Resize += new System.EventHandler(this.BankVisual_Resize);
            this.tabStat.ResumeLayout(false);
            this.tpgStat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).EndInit();
            this.tpgHist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHist)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblQueue;
        private System.Windows.Forms.Label lblCash;
        private System.Windows.Forms.Label lblServiced;
        private System.Windows.Forms.Label lblNotWaited;
        private System.Windows.Forms.Label lblSimTime;
        private System.Windows.Forms.Timer tmrBank;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ProgressBar prbCash;
        private System.Windows.Forms.TabControl tabStat;
        private System.Windows.Forms.TabPage tpgStat;
        private System.Windows.Forms.TabPage tpgHist;
        private System.Windows.Forms.DataGridView dgvStat;
        private System.Windows.Forms.DataGridView dgvCash;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridView dgvHist;
    }
}

