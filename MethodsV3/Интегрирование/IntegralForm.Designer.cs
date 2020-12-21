namespace MethodsV3.Интегрирование
{
    partial class IntegralForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridResult = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxN1 = new System.Windows.Forms.TextBox();
            this.tbxN2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxFunc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxSection = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxResult = new System.Windows.Forms.RichTextBox();
            this.btnCreateTable = new System.Windows.Forms.Button();
            this.dataGridN = new System.Windows.Forms.DataGridView();
            this.tbxTakeTotalResult = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridN)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridResult
            // 
            this.dataGridResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridResult.Location = new System.Drawing.Point(9, 390);
            this.dataGridResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridResult.Name = "dataGridResult";
            this.dataGridResult.RowHeadersWidth = 70;
            this.dataGridResult.RowTemplate.Height = 24;
            this.dataGridResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dataGridResult.Size = new System.Drawing.Size(685, 180);
            this.dataGridResult.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(5, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "n1:";
            // 
            // tbxN1
            // 
            this.tbxN1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxN1.Location = new System.Drawing.Point(9, 24);
            this.tbxN1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbxN1.Multiline = true;
            this.tbxN1.Name = "tbxN1";
            this.tbxN1.Size = new System.Drawing.Size(128, 25);
            this.tbxN1.TabIndex = 2;
            // 
            // tbxN2
            // 
            this.tbxN2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxN2.Location = new System.Drawing.Point(141, 24);
            this.tbxN2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbxN2.Multiline = true;
            this.tbxN2.Name = "tbxN2";
            this.tbxN2.Size = new System.Drawing.Size(128, 25);
            this.tbxN2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(137, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "n2:";
            // 
            // tbxFunc
            // 
            this.tbxFunc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxFunc.Location = new System.Drawing.Point(9, 73);
            this.tbxFunc.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbxFunc.Multiline = true;
            this.tbxFunc.Name = "tbxFunc";
            this.tbxFunc.Size = new System.Drawing.Size(686, 30);
            this.tbxFunc.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(3, 51);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Функция:";
            // 
            // tbxSection
            // 
            this.tbxSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxSection.Location = new System.Drawing.Point(273, 24);
            this.tbxSection.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbxSection.Multiline = true;
            this.tbxSection.Name = "tbxSection";
            this.tbxSection.Size = new System.Drawing.Size(167, 25);
            this.tbxSection.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(269, 2);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(304, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Границы интегрирования ({низ};{верх}):";
            // 
            // tbxResult
            // 
            this.tbxResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxResult.Location = new System.Drawing.Point(9, 272);
            this.tbxResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbxResult.Name = "tbxResult";
            this.tbxResult.Size = new System.Drawing.Size(686, 114);
            this.tbxResult.TabIndex = 9;
            this.tbxResult.Text = "";
            // 
            // btnCreateTable
            // 
            this.btnCreateTable.Location = new System.Drawing.Point(444, 24);
            this.btnCreateTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCreateTable.Name = "btnCreateTable";
            this.btnCreateTable.Size = new System.Drawing.Size(134, 25);
            this.btnCreateTable.TabIndex = 10;
            this.btnCreateTable.Text = "Создать таблицу";
            this.btnCreateTable.UseVisualStyleBackColor = true;
            // 
            // dataGridN
            // 
            this.dataGridN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridN.Location = new System.Drawing.Point(9, 108);
            this.dataGridN.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridN.Name = "dataGridN";
            this.dataGridN.RowHeadersWidth = 70;
            this.dataGridN.RowTemplate.Height = 24;
            this.dataGridN.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dataGridN.Size = new System.Drawing.Size(685, 158);
            this.dataGridN.TabIndex = 11;
            // 
            // tbxTakeTotalResult
            // 
            this.tbxTakeTotalResult.Location = new System.Drawing.Point(9, 597);
            this.tbxTakeTotalResult.Multiline = true;
            this.tbxTakeTotalResult.Name = "tbxTakeTotalResult";
            this.tbxTakeTotalResult.Size = new System.Drawing.Size(276, 30);
            this.tbxTakeTotalResult.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(11, 572);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Итого:";
            // 
            // IntegralForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 639);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxTakeTotalResult);
            this.Controls.Add(this.dataGridN);
            this.Controls.Add(this.btnCreateTable);
            this.Controls.Add(this.tbxResult);
            this.Controls.Add(this.tbxSection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxFunc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxN2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxN1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridResult);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "IntegralForm";
            this.Text = "IntegralForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxN1;
        private System.Windows.Forms.TextBox tbxN2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxFunc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxSection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox tbxResult;
        private System.Windows.Forms.Button btnCreateTable;
        private System.Windows.Forms.DataGridView dataGridN;
        private System.Windows.Forms.TextBox tbxTakeTotalResult;
        private System.Windows.Forms.Label label5;
    }
}