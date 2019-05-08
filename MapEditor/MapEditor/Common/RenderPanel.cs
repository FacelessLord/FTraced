using System;
using System.Drawing;
using System.Windows.Forms;

namespace MapEditor.Common
{
    public class RenderPanel : Panel
    {
        public  TerrainBlock Brush { get; set; }
        private const int ColumnWeight = 44;
        private const int ChunkSize = 16;
        private TerrainBlock[,] Blocks = new TerrainBlock[ChunkSize, ChunkSize];
        private bool drawing = false;

        public RenderPanel()
        {
            Brush = new GrassBlock();
            //Problem with graphics painter 
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        public new Point MousePosition { get; set; }

        protected override void OnMouseMove(MouseEventArgs _e)
        {
            MousePosition = new Point(_e.X / ColumnWeight * ColumnWeight, _e.Y / ColumnWeight * ColumnWeight);
            base.OnMouseMove(_e);
            if (!drawing) return;
            try
            {
                Blocks[_e.X / ColumnWeight, _e.Y / ColumnWeight] = Brush;
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        protected override void OnMouseClick(MouseEventArgs _e)
        {
            Blocks[_e.X / ColumnWeight , _e.Y / ColumnWeight] = Brush;
        }

        protected override void OnMouseDown(MouseEventArgs _e)
        {
            drawing = true;
        }

        protected override void OnMouseUp(MouseEventArgs _e)
        {
            drawing = false;
        }


        internal void DrawField(Graphics _e)
        {
            var p = new Pen(Color.Black, 3);
            for (int i = 0; i <= 16; i++)
            {
                _e.DrawLine(p, i * ColumnWeight, 0, ColumnWeight * i, ColumnWeight * ChunkSize);
                _e.DrawLine(p, 0, i * ColumnWeight, ColumnWeight * ChunkSize, i * ColumnWeight);
            }

            for (int i = 0; i < Blocks.GetLength(0); i++)
            for (int j = 0; j < Blocks.GetLength(1); j++)
            {
                if (!(Blocks[i,j] is null))
                {
                    var position = new Point(i * ColumnWeight + 1, j * ColumnWeight + 1);
                    var image = Image.FromFile("textures/" + Blocks[i, j].GetTextureName(0,0));
                    _e.DrawImage(image, position);
                }
            }

            p = new Pen(Color.Green, 5);
            var rectSize = new Size(ColumnWeight, ColumnWeight);
            var rect = new Rectangle(MousePosition, rectSize);
            _e.DrawRectangle(p, rect);
        }
    }
}