using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using MethodsV3.Вспомогательные_классы;

namespace MethodsV3.Интерполяция
{
    public partial class InterpolationForm : Form
    {
        Interpolation interpolation;
        private (double, double) Section = (double.NaN, double.NaN);
        double step1 = 0, step2 = 0;
        public InterpolationForm()
        {
            InitializeComponent();
            interpolation = new Interpolation(tbxFunc, dataGridView1, tbxResult, tbxEps,
                dataGridView2, tbxR);
            dataGridView1.Scroll += (sender, ev) =>
            { dataGridView1.Refresh(); };
            
            tbxFunc.TextChanged += tbxFunc_TextChanged;
            tbxh.TextChanged += tbxh_TextChanged;
            tbxSection.TextChanged += TbxSection_TextChanged;
            tbxh2.TextChanged += tbxh2_TextChanged;

            dataGridView1.DoubleBuffered(true);
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView2.DoubleBuffered(true);
            dataGridView2.CellValueChanged += DataGridView2_CellValueChanged;
        }

        private void TbxSection_TextChanged(object sender, EventArgs e)
        {
            if (tbxSection.Text.Contains("\r\n"))
            {
                tbxSection.Text = tbxSection.Text.Replace("\r\n", "");
                tbxSection.Select(tbxSection.Text.Length, 0);
            }
            try
            {
                Section = AuxiliaryClass.ConvertSectionAsPublic(tbxSection.Text);
                if (step1 > 0 && !double.IsNaN(Section.Item1) && !double.IsNaN(Section.Item2))
                    CreateTableView();
            }
            catch { };
        }

        private void tbxh2_TextChanged(object sender, EventArgs e)
        {
            if (tbxh2.Text.Contains("\r\n"))
            {
                tbxh2.Text = tbxh2.Text.Replace("\r\n", "");
                tbxh2.Select(tbxh2.Text.Length, 0);
            }
            if (double.TryParse(tbxh2.Text, out step2) && !double.IsNaN(Section.Item1) && !double.IsNaN(Section.Item2))
                if (step2 > 0)
                    CreateTableViewCheck();
        }

        private void tbxh_TextChanged(object sender, EventArgs e)
        {
            if (tbxh.Text.Contains("\r\n"))
            {
                tbxh.Text = tbxh.Text.Replace("\r\n", "");
                tbxh.Select(tbxh.Text.Length, 0);
            }
            if (double.TryParse(tbxh.Text, out step1) && !double.IsNaN(Section.Item1) && !double.IsNaN(Section.Item2))
                if(step1 > 0)
                    CreateTableView();
        }

        private void tbxFunc_TextChanged(object sender, EventArgs e)
        { 
            Interpolation.max = 0;
            Interpolation.maxR = 0;
            
            if(!Interpolation.eventSlimMain.IsSet)
                Interpolation.eventSlimMain.Set();
            if (!Interpolation.eventSlimCheck.IsSet)
                Interpolation.eventSlimCheck.Set();
            if (tbxFunc.Text.Contains("\r\n"))
            {
                tbxFunc.Text = tbxFunc.Text.Replace("\r\n", "");
                tbxFunc.Select(tbxFunc.Text.Length, 0);
            }
        }

        private void CreateTableView()
        {
            Interpolation.eventSlimMain.Reset();
            int xes = (int)((Section.Item2 - Section.Item1) / step1) + 1;
            if (dataGridView1.Columns.Count == xes) return;
            else if (dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
            }
            for (int i = 0; i < xes; i++)
            {
                dataGridView1.Columns.Add($"c{i}", $"{i + 1}");
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].Width = 150;
            }

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].HeaderCell.Value = "x";
            dataGridView1.Rows[0].Height = 30;
            dataGridView1.Rows[1].HeaderCell.Value = "y";
            dataGridView1.Rows[1].Height = 30;
            dataGridView1.Height = 100;
            Interpolation.eventSlimMain.Set();
            int pos = 0;
            for (double i = Section.Item1; i <= Section.Item2; i += step1)
            {
                dataGridView1[pos, 0].Value = i;
                pos++;
            }
            
        }
        private void CreateTableViewCheck()
        {
            Interpolation.eventSlimCheck.Reset();
            int count = (int)((Section.Item2 - Section.Item1) / step2) + 1;
            if (dataGridView2.Columns.Count == count) return;
            else if (dataGridView2.Columns.Count != 0)
            {
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
            }
            for (int i = 0; i < count; i++)
            {
                dataGridView2.Columns.Add($"c{i}", $"{i + 1}");
                dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[i].Width = 150;
            }
            dataGridView2.Rows.Add(3);

            dataGridView2.Rows[0].HeaderCell.Value = "x";
            dataGridView2.Rows[0].Height = 30;
            dataGridView2.Rows[1].HeaderCell.Value = "P(x)";
            dataGridView2.Rows[1].Height = 30;
            dataGridView2.Rows[2].HeaderCell.Value = "f(x)"; 
            dataGridView2.Rows[2].Height = 30;
            dataGridView2.Rows[3].HeaderCell.Value = "Δ";
            dataGridView2.Rows[3].Height = 30;
            Interpolation.eventSlimCheck.Set();
            int pos = 0;
            for (double i = Section.Item1; i < Section.Item2; i += step2)
            {
                dataGridView2[pos, 0].Value = i;
                pos++;
            }   
        }

        private void DataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!Interpolation.eventSlimCheck.IsSet)
                Interpolation.eventSlimCheck.Set();
            Interpolation.stopwatchGrid2.Restart();
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(!Interpolation.eventSlimMain.IsSet)
                Interpolation.eventSlimMain.Set();
            Interpolation.stopwatchGrid1.Restart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            interpolation.RunMethod(Interpolation.InterPolationMethods.Polinomial);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            interpolation.RunMethod(Interpolation.InterPolationMethods.Langrage);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            interpolation.RunMethod(Interpolation.InterPolationMethods.NewtonOne);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            interpolation.RunMethod(Interpolation.InterPolationMethods.NewtonTwo);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            interpolation.RunMethod(Interpolation.InterPolationMethods.Square);
        }
    }
}
