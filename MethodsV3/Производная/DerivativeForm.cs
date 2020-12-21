using MethodsV3.Вспомогательные_классы;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace MethodsV3.Производная
{
    public partial class DerivativeForm : Form
    {
        Derivative ownDerivative;
        private double stepH = 0;
        private (double, double) Section;
        public DerivativeForm()
        {
            InitializeComponent();
            ownDerivative = new Derivative(tbxFunc, dataGridY, tbxPolinomResult, tbxR, tbxDerResult);
            tbxh.TextChanged += Tbxh_TextChanged;
            tbxSection.TextChanged += TbxSection_TextChanged;
            
            dataGridY.DoubleBuffered(true);
        }

        private void TbxSection_TextChanged(object sender, EventArgs e)
        {
            if (tbxSection.Text.Contains("\r\n"))
            {
                tbxSection.Text = tbxSection.Text.Replace("\r\n", "");
                tbxSection.Select(tbxSection.Text.Length, 0);
            }
        }

        private void Tbxh_TextChanged(object sender, EventArgs e)
        {
            if (tbxh.Text.Contains("\r\n"))
            {
                tbxh.Text = tbxh.Text.Replace("\r\n", "");
                tbxh.Select(tbxh.Text.Length, 0);
            }
        }

        private async void CreateTableView()
        { 
            if(Derivative.eventSlimGrid.IsSet)
                Derivative.eventSlimGrid.Reset();
            button1.Enabled = false;
            button2.Enabled = false;
            await Task.Run(() =>
            {
                
                int size = (int)((Section.Item2 - Section.Item1) / stepH) + 2;
                if (size > 655) size = 650;
                if (dataGridY.Columns.Count == size)
                {
                    button2.Invoke((Action)(() => { button2.Enabled = true; }));
                    return;
                }
                else if (dataGridY.Columns.Count != 0)
                {
                    dataGridY.Invoke((Action)(() =>
                    {
                        dataGridY.Rows.Clear();
                        dataGridY.Columns.Clear();
                    }));
                }

                dataGridY.Invoke((Action)(() =>
                {
                    dataGridY.Columns.Add($"c{0}", "x");
                    dataGridY.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridY.Columns[0].Width = 150;
                }));
                int curSize = 1;
                bool isBusy = false;
                while (curSize < size)
                {
                    if (!isBusy)
                    {
                        isBusy = true;
                        dataGridY.BeginInvoke((Action)(() =>
                        {
                            dataGridY.Columns.Add($"c{curSize}", $"Dy{curSize - 1}");
                            dataGridY.Columns[curSize].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dataGridY.Columns[curSize].Width = 150;
                            dataGridY.Rows.Add();
                            dataGridY.Rows[curSize - 1].HeaderCell.Value = $"{curSize - 1}";
                            dataGridY.Rows[curSize - 1].Height = 30;
                            curSize++;
                            isBusy = false;
                        }));
                    }
                    Thread.Sleep(10);
                }
                for (double i = 0, step = 0; i < size - 1; i += 1, step += stepH)
                    dataGridY[0, (int)i].Value = Section.Item1 + step;
                Derivative.lastChanged = true;
                Derivative.eventSlimGrid.Set();
                Derivative.stopwatchGrid.Restart();
                button1.Invoke((Action)(() => { button1.Enabled = true; }));
                button2.Invoke((Action)(() => { button2.Enabled = true; }));         
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ownDerivative.RunMethod(Derivative.DiribativeMethods.FirstNewton);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Section = AuxiliaryClass.ConvertSectionAsPublic(tbxSection.Text);
            if (Section.Item1 == 0.0001)
                Section.Item1 = 0;
            if (double.TryParse(tbxh.Text, out stepH) && !double.IsNaN(Section.Item1) && !double.IsNaN(Section.Item2))
            {
                if (stepH > 0)
                    CreateTableView();
            }
            else
            {
                MessageBox.Show("Проверьте данные");
            }

        }
    }
}
