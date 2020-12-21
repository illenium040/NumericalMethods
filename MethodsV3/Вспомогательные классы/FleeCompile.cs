using Flee.PublicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MethodsV3.Вспомогательные_классы
{
    class FleeCompile
    {
        protected string CurrentFunction { get; set; } = string.Empty;
        bool IsSqrt = false;

        private IDynamicExpression dynamic;
        private ExpressionContext context;

        public FleeCompile()
        {
            context = new ExpressionContext();
            context.Imports.AddType(typeof(Math));
        }

        protected double CalculateByPoint(double x)
        {
            try
            {
                string tmpStr = CurrentFunction.Replace("x", x.ToString());
                dynamic = context.CompileDynamic(tmpStr);
                return Convert.ToDouble(dynamic.Evaluate());
            }
            catch
            {
                return 0;
            }
        }

        protected double CalculateByPoint(string func)
        {
            try
            {
                dynamic = context.CompileDynamic(func);
                return Convert.ToDouble(dynamic.Evaluate());
            }
            catch
            {
                return 0;
            }
        }

        protected string ConvertInputText(string txt)
        {
            txt = txt.ToLower();
            //фиксим начало строки
            while (txt.Contains("--"))
                txt = txt.Replace("--", "+");
            while (txt.Contains("++"))
                txt = txt.Replace("++", "+");
            try
            {
                //натсроить (-10)^(x/n), где n - нечетное число
                IsSqrt = false;
                txt = txt.Replace("-", "+(-1)*");
                txt = txt.Replace("/", "*1,0/");
                if (txt[0] == '+' || txt[0] == '-' || txt[0] == '/' || txt[0] == '*' || txt[0] == '^')
                    txt = txt.Remove(0, 1);
                for (int i = txt.IndexOf('(') + 1; i < txt.Length; i = txt.IndexOf('(', i) + 1)
                {
                    if (i == 0)
                        break;
                    if (txt[i] == '+')
                        txt = txt.Remove(i, 1);
                }
                if (txt.Contains("e")) txt = txt.Replace("e", Math.Exp(1).ToString());
                if (txt.Contains("log")) txt = txt.Replace("log", "log10");
                if (txt.Contains("ln")) txt = txt.Replace("ln", "log");
                if (txt.Contains("ctan")) txt = txt.Replace("ctan", "1/tan");
                if (txt.Contains("ctg")) txt = txt.Replace("ctg", "1/tan");
                if (txt.Contains("tg")) txt = txt.Replace("tg", "tan");
                if (txt.Contains(".")) txt = txt.Replace(".", ",");
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
