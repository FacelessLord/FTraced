using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MapEditor.BlocksStruct;

namespace MapEditor.Common
{
    public class RenderPanel : Panel
    {
        public IBlock Brush { get; internal set; }
        private const int ColumnWeight = 44;
        private const int ChunkSize = 16;
        internal IBlock[,] blocks = new IBlock[ChunkSize, ChunkSize];
        private bool drawing = false;

        public RenderPanel()
        {
            Brush = new BlocksStruct.Bricks();
            //Problem with graphics painter 
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        public new Point? MousePosition { get; internal set; }

        protected override void OnMouseMove(MouseEventArgs _e)
        {

            if (_e.X / ColumnWeight < ChunkSize && _e.Y / ColumnWeight < ChunkSize)
                MousePosition = new Point(_e.X / ColumnWeight * ColumnWeight, _e.Y / ColumnWeight * ColumnWeight);
            else MousePosition = null;
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
            try
            {
                blocks[_e.X / ColumnWeight, _e.Y / ColumnWeight] = Brush;
            }
            catch (Exception)
            {
                // ignored
            }
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
                    var image = (Bitmap) Image.FromFile(blocks[i, j].GetTexturePath()).Clone();
                    _e.DrawImage(image, position.X,position.Y, ColumnWeight - 1, ColumnWeight -1 );
                }
            }

            if (MousePosition is null)
                return;
            p = new Pen(Color.Green, 5);
            var rectSize = new Size(ColumnWeight, ColumnWeight);
            var rect = new Rectangle((Point) MousePosition, rectSize);
            _e.DrawRectangle(p, rect);
        }
    }
}