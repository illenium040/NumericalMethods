using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MethodsV3.Вспомогательные_классы;

namespace MethodsV3.Интерполяция
{
    //оптимизировать работу потоков
    class Interpolation : AuxiliaryClass, IMethod
    {
        Matrix<double> matrix, freeMembers;
        DataGridView grid;
        Control tbxResult, tbxEps, tbxR;
        string resultPolinom;
        string trueFunc;
        double[,] storage, freeY, freeX;
        int columnCount;
        const long timeDelay = 500;

        public static ManualResetEventSlim eventSlimMain = new ManualResetEventSlim(false);
        public static ManualResetEventSlim eventSlimCheck = new ManualResetEventSlim(false);
        public static double max = 0, maxR = 0;
        public static Stopwatch stopwatchGrid1 = new Stopwatch();
        public static Stopwatch stopwatchGrid2 = new Stopwatch();

        public enum InterPolationMethods
        {
            Polinomial,
            Langrage,
            NewtonOne,
            NewtonTwo,
            Square
        }
        public void RunMethod(Enum e)
        {
            if (IsPosiableToRunMethod())
            {
                if (!eventSlimCheck.IsSet)
                    eventSlimCheck.Set();
                max = 0; maxR = 0;
                switch (e)
                {
                    case InterPolationMethods.Polinomial:
                        {
                            PolinomialFunc();
                            break;
                        }
                    case InterPolationMethods.Langrage:
                        {
                            LangrageFunc();
                            break;
                        }
                    case InterPolationMethods.NewtonOne:
                        {
                            NewtonOneFunc();
                            break;
                        }
                    case InterPolationMethods.NewtonTwo:
                        {
                            NewtonTwoFunc();
                            break;
                        }
                    case InterPolationMethods.Square:
                        {
                            SquareFunc();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        public Interpolation(Control tbxFunc, DataGridView grid, Control result, Control tbxEps,
            DataGridView checkGrid, Control tbxR) : base()
        {
            this.tbxR = tbxR;
            this.tbxEps = tbxEps;
            tbxResult = result;
            this.grid = grid;
            tbxFunc.TextChanged += (sender, ev) =>
            {
                CurrentFunction = ConvertInputText(tbxFunc.Text);
                trueFunc = CurrentFunction;
            };
            CountYAsync(this.grid);
            CheckTable(checkGrid);
        }

        async void CountYAsync(DataGridView grid)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    eventSlimMain.Wait();
                    for (int i = 0; i < grid.ColumnCount; i++)
                    {
                        try
                        {
                            if (grid.Rows[0].Cells[i].Value != null)
                                grid.Rows[1].Cells[i].Value = 
                                CalculateByPoint(Convert.ToDouble(grid.Rows[0].Cells[i].Value));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    columnCount = grid.ColumnCount;
                    if (stopwatchGrid1.ElapsedMilliseconds > timeDelay)
                    {
                        eventSlimMain.Reset();
                        stopwatchGrid1.Reset();
                    }
                    Thread.Sleep(50);
                }
            });
        }
        
        async void CheckTable(DataGridView grid)
        {
            double val1, val2, val3;
            await Task.Run(() =>
            {
                while (true)
                {
                    eventSlimCheck.Wait();
                    for (int i = 0; i < grid.ColumnCount; i++)
                    {
                        Thread.Sleep(1);
                        CurrentFunction = resultPolinom;
                        try
                        {
                            double point = Convert.ToDouble(grid.Rows[0].Cells[i].Value);
                            val1 = CalculateByPoint(point);
                            grid.Rows[1].Cells[i].Value = val1;
                            CurrentFunction = trueFunc;
                            val2 = CalculateByPoint(point);
                            grid.Rows[2].Cells[i].Value = val2;
                            grid.Rows[3].Cells[i].Value = Math.Abs(val1 - val2);
                            val3 = Convert.ToDouble(grid.Rows[3].Cells[i].Value);
                            if (max < val3)
                            {
                                max = val3;
                                tbxEps.BeginInvoke((Action)(() => { tbxEps.Text = max.ToString(); }));
                            }
                            CalcR(point);
                            double val4 = 0;
                            if (strR != "")
                                val4 = Math.Abs(CalculateByPoint(strR));
                            if (val4 == 0)
                                maxDeriv = 0;
                            if (maxR < val4)
                            {
                                maxR = val4;
                                tbxR.BeginInvoke((Action)(() => { tbxR.Text = maxR.ToString(); }));
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (stopwatchGrid2.ElapsedMilliseconds > timeDelay)
                    {
                        eventSlimCheck.Reset();
                        stopwatchGrid2.Reset();
                        max = 0; maxR = 0;
                    }
                    Thread.Sleep(50);
                }
            });
        }

        protected override bool IsPosiableToRunMethod()
        {
            try
            {
                eventSlimMain.Reset();
                
                resultPolinom = "";
                max = 0; maxR = 0;
                freeY = new double[columnCount, 1];
                for (int i = 0; i < columnCount; i++)
                    freeY[i, 0] = Convert.ToDouble(grid.Rows[1].Cells[i].Value);
                freeX = new double[columnCount, 1];
                for (int i = 0; i < columnCount; i++)
                    freeX[i, 0] = Convert.ToDouble(grid.Rows[0].Cells[i].Value);
                trueFunc = CurrentFunction;
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private void EpsilonCalc()
        {
            Rq = ""; maxDeriv = 0;
            resultPolinom = resultPolinom.Replace("+ -", "-");
            resultPolinom = resultPolinom.Replace("+-", "-");
            CurrentFunction = ConvertInputText(resultPolinom);
            double max = 0;
            double[,] eps = new double[columnCount, 1];
            for (int i = 0; i < columnCount; i++)
            {
                eps[i, 0] = Math.Abs(freeY[i, 0] - CalculateByPoint(Convert.ToDouble(grid.Rows[0].Cells[i].Value)));
                if (eps[i, 0] > max)
                    max = eps[i, 0];
            }
            tbxEps.Text = max.ToString();
            tbxResult.Text = resultPolinom;
            for (int i = 0; i < columnCount - 1; i++)
                Rq += $"(x-{freeX[i, 0]})*";
            Rq += $"(x-{freeX[columnCount - 1, 0]})/({Factorial(columnCount)})";
            
            eventSlimMain.Set();
        }
        double maxDeriv = 0;
        string strR = "";
        string Rq;
        private void CalcR(double point)
        {
            if (eventSlimCheck.IsSet)
            {
                double deriv = Math.Abs(Derivative(point, columnCount));
                if (maxDeriv < deriv)
                {
                    maxDeriv = deriv;
                    strR = $"{maxDeriv}*" + Rq.Replace("x", point.ToString());
                }
            }
            else
            {
                strR = "";
            }
        }
        private void PolinomialFunc()
        {
            storage = new double[columnCount, columnCount];
            for (int i = 0; i < columnCount; i++)
                for (int j = 0; j < 1; j++)
                    storage[i, j] = 1;
            for (int i = 0; i < columnCount; i++)
                for (int j = 1; j < columnCount; j++)
                    storage[i, j] = Math.Pow(Convert.ToDouble(grid.Rows[0].Cells[i].Value), j);
                matrix = Matrix<double>.Build.DenseOfArray(storage);

                if (matrix.Determinant() == 0) return;
                var inversedMatrix = matrix.Inverse();

                freeMembers = inversedMatrix.Multiply(Matrix<double>.Build.DenseOfArray(freeY));
                resultPolinom = $"{freeMembers[0, 0]} + {freeMembers[1, 0]}*x + ";

                for (int i = 2; i < columnCount - 1; i++)
                    resultPolinom += $"{freeMembers[i, 0]}*x^{i} + ";

                resultPolinom += $"{freeMembers[columnCount - 1, 0]}*x^{columnCount - 1}";
            EpsilonCalc();
        }
        private void LangrageFunc()
        {
            for (int y = 0; y < columnCount; y++)
            {
                resultPolinom += $"{freeY[y, 0]}";
                string tmp = "";
                for (int x = 0; x < columnCount; x++)
                {
                    if (y == x) continue;
                    resultPolinom += $"*(x-{freeX[x, 0]})";
                    tmp += $"({freeX[y, 0]-freeX[x, 0]})*";
                }
                tmp = tmp.Remove(tmp.Length - 1);
                resultPolinom += "/" + CalculateByPoint(tmp).ToString();
                resultPolinom += "+";
            }
            resultPolinom = resultPolinom.Remove(resultPolinom.Length - 1);

            EpsilonCalc();
        }
        double[][] dy;
        double stepOfInterpolation;
        private void NewtonOneFunc()
        {
            CreateDy();
            
            
            resultPolinom = $"{freeY[0, 0]}";
            for (int i = 1; i < columnCount; i++)
            {
                resultPolinom += $"+{dy[i][0]}/({Factorial(i)}*{stepOfInterpolation}^{i})*";
                for (int j = 0; j < i; j++)
                    resultPolinom += $"(x-{freeX[j, 0]})*";
            }
            resultPolinom = resultPolinom.Remove(resultPolinom.Length - 1);
            resultPolinom = resultPolinom.Replace("*+", "+");

            EpsilonCalc();
        }
        private void NewtonTwoFunc()
        {
            CreateDy();
            resultPolinom = $"{freeY[columnCount-1, 0]}";
            for (int i = 1; i < columnCount; i++)
            {
                resultPolinom += $"+{dy[i][dy[i].Length - 1]}/({Factorial(i)}*{stepOfInterpolation}^{i})*";
                for (int j = 0; j < columnCount - dy[i].Length; j++)
                    resultPolinom += $"(x-{freeX[columnCount - 1 - j, 0]})*";
            }
            resultPolinom = resultPolinom.Remove(resultPolinom.Length - 1);
            resultPolinom = resultPolinom.Replace("*+", "+");

            EpsilonCalc();
        }
        private void CreateDy()
        {
            dy = new double[columnCount][];

            dy[0] = new double[columnCount];
            for (int i = 0; i < freeY.Length; i++)
                dy[0][i] = freeY[i, 0];
            
            for (int i = 1; i < columnCount; i++)
            {
                dy[i] = new double[freeY.Length - i];
                for (int j = 0; j < dy[i].Length; j++)
                    dy[i][j] = dy[i - 1][j + 1] - dy[i - 1][j];
            }
            if (freeX.Length > 1)
                stepOfInterpolation = freeX[1, 0] - freeX[0, 0];
            else stepOfInterpolation = 1;
        }

        private void SquareFunc()
        {
            double my = 0;
            double[] mx = new double[(columnCount - 1) * 2];
            double[] myx = new double[columnCount - 1];
            double[,] mMatrix = new double[columnCount, columnCount];
            double[,] myMatrix = new double[columnCount, 1];
            for (int i = 0; i < columnCount; i++)
                my += freeY[i, 0];
            for (int i = 0; i < mx.Length; i++)
                for (int j = 0; j < freeX.Length; j++)
                    mx[i] += Math.Pow(freeX[j, 0], i + 1);
            for (int i = 0; i < columnCount - 1; i++)
                for (int j = 0; j < columnCount; j++)
                    myx[i] += freeY[j, 0] * Math.Pow(freeX[j, 0], i + 1);

            for (int i = 0; i < columnCount - 1; i++)
                mMatrix[0, i] = mx[(columnCount - 2) - i];
            mMatrix[0, columnCount - 1] = columnCount;

            for (int i = 1; i < columnCount - 1; i++)
                for (int j = 0; j < columnCount; j++)
                    mMatrix[i, j] = mx[columnCount - 2 + i - j];

            for (int i = 0; i < columnCount; i++)
                mMatrix[columnCount - 1, i] = mx[2 * (columnCount - 1) - 1 - i];

            myMatrix[0,0] = my;
            for (int i = 1; i < columnCount; i++)
                myMatrix[i,0] = myx[i-1];

            Matrix<double> matrixOfA = Matrix<double>.Build.DenseOfArray(mMatrix);

            var mm = (matrixOfA.Transpose().Multiply(matrixOfA)).Inverse();
            var mmr = mm.Multiply(matrixOfA.Transpose().Multiply(Matrix<double>.Build.DenseOfArray(myMatrix)));

            for (int i = 0, pow = columnCount-1; i < columnCount-2; i++, pow--)
                resultPolinom += $"{mmr[i, 0]}*x^{pow}+";
            resultPolinom += $"{mmr[columnCount - 2, 0]}*x+{mmr[columnCount-1, 0]}";

            EpsilonCalc();
        }

        private int Factorial(int count)
        {
            int factorial = 1;
            for (int i = count; i > 0; i--)
                factorial *= i;
            return factorial;
        }
    }
}
