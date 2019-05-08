using System.Drawing;
using System.Windows.Forms;

namespace MapEditor.Common
{
    public class RenderPanel : Panel
    {
        private const int ColumnWeight = 32;
        private const int ChunkSize = 16;

        public RenderPanel()
        {
            //Problem with graphics painter 
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        public  new Point MousePosition { get; set; }

        protected override void OnMouseMove(MouseEventArgs _e)
        {
            MousePosition = new Point(_e.X / ColumnWeight * ColumnWeight, _e.Y / ColumnWeight * ColumnWeight);
            base.OnMouseMove(_e);
        }

        internal void DrawField(Graphics _e)
        {
            var p = new Pen(Color.Black, 3);
            for (int i = 0; i <= 16; i++)
            {
                _e.DrawLine(p, i * ColumnWeight, 0, ColumnWeight * i, ColumnWeight * ChunkSize);
                _e.DrawLine(p, 0, i * ColumnWeight, ColumnWeight * ChunkSize, i * ColumnWeight);
            }

            p = new Pen(Color.Green, 5);
            var rectSize = new Size(ColumnWeight, ColumnWeight);
            var rect = new Rectangle(MousePosition, rectSize);
            _e.DrawRectangle(p, rect);
        }
    }
}