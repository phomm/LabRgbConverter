using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Cocon
{
    //using Microsoft.Office.Interop.Excel;
    using Excel = Microsoft.Office.Interop.Excel;
    using ColorMine;
    using ColorMine.ColorSpaces;
    using ColorMine.ColorSpaces.Conversions;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolcomboRows.Items.AddRange(RowsValues);
            toolcomboSize.Items.AddRange(SizeValues);
            OptionSize = (int)SizeValues[0];
            OptionRows = (int)RowsValues[0];
            toollabelCount.Text = "Count: 0";
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DoubleBuffered = true;
            ZoneSize = new Size(Zone.Size.Width, Zone.Size.Height);
        }
        const string RowMajor = "RowMajorDrawing";
        const string ColMajor = "ColMajorDrawing";

        const string AlgoNew = "New";
        const string AlgoOld = "Old";

        private List<Tuple<Lab, Rgb>> Labs = new List<Tuple<Lab, Rgb>>();

        Bitmap bmp = null;
        Graphics g;
        bool Generated = false;
        bool OptionRowMajor = false;
        int optionSize;
        int optionRows;
        //bool optionNewAlgo = false;

        object[] RowsValues = new object[] { 10, 15, 20, 25, 30 };
        object[] SizeValues = new object[] { 10, 15, 20, 25, 30 };

        int OptionSize { get { return optionSize; } set { optionSize = value; toolcomboSize.Text = value.ToString(); } }
        int OptionRows { get { return optionRows; } set { optionRows = value; toolcomboRows.Text = value.ToString(); } }

        Size zoneSize;
        /// <summary>
        /// регулирование размеров рабочей области
        /// </summary>
        Size ZoneSize
        {
            get { return zoneSize; }
            set
            {
                zoneSize = value;
                if (Zone.Image != null)
                {
                    bmp = new Bitmap(Zone.Image);
                    Zone.Image.Dispose();
                }
                else
                    bmp = new Bitmap(ZoneSize.Width, ZoneSize.Height);

                Zone.Size = ZoneSize;
                Zone.Image = new Bitmap(ZoneSize.Width, ZoneSize.Height);
                g = Graphics.FromImage(Zone.Image);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, ZoneSize.Width, ZoneSize.Height);
                g.DrawImage(bmp, 0, 0);
                g.Dispose();
            }
        }

        private void toolbtnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;            
            List<Tuple<Lab, Rgb>> oldlabs = new List<Tuple<Lab, Rgb>>(Labs);
            string oldfilename = openFileDialog1.FileName;
            bool isexcelformat = openFileDialog1.FileName.EndsWith(".xlsx");
            Excel.Application xlApp = null;
            Excel.Workbook xlWorkbook = null;
            Excel._Worksheet xlWorksheet = null;
            Excel.Range xlRange = null;            
            try
            {
                Labs.Clear();                    
                if (isexcelformat)
                {
                    xlApp = new Excel.Application();
                    xlWorkbook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
                    xlRange = xlWorksheet.UsedRange;
                    for (int r = 2; r <= xlRange.Rows.Count; r++)
                    {
                        double l = (double)(xlRange.Cells[r, 1] as Excel.Range).Value2;
                        double a = (double)(xlRange.Cells[r, 2] as Excel.Range).Value2;
                        double b = (double)(xlRange.Cells[r, 3] as Excel.Range).Value2;
                        Labs.Add(new Tuple<Lab, Rgb>(new Lab { L = l, A = a, B = b }, new Rgb { R = 0, G = 0, B = 0 }));
                    }
                }
                else
                {
                    string[] lines = File.ReadAllLines(openFileDialog1.FileName, Encoding.Default);
                    CultureInfo cult = new CultureInfo("ru-RU");
                    foreach (var line in lines.Where(x => !string.IsNullOrWhiteSpace(x)))
                    {
                        double l = 0;
                        double a = 0;
                        double b = 0;
                        string[] tokens = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        double.TryParse(tokens[0], NumberStyles.Float, cult, out l);
                        double.TryParse(tokens[1], NumberStyles.Float, cult, out a);
                        double.TryParse(tokens[2], NumberStyles.Float, cult, out b);
                        Labs.Add(new Tuple<Lab, Rgb>(new Lab { L = l, A = a, B = b }, new Rgb { R = 0, G = 0, B = 0 }));
                    }
                }
                Generated = false;
            }
            catch (Exception ex)
            {
                Labs = oldlabs;
                openFileDialog1.FileName = oldfilename;
                MessageBox.Show("Ошибка чтения файла, предыдущие данные не были изменены. Подробности об ошибке\n" + ex.Message);
            }
            finally
            {
                if (isexcelformat)
                {
                    Marshal.FinalReleaseComObject(xlRange);
                    Marshal.FinalReleaseComObject(xlWorksheet);
                    xlWorkbook.Close(Type.Missing, Type.Missing, Type.Missing);
                    Marshal.FinalReleaseComObject(xlWorkbook);
                    xlApp.Quit();
                    Marshal.FinalReleaseComObject(xlApp);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            toollabelCount.Text = "Count: " + Labs.Count.ToString();
        }

        private void toolbtnGenerate_Click(object sender, EventArgs e)
        {
            if (Labs.Count == 0)
                return;
            int pixelsize = int.Parse(toolcomboSize.Text);
            OptionSize = pixelsize;
            int rows = int.Parse(toolcomboRows.Text);
            OptionRows = rows;
            int cols = (int)Math.Ceiling((double)Labs.Count / rows);
            //Bitmap bmp = new Bitmap(cols * pixelsize, rows * pixelsize);
            //Graphics graf = Graphics.FromImage(bmp);
            ResizeZone(new System.Drawing.Rectangle(0, 0, cols * pixelsize, rows * pixelsize));
            bmp = new Bitmap(cols * pixelsize, rows * pixelsize);
            g = Graphics.FromImage(bmp);
            bool vert = toolbtnMatrixDrawingMajority.Text == RowMajor;
            OptionRowMajor = vert;
            bool optionNewAlgo = toolbtnAlgo.Text == AlgoNew;

            //int labidx = 0;

            //for (int i = 0; i <= (vert ? cols : rows); i++)
            //    for (int j = 0; j <= (vert ? rows : cols); j++)
            for (int labidx = 0; labidx < Labs.Count && labidx < 100; labidx++)
            {
                if (labidx >= Labs.Count || labidx > 100)
                    return;
                int R = 0;
                int G = 0;
                int B = 0;

                if (optionNewAlgo)
                {
                    int L = Convert.ToInt32(Labs[labidx].Item1.L);
                    int a = Convert.ToInt32(Labs[labidx].Item1.A);
                    int b = Convert.ToInt32(Labs[labidx].Item1.B);
                    LAB2RGB(L, a, b, ref R, ref G, ref B);
                }
                else
                {
                    IRgb rgb = Labs[labidx].Item1.To<Rgb>();
                    R = Convert.ToInt32(rgb.R);
                    G = Convert.ToInt32(rgb.G);
                    B = Convert.ToInt32(rgb.B);
                }
                Labs[labidx].Item2.R = R;
                Labs[labidx].Item2.G = G;
                Labs[labidx].Item2.B = B;

                Brush brush = new SolidBrush(Color.FromArgb(R, G, B));
                int x = vert ? labidx / rows : labidx % cols;
                int y = vert ? labidx % rows : labidx / cols;
                g.FillRectangle(brush, x * pixelsize, y * pixelsize, pixelsize, pixelsize);
            }
            Zone.Image = bmp;
            new Bitmap(1, 1)
                //bmp
                .Save(Path.ChangeExtension(openFileDialog1.FileName, ".bmp"), ImageFormat.Bmp);
            Generated = true;
        }

        private void toolbtnMatrixDrawingMajority_Click(object sender, EventArgs e)
        {
            toolbtnMatrixDrawingMajority.Text = toolbtnMatrixDrawingMajority.Text == RowMajor ? ColMajor : RowMajor;
        }

        private void ResizeZone(System.Drawing.Rectangle candidate)
        {
            if (ZoneSize.Width < candidate.Right || ZoneSize.Height < candidate.Bottom)
                ZoneSize = new Size(Math.Max(candidate.Right, ZoneSize.Width),
                    Math.Max(candidate.Bottom, ZoneSize.Height));
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeZone(ZoneHolder.ClientRectangle);
        }

        private void Zone_MouseMove(object sender, MouseEventArgs e)
        {
            statusStrip1.Items[0].Text = "";
            if (!Generated)
                return;
            int labidx = (OptionRowMajor ? e.X : e.Y) / OptionSize * OptionRows + (OptionRowMajor ? e.Y : e.X) / OptionSize;
            if (labidx >= Labs.Count)
                return;
            Rgb rgb = Labs[labidx].Item2;
            Lab lab = Labs[labidx].Item1;
            statusStrip1.Items[0].Text = string.Format("L:{0} A:{1} B:{2} R:{3} G:{4} B:{5}", lab.L, lab.A, lab.B, rgb.R, rgb.G, rgb.B);
        }

        // http://stackoverflow.com/questions/7880264/convert-lab-color-to-rgb
        void LAB2RGB(int L, int a, int b, ref int R, ref int G, ref int B)
        {
            double X, Y, Z, fX, fY, fZ;
            int RR, GG, BB;

            fY = Math.Pow((L + 16.0) / 116.0, 3.0);
            if (fY < 0.008856)
                fY = L / 903.3;
            Y = fY;

            if (fY > 0.008856)
                fY = Math.Pow(fY, 1.0 / 3.0);
            else
                fY = 7.787 * fY + 16.0 / 116.0;

            fX = a / 500.0 + fY;
            if (fX > 0.206893)
                X = Math.Pow(fX, 3.0);
            else
                X = (fX - 16.0 / 116.0) / 7.787;

            fZ = fY - b / 200.0;
            if (fZ > 0.206893)
                Z = Math.Pow(fZ, 3.0);
            else
                Z = (fZ - 16.0 / 116.0) / 7.787;

            X *= (0.950456 * 255);
            Y *= 255;
            Z *= (1.088754 * 255);

            RR = (int)(3.240479 * X - 1.537150 * Y - 0.498535 * Z + 0.5);
            GG = (int)(-0.969256 * X + 1.875992 * Y + 0.041556 * Z + 0.5);
            BB = (int)(0.055648 * X - 0.204043 * Y + 1.057311 * Z + 0.5);

            R = (int)(RR < 0 ? 0 : RR > 255 ? 255 : RR);
            G = (int)(GG < 0 ? 0 : GG > 255 ? 255 : GG);
            B = (int)(BB < 0 ? 0 : BB > 255 ? 255 : BB);
        }

        private void toolbtnAlgo_Click(object sender, EventArgs e)
        {
            if (toolbtnAlgo.Text == AlgoNew)
                toolbtnAlgo.Text = AlgoOld;
            else
                toolbtnAlgo.Text = AlgoNew;
        }

        private void toolbtnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            File.WriteAllLines(
                saveFileDialog1.FileName,
                Labs.Select(x => string.Join("\t", new string[] { 
                    x.Item1.L.ToString(), x.Item1.A.ToString(), x.Item1.B.ToString(), 
                    x.Item2.R.ToString(), x.Item2.G.ToString(), x.Item2.B.ToString() })),
                Encoding.Default);
        }
    }
}
