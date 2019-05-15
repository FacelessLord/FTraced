using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MapEditor.BlocksStruct;
using MapEditor.Common;

namespace MapEditor
{
    public class Window : Form
    {
        private readonly IBlock[] Brushes = {new BlocksStruct.Bricks(), new Grass()};
        private PictureBox BrushPicture { get; set; }

        private ListView BrushView { get; set; }

        private ChunkMapRender ChunkMap { get; set; }
        private IContainer Components { get; set; }

        private MenuStrip MenuStrip { get; set; }
        private OpenFileDialog OpenFileDialog { get; set; }

        private RenderPanel PaintField { get; set; }
        private SaveFileDialog SaveFileDialog { get; set; }

        private Button AddBrush { get; set; }

        public Window()
        {
            //SetStyle(
            //    ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint
            //                               | ControlStyles.UserPaint, true);
            UpdateStyles();

            var timer = new Timer
            {
                Interval = 40
            };
            timer.Tick += TimerTick;
            timer.Start();
            InitializeComponent();

            BrushView.SmallImageList = new ImageList();
            PaintField.Paint += OnFieldPaint;
            BrushView.ItemActivate += BrushItemActivate;
            ChunkMap.Paint += ChunkMap_Paint;
            Load += OnLoad;
            AddBrush.Click += AddBrush_Click;
        }

        private void AddBrush_Click(object _sender, EventArgs _e)
        {
            MessageBox.Show("");
        }

        private void ChunkMap_Paint(object _sender, PaintEventArgs _e)
        {
            var g = _e.Graphics;
            ChunkMap.DrawField(g);
        }

        private void TimerTick(object _sender, EventArgs _e)
        {
            BrushPicture.Image = Image.FromFile(PaintField.Brush.GetTexturePath());
            PaintField.Invalidate();
            ChunkMap.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs _e) { }

        protected void OnFieldPaint(object _sender, PaintEventArgs _e)
        {
            var g = _e.Graphics;
            PaintField.DrawField(g);
        }

        private void OnLoad(object _sender, EventArgs _e)
        {
            var items = BrushView.Items;
            int imageNumber = 0;

            foreach (var value in Brushes)
                try
                {
                    var image = Image.FromFile(value.GetTexturePath());
                    BrushView.SmallImageList.Images.Add(image);
                    items.Add(value.GetName(), imageNumber);
                    imageNumber++;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(@"Images adding exception: " + exp.Message);
                }
        }

        private void BrushItemActivate(object sender, EventArgs e)
        {
            PaintField.Brush = Brushes[BrushView.SelectedItems[0].Index];
        }


        
        private void InitializeComponent()
        {
            BrushView = new ListView();
            BrushPicture = new PictureBox();
            PaintField = new RenderPanel();
            ChunkMap = new ChunkMapRender(PaintField, this);
            SaveFileDialog = new SaveFileDialog();
            OpenFileDialog = new OpenFileDialog();
            MenuStrip = new MenuStrip();
            AddBrush = new Button();
            ((ISupportInitialize) BrushPicture).BeginInit();
            SuspendLayout();
            //
            // AddBrush button
            //
            AddBrush.Location = new Point(15, 402);
            AddBrush.Name = "AddBrushButton";
            AddBrush.Size = new Size(70, 25);
            AddBrush.Text = "Add Brush";
            AddBrush.DialogResult = DialogResult.Yes;
            // 
            // brushView
            // 
            BrushView.Location = new Point(16, 100);
            BrushView.Name = "BrushView";
            BrushView.Size = new Size(150, 300);
            BrushView.TabIndex = 1;
            BrushView.UseCompatibleStateImageBehavior = false;
            BrushView.View = View.List;
            // 
            // BrushPicture
            // 
            BrushPicture.Location = new Point(50, 12);
            BrushPicture.Name = "BrushPicture";
            BrushPicture.Size = new Size(66, 65);
            BrushPicture.SizeMode = PictureBoxSizeMode.Zoom;
            BrushPicture.TabIndex = 2;
            BrushPicture.TabStop = false;
            // 
            // paintField
            // 
            PaintField.Location = new Point(180, 0);
            PaintField.MousePosition = new Point(0, 0);
            PaintField.Name = "PaintField";
            PaintField.Size = new Size(705, 705);
            PaintField.TabIndex = 0;
            // 
            // ChunkMap
            // 
            ChunkMap.Location = new Point(920, 23);
            ChunkMap.Name = "ChunkMap";
            PaintField.MousePosition = new Point(0, 0);
            ChunkMap.Size = new Size(256, 256);
            ChunkMap.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            OpenFileDialog.FileName = "openFileDialog1";
            OpenFileDialog.FileOk += openFileDialog1_FileOk;
            // 
            // menuStrip
            // 
            MenuStrip.Location = new Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Size = new Size(1284, 24);
            MenuStrip.TabIndex = 4;
            MenuStrip.Text = @"menuStrip";
            // 
            // Window
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1284, 711);
            Controls.Add(ChunkMap);
            Controls.Add(BrushPicture);
            Controls.Add(BrushView);
            Controls.Add(PaintField);
            Controls.Add(MenuStrip);
            Controls.Add(AddBrush);
            MainMenuStrip = MenuStrip;
            MaximumSize = new Size(1300, 750);
            MinimumSize = new Size(1300, 726);
            Name = "Window";
            ((ISupportInitialize) BrushPicture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            SaveFileDialog.Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*";
            SaveFileDialog.FilterIndex = 2;
            SaveFileDialog.RestoreDirectory = true;

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                // Can use dialog.FileName
                using (Stream stream = SaveFileDialog.OpenFile())
                {
                    // Save data
                }
        }
    }
}