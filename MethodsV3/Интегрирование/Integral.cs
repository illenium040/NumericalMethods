using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MethodsV3.Вспомогательные_классы;

namespace MethodsV3.Интегрирование
{
    class Integral : AuxiliaryClass
    {
        RichTextBox tbxResult;
        DataGridView dataGridN, dataGridResult;
        const string spliter = "=========================================================================";
        const string spliterM = "-----------------------------------------------------------------------------------------------------------------------------------";
        public Integral(TextBox tbxFunc, RichTextBox tbxResult,
            DataGridView dataGridResult, DataGridView dataGridN)
        {
            this.tbxResult = tbxResult;
            this.dataGridN = dataGridN;
            this.dataGridResult = dataGridResult;
            CurrentFunction = tbxFunc.Text;
            tbxFunc.TextChanged += TbxFunc_TextChanged;
            
        }

        private async void TbxFunc_TextChanged(object sender, EventArgs e)
        {
            await Task.Run(() => { CurrentFunction = ConvertInputText((sender as TextBox).Text); });         
        }

        public void FillTableResult(double stepN, double stepBiggerN)
        {
            tbxResult.Text = "";
            LeftMethod(stepN, stepBiggerN);
            tbxResult.Text += spliter;
            RightMethod(stepN, stepBiggerN);
            tbxResult.Text += spliter;
            CentralMethod(stepN, stepBiggerN);
            tbxResult.Text += spliter;
            TrapeciaMethod(stepN, stepBiggerN);
            tbxResult.Text += spliter;
            SimpsomMethod(stepN, stepBiggerN);
        }
        public void FillEpsilon((double, double) Section, int N, TextBox tbxRes)
        {
            dataGridResult[2, 0].Value = Math.Abs((double)dataGridResult[0, 0].Value - (double)dataGridResult[1, 0].Value);
            dataGridResult[2, 1].Value = Math.Abs((double)dataGridResult[0, 1].Value - (double)dataGridResult[1, 1].Value);
            dataGridResult[2, 2].Value = Math.Abs((double)dataGridResult[0, 2].Value - (double)dataGridResult[1, 2].Value)/3;
            dataGridResult[2, 3].Value = Math.Abs((double)dataGridResult[0, 3].Value - (double)dataGridResult[1, 3].Value)/3;
            dataGridResult[2, 4].Value = Math.Abs((double)dataGridResult[0, 4].Value - (double)dataGridResult[1, 4].Value)/2;

            double derivA = FirstDerivative(Section.Item1), derivB = FirstDerivative(Section.Item2);
            double delta = derivA > derivB ? Math.Pow((Section.Item2 - Section.Item1), 2) / (2 * N) * derivA :
                Math.Pow((Section.Item2 - Section.Item1), 2) / (2 * N) * derivB;

            dataGridResult[3, 0].Value = Math.Abs(delta);
            dataGridResult[3, 1].Value = Math.Abs(delta);

            derivA = SecondDerivative(Section.Item1);
            derivB = SecondDerivative(Section.Item2);

            delta = derivA > derivB ? Math.Pow((Section.Item2 - Section.Item1), 3) / (24 * Math.Pow(N,2)) * derivA :
                Math.Pow((Section.Item2 - Section.Item1), 3) / (24 * Math.Pow(N, 2)) * derivB;
            dataGridResult[3, 2].Value = Math.Abs(delta);

            delta = derivA > derivB ? Math.Pow((Section.Item2 - Section.Item1), 3) / (12 * Math.Pow(N, 2)) * derivA :
                Math.Pow((Section.Item2 - Section.Item1), 3) / (12 * Math.Pow(N, 2)) * derivB;
            dataGridResult[3, 3].Value = Math.Abs(delta);

            derivA = Derivative(Section.Item1, N);
            derivB = Derivative(Section.Item2, N);

            delta = derivA > derivB ? Math.Pow((Section.Item2 - Section.Item1), 5) / (180 * Math.Pow(N, 4)) * derivA :
                Math.Pow((Section.Item2 - Section.Item1), 5) / (180 * Math.Pow(N, 4)) * derivB;
            dataGridResult[3, 4].Value = Math.Abs(delta);

            double avgFunc = 0, avgEps = 0;
            for (int i = 0; i < 5; i++)
            {
                dataGridResult[4, i].Value = (double)dataGridResult[3, i].Value < (double)dataGridResult[2, i].Value ?
                    dataGridResult[3, i].Value : dataGridResult[2, i].Value;
                dataGridResult[5, i].Value = $"{Math.Round((double)dataGridResult[1, i].Value, 4)}+-{Math.Round((double)dataGridResult[4, i].Value, 4)}";
                avgFunc += (double)dataGridResult[1, i].Value;
                avgEps += (double)dataGridResult[4, i].Value;
            }
            tbxRes.Text = $"{Math.Round(avgFunc / 5, 4)}+-{Math.Round(avgEps/5, 4)}";
        }
        public bool FillTableN(int N, int biggerN, (double, double) Section, double h)
        {
            if (CurrentFunction == null)
                return false;
            int step = biggerN / N;
            int maxN = (int)((double)biggerN / ((double)biggerN / (double)N));
            dataGridN[0, 0].Value = 0;
            dataGridN[0, 1].Value = 0;
            dataGridN[0, 2].Value = Section.Item1;
            dataGridN[0, 3].Value = CalculateByPoint(Section.Item1);
            for (int i = 1, j = 0; i < biggerN+1; i++)
            {
                if (i % step == 0 && j < maxN)
                {
                    dataGridN[i, 0].Value = j+1;
                    j++;
                }
                dataGridN[i, 1].Value = i;
                dataGridN[i, 2].Value = (double)dataGridN[i - 1, 2].Value + h;
                dataGridN[i, 3].Value = CalculateByPoint((double)dataGridN[i, 2].Value);
            }
            return true;
        }
        string resultN, resultBiggerN;
        private void LeftMethod(double stepN, double stepBiggerN)
        {
            resultN = $"{stepN}*({dataGridN[0, 3].Value}";
            resultBiggerN = $"{stepBiggerN}*({dataGridN[0, 3].Value}";
            for (int i = 1; i < dataGridN.ColumnCount - 1; i++)
            {
                if(dataGridN[i, 0].Value != null)
                {
                    resultN += $"+{dataGridN[i, 3].Value}";
                }
                resultBiggerN += $"+{dataGridN[i, 3].Value}";
            }
            resultN = resultN.Replace("+-", "-");
            resultBiggerN = resultBiggerN.Replace("+-", "-");
            resultN += ")";
            resultBiggerN += ")";
            tbxResult.Text += "Левые прямоугольники\r\n" + resultN + "\r\n" + spliterM + resultBiggerN + "\r\n";
            dataGridResult[0, 0].Value = CalculateByPoint(resultN);
            dataGridResult[1, 0].Value = CalculateByPoint(resultBiggerN);
        }
        private void RightMethod(double stepN, double stepBiggerN)
        {
            resultN = $"{stepN}*(+";
            resultBiggerN = $"{stepBiggerN}*({dataGridN[1, 3].Value}";
            for (int i = 2; i < dataGridN.ColumnCount; i++)
            {
                if (dataGridN[i, 0].Value != null)
                {
                    resultN += $"+{dataGridN[i, 3].Value}";
                }
                resultBiggerN += $"+{dataGridN[i, 3].Value}";
            }
            resultN = resultN.Replace("++", "");
            resultN = resultN.Replace("+-", "-");
            resultBiggerN = resultBiggerN.Replace("+-", "-");
            resultN += ")";
            resultBiggerN += ")";
            tbxResult.Text += "Правые прямоугольники\r\n" + resultN + "\r\n" + spliterM + resultBiggerN + "\r\n";
            dataGridResult[0, 1].Value = CalculateByPoint(resultN);
            dataGridResult[1, 1].Value = CalculateByPoint(resultBiggerN);
        }
        private void CentralMethod(double stepN, double stepBiggerN)
        {
            resultN = $"{stepN}*(+";
            resultBiggerN = $"{stepBiggerN}*(({(double)dataGridN[0, 3].Value + (double)dataGridN[1, 3].Value})/2";
            int stepj = (int)Math.Round(stepN / stepBiggerN);
            for (int i = 2, j = 0; i < dataGridN.ColumnCount; i++)
            {
                if (dataGridN[i, 0].Value != null)
                {
                    try
                    {
                        resultN += $"+({(double)dataGridN[j, 3].Value + (double)dataGridN[j + stepj, 3].Value})/2";
                        j += stepj;
                    }
                    catch
                    { }
                }
                resultBiggerN += $"+({((double)dataGridN[i - 1, 3].Value + (double)dataGridN[i, 3].Value)})/2";
            }
            resultN = resultN.Replace("++", "");
            resultN = resultN.Replace("+-", "-");
            resultBiggerN = resultBiggerN.Replace("+-", "-");
            resultN += ")";
            resultBiggerN += ")";
            tbxResult.Text += "Центральные прямоугольники\r\n" + resultN + "\r\n" + spliterM + resultBiggerN + "\r\n";
            dataGridResult[0, 2].Value = CalculateByPoint(resultN);
            dataGridResult[1, 2].Value = CalculateByPoint(resultBiggerN);
        }
        private void TrapeciaMethod(double stepN, double stepBiggerN)
        {
            resultN = $"{stepN}*({(double)dataGridN[0, 3].Value/2}";
            resultBiggerN = $"{stepBiggerN}*({(double)dataGridN[0, 3].Value / 2}";
            for (int i = 1; i < dataGridN.ColumnCount; i++)
            {
                if (dataGridN[i, 0].Value != null)
                {
                    resultN += $"+{dataGridN[i, 3].Value}";
                }
                resultBiggerN += $"+{dataGridN[i, 3].Value}";
            }
            resultN += "/2)";
            resultBiggerN += "/2)";
            resultN = resultN.Replace("+-", "-");
            resultBiggerN = resultBiggerN.Replace("+-", "-");
            tbxResult.Text += "Трапеция\r\n" + resultN + "\r\n" + spliterM + resultBiggerN + "\r\n";
            dataGridResult[0, 3].Value = CalculateByPoint(resultN);
            dataGridResult[1, 3].Value = CalculateByPoint(resultBiggerN);
        }
        private void SimpsomMethod(double stepN, double stepBiggerN)
        {
            resultN = $"{stepN/3}*({dataGridN[0, 3].Value}";
            resultBiggerN = $"{stepBiggerN/3}*({dataGridN[0, 3].Value}";
            int nCount = (int)((dataGridN.ColumnCount - 1) / (stepN / stepBiggerN)) - 1;
            for (int i = 1, customCounter = 0; i < dataGridN.ColumnCount - 1; i++)
            {
                if((int)dataGridN[i, 1].Value % 2 == 0)
                {
                    resultBiggerN += $"+{(double)dataGridN[i, 3].Value * 2}";
                }
                else
                {
                    resultBiggerN += $"+{(double)dataGridN[i, 3].Value * 4}";
                }
                if (dataGridN[i, 0].Value != null && customCounter < nCount)
                {
                    if ((int)dataGridN[i, 0].Value % 2 == 0)
                    {
                        resultN += $"+{(double)dataGridN[i, 3].Value * 2}";
                    }
                    else
                    {
                        resultN += $"+{(double)dataGridN[i, 3].Value * 4}";
                    }
                    customCounter++;
                    if (customCounter == nCount)
                    {
                        resultN += $"+{dataGridN[i + (int)(stepN / stepBiggerN), 3].Value}";
                    }
                }
                
            }
            resultN += ")";
            resultBiggerN += $"+{dataGridN[dataGridN.ColumnCount - 1, 3].Value})";
            resultN = resultN.Replace("+-", "-");
            resultBiggerN = resultBiggerN.Replace("+-", "-");
            tbxResult.Text += "Симпсон\r\n" + resultN + "\r\n" + spliterM + resultBiggerN + "\r\n";
            dataGridResult[0, 4].Value = CalculateByPoint(resultN);
            dataGridResult[1, 4].Value = CalculateByPoint(resultBiggerN);
        }
    }
}
