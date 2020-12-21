using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodsV3
{
    public partial class TableDataForm : Form
    {
        private DataTable mainTable;
        private int curWidth = 0;
        private int previousWidth;
        private int lastIndex;
        public TableDataForm(DataTable mainData)
        {
            InitializeComponent();
            lastIndex = mainData.Rows.Count - 1;
            mainTable = mainData;
            SetDataGridsettings();
            
            this.Show();
        }

        private void TableDataForm_SizeChanged(object sender, EventArgs e)
        {
            //dataGrid.Size = new Size(Size.Width - 40, Size.Height - 60);
            SetRows();
            SetColumns();
        }
        private void SetDataGridsettings()
        {
            dataGrid.Font = new Font("Calibri", 14);
            dataGrid.DataSource = mainTable;
            SetRows();
            dataGrid.Columns[0].Width = 50;
            SetColumns();
            dataGrid.Dock = DockStyle.Fill;

            this.SizeChanged += TableDataForm_SizeChanged;
            foreach(DataGridViewColumn t in dataGrid.Columns)
                t.SortMode = DataGridViewColumnSortMode.Programmatic;
            dataGrid.ColumnHeaderMouseClick += DataGrid_ColumnHeaderMouseClick;
        }
        int counter = 0;
        
        private void DataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            while (lastIndex - counter*2 - 1 > 0)
            {
                SortRows(lastIndex - counter);
                counter++;
            }
            counter = 0;
        }
        private void SortRows(int rowindex)
        {
            var tmp = mainTable.Rows[rowindex].ItemArray;
            dataGrid.Rows[rowindex].Dispose();
            dataGrid.Rows[rowindex].SetValues(mainTable.Rows[counter].ItemArray);
            dataGrid.Rows[counter].SetValues(tmp);
        }
        private void SetRows()
        {
            int tmpRowH = dataGrid.Rows[0].Height;
            int tmpH = Height - (tmpRowH * mainTable.Rows.Count);

            if (tmpH >= tmpRowH)
            {
                for (int i = 0; i < tmpH / tmpRowH + 1; i++)
                    mainTable.Rows.Add();
            }
            else 
            {
                tmpH = Math.Abs(tmpH) / tmpRowH;
                var s = mainTable.Rows[mainTable.Rows.Count - 2].ItemArray[0].GetType();
                if (tmpH != 0)
                    for (int i = 0; i < tmpH; i++)
                        if (mainTable.Rows[mainTable.Rows.Count - 2].ItemArray[0] == DBNull.Value)
                            mainTable.Rows.RemoveAt(mainTable.Rows.Count - 2);
            }
        }
        private void SetColumns()
        {
            curWidth = (this.Width / mainTable.Columns.Count +
                (this.Width / mainTable.Columns.Count - 50) / mainTable.Columns.Count + 16);
            for (int i = 1; i < mainTable.Columns.Count; i++)
                dataGrid.Columns[i].Width = curWidth;
            previousWidth = Width;
        }
    }
}
