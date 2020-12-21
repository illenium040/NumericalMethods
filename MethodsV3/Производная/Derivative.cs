using MethodsV3.Вспомогательные_классы;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodsV3.Производная
{
    class Derivative : AuxiliaryClass,  IMethod
    {
        public enum DiribativeMethods
        {
            FirstNewton
        }
        public void RunMethod(Enum e)
        {
            if(IsPosiableToRunMethod())
            {
                tbxPolinomResult.Text = "";
                tbxDerResult.Text = "";
                tbxR.Text = "";
                switch (e)
                {
                    case DiribativeMethods.FirstNewton:
                        {
                            FirstNewtonDerivative();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        protected override bool IsPosiableToRunMethod()
        {
            eventSlimGrid.Reset();
            return true;
        }

        public static Stopwatch stopwatchGrid = new Stopwatch();
        public static ManualResetEventSlim eventSlimGrid = new ManualResetEventSlim();
        public static bool lastChanged = false;
        private Control tbxPolinomResult, tbxR, tbxDerResult;
        private DataGridView dataGrid;
        public Derivative(
                Control tbxFunc, DataGridView dataGridY,
                Control tbxPolinomResult, Control tbxR, Control tbxDerResult)
        {
            this.tbxDerResult = tbxDerResult;
            this.tbxR = tbxR;
            this.tbxPolinomResult = tbxPolinomResult;
            dataGrid = dataGridY;
            tbxFunc.TextChanged += (sender, ev) =>
            {
                CurrentFunction = ConvertInputText(tbxFunc.Text);
                stopwatchGrid.Restart();
                lastChanged = true;
                eventSlimGrid.Set();
            };
            dataGridY.CellValueChanged += (sender, ev) =>
            {
                stopwatchGrid.Restart();
            };
            CountYGridAsync();
        }
        
        async void CountYGridAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1);
                    eventSlimGrid.Wait();
                    if (dataGrid.ColumnCount == 0) continue;
                    if (lastChanged)
                    {
                        lastChanged = false;
                        for (int i = 0; i < dataGrid.RowCount; i++)
                        {
                            try
                            {
                                dataGrid[1, i].Value = CalculateByPoint((double)dataGrid[0, i].Value);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        for (int i = 1; i < dataGrid.ColumnCount - 1; i++)
                        {
                            for (int j = 0; j < dataGrid.RowCount - 1; j++)
                            {
                                try
                                {
                                    dataGrid[i + 1, j].Value = Math.Abs(
                                    (double)dataGrid[i, j + 1].Value - (double)dataGrid[i, j].Value);
                                }
                                catch
                                {
                                    continue;
                                }

                            }
                        }
                    }
                    else if (stopwatchGrid.Elapsed.TotalMilliseconds > 500)
                    {
                        eventSlimGrid.Reset();
                        stopwatchGrid.Reset();
                    }
                    Thread.Sleep(10);
                }
            });
        }

        private void FirstNewtonDerivative()
        {
            try
            {
                string polinomres = "";
                tbxPolinomResult.Text += $"(({Math.Round((double)dataGrid[2, 0].Value, 4)})";
                polinomres += $"(({(double)dataGrid[2, 0].Value})";
                for (int i = 3; i < dataGrid.ColumnCount - 1; i++)
                {
                    var curValue = (double)dataGrid[i, 0].Value;
                    if (curValue != 0)
                    {
                        if (i % 2 == 0)
                        {
                            tbxPolinomResult.Text += $"+({Math.Round(curValue, 6)}/{i - 1})";
                            polinomres += $"+({curValue}/{i - 1})";
                        }
                        else
                        {
                            tbxPolinomResult.Text += $"-({Math.Round(curValue, 6)}/{i - 1})";
                            polinomres += $"-({curValue}/{i - 1})";
                        }
                    }
                }
                double h = (double)dataGrid[0, 2].Value - (double)dataGrid[0, 1].Value;
                tbxPolinomResult.Text += $")*(1/{h})";
                tbxDerResult.Text = CalculateByPoint(polinomres + $")*{1/h}").ToString();
                tbxR.Text = Math.Abs(CalculateByPoint($"" +
                    $"{(double)dataGrid[dataGrid.ColumnCount - 1, 0].Value}/{h * dataGrid.ColumnCount - 2}")).ToString();
            }
            catch
            {
                return;
            }
        }
    }
}
