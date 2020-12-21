namespace MethodsV3.Производная
{
    partial class DerivativeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbxh = new System.Windows.Forms.TextBox();
            this.tbxSection = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxFunc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxPolinomResult = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridY = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbxR = new System.Windows.Forms.TextBox();
            this.tbxDerResult = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridY)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Шаг:";
            // 
            // tbxh
            // 
            this.tbxh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxh.Location = new System.Drawing.Point(12, 32);
            this.tbxh.Multiline = true;
            this.tbxh.Name = "tbxh";
            this.tbxh.Size = new System.Drawing.Size(100, 30);
            this.tbxh.TabIndex = 1;
            // 
            // tbxSection
            // 
            this.tbxSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxSection.Location = new System.Drawing.Point(118, 32);
            this.tbxSection.Multiline = true;
            this.tbxSection.Name = "tbxSection";
            this.tbxSection.Size = new System.Drawing.Size(100, 30);
            this.tbxSection.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(118, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "(x0;b):";
            // 
            // tbxFunc
            // 
            this.tbxFunc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxFunc.Location = new System.Drawing.Point(224, 32);
            this.tbxFunc.Multiline = true;
            this.tbxFunc.Name = "tbxFunc";
            this.tbxFunc.Size = new System.Drawing.Size(263, 30);
            this.tbxFunc.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(224, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Функция:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(12, 340);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Полином производной:";
            // 
            // tbxPolinomResult
            // 
            this.tbxPolinomResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxPolinomResult.Location = new System.Drawing.Point(11, 363);
            this.tbxPolinomResult.Name = "tbxPolinomResult";
            this.tbxPolinomResult.Size = new System.Drawing.Size(658, 96);
            this.tbxPolinomResult.TabIndex = 7;
            this.tbxPolinomResult.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(12, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Таблица Δy:";
            // 
            // dataGridY
            // 
            this.dataGridY.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridY.Location = new System.Drawing.Point(12, 88);
            this.dataGridY.Name = "dataGridY";
            this.dataGridY.RowHeadersWidth = 70;
            this.dataGridY.Size = new System.Drawing.Size(657, 213);
            this.dataGridY.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(12, 519);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(272, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Значение производной в точке x0:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(12, 463);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(212, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "Абсолютная погрешность:";
            // 
            // tbxR
            // 
            this.tbxR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxR.Location = new System.Drawing.Point(11, 486);
            this.tbxR.Multiline = true;
            this.tbxR.Name = "tbxR";
            this.tbxR.Size = new System.Drawing.Size(272, 30);
            this.tbxR.TabIndex = 12;
            // 
            // tbxDerResult
            // 
            this.tbxDerResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxDerResult.Location = new System.Drawing.Point(11, 542);
            this.tbxDerResult.Multiline = true;
            this.tbxDerResult.Name = "tbxDerResult";
            this.tbxDerResult.Size = new System.Drawing.Size(272, 30);
            this.tbxDerResult.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(11, 307);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 14;
            this.button1.Text = "Ньютон(1)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(493, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(176, 30);
            this.button2.TabIndex = 15;
            this.button2.Text = "Создать таблицу";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DerivativeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 616);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbxDerResult);
            this.Controls.Add(this.tbxR);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataGridY);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxPolinomResult);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxFunc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxSection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxh);
            this.Controls.Add(this.label1);
            this.Name = "DerivativeForm";
            this.Text = "DerivativeForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxh;
        private System.Windows.Forms.TextBox tbxSection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxFunc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox tbxPolinomResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbxR;
        private System.Windows.Forms.TextBox tbxDerResult;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}