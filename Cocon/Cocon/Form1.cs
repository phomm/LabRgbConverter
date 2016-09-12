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
    using Microsoft.Office.Interop.Excel;
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
            DoubleBuffered = true;
            ZoneSize = new Size(Zone.Size.Width, Zone.Size.Height);

        }
        const string RowMajor = "RowMajorDrawing";
        const string ColMajor = "ColMajorDrawing";

        private List<Tuple<Lab, Rgb>> Labs = new List<Tuple<Lab, Rgb>>();

        Bitmap bmp = null;
        Graphics g;
        bool Generated = false;        
        bool OptionRowMajor = false;        
        int optionSize;
        int optionRows;
        
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
            List<Tuple<Lab, Rgb>> oldlabs = new List<Tuple<Lab, Rgb>>(Labs);
            string oldfilename = openFileDialog1.FileName;
            Excel.Application xlApp = null;
            Excel.Workbook xlWorkbook = null;
            Excel._Worksheet xlWorksheet = null;
            Excel.Range xlRange = null;
            try
            {
                
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    xlApp = new Excel.Application();
                    xlWorkbook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
                    xlRange = xlWorksheet.UsedRange;
                    Labs.Clear();
                    //CultureInfo cult = new CultureInfo("ru-RU");
                    for (int r = 2; r <= xlRange.Rows.Count; r++)
                    {
                        double l = (double)(xlRange.Cells[r, 1] as Excel.Range).Value2;
                        double a = (double)(xlRange.Cells[r, 2] as Excel.Range).Value2;
                        double b = (double)(xlRange.Cells[r, 3] as Excel.Range).Value2;
                        Labs.Add(new Tuple<Lab, Rgb> (new Lab{ L = l, A = a, B = b}, new Rgb{R = 0, G = 0, B = 0})); 
                    }
                    Generated = false;                    
                }
            }
            catch (Exception ex)
            {
                Labs = oldlabs;
                openFileDialog1.FileName = oldfilename;
                MessageBox.Show("Ошибка чтения файла, предыдущие данные не были изменены. Подробности об ошибке\n" + ex.Message);
            }
            finally
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
            //int labidx = 0;

            //for (int i = 0; i <= (vert ? cols : rows); i++)
            //    for (int j = 0; j <= (vert ? rows : cols); j++)
            for (int labidx = 0; labidx < Labs.Count && labidx < 100; labidx++)
                {
                    //if (labidx >= Labs.Count || labidx > 100)
                        
                    IRgb rgb = Labs[labidx].Item1.To<Rgb>();
                    Labs[labidx].Item2.R = rgb.R;
                    Labs[labidx].Item2.G = rgb.G;
                    Labs[labidx].Item2.B = rgb.B;
                    Brush brush = new SolidBrush(Color.FromArgb((int)Math.Round(rgb.R), (int)Math.Round(rgb.G), (int)Math.Round(rgb.B)));
                    int x = vert ? labidx / rows : labidx % cols;
                    int y = vert ? labidx % rows : labidx / cols;
                    g.FillRectangle(brush, x * pixelsize, y * pixelsize, pixelsize, pixelsize);
                }
            Zone.Image = bmp;
            //new Bitmap(1, 1)
                bmp.Save(Path.ChangeExtension(openFileDialog1.FileName, ".bmp"), ImageFormat.Bmp);
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
    }
}
