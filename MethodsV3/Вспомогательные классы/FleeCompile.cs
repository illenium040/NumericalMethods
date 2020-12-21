using Flee.PublicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MethodsV3
{
    class FleeCompile
    {
        private IDynamicExpression dynamic;
        private ExpressionContext context;
        protected string CurrentFunction { get; set; } = string.Empty;
        bool IsSqrt = false;
        public FleeCompile()
        {
            context = new ExpressionContext();
            context.Imports.AddType(typeof(Math));
        }
        protected double CalculateByPoint(double x)
        {
            if (CurrentFunction != "" && !double.IsNaN(x))
            {
                string tmpStr = CurrentFunction.Replace("x", x.ToString());
                dynamic = context.CompileDynamic(tmpStr);
                return Convert.ToDouble(dynamic.Evaluate());
            }
            return double.NaN;
        }
        private List<int> indexOfSqrt;
        private List<string> subStr;
        protected string ConvertInputText(string txt)
        {
            txt = txt.ToLower();
            if (txt.Contains("e")) txt = txt.Replace("e", Math.Exp(1).ToString());
            if (txt.Contains("log")) txt = txt.Replace("log", "log10");
            if (txt.Contains("ln")) txt = txt.Replace("ln", "log");
            if (txt.Contains("ctan")) txt = txt.Replace("ctan", "1/tan");
            if (txt.Contains("ctg")) txt = txt.Replace("ctg", "1/tan");
            if (txt.Contains("tg")) txt = txt.Replace("tg", "tan");
            if (txt.Contains(".")) txt = txt.Replace(".", ",");

            int indexOfFirstX = txt.IndexOf('x');
            indexOfSqrt = new List<int>();
            subStr = new List<string>();
            //фиксим начало строки
            while (txt.Contains("--"))
                txt = txt.Replace("--", "+");
            while (txt.Contains("++"))
                txt = txt.Replace("++", "+");
            try
            {
                for (int i = 0, counter = 0; i < txt.Length; i++)
                {
                    
                    if ((i + 1) >= txt.Length)
                    {
                        if (txt[i] == '+' || txt[i] == '-' || txt[i] == '/' || txt[i] == '*' || txt[i] == '^')
                        {
                            txt = txt.Remove(i);
                            break;
                        }
                        else break;
                    }
                    if (txt[i] == '^')
                    {
                        if (txt[i + 1] == '(')
                        {
                            while (txt[i + 2 + counter] != ')')
                                counter++;
                            if (txt.Substring(i, counter).Contains('/'))
                            {
                                counter = 0;
                                while (txt[i + 2 + counter] != '/')
                                    counter++;
                                double tmp = Convert.ToDouble(txt[i + 2 + counter + 1].ToString());
                                txt = txt.Insert(i + 2 + counter, ",0");
                                if (tmp % 2 != 0)
                                {

                                    if (txt[i - 1] == ')')
                                    {
                                        counter = 2;
                                        while (txt[i - counter] != '(')
                                            counter++;
                                        indexOfSqrt.Add(i - counter);
                                        subStr.Add(txt.Substring(i - counter, i - i + counter));
                                        counter = 0;
                                        txt = txt.Insert(i - 1, ")");
                                        i++;
                                    }
                                    else
                                    {
                                        indexOfSqrt.Add(i - 1);
                                        subStr.Add(txt.Substring(i - 1, 1));
                                        txt = txt.Insert(i, ")");
                                        i++;
                                    }
                                    IsSqrt = true;
                                }
                            }
                        }
                    }
                    if (txt[i] == '-')
                        if (txt[i + 1] == 'x')
                        {
                            txt = txt.Insert(i + 1, "(");
                            txt = txt.Insert(i + 3, ")");
                            i += 3;
                        }
                    counter = 0;
                }
                //функция для извлечения корней нечетной степени
                //Math.Sign(x)*Math.Pow(Math.Abs(x),1/n), где n - нечетное число
                if (IsSqrt)
                {
                    for (int i = indexOfSqrt.Count - 1; i >= 0; i--)
                    {
                        txt = txt.Insert(indexOfSqrt[i], $"sign({subStr[i]})*abs(");
                    }
                }
                dynamic = context.CompileDynamic(txt.Replace("x", "0,01"));
                return txt;
            }
            catch
            {
                return null;
            }
        }
    }
}
