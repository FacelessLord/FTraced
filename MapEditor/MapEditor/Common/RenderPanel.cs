using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MapEditor.Common
{
    public class RenderPanel : Panel
    {
        public  TerrainBlock Brush { get; set; }
        private const int ColumnWeight = 44;
        private const int ChunkSize = 16;
        internal TerrainBlock[,] blocks = new TerrainBlock[ChunkSize, ChunkSize];
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
                blocks[_e.X / ColumnWeight, _e.Y / ColumnWeight] = Brush;
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        protected override void OnMouseClick(MouseEventArgs _e)
        {
            blocks[_e.X / ColumnWeight , _e.Y / ColumnWeight] = Brush;
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

            for (int i = 0; i < blocks.GetLength(0); i++)
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                if (!(blocks[i,j] is null))
                {
                    var position = new Point(i * ColumnWeight + 1, j * ColumnWeight + 1);
                    var image = (Bitmap) Image.FromFile("textures/" + blocks[i, j].GetTextureName(1,1)).Clone();
                    _e.DrawImage(image, position.X,position.Y, ColumnWeight - 1, ColumnWeight -1 );
                }
            }

            p = new Pen(Color.Green, 5);
            var rectSize = new Size(ColumnWeight, ColumnWeight);
            var rect = new Rectangle(MousePosition, rectSize);
            _e.DrawRectangle(p, rect);
        }
    }
}