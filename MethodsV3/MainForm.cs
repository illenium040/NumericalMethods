using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MethodsV3
{
    //В формах создается визуальный интерфейс, который передается в TabPage,
    //Сохраняя функционал; формы загружается в потоках, что нескольуо ускоряет загрузку
    /// <summary>
    /// Добавить метод на выгрузку из формы контролов в tabPage
    /// </summary>
    public partial class MainForm : Form
    {
        private string[] Tabs;
        MethodsEquationsForm form1;
        MethodsSystemEquationForm form2;
        List<Control> controls;
        public MainForm()
        {
            InitializeComponent();
            tabControl.ItemSize = new Size(50, 130);
            tabControl.Dock = DockStyle.Fill;
            f1();//создание потока на инициализацию компонентов формы 1
            f2();//создание потока на инициализацию компонентов формы 2
            Tabs = new string[]
            {
                "Главная",
                "Решение уравнений",
                "Решение систем \r\nуравнений",
                "Интерполяция"
            };
            tabControl.DrawItem += TabControl_DrawItem;
            FormClosing += MainForm_FormClosing;
            tabControl.TabPages[1].Enter += (sender, ev) => Size = new Size(856, 836);
            tabControl.TabPages[2].Enter += (sender, ev) => Size = new Size(912, 636);
            Show();
        }

        #region MethodsEquationForm
        async void f1()
        {
            await Task.Run(() =>
            {
                form1 = new MethodsEquationsForm();
                while (form1.Controls.Count != 0)
                    tabControl.TabPages[1].Controls.Add(form1.Controls[form1.Controls.Count - 1]);
                tabControl.TabPages[1].Controls[0].Location = new Point(-29 /*-29 : Bounds*/, tabControl.TabPages[1].Controls[0].Location.Y);
                form1.Dispose();
            });
        }
        #endregion
        #region MethodsSystemEquationForm
        async void f2()
        {
            await Task.Run(() => 
            {
                form2 = new MethodsSystemEquationForm();
                controls = new List<Control>();
                while (form2.Controls.Count != 0)
                    tabControl.TabPages[2].Controls.Add(form2.Controls[form2.Controls.Count - 1]);
                form2.Owner = this;
                form2.ControlAdded += MethodsForm_ControlAdded;
            });
        }

        private void MethodsForm_ControlAdded(object sender, ControlEventArgs e)
        {
            if (controls.Count == tabControl.TabPages[2].Controls.Count - 7)
            {
                while (tabControl.TabPages[2].Controls.Count > 7)
                    tabControl.TabPages[2].Controls.RemoveAt(tabControl.TabPages[2].Controls.Count - 1);
                controls.Clear();
            }
            if (e.Control.Name != "NULL")
            {
                controls.Add(e.Control);
                return;
            }
            tabControl.TabPages[2].Controls.AddRange(controls.ToArray()); 
        }
        #endregion

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g;
            string sText;
            int iX;
            float iY;

            SizeF sizeText;
            TabControl ctlTab;

            ctlTab = (TabControl)sender;
            g = e.Graphics;
            sText = Tabs[e.Index];
            sizeText = g.MeasureString(sText, ctlTab.Font);
            iX = e.Bounds.Left + 6;
            iY = e.Bounds.Top + (e.Bounds.Height - sizeText.Height) / 2;
            g.DrawString(sText, ctlTab.Font, Brushes.Black, iX, iY);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
