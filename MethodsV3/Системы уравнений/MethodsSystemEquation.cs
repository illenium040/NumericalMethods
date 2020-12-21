using System;
using MathNet.Numerics.LinearAlgebra;
using System.Windows.Forms;
using MethodsV3.Вспомогательные_классы;

namespace MethodsV3.Системы_уравнений
{
    class MethodsSystemEquation : AuxiliaryClass, IMethod
    {
        private double[,] doubleMatrix, qMatrix;
        private bool[] setZeroesMatrix;
        private int columnsRowsCount;
        private Matrix<double> mainMatrix, subMatrixX0, constMatrix;
        private double max = 0, qMultiplier = 0, qMax = 0;

        public enum EMethodsSystem
        {
            SimpleIterations,
            Zeidel
        }

        public MethodsSystemEquation(string epsilon, int xcount, string[,] stringMatrix)
        {
            columnsRowsCount = xcount;
            doubleMatrix = new double[columnsRowsCount + 1, columnsRowsCount];
            if(!ConvertCurrentStringMatrix(stringMatrix))
                return;
            Epsilon = ConvertEpsilon(epsilon);
            if (double.IsNaN(Epsilon))
                return;
            tableData = new TableData(columnsRowsCount+1);
            qMatrix = new double[columnsRowsCount, 1];
            setZeroesMatrix = new bool[columnsRowsCount];
        }

        private bool ConvertCurrentStringMatrix(string[,] curMatrix)
        {
            try
            {
                for (int i = 0; i < columnsRowsCount + 1; i++)
                    for (int j = 0; j < columnsRowsCount; j++)
                        doubleMatrix[i, j] = double.Parse(curMatrix[i, j]);
                return true;
            }
            catch
            {
                MessageBox.Show("Проверьте данные");
                return false;
            }
        }

        protected override bool IsPosiableToRunMethod()
        {
            
            mainMatrix = Matrix<double>.Build.DenseOfArray(doubleMatrix);
            subMatrixX0 = mainMatrix.SubMatrix(mainMatrix.RowCount - 1, 1, 0, mainMatrix.ColumnCount);
            subMatrixX0 = subMatrixX0.Transpose();
            mainMatrix = mainMatrix.RemoveRow(mainMatrix.ColumnCount);
            
            for (int i = 0; i < mainMatrix.RowCount; i++)
            {
                for (int j = 0; j < mainMatrix.ColumnCount; j++)
                {
                    if (Math.Abs(mainMatrix[i, j]) > Math.Abs(max) && setZeroesMatrix[j] == false)
                        max = mainMatrix[i, j];
                }
                for (int j = 0; j < mainMatrix.ColumnCount; j++)
                {
                    mainMatrix[i, j] = mainMatrix[i, j] / -max;
                    if(mainMatrix[i, j] == -1)
                    {
                        mainMatrix[i, j] = 0;
                        setZeroesMatrix[j] = true;
                    }
                    qMatrix[i, 0] += Math.Abs(mainMatrix[i, j]);
                    if (qMax < qMatrix[i, 0])
                        qMax = qMatrix[i, 0];
                }
                subMatrixX0[i, 0] = subMatrixX0[i, 0] / max;
                max = 0;
            }
            
            for (int i = 0; i < mainMatrix.RowCount; i++)
            {
                for (int j = 0; j < mainMatrix.ColumnCount; j++)
                {
                    if (mainMatrix[i, j] == 0 && i!=j)
                    {
                        var tmp = mainMatrix.Row(i);
                        mainMatrix = mainMatrix.RemoveRow(i);
                        mainMatrix = mainMatrix.InsertRow(j, tmp);
                        tmp = subMatrixX0.Row(i);
                        subMatrixX0 = subMatrixX0.RemoveRow(i);
                        subMatrixX0 = subMatrixX0.InsertRow(j, tmp);
                        i = 0;
                    }
                }
            }
            qMultiplier = qMax / (1 - qMax);
            if (qMax > 1)
                return false;
            else
            {
                constMatrix = subMatrixX0.Clone();
                return true;
            }
            
        }
        public void RunMethod(Enum e)
        {
            if (tableData == null) return;
            if (IsPosiableToRunMethod())
            {
                switch (e)
                {
                    case EMethodsSystem.SimpleIterations:
                        {
                            SimpleIteartions();
                            break;
                        }
                    case EMethodsSystem.Zeidel:
                        {
                            Zeidel();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        private int counter;
        private void SimpleIteartions()
        {
            counter = 0;
            max = 0;
            do
            {
                var tmpMatrix = mainMatrix.Multiply(subMatrixX0) + constMatrix;
                if (counter == 0)
                {
                    for (int i = 0; i < tmpMatrix.RowCount; i++)
                        for (int j = 0; j < tmpMatrix.ColumnCount; j++)
                        {

                            var temp = Math.Abs(tmpMatrix[i, j] - subMatrixX0[i, j]);
                            if (max < temp)
                                max = temp;
                        }
                }
                subMatrixX0 = tmpMatrix;
                qMultiplier = Math.Pow(qMax, counter += 1) / (1 - qMax);
                tableData.AddToTable(qMultiplier * max, tmpMatrix);
            } while (Epsilon < qMultiplier * max);
            tableData.CreateTable();
        }
        private void Zeidel()
        {
            counter = 0;
            max = 0;
            do
            {
                double tmpX = 0;
                for (int i = 0; i < mainMatrix.RowCount; i++)
                {
                    for (int j = 0; j < mainMatrix.ColumnCount; j++)
                        tmpX += mainMatrix[i, j] * subMatrixX0[j, 0];

                    tmpX += constMatrix[i, 0];
                    if (counter == 0)
                    {
                        var temp = Math.Abs(subMatrixX0[i, 0] - tmpX);
                        if (max < temp)
                            max = temp;
                    }
                    subMatrixX0[i, 0] = tmpX;
                    tmpX = 0;
                }
                qMultiplier = Math.Pow(qMax, counter += 1) / (1 - qMax);
                tableData.AddToTable(qMultiplier * max, subMatrixX0);
            } while (Epsilon < qMultiplier * max);
            tableData.CreateTable();
        }
        //private void Zeidel()
        //{
        //    do
        //    {
        //        max = 0;
        //        double tmpX = 0;
        //        for (int i = 0; i < mainMatrix.RowCount; i++)
        //        {
        //            for (int j = 0; j < mainMatrix.ColumnCount; j++)
        //                tmpX += mainMatrix[i, j] * subMatrixX0[j, 0];

        //            tmpX += constMatrix[i, 0];
        //            var temp = Math.Abs(subMatrixX0[i, 0] - tmpX);
        //            if (max < temp)
        //                max = temp;
        //            subMatrixX0[i, 0] = tmpX;
        //            tmpX = 0;
        //        }
        //        tableData.AddToTable(qMultiplier * max, subMatrixX0);
        //    } while (Epsilon < qMultiplier * max);
        //    tableData.CreateTable();
        //}
    }
}
