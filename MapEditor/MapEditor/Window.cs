using System;
using System.Drawing;
using System.Windows.Forms;
using MapEditor.Common;

namespace MapEditor
{
    public partial class Window : Form
    {
        private const int ColumnWeight = 32;
        private const int ChunkSize = 16;
        private new Point MousePosition => PaintField.MousePosition;
        
        public Window()
        {
            SetStyle(
                ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint
                                           | ControlStyles.UserPaint, true);
            UpdateStyles();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            var timer = new Timer
            {
                Interval = 60
            };
            timer.Tick += TimerTick;
            timer.Start();
            InitializeComponent();
            PaintField.Paint += OnPaint;
        }

        private void TimerTick(object _sender, EventArgs _e)
        {
            PaintField.Invalidate();
        }


        protected override void OnMouseMove(MouseEventArgs _e)
        {
            //MousePosition = new Point(_e.X , _e.Y);
        }
        protected void OnPaint(object _sender, PaintEventArgs _e)
        {
            //var g = PaintField.CreateGraphics();
            var g = _e.Graphics;
            PaintField.DrawField(g);


        }

        private void DrawField(Graphics _e)
        {
            var p = new Pen(Color.Black, 3);
            for (int i = 0; i <= 16; i++)
            {
                _e.DrawLine(p, i * ColumnWeight, 0, ColumnWeight *  i, ColumnWeight * ChunkSize);
                _e.DrawLine(p,0,  i * ColumnWeight, ColumnWeight * ChunkSize, i * ColumnWeight);
            }
            p = new Pen(Color.Green, 5);
            var rectSize = new Size(ColumnWeight, ColumnWeight);
            var rect = new Rectangle(MousePosition, rectSize);
            _e.DrawRectangle(p, rect);
        }

        private RenderPanel PaintField;

        private void InitializeComponent()
        {
            this.PaintField = new RenderPanel();
            this.SuspendLayout();
            // 
            // PaintField
            // 
            this.PaintField.Location = new System.Drawing.Point(180, 24);
            this.PaintField.MousePosition = new System.Drawing.Point(0, 0);
            this.PaintField.Name = "PaintField";
            this.PaintField.Size = new System.Drawing.Size(515, 515);
            this.PaintField.TabIndex = 0;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 582);
            this.Controls.Add(this.PaintField);
            this.Name = "Window";
            this.Text = "Window";
            this.ResumeLayout(false);

        }
    }
}
