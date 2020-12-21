using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using MethodsV3.Интерполяция;
using MethodsV3.Нелинейные_уравнения;
using MethodsV3.Системы_уравнений;
using MethodsV3.Производная;
using MethodsV3.Интегрирование;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace MethodsV3
{
    public partial class MainForm : Form
    {
        private string[] Tabs;
        MethodsEquationsForm form1;
        MethodsSystemEquationForm form2;
        InterpolationForm form3;
        DerivativeForm form4;
        IntegralForm form5;
        List<Control> controls;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; //WS_EX_COMPOSITED(works by forcing child windows to draw back to front and by double buffering them) turned on
                //cp.Style &= ~0x04000000;
                //cp.Style &= ~0x02000000; // Turn off WS_CLIPCHILDREN(appropriately can help reduce the amount of flicker.)
                return cp;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            tabControl.ItemSize = new Size(50, 130);
            tabControl.Dock = DockStyle.Fill;
            tbxMethodsEnum.Dock = DockStyle.Fill;
            TabPage0();
            TabPage1();
            TabPage2();
            TabPage3();
            TabPage4();
            TabPage5();
            Tabs = new string[]
            {
                "Главная",
                "Решение уравнений",
                "Решение систем \r\nуравнений",
                "Интерполяция",
                "Производная",
                "Интеграл"
            };
            tabControl.DrawItem += TabControl_DrawItem;
            FormClosing += MainForm_FormClosing;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #region Main
        void TabPage0()
        {
            var links = ParseTextIntoLinks();
            int labelHeight = 23;
            int curLocY = 0;
            int y = 0;
            for (int i = 0; i < links.Count; i++)
            {
                if (links[i][0] == '\t')
                {
                    y = labelHeight * (int)Char.GetNumericValue(links[i][3]) + curLocY;
                    int index = links[i].IndexOf("#");
                    string takeLink = "";
                    if (index > 0)
                    {
                        takeLink = links[i].Substring(index + 1);
                        links[i] = links[i].Remove(index - 1);
                    }
                        
                    var linklabel = new LinkLabel
                    {
                        Text = links[i],
                        Width = this.Width,
                        Height = labelHeight,
                        Location = new Point(25, y),
                    };
                    linklabel.LinkClicked += (sender, e) => 
                    {
                        Process.Start(takeLink);
                    };
                    tbxMethodsEnum.Controls.Add(linklabel);
                    tbxMethodsEnum.Controls[tbxMethodsEnum.Controls.Count - 1].SendToBack();
                }
                else
                {
                    
                    tbxMethodsEnum.Controls.Add(new Label
                    {
                        Text = links[i],
                        Width = 250,
                        Height = labelHeight,
                        Location = new Point(0, y == 0 ? 0 : y+ labelHeight)
                    });
                    curLocY = y == 0 ? 0 : y + labelHeight;
                }
            }
        }
        List<string> ParseTextIntoLinks()
        {
            List<string> linksList = new List<string>();
            using (StreamReader reader = new StreamReader(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("MethodsV3.links.txt"), 
                Encoding.UTF8, false))
            {
                while(!reader.EndOfStream)
                    linksList.Add(reader.ReadLine());
            }
            return linksList;
        }
        #endregion

        #region MethodsEquationForm
        void TabPage1()
        {
            UploadFromForm(form1 = new MethodsEquationsForm(), 1);
            //Передвигаем график немного влево
            tabControl.TabPages[1].Controls[0].Location = new Point(-29 /*-29 : Bounds*/, tabControl.TabPages[1].Controls[0].Location.Y);
            tabControl.TabPages[1].Enter += (sender, ev) =>
            {
                Size = new Size(856, 836);
            };
        }
        #endregion

        #region MethodsSystemEquationForm
        void TabPage2()
        {
            controls = new List<Control>();
            UploadFromForm(form2 = new MethodsSystemEquationForm(), 2);
            form2.Owner = this;
            //Создание и удаление текстбоксов
            form2.ControlAdded += MethodsForm_ControlAdded;
            tabControl.TabPages[2].Enter += (sender, ev) => Size = new Size(1012, 636);
        }
        private void MethodsForm_ControlAdded(object sender, ControlEventArgs e)
        {
            //добавляем контролы из внутренней формы, пока не дойдем до последнего
            //последний контролл с именем NULL
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
            //добавляем массив контролов в главную форму
            tabControl.TabPages[2].Controls.AddRange(controls.ToArray()); 
        }
        #endregion

        #region InterpolationForm
        void TabPage3()
        {
            form3 = new InterpolationForm();
            UploadFromForm(form3, 3);
            tabControl.TabPages[3].LostFocus += (sender, ev) =>
            {
                 Interpolation.eventSlimCheck.Reset();
                 Interpolation.eventSlimMain.Reset();
            };
            tabControl.TabPages[3].Enter += (sender, e) =>
            {
                Size = new Size(912, 736);
            };
            
        }
        #endregion

        #region DerivativeForm
        void TabPage4()
        {
            form4 = new DerivativeForm();
            UploadFromForm(form4, 4);
        }
        #endregion

        #region IntegrakForm
        void TabPage5()
        {
            form5 = new IntegralForm();
            UploadFromForm(form5, 5);
        }
        #endregion
        //выгружаем контролы из формы
        private void UploadFromForm(Form curForm, int indexOfTabPage)
        {
            while (curForm.Controls.Count != 0)
                tabControl.TabPages[indexOfTabPage].Controls.Add(curForm.Controls[curForm.Controls.Count - 1]);
        }

        Graphics g;
        string sText;
        int iX;
        float iY;
        SizeF sizeText;
        TabControl ctlTab;
        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
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
