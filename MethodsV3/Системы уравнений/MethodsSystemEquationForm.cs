using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MethodsV3.Системы_уравнений
{
    public partial class MethodsSystemEquationForm : Form
    {
        IMethod method;
        int preControlsCount;
        int counter = 0;
        int tbxCounts = 0;
        int beginingControlsCount;
        string[,] currentMatrix;
        public MethodsSystemEquationForm()
        {
            InitializeComponent();
            preControlsCount = beginingControlsCount = Controls.Count;
            tbxCount.KeyDown += TbxCount_KeyDown;
            tbxCount.TextChanged += TbxCount_TextChanged;
        }

        private void TbxCount_TextChanged(object sender, EventArgs e)
        {
            tbxCount.Text = tbxCount.Text.Replace("\r\n", "");
        }

        private void TbxCount_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(tbxCount.Text, out tbxCounts))
                {
                    CreateTextBosex(tbxCounts);
                }
                else
                {
                    MessageBox.Show("Неверные данные");
                }
            }
        }


        void CreateTextBosex(int tbxCount)
        { 
            if (tbxCount > 6) tbxCount = 6;
            if (beginingControlsCount + (tbxCount * tbxCount)*2 + tbxCount == preControlsCount) return;
            preControlsCount = beginingControlsCount + (tbxCount * tbxCount)*2 + tbxCount;
            Control tbx = null, lbl = null;
            int locPlusX = 150, locPlusY = buttonUpload.Location.Y;
            for (int i = 0; i < tbxCount; i++)
            {
                for (int j = 0; j < tbxCount; j++)
                {
                    tbx = new TextBox
                    {
                        Name = "tbx" + $"{counter}",
                        Size = new Size(70, 30),
                        Font = new Font("Microsoft Sans Serif", 12),

                    };
                    tbx.Location = new Point(locPlusX + tbx.Width * j + 10, locPlusY);
                    tbx.TextChanged += Tbx_TextChanged;
                    lbl = new Label
                    {
                        Location = new Point(tbx.Location.X + tbx.Width, tbx.Location.Y),
                        Name = $"lbl{counter}",
                        Text = "x"+$"{j + 1}",
                        AutoSize = true,
                        Font = new Font("Microsoft Sans Serif", 12)
                    };
                    locPlusX += 30;
                    Controls.Add(tbx);
                    Controls.Add(lbl);
                    counter++;
                }
                locPlusY += 36;
                locPlusX = 150;
                
            }
            locPlusY = buttonUpload.Location.Y;
            for (int i = 0; i < tbxCount; i++)
            {
                tbx = new TextBox
                {
                    Location = new Point(tbx.Width*tbxCount + locPlusX + 10 + 30*tbxCount, locPlusY),
                    Name = "tbx" + $"{counter}",
                    Size = new Size(70, 30),
                    Font = new Font("Microsoft Sans Serif", 12),
                };
                locPlusY += 36;
                tbx.TextChanged += Tbx_TextChanged;
                Controls.Add(tbx);
                counter++;
            }
            counter = 0;
            tbx = null;
            currentMatrix = new string[tbxCounts + 1, tbxCounts];
            Controls.Add(new Control{
               Name = "NULL"
            });
        }

        private void Tbx_TextChanged(object sender, EventArgs e)
        {
            int a = int.Parse((sender as TextBox).Name.Replace("tbx", ""));
            currentMatrix[a / tbxCounts, a % tbxCounts] = (sender as TextBox).Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SaveData();
            method = new MethodsSystemEquation(tbxEps.Text, tbxCounts, currentMatrix);
            method.RunMethod(MethodsSystemEquation.EMethodsSystem.SimpleIterations);
            counter = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveData();
            method = new MethodsSystemEquation(tbxEps.Text, tbxCounts, currentMatrix);
            method.RunMethod(MethodsSystemEquation.EMethodsSystem.Zeidel);
            counter = 0;
        }

        //Функия, реализующая сохранение данных в файл
        private void SaveData()
        {
            //Получаем поток записи в файл
            using (var writer = new StreamWriter("savedMatrix.txt"))
            {
                int counter = 0;
                writer.WriteLine(tbxCounts);
                writer.WriteLine(tbxEps.Text);
                while (tbxCounts * tbxCounts + tbxCounts > counter)
                {
                    var s = this.Owner.Controls.Find($"tbx{counter}", true)[0].Text;
                    writer.WriteLine(s);
                    counter++;
                }
            }
        }
        //Событие кнопки, по нажатию которой загружаются данные из файла
        private void buttonUpload_Click(object sender, EventArgs e)
        {
            //Получаем поток чтения файла
            var ssss = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            using (StreamReader reader = new StreamReader("savedMatrix.txt"))
            {
                int counter = 0;
                try
                {
                    int tbc = int.Parse(reader.ReadLine());
                    tbxCount.Text = tbc.ToString();
                    tbxCounts = tbc;
                    CreateTextBosex(tbc);
                    tbxEps.Text = reader.ReadLine();
                }
                catch
                {
                    return;
                }
                while (tbxCounts * tbxCounts + tbxCounts > counter)
                {
                    Owner.Controls.Find($"tbx{counter}", true)[0].Text = reader.ReadLine();
                    counter++;
                }
            }
        }
    }
}
