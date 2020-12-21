using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using MethodsV3.Вспомогательные_классы;

namespace MethodsV3.Нелинейные_уравнения
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
        (double, double) SectionSaved;
        //Конструктор класса "Нелинейные уравнения"
        //Конструктор принимает ссылки на эллементы управления с вызвавшей его формы
        public MethodsEquations(Control tbxFunc, Control tbxEps,
            Control tbxSec1, Control tbxSec2) : base()
        {
            //Функция
            tbxFunc.TextChanged += (sender, ev) =>
            { CurrentFunction = tbxFunc.Text; };
            //Погрешность
            tbxEps.TextChanged += (sender, ev) =>
            { Epsilon = ConvertEpsilon(tbxEps.Text); };
            //Начало отрезка
            tbxSec1.TextChanged += (sender, ev) =>
            { Section = ConvertSection(tbxSec1.Text + ";" + tbxSec2.Text); };
            //Конец отрезка
            tbxSec2.TextChanged += (sender, ev) =>
            {
                Section = ConvertSection(tbxSec1.Text + ";" + tbxSec2.Text);
                SectionSaved = (Section.Item1, Section.Item2);
            };
        }
        
        //x^3-tg(x)+sin(x)-x^(1/3)+1
        //3*x^2-(cos(x))^(-2)+cos(x)-1/(3*x^(2/3)) - 1 порядка
        //6*x-2*sin(x)*cos(x)^(-3)-sin(x)+2/(9*x^(5/3)) - 2 порядка
        //Необходимо найти нули 2-ой производной, иначе методы могут продолжаться бесконечно

        double x1 = 0, x2 = 0, ansMin = 0, ansMax = 0;
        const double goldenTan = 1.61803398875;
        protected override bool IsPosiableToRunMethod()
        {
            CurrentFunction = ConvertInputText(CurrentFunction);
            if (double.IsNaN(Epsilon) || (double.IsNaN(Section.Item1) && double.IsNaN(Section.Item2)) || CurrentFunction == null)
                return false;
            double h = Section.Item2 - Section.Item1;
            (double, double) tmpSection = (-(Section.Item2 + h), Section.Item2 + h);
            ReSection(tmpSection, h);
            while(!((ansMin < Section.Item1 || ansMin > Section.Item2)
                    && (ansMax < Section.Item1 || ansMax > Section.Item2)))
            {
                Section.Item1 += Math.Sign(Section.Item1) * 0.1;
                h = Section.Item2 - Section.Item1;
                if (h == 0) break;
                tmpSection = (-(Section.Item2 + h), Section.Item2 + h);
                ReSection(tmpSection, h);
            }
            double multiplyDer = FirstDerivative(Section.Item1) * SecondDerivative(Section.Item1);
            Section = SectionSaved;
            if (multiplyDer != 0)
            {
                if (multiplyDer > 0) IsSameSign = true;
                else IsSameSign = false;
                return true;
            }
            
            MessageBox.Show("Производная равна 0");
            return false;
        }
        private void ReSection((double, double) tmpSection, double h)
        {
            while (Math.Abs(tmpSection.Item2 - tmpSection.Item1) > 0.00001)
            {
                x1 = tmpSection.Item2 - (tmpSection.Item2 - tmpSection.Item1) / goldenTan;
                x2 = tmpSection.Item1 + (tmpSection.Item2 - tmpSection.Item1) / goldenTan;
                if (CalculateByPoint(x1) >= CalculateByPoint(x2))
                    tmpSection.Item1 = x1;
                else tmpSection.Item2 = x2;
            }
            ansMin = (x1 + x2) / 2;
        }
        public void RunMethod(Enum e)
        {
            if (IsPosiableToRunMethod())
            {
                switch (e)
                {
                    case EMethods.HalfDivision:
                        {
                            HalfDivision();
                            break;
                        }
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
    }
}
