using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Drawing;

namespace MethodsV3
{
    class CreateChart : FleeCompile, IDisposable
    {
        private Chart mainChart;
        private Panel parentPanel;
        private int preX, preY;
        private float zoomMultiplier = 1;
        private bool isDisposed = false;

        public CreateChart(Panel panel)
            : base()
        {
           
            parentPanel = panel;
            mainChart = new Chart();
            mainChart.Parent = panel;
            mainChart.Size = panel.Size;
            mainChart.MouseWheel += MainChart_MouseWheel;
            mainChart.MouseUp += MainChart_MouseUp;
            mainChart.MouseDown += MainChart_MouseDown;
            mainChart.MouseMove += MainChart_MouseMove;
            InitializingChart();
            isDisposed = false;
        }
        #region ChartActions
        private void MainChart_MouseDown(object sender, MouseEventArgs e)
        {
            preX = e.X;
            preY = e.Y;
        }
        private void MainChart_MouseUp(object sender, MouseEventArgs e)
        {
            preX = preY = 0;
        }
        private void MainChart_MouseWheel(object sender, MouseEventArgs e)
        {
            //нагружает память
            if (e.Delta > 0 && zoomMultiplier < 2)
            {
                mainChart.Parent.Size = parentPanel.Size;
                zoomMultiplier += 0.2f;
                mainChart.Scale(new SizeF(zoomMultiplier, zoomMultiplier));
            }
            else if (e.Delta < 0 && zoomMultiplier > 1)
            {
                mainChart.Parent.Size = parentPanel.Size;
                mainChart.Scale(new SizeF(1f / (zoomMultiplier), 1f / (zoomMultiplier)));
                mainChart.Top -= (int)(mainChart.Top / zoomMultiplier);
                mainChart.Left -= (int)(mainChart.Left / zoomMultiplier) + 1;
                zoomMultiplier -= 0.2f;
            }
        }
        private void MainChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mainChart.Width > parentPanel.Width)
                {

                    if (mainChart.Width - parentPanel.Width >= -mainChart.Left)
                        mainChart.Left += (e.X - preX);
                    else if (e.X - preX > 0)
                        mainChart.Left += (e.X - preX);

                    if (mainChart.Height - parentPanel.Height >= -mainChart.Top)
                        mainChart.Top += (e.Y - preY);
                    else if (e.Y - preY > 0)
                        mainChart.Top += (e.Y - preY);

                    if (mainChart.Left > 0 && e.X - preX > 0)
                        mainChart.Left -= (e.X - preX);
                    if (mainChart.Top > 0 && e.Y - preY > 0)
                        mainChart.Top -= (e.Y - preY);
                }
            }
        }
        #endregion 
        private void InitializingChart()
        {
            mainChart.ChartAreas.Add(new ChartArea("mainArea"));

            mainChart.Series.Add(new Series("ZX"));
            mainChart.Series.Add(new Series("ZY"));
            mainChart.Series.Add(new Series("mainSeries"));

            mainChart.ChartAreas[0].AxisX.Interval = 1;
            mainChart.ChartAreas[0].AxisY.Interval = 1;
            mainChart.ChartAreas[0].AxisX.Maximum = 10;
            mainChart.ChartAreas[0].AxisY.Maximum = 10;
            mainChart.ChartAreas[0].AxisX.Minimum = -10;
            mainChart.ChartAreas[0].AxisY.Minimum = -10;

            mainChart.Series[0].ChartType = SeriesChartType.Line;
            mainChart.Series[1].ChartType = SeriesChartType.Line;
            mainChart.Series[2].ChartType = SeriesChartType.Line;

            //Линия по горизонтали
            mainChart.Series[0].Points.AddXY(mainChart.ChartAreas[0].AxisX.Minimum, 0);
            mainChart.Series[0].Points.AddXY(mainChart.ChartAreas[0].AxisX.Maximum, 0);
            //Линия по вертикали
            mainChart.Series[1].Points.AddXY(0, mainChart.ChartAreas[0].AxisY.Minimum);
            mainChart.Series[1].Points.AddXY(0, mainChart.ChartAreas[0].AxisY.Maximum);
            for (int i = 0; i < 2; i++)
            {
                mainChart.Series[i].BorderWidth = 2;
                mainChart.Series[i].Color = Color.Black;
            }
            mainChart.Series[2].BorderWidth = 3;
        }
        //необходима оптимизация
        public void CreateSchedule(string curFunc)
        {
            CurrentFunction = ConvertInputText(curFunc);
            if (CurrentFunction == null)
            {
                mainChart.Series[2].Points.Clear();
                return;
            }

            if (mainChart.Series[2].Points.Count != 0)
                mainChart.Series[2].Points.Clear();
            double min = CurrentFunction.Contains("log") || CurrentFunction.Contains("log10") ?
                  mainChart.ChartAreas[0].AxisX.Minimum * -0.0001 : mainChart.ChartAreas[0].AxisX.Minimum;
            double max = min < 0 ? min + mainChart.ChartAreas[0].AxisX.Maximum * 2 : mainChart.ChartAreas[0].AxisX.Maximum;
            if (CurrentFunction.Contains("1/tan"))
            {
                for (double i = min; i < 10; i += 0.01)
                    mainChart.Series[2].Points.AddXY(Math.Round(i, 2), CalculateByPoint(i));
                //производная графика ctg(x) являтся график -1/(sin(x))^2
                //шаг на графике PI, начиная с PI
                NormalizeCtg(min, max);
                CurrentFunction = CurrentFunction.Replace("1/tan", "ctg");
                if (CurrentFunction.Contains("tan"))
                {
                    CurrentFunction = CurrentFunction.Replace("ctg", "1/tan");
                    NormalizeTg(min, max);
                }
                return;
            }
            if (CurrentFunction.Contains("tan"))
            {
                //CurrentFunction = CurrentFunction.Replace("ctg", "1/tan");
                for (double i = min; i < max; i += 0.01)
                    mainChart.Series[2].Points.AddXY(Math.Round(i, 2), Math.Round(CalculateByPoint(i), 6));
                //производная графика tg(x) являтся график 1/(cos(x))^2
                //шаг на графике PI, начиная с PI/2
                NormalizeTg(min, max);
                return;
            }
            if(mainChart.Series[2].Points.Count == 0)
                for (double i = min; i < max; i += 0.05)
                    mainChart.Series[2].Points.AddXY(i, CalculateByPoint(i));

        }
        double piValueMin, piValueMax;

        private void NormalizeTg(double min,double max)
        {
            piValueMin = min == 0.001 ? 1.57 : -Math.Round(((2 * Math.PI + (int)(Math.Abs(min) / Math.PI) * Math.PI) / 2), 2);
            piValueMax = Math.Round(((2 * Math.PI + (int)(Math.Abs(max) / Math.PI) * Math.PI) / 2), 2);
            for (double i = piValueMin; Math.Round(i, 2) <= piValueMax; i += 3.14)
                mainChart.Series[2].Points.FindByValue(Math.Round(i, 2), "X").IsEmpty = true;
        }
        private void NormalizeCtg(double min, double max)
        {
            piValueMin = min == 0.001 ? 3.14 : Math.Round(((int)(min / Math.PI) * Math.PI), 2);
            piValueMax = Math.Round(((int)(Math.Abs(max) / Math.PI) * Math.PI), 2);
            for (double i = piValueMin; Math.Round(i, 2) <= piValueMax; i += 3.14)
                mainChart.Series[2].Points.FindByValue(Math.Round(i, 2), "X").IsEmpty = true;
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
                    parentPanel.Dispose();
                    mainChart.Dispose();
                }
                preX = preY = 0;
                zoomMultiplier = 0;
            }
            isDisposed = true;
        }
        ~CreateChart()
        {
            Dispose(false);
        }
    }


}
