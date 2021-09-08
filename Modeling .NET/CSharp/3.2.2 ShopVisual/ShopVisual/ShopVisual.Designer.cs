namespace ShopVisual
{
    partial class ShopVisual
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle51 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle52 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle53 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle54 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInShop = new System.Windows.Forms.Label();
            this.lblQueue = new System.Windows.Forms.Label();
            this.lblCash = new System.Windows.Forms.Label();
            this.lblServiced = new System.Windows.Forms.Label();
            this.lblSimTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbcStat = new System.Windows.Forms.TabControl();
            this.tpgStat = new System.Windows.Forms.TabPage();
            this.tpgHist = new System.Windows.Forms.TabPage();
            this.dgvStat = new System.Windows.Forms.DataGridView();
            this.dgvInShop = new System.Windows.Forms.DataGridView();
            this.dgvCash = new System.Windows.Forms.DataGridView();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.tmrShop = new System.Windows.Forms.Timer(this.components);
            this.dgvHist = new System.Windows.Forms.DataGridView();
            this.tbcStat.SuspendLayout();
            this.tpgStat.SuspendLayout();
            this.tpgHist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInShop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHist)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Торговый зал";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Очередь";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Кассир";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Обслужено";
            // 
            // lblInShop
            // 
            this.lblInShop.Location = new System.Drawing.Point(12, 34);
            this.lblInShop.Name = "lblInShop";
            this.lblInShop.Size = new System.Drawing.Size(148, 25);
            this.lblInShop.TabIndex = 0;
            this.lblInShop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQueue
            // 
            this.lblQueue.Location = new System.Drawing.Point(166, 34);
            this.lblQueue.Name = "lblQueue";
            this.lblQueue.Size = new System.Drawing.Size(98, 25);
            this.lblQueue.TabIndex = 0;
            this.lblQueue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCash
            // 
            this.lblCash.Location = new System.Drawing.Point(270, 34);
            this.lblCash.Name = "lblCash";
            this.lblCash.Size = new System.Drawing.Size(84, 25);
            this.lblCash.TabIndex = 0;
            this.lblCash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServiced
            // 
            this.lblServiced.Location = new System.Drawing.Point(360, 34);
            this.lblServiced.Name = "lblServiced";
            this.lblServiced.Size = new System.Drawing.Size(125, 25);
            this.lblServiced.TabIndex = 0;
            this.lblServiced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSimTime
            // 
            this.lblSimTime.Location = new System.Drawing.Point(491, 34);
            this.lblSimTime.Name = "lblSimTime";
            this.lblSimTime.Size = new System.Drawing.Size(76, 25);
            this.lblSimTime.TabIndex = 0;
            this.lblSimTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(491, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "Время";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(496, 62);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(71, 38);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Пуск";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbcStat
            // 
            this.tbcStat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcStat.Controls.Add(this.tpgStat);
            this.tbcStat.Controls.Add(this.tpgHist);
            this.tbcStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbcStat.Location = new System.Drawing.Point(17, 106);
            this.tbcStat.Name = "tbcStat";
            this.tbcStat.SelectedIndex = 0;
            this.tbcStat.Size = new System.Drawing.Size(893, 423);
            this.tbcStat.TabIndex = 2;
            // 
            // tpgStat
            // 
            this.tpgStat.Controls.Add(this.dgvQueue);
            this.tpgStat.Controls.Add(this.dgvCash);
            this.tpgStat.Controls.Add(this.dgvInShop);
            this.tpgStat.Controls.Add(this.dgvStat);
            this.tpgStat.Location = new System.Drawing.Point(4, 25);
            this.tpgStat.Name = "tpgStat";
            this.tpgStat.Padding = new System.Windows.Forms.Padding(3);
            this.tpgStat.Size = new System.Drawing.Size(885, 394);
            this.tpgStat.TabIndex = 0;
            this.tpgStat.Text = "Статистика";
            this.tpgStat.UseVisualStyleBackColor = true;
            // 
            // tpgHist
            // 
            this.tpgHist.Controls.Add(this.dgvHist);
            this.tpgHist.Location = new System.Drawing.Point(4, 25);
            this.tpgHist.Name = "tpgHist";
            this.tpgHist.Padding = new System.Windows.Forms.Padding(3);
            this.tpgHist.Size = new System.Drawing.Size(885, 394);
            this.tpgHist.TabIndex = 1;
            this.tpgHist.Text = "Гистограмма";
            this.tpgHist.UseVisualStyleBackColor = true;
            // 
            // dgvStat
            // 
            this.dgvStat.AllowUserToAddRows = false;
            this.dgvStat.AllowUserToDeleteRows = false;
            this.dgvStat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle49.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle49.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle49.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle49.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle49.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle49.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle49.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStat.DefaultCellStyle = dataGridViewCellStyle49;
            this.dgvStat.Location = new System.Drawing.Point(6, 6);
            this.dgvStat.Name = "dgvStat";
            this.dgvStat.ReadOnly = true;
            this.dgvStat.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvStat.Size = new System.Drawing.Size(873, 68);
            this.dgvStat.TabIndex = 0;
            // 
            // dgvInShop
            // 
            this.dgvInShop.AllowUserToAddRows = false;
            this.dgvInShop.AllowUserToDeleteRows = false;
            this.dgvInShop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInShop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle50.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle50.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle50.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle50.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle50.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInShop.DefaultCellStyle = dataGridViewCellStyle50;
            this.dgvInShop.Location = new System.Drawing.Point(6, 80);
            this.dgvInShop.Name = "dgvInShop";
            this.dgvInShop.ReadOnly = true;
            this.dgvInShop.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvInShop.Size = new System.Drawing.Size(873, 70);
            this.dgvInShop.TabIndex = 0;
            // 
            // dgvCash
            // 
            this.dgvCash.AllowUserToAddRows = false;
            this.dgvCash.AllowUserToDeleteRows = false;
            this.dgvCash.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCash.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle51.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle51.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle51.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle51.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle51.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle51.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCash.DefaultCellStyle = dataGridViewCellStyle51;
            this.dgvCash.Location = new System.Drawing.Point(6, 156);
            this.dgvCash.Name = "dgvCash";
            this.dgvCash.ReadOnly = true;
            this.dgvCash.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvCash.Size = new System.Drawing.Size(873, 85);
            this.dgvCash.TabIndex = 0;
            // 
            // dgvQueue
            // 
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            this.dgvQueue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle52.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle52.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle52.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle52.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle52.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle52.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueue.DefaultCellStyle = dataGridViewCellStyle52;
            this.dgvQueue.Location = new System.Drawing.Point(6, 247);
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvQueue.Size = new System.Drawing.Size(873, 90);
            this.dgvQueue.TabIndex = 0;
            // 
            // tmrShop
            // 
            this.tmrShop.Tick += new System.EventHandler(this.tmrShop_Tick);
            // 
            // dgvHist
            // 
            this.dgvHist.AllowUserToAddRows = false;
            this.dgvHist.AllowUserToDeleteRows = false;
            this.dgvHist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle53.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle53.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle53.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle53.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle53.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle53.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHist.DefaultCellStyle = dataGridViewCellStyle53;
            this.dgvHist.Location = new System.Drawing.Point(6, 6);
            this.dgvHist.Name = "dgvHist";
            this.dgvHist.ReadOnly = true;
            dataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle54.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle54.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle54.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle54.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle54.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHist.RowHeadersDefaultCellStyle = dataGridViewCellStyle54;
            this.dgvHist.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvHist.Size = new System.Drawing.Size(873, 382);
            this.dgvHist.TabIndex = 0;
            this.dgvHist.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvHist_Scroll);
            this.dgvHist.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvHist_CellPainting);
            // 
            // ShopVisual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 541);
            this.Controls.Add(this.tbcStat);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSimTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblServiced);
            this.Controls.Add(this.lblCash);
            this.Controls.Add(this.lblQueue);
            this.Controls.Add(this.lblInShop);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "ShopVisual";
            this.Text = "Модель магазина";
            this.tbcStat.ResumeLayout(false);
            this.tpgStat.ResumeLayout(false);
            this.tpgHist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInShop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHist)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblInShop;
        private System.Windows.Forms.Label lblQueue;
        private System.Windows.Forms.Label lblCash;
        private System.Windows.Forms.Label lblServiced;
        private System.Windows.Forms.Label lblSimTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TabControl tbcStat;
        private System.Windows.Forms.TabPage tpgStat;
        private System.Windows.Forms.TabPage tpgHist;
        private System.Windows.Forms.DataGridView dgvCash;
        private System.Windows.Forms.DataGridView dgvInShop;
        private System.Windows.Forms.DataGridView dgvStat;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.Timer tmrShop;
        private System.Windows.Forms.DataGridView dgvHist;
    }
}

