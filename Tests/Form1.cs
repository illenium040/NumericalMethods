using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.KeyDown += textBox1_KeyDown;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int tableXCount;
                if (int.TryParse(textBox1.Text, out tableXCount))
                {
                    CreateTableView(tableXCount);
                }
            }
        }

        private void CreateTableView(int xes)
        {
            if (dataGridView1.Columns.Count == xes) return;
            else if (dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
            }
            for (int i = 0; i < xes; i++)
                dataGridView1.Columns.Add($"c{i}", $"{i+1}");
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].HeaderCell.Value = "x";
            dataGridView1.Rows[0].Height = 30;
            dataGridView1.Rows[1].HeaderCell.Value = "y";
            dataGridView1.Rows[1].Height = 30;
            dataGridView1.Height = 100;
        }
    }
}
