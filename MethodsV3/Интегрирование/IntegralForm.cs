using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MethodsV3.Вспомогательные_классы;

namespace MethodsV3.Интегрирование
{
    public partial class IntegralForm : Form
    {
        Integral integral;
        double h = 0; int n1 = 0, n2 = 0;
        (double, double) Section;
        public IntegralForm()
        {
            InitializeComponent();
            integral = new Integral(tbxFunc, tbxResult,
                dataGridResult, dataGridN);
            dataGridN.DoubleBuffered(true);
            dataGridResult.DoubleBuffered(true);
            for (int i = 0; i < 6; i++)
            {
                dataGridResult.Columns.Add($"column{i + 1}", "");
                dataGridResult.Columns[i].Width = 120;
                dataGridResult.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridResult.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridResult.Rows.Add(5);

            dataGridResult.Rows[0].HeaderCell.Value = "I(л.п.)";
            dataGridResult.Rows[1].HeaderCell.Value = "I(п.п.)";
            dataGridResult.Rows[2].HeaderCell.Value = "I(с.п.)";
            dataGridResult.Rows[3].HeaderCell.Value = "I(тр)";
            dataGridResult.Rows[4].HeaderCell.Value = "I(с)";
            dataGridResult.Columns[0].HeaderCell.Value = "n1";
            dataGridResult.Columns[1].HeaderCell.Value = "n2";
            dataGridResult.Columns[2].HeaderCell.Value = "Двойной пересчет";
            dataGridResult.Columns[3].HeaderCell.Value = "Строгая погрешность";
            dataGridResult.Columns[4].HeaderCell.Value = "Лучшая погрешность";
            dataGridResult.Columns[5].HeaderCell.Value = "Интервал";

            btnCreateTable.Click += BtnCreateTable_Click;
        }

        private void BtnCreateTable_Click(object sender, EventArgs e)
        {
            Section = AuxiliaryClass.ConvertSectionAsPublic(tbxSection.Text);
            if (double.IsNaN(Section.Item1) && double.IsNaN(Section.Item2))
            {
                MessageBox.Show("Неверные границы интегрирования");
                return;
            }
            if(int.TryParse(tbxN1.Text, out n1) && int.TryParse(tbxN2.Text, out n2))
            {
                double part = Section.Item2 - Section.Item1;
                if (n1 > n2)
                {
                    h = (Section.Item2 - Section.Item1) / n1;
                    CreateTableN(n1);
                    if (integral.FillTableN(n2, n1, Section, h))
                    {
                        integral.FillTableResult(part / n2, part / n1);
                        integral.FillEpsilon(Section, n1, tbxTakeTotalResult);
                    }
                }
                else
                {
                    h = (Section.Item2 - Section.Item1) / n2;
                    CreateTableN(n2);
                    if (integral.FillTableN(n1, n2, Section, h))
                    {
                        integral.FillTableResult(part / n1, part / n2);
                        integral.FillEpsilon(Section, n2, tbxTakeTotalResult);
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Введите целое число");
            }
        }
        bool isCreated = false;
        private void CreateTableN(int biggerNum)
        {
            if(isCreated)
            {
                dataGridN.Rows.Clear();
                dataGridN.Columns.Clear();
                isCreated = false;
            }
            dataGridN.Columns.Add("-1", "");
            dataGridN.Rows.Add(4);
            dataGridN.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridN.Rows[0].HeaderCell.Value = "n1";
            dataGridN.Rows[1].HeaderCell.Value = "n2";
            dataGridN.Rows[2].HeaderCell.Value = "x";
            dataGridN.Rows[3].HeaderCell.Value = "y";
            for (int i = 0; i < biggerNum; i++)
                dataGridN.Columns.Add($"{i}", "");
            isCreated = true;
        }
    }
}
