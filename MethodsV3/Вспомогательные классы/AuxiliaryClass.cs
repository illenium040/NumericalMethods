using MathNet.Numerics;
using System;

namespace MethodsV3.Вспомогательные_классы
{
    class AuxiliaryClass : FleeCompile, IDisposable
    {
        protected double Epsilon { get; set; }
        protected TableData tableData;
        private bool isDisposed = false;

        protected virtual bool IsPosiableToRunMethod()
        {
            return false;
        }
        private void SimpleInitializing(string epsilon)
        {
            isDisposed = false;
        }
        //Конструктор в общем виде
        //Конструктор для методов решения нелинейных уравнений
        public AuxiliaryClass() : base() {  }

        protected double ConvertEpsilon(string eps)
        {
            try
            {
                eps = eps.Replace('.', ',');
                return double.Parse(eps); ;
            }
            catch
            {
                return double.NaN;
            }
        }
        protected (double, double) ConvertSection(string Section)
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
                    SectionDouble.Item1 = 0.00000001;
                if (SectionDouble.Item2 == 0)
                    SectionDouble.Item2 = 0.00000001;
                return SectionDouble;
            }
            catch
            {
                return (double.NaN, double.NaN);
            }
        }
        public static (double, double) ConvertSectionAsPublic(string Section)
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
        protected double FirstDerivative(double inputX)
        {
            return Differentiate.FirstDerivative(CalculateByPoint, inputX);
        }
        protected double SecondDerivative(double inputX)
        {
            return Differentiate.SecondDerivative(CalculateByPoint, inputX);
        }
        protected double Derivative(double inputX, int order)
        {
            return Differentiate.Derivative(CalculateByPoint, inputX, order);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(!isDisposed)
            {
                if(disposing)
                {
                    tableData = null;
                    
                }
            }
            isDisposed = true;
        }
        ~AuxiliaryClass()
        {
            Dispose(false);
        }
    }
}
