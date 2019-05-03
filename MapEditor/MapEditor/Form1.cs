using System;
using System.Drawing;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class Form1 : Form
    {
        private const int ColumnWeight = 32;
        private const int ChankSize = 16;
        private new Point MousePosition;
        
        public Form1()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            InitializeComponent();
            var timer = new Timer();
            timer.Interval = 1; 
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            OnPaint(null);
        }

        private void OnFrameChanged(object o, EventArgs e)
        {
            Invalidate();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            MousePosition = new Point(e.X / ColumnWeight, e.Y / ColumnWeight);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            panel1.Invalidate();
            base.OnPaint(e);
            var g = panel1.CreateGraphics();
            DrawField(g);
            //panel1.Update();
            //panel1.Refresh();
            //Application.DoEvents();

        }

        private void DrawField(Graphics e)
        {
            var p = new Pen(Color.Black, 3);
            for (int i = 0; i <= 16; i++)
            {
                e.DrawLine(p, i * ColumnWeight, 0, ColumnWeight *  i, ColumnWeight * ChankSize);
                e.DrawLine(p,0,  i * ColumnWeight, ColumnWeight * ChankSize, i * ColumnWeight);
            }
            var rectSize = new Size(ColumnWeight, ColumnWeight);
            var rect = new Rectangle(MousePosition, rectSize);
            e.DrawRectangle(p, rect);
        }

        private Panel panel1;

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(197, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 389);
            this.panel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 515);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }
    }
}
