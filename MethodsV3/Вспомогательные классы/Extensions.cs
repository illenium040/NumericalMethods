using System;
using System.Reflection;
using System.Windows.Forms;

namespace MethodsV3.Вспомогательные_классы
{
    public static class Extension
    {
        public static void DoubleBuffered(this Control dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
