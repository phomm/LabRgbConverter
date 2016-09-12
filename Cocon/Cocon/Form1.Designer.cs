namespace Cocon
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toollabelCount = new System.Windows.Forms.ToolStripLabel();
            this.toolbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.toolbtnMatrixDrawingMajority = new System.Windows.Forms.ToolStripButton();
            this.toollabelRows = new System.Windows.Forms.ToolStripLabel();
            this.toolcomboRows = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolcomboSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolbtnGenerate = new System.Windows.Forms.ToolStripButton();
            this.Zone = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ZoneHolder = new System.Windows.Forms.Panel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Zone)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.ZoneHolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toollabelCount,
            this.toolbtnOpen,
            this.toolbtnMatrixDrawingMajority,
            this.toollabelRows,
            this.toolcomboRows,
            this.toolStripLabel1,
            this.toolcomboSize,
            this.toolbtnGenerate});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(551, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toollabelCount
            // 
            this.toollabelCount.Name = "toollabelCount";
            this.toollabelCount.Size = new System.Drawing.Size(40, 22);
            this.toollabelCount.Text = "Count";
            // 
            // toolbtnOpen
            // 
            this.toolbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnOpen.Name = "toolbtnOpen";
            this.toolbtnOpen.Size = new System.Drawing.Size(40, 22);
            this.toolbtnOpen.Text = "Open";
            this.toolbtnOpen.Click += new System.EventHandler(this.toolbtnOpen_Click);
            // 
            // toolbtnMatrixDrawingMajority
            // 
            this.toolbtnMatrixDrawingMajority.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnMatrixDrawingMajority.Name = "toolbtnMatrixDrawingMajority";
            this.toolbtnMatrixDrawingMajority.Size = new System.Drawing.Size(109, 22);
            this.toolbtnMatrixDrawingMajority.Text = "RowMajorDrawing";
            this.toolbtnMatrixDrawingMajority.ToolTipText = "Vertical Fill instead of usual Horizontal";
            this.toolbtnMatrixDrawingMajority.Click += new System.EventHandler(this.toolbtnMatrixDrawingMajority_Click);
            // 
            // toollabelRows
            // 
            this.toollabelRows.Name = "toollabelRows";
            this.toollabelRows.Size = new System.Drawing.Size(38, 22);
            this.toollabelRows.Text = "Rows:";
            // 
            // toolcomboRows
            // 
            this.toolcomboRows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcomboRows.Name = "toolcomboRows";
            this.toolcomboRows.Size = new System.Drawing.Size(75, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabel1.Text = "Size:";
            // 
            // toolcomboSize
            // 
            this.toolcomboSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolcomboSize.Name = "toolcomboSize";
            this.toolcomboSize.Size = new System.Drawing.Size(75, 25);
            // 
            // toolbtnGenerate
            // 
            this.toolbtnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbtnGenerate.Name = "toolbtnGenerate";
            this.toolbtnGenerate.Size = new System.Drawing.Size(58, 22);
            this.toolbtnGenerate.Text = "Generate";
            this.toolbtnGenerate.Click += new System.EventHandler(this.toolbtnGenerate_Click);
            // 
            // Zone
            // 
            this.Zone.Location = new System.Drawing.Point(0, 25);
            this.Zone.Name = "Zone";
            this.Zone.Size = new System.Drawing.Size(551, 237);
            this.Zone.TabIndex = 2;
            this.Zone.TabStop = false;
            this.Zone.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Zone_MouseMove);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xlsx";
            this.openFileDialog1.Filter = "xlsx files|*.xlsx";
            this.openFileDialog1.ShowReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(551, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ZoneHolder
            // 
            this.ZoneHolder.AutoScroll = true;
            this.ZoneHolder.Controls.Add(this.Zone);
            this.ZoneHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZoneHolder.Location = new System.Drawing.Point(0, 25);
            this.ZoneHolder.Name = "ZoneHolder";
            this.ZoneHolder.Size = new System.Drawing.Size(551, 215);
            this.ZoneHolder.TabIndex = 4;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 262);
            this.Controls.Add(this.ZoneHolder);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Converter";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Zone)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ZoneHolder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolbtnOpen;
        private System.Windows.Forms.ToolStripButton toolbtnMatrixDrawingMajority;
        private System.Windows.Forms.ToolStripComboBox toolcomboRows;
        private System.Windows.Forms.ToolStripButton toolbtnGenerate;
        private System.Windows.Forms.PictureBox Zone;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripLabel toollabelCount;
        private System.Windows.Forms.ToolStripLabel toollabelRows;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolcomboSize;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel ZoneHolder;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

    }
}

