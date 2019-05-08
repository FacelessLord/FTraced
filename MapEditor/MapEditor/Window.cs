using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapEditor.Common;

namespace MapEditor
{
    public class Window : Form
    {
        private readonly TerrainBlock[] Brushes = {new Bricks(), new GrassBlock()};
        private PictureBox BrushPicture;

        private ListView brushView;
        private IContainer components;

        private RenderPanel PaintField;

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
            brushView.SmallImageList = new ImageList();
            PaintField.Paint += OnPaint;
            brushView.ItemActivate += ItemActivate;
            Load += OnLoad;
        }

        private void TimerTick(object _sender, EventArgs _e)
        {
            BrushPicture.Image = Image.FromFile(
                "textures/"
                + PaintField.Brush.GetTextureName(0,0));
            PaintField.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs _e)
        {
            //MousePosition = new Point(_e.X , _e.Y);
        }

        protected void OnPaint(object _sender, PaintEventArgs _e)
        {
            var g = _e.Graphics;
            PaintField.DrawField(g);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            var items = brushView.Items;
            int imageNumber = 0;

            foreach (var value in Brushes)
                try
                {
                    var image = Image.FromFile("textures/" + value.GetTextureName(0, 0));
                    brushView.SmallImageList.Images.Add(image);
                    items.Add(value.GetName(), imageNumber);
                    imageNumber++;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Images adding exception: " + exp.Message);
                }
        }

        private void ItemActivate(object sender, EventArgs e)
        {
            PaintField.Brush = Brushes[brushView.SelectedItems[0].Index];
        }

        private void InitializeComponent()
        {

            this.brushView = new System.Windows.Forms.ListView();
            this.BrushPicture = new System.Windows.Forms.PictureBox();
            this.PaintField = new RenderPanel();
            ((System.ComponentModel.ISupportInitialize)(this.BrushPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // brushView
            // 
            this.brushView.Location = new System.Drawing.Point(16, 100);
            this.brushView.Name = "brushView";
            this.brushView.Size = new System.Drawing.Size(151, 300);
            this.brushView.TabIndex = 1;
            this.brushView.UseCompatibleStateImageBehavior = false;
            this.brushView.View = System.Windows.Forms.View.List;
            // 
            // BrushPicture
            // 
            this.BrushPicture.Location = new System.Drawing.Point(50, 12);
            this.BrushPicture.Name = "BrushPicture";
            this.BrushPicture.Size = new System.Drawing.Size(66, 65);
            this.BrushPicture.TabIndex = 2;
            this.BrushPicture.TabStop = false;
            this.BrushPicture.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // PaintField
            // 
            this.PaintField.AutoScroll = true;
            this.PaintField.Location = new System.Drawing.Point(180, 0);
            this.PaintField.MousePosition = new System.Drawing.Point(0, 0);
            this.PaintField.Name = "PaintField";
            this.PaintField.Size = new System.Drawing.Size(705, 705);
            this.PaintField.TabIndex = 0;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 711);
            this.Controls.Add(this.BrushPicture);
            this.Controls.Add(this.brushView);
            this.Controls.Add(this.PaintField);
            this.MaximumSize = new System.Drawing.Size(1300, 750);
            this.MinimumSize = new System.Drawing.Size(1300, 750);
            this.Name = "Window";
            ((System.ComponentModel.ISupportInitialize)(this.BrushPicture)).EndInit();
            this.ResumeLayout(false);

        }
    }
}