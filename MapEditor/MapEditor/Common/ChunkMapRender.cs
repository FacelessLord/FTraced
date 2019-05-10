using System.Drawing;
using System.Windows.Forms;
using MapEditor.BlocksStruct;

namespace MapEditor.Common
{
    public class ChunkMapRender : Panel
    {
        private const int ColumnWeight = 256 / ChunkCount;
        private const int ChunkCount = 6;

        public ChunkMapRender(RenderPanel _translate, Window _parent)
        {
            Parent = _parent;
            Translate = _translate;
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);

            Chunks = new IBlock[6, 6][,];
            for (int i = 0; i < ChunkCount; i++)
            for (int j = 0; j < ChunkCount; j++)
                Chunks[i, j] = new IBlock[16, 16];
        }

        public Window Parent { get; set; }

        private RenderPanel Translate { get; }
        protected Point ChosenChunk { get; set; }

        private IBlock[,][,] Chunks { get; }


        public new Point? MousePosition { get; set; }

        protected override void OnMouseMove(MouseEventArgs _e)
        {
            base.OnMouseMove(_e);
            if (_e.X / ColumnWeight <= ChunkCount - 1
                && _e.Y / ColumnWeight <= ChunkCount - 1)
                MousePosition = new Point(_e.X / ColumnWeight * ColumnWeight,
                    _e.Y / ColumnWeight * ColumnWeight);
            else
                MousePosition = null;
        }

        protected override void OnMouseClick(MouseEventArgs _e)
        {
            if (_e.X / ColumnWeight <= ChunkCount - 1
                && _e.Y / ColumnWeight <= ChunkCount - 1)
            {
                Chunks[ChosenChunk.X, ChosenChunk.Y] = Translate.blocks;
                ChosenChunk = new Point(_e.X / ColumnWeight, _e.Y / ColumnWeight);
                Translate.blocks = Chunks[ChosenChunk.X, ChosenChunk.Y];
            }
        }


        internal void DrawField(Graphics _e)
        {
            var p = new Pen(Color.Black, 3);
            var rectSize = new Size(ColumnWeight, ColumnWeight);
            var rect = new Rectangle();


            for (int i = 0; i <= 16; i++)
            {
                _e.DrawLine(p, i * ColumnWeight, 0, ColumnWeight * i, ColumnWeight * ChunkCount);
                _e.DrawLine(p, 0, i * ColumnWeight, ColumnWeight * ChunkCount, i * ColumnWeight);
            }


            //Check if empty (almost useless)
            for (int x = 0; x < ChunkCount; x++)
            for (int y = 0; y < ChunkCount; y++)
            for (int i = 0; i < Chunks[x, y].GetLength(0); i++)
            for (int j = 0; j < Chunks[x, y].GetLength(1); j++)
                if (!(Chunks[x, y][i, j] is null))
                    using (Brush brush = new SolidBrush(Color.FromArgb(1, 0, 0, 0)))
                    {
                        _e.FillRectangle(brush,
                            x * ColumnWeight,
                            y * ColumnWeight,
                            rectSize.Width,
                            rectSize.Height);
                    }

            //MousePosition
            if (!(MousePosition is null))
            {
                rect = new Rectangle((Point) MousePosition, rectSize);
                p = new Pen(Color.Green, 5);
                _e.DrawRectangle(p, rect);
            }

            //ChosenChunk
            p = new Pen(Color.Red, 5);
            rect = new Rectangle(ChosenChunk.X * ColumnWeight - 1, ChosenChunk.Y * ColumnWeight - 1, rectSize.Width,
                rectSize.Height);
            _e.DrawRectangle(p, rect);
        }
    }
}