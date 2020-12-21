using MathNet.Numerics;
using System;

namespace MethodsV3
{
    class AuxiliaryClass : FleeCompile, IDisposable
    {
        
        protected double Epsilon { get; set; }
        protected bool isSameFunc;
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
        public AuxiliaryClass() : base()
        {

        }

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

        protected double FirstDerivative(double inputX)
        {
            return Differentiate.FirstDerivative(CalculateByPoint, inputX);
        }

        protected double SecondDerivative(double inputX)
        {
            return Differentiate.SecondDerivative(CalculateByPoint, inputX);
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
