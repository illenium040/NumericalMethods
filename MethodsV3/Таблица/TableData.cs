using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsV3
{
    class TableData : IDisposable
    {

        private TableDataForm tdForm;
        private bool isCreated;
        private bool isDisposed = false;
        private DataTable mainDataTable;
        private int customCounter = 1;

        public TableData(int headersCount)
        {
            mainDataTable = new DataTable();
            CreateColumns(headersCount);
            isDisposed = false;
            isCreated = false;
        }
        public TableData(int headersCount, int deltasCount) 
        {
            mainDataTable = new DataTable();
            CreateColumns(headersCount, deltasCount);
            isDisposed = false;
            isCreated = false;
        }
        public void CreateTable()
        {
            tdForm = new TableDataForm(mainDataTable);
        }
        private void CreateColumns(int headersCount)
        {
            if (!isCreated)
            {
                mainDataTable.Columns.Add("№");
                if (headersCount == 1)
                    mainDataTable.Columns.Add("x");
                else
                {
                    for (int i = 1; i < headersCount; i++)
                        mainDataTable.Columns.Add($"x{i}");
                }
                mainDataTable.Columns.Add("delta");
                isCreated = true;
            }
        }
        private void CreateColumns(int xes, int deltasCount)
        {
            if (!isCreated)
            {
                mainDataTable.Columns.Add("№");
                if (xes == 1)
                    mainDataTable.Columns.Add("x");
                else
                {
                    for (int i = 1; i < xes; i++)
                        mainDataTable.Columns.Add($"x{i}");
                }
                for (int i = 0; i < deltasCount; i++)
                    mainDataTable.Columns.Add($"delta{i+1}");
                isCreated = true;
            }
        }
        public void AddToTable(double delta, params double[] xn)
        {
            mainDataTable.Rows.Add();
            mainDataTable.Rows[customCounter - 1][0] = customCounter;
            for (int i = 0; i < xn.Count(); i++)
                mainDataTable.Rows[customCounter - 1][i+1] = xn[i];
                mainDataTable.Rows[customCounter - 1][xn.Count()+1] = delta;
            customCounter++;
        }
        public void AddToTable(double[] delta, params double[] xn)
        {
            mainDataTable.Rows.Add();
            mainDataTable.Rows[customCounter - 1][0] = customCounter;
            for (int i = 0; i < xn.Count(); i++)
                mainDataTable.Rows[customCounter - 1][i + 1] = xn[i];
            for (int i = 0; i < delta.Count(); i++)
                mainDataTable.Rows[customCounter - 1][xn.Count() + i+1] = delta[i];
            customCounter++;
        }
        public void AddToTable(double delta, Matrix<double> matrix)
        {
            mainDataTable.Rows.Add();
            mainDataTable.Rows[customCounter - 1][0] = customCounter;
            for (int i = 0; i < matrix.RowCount; i++)
                for (int j = 0; j < matrix.ColumnCount; j++)
                    mainDataTable.Rows[customCounter - 1][i+1] = matrix[i, j];
            mainDataTable.Rows[customCounter - 1][matrix.RowCount + 1] = delta;
            customCounter++;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if(!isDisposed)
            {
                if(disposing)
                {

                }
            }
        }
        ~TableData()
        {
            Dispose(false);
        }
    }
}
