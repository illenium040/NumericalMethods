using System;
using System.Windows.Forms;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MethodsV3
{
    class MethodsEquations : AuxiliaryClass, IMethod
    {
        public enum EMethods
        {
            HalfDivision,
            Hord,
            Tangent,
            Combine,
            SimpleIteration
        }
        private readonly object sync = new object();
        private (double, double) Section;
        private bool IsSameSign;
        private bool isRun;
        //Конструктор принимает ссылки на констролы с формы
        public MethodsEquations(System.Windows.Forms.Control tbxFunc, System.Windows.Forms.Control tbxEps,
            System.Windows.Forms.Control tbxSec1, System.Windows.Forms.Control tbxSec2) : base()
        {
            //Вызов асинхронного потока
            ChangeAsync(tbxFunc, tbxEps, tbxSec1, tbxSec2);
        }

        
        void Change(System.Windows.Forms.Control tbxFunc, System.Windows.Forms.Control tbxEps,
            System.Windows.Forms.Control tbxSec1, System.Windows.Forms.Control tbxSec2)
        {
            while (true)
            {
                lock (sync)
                {
                    while (!isRun)
                    {
                        //Изменяем свойства класса, передаваемые с формы с переодичностью в 10ms 
                        CurrentFunction = tbxFunc.Text;
                        Epsilon = ConvertEpsilon(tbxEps.Text);
                        Section = ConvertSection(tbxSec1.Text + ";" + tbxSec2.Text);
                        Thread.Sleep(10);
                    }
                    Monitor.Wait(sync);
                }
            }
        }
        
        async void ChangeAsync(System.Windows.Forms.Control tbxFunc, System.Windows.Forms.Control tbxEps,
            System.Windows.Forms.Control tbxSec1, System.Windows.Forms.Control tbxSec2)
        {
           //выполняем запуск асинхронного метода
           await Task.Run(()=> Change(tbxFunc, tbxEps, tbxSec1, tbxSec2));
        }
        //x^3-tg(x)+sin(x)-x^(1/3)+1
        //3*x^2-(cos(x))^(-2)+cos(x)-1/(3*x^(2/3)) - 1 порядка
        //6*x-2*sin(x)*cos(x)^(-3)-sin(x)+2/(9*x^(5/3)) - 2 порядка
        //Необходимо найти нули 2-ой производной, иначе методы могут продолжаться бесконечно

        protected override bool IsPosiableToRunMethod()
        {
            isRun = true;
            lock (sync)
                Monitor.Pulse(sync);
            CurrentFunction = ConvertInputText(CurrentFunction);
            if (double.IsNaN(Epsilon) || (double.IsNaN(Section.Item1) && double.IsNaN(Section.Item2)) || CurrentFunction == null)
                return false;
            const double goldenTan = 1.61803398875;
            double x1 = 0, x2 = 0, ansMin, ansMax;
            double h = Section.Item2 - Section.Item1;
            (double, double) tmpSection = (-(Section.Item2 + h), Section.Item2 + h);
            while (Math.Abs(tmpSection.Item2 - tmpSection.Item1) > 0.00001)
            {
                x1 = tmpSection.Item2 - (tmpSection.Item2 - tmpSection.Item1) / goldenTan;
                x2 = tmpSection.Item1 + (tmpSection.Item2 - tmpSection.Item1) / goldenTan;
                if (CalculateByPoint(x1) >= CalculateByPoint(x2))
                    tmpSection.Item1 = x1;
                else tmpSection.Item2 = x2;
            }
            ansMin = (x1 + x2) / 2;
            tmpSection = (-(Section.Item2 + h), Section.Item2 + h);
            while (Math.Abs(tmpSection.Item2 - tmpSection.Item1) > 0.00001)
            {
                x1 = tmpSection.Item2 - (tmpSection.Item2 - tmpSection.Item1) / goldenTan;
                x2 = tmpSection.Item1 + (tmpSection.Item2 - tmpSection.Item1) / goldenTan;
                if (CalculateByPoint(x1) <= CalculateByPoint(x2))
                    tmpSection.Item1 = x1;
                else tmpSection.Item2 = x2;
            }
            ansMax = (x1 + x2) / 2;
            double multiplyDer = FirstDerivative(Section.Item1) * SecondDerivative(Section.Item1);
            if (multiplyDer != 0)
            {
                if ((ansMin < Section.Item1 || ansMin > Section.Item2)
                    && (ansMax < Section.Item1 || ansMax > Section.Item2))
                {
                    if (multiplyDer > 0) IsSameSign = true;
                    else IsSameSign = false;
                    return true;
                }
                else
                {
                    MessageBox.Show($"Нули первой производной (max:{ansMax} min:{ansMin}) входят в заданный отрезок");
                    return false;
                }
            }
            MessageBox.Show("Производная равна 0");
            return false;
        }

        public void RunMethod(Enum e)
        {
            switch (e)
            {
                case EMethods.HalfDivision:
                    {
                        CurrentFunction = ConvertInputText(CurrentFunction);
                        HalfDivision();
                        return;
                    }
                default:
                    break;
            }
            if (IsPosiableToRunMethod())
            {
                switch (e)
                {
                    case EMethods.Hord:
                        {
                            Hord();
                            break;
                        }
                    case EMethods.Tangent:
                        {
                            Tangent();
                            break;
                        }
                    case EMethods.Combine:
                        {
                            Combine();
                            break;
                        }
                    default:
                        break;
                }
            }
            isRun = false;
            lock (sync)
                Monitor.Pulse(sync);
        }
        private void HalfDivision()
        {
            double c = 0;
            bool isBeginingNegative = CalculateByPoint(Section.Item1) < 0 ? true : false;
            int iterationsCount = (int)Math.Round(
                Math.Log((Section.Item2 - Section.Item1) / (2 * Epsilon), 2), 0) + 1;

            tableData = new TableData(3);
            for (int i = 0; i < iterationsCount; i++)
            {
                tableData.AddToTable(Math.Abs(Section.Item2 - Section.Item1), Section.Item1, Section.Item2);

                c = (Section.Item1 + Section.Item2) / 2;
                if (CalculateByPoint(c) > 0)
                {
                    if (isBeginingNegative)
                        Section.Item2 = c;
                    else
                        Section.Item1 = c;
                }
                else
                {
                    if (!isBeginingNegative)
                        Section.Item2 = c;
                    else
                        Section.Item1 = c;
                }
            }
            tableData.CreateTable();
        }
        private void Hord()
        {
            double x0 = (Section.Item1 + Section.Item2) / 2;
            double xn = 0;
            double m = Math.Abs(FirstDerivative(x0));
            double M = Math.Abs(SecondDerivative(x0));
            double mM = (M - m) / m;
            double[] delta = new double[2];
            tableData = new TableData(1,2);
            if (IsSameSign)
            {
                do
                {
                    xn = x0 - (Section.Item2 - x0) * CalculateByPoint(x0) /
                        (CalculateByPoint(Section.Item2) - CalculateByPoint(x0));
                    delta[0] = Math.Abs(CalculateByPoint(xn) / m);
                    delta[1] = Math.Abs((xn - x0) * mM);
                    tableData.AddToTable(delta, xn);
                    x0 = xn;
                } while (Epsilon < delta[0] || Epsilon < delta[1]);
            }
            else
            {
                do
                {
                    xn = x0 - (x0 - Section.Item1) * CalculateByPoint(x0) /
                        (CalculateByPoint(x0) - CalculateByPoint(Section.Item1));
                    delta[0] = Math.Abs(CalculateByPoint(xn) / m);
                    delta[1] = Math.Abs((xn - x0) * mM);
                    tableData.AddToTable(delta, xn);
                    x0 = xn;
                } while (Epsilon < delta[0] || Epsilon < delta[1]);
            }
            tableData.CreateTable();
        }
        private void Tangent()
        {
            double x0 = IsSameSign ? Section.Item2 : Section.Item1;
            double xn = 0;
            double mM = Math.Abs(SecondDerivative(x0) / (2 * Math.Abs(FirstDerivative(x0))));
            double delta = 0;
            tableData = new TableData(2);
            do
            {
                xn = x0 - CalculateByPoint(x0) / FirstDerivative(x0);
                delta = Math.Abs((Math.Pow((xn - x0), 2)) * mM);
                tableData.AddToTable(delta, xn);
                x0 = xn;
            } while (Epsilon < delta);
            tableData.CreateTable();
        }
        private void Combine()
        {
            double x0 = IsSameSign ? Section.Item2 : Section.Item1;
            double y0 = (Section.Item1 + Section.Item2) / 2;
            double delta = 0;
            double xn = 0;
            double yn = 0;
            tableData = new TableData(3);
            do
            {
                yn = y0 - CalculateByPoint(y0) / FirstDerivative(y0);
                xn = x0 - (y0 - x0) / (CalculateByPoint(y0) - CalculateByPoint(x0)) * CalculateByPoint(x0);
                delta = Math.Abs(x0 - y0);
                tableData.AddToTable(delta, xn, yn);
                x0 = xn;
                y0 = yn;
            } while (Epsilon < delta);
            tableData.CreateTable();
        }

        private (double, double) ConvertSection(string Section)
        {
            (double, double) SectionDouble;
            try
            {
                string[] temp = (Section.Replace(" ", "")).Split(';');
                SectionDouble = (Convert.ToDouble(temp[0]), Convert.ToDouble(temp[1]));
                if (SectionDouble.Item1 > SectionDouble.Item2)
                {
                    double temp1 = SectionDouble.Item1;
                    SectionDouble.Item1 = SectionDouble.Item2;
                    SectionDouble.Item2 = temp1;
                }
                if (SectionDouble.Item1 == 0)
                    SectionDouble.Item1 = 0.0001;
                if (SectionDouble.Item2 == 0)
                    SectionDouble.Item2 = 0.0001;
                return SectionDouble;
            }
            catch
            {
                return (double.NaN, double.NaN);
            }
        }
    }
}
