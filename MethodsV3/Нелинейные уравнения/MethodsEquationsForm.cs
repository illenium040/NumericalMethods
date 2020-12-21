using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodsV3
{
    public partial class MethodsEquationsForm : Form
    {
        IMethod functions;
        CreateChart createChart;

        public MethodsEquationsForm()
        {
            InitializeComponent();
            functions = new MethodsEquations(textBox1,textBox2, textBox3, textBox4);
            createChart = new CreateChart(ChartPanel);
            foreach(Control tbx in Controls)
            {
                if (tbx.GetType() == typeof(TextBox))
                    tbx.TextChanged += tbx_TextChanged;
            }
            textBox1.KeyDown += TextBox1_KeyDown;
        }

        private void tbx_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text.Contains("\r\n"))
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace("\r\n", "");
                (sender as TextBox).Select((sender as TextBox).Text.Length, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createChart.CreateSchedule(textBox1.Text);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            functions.RunMethod(MethodsEquations.EMethods.HalfDivision);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            functions.RunMethod(MethodsEquations.EMethods.Hord);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            functions.RunMethod(MethodsEquations.EMethods.Tangent);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            functions.RunMethod(MethodsEquations.EMethods.Combine);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            functions.RunMethod(MethodsEquations.EMethods.SimpleIteration);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                createChart.CreateSchedule(textBox1.Text);
        }

    }
}
