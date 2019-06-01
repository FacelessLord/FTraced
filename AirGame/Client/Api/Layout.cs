namespace GlLib.Client.API
{
    public class Layout
    {
        public int countX;
        public int countY;
        public int endU;
        public int endV;
        public int height;
        public int startU;
        public int startV;
        public int width;

        public Layout(int _width, int _height, int _startU, int _startV, int _endU, int _endV, int _countX, int _countY)
        {
            (width, height) = (_width, _height);
            (startU, startV) = (_startU, _startV);
            (endU, endV) = (_endU, _endV);
            countX = _countX;
            countY = _countY;
        }

        public Layout(int _width, int _height, int _countX, int _countY)
        {
            (width, height) = (_width, _height);
            (startU, startV) = (0, 0);
            (endU, endV) = (_width, _height);
            countX = _countX;
            countY = _countY;
        }

        public (long, long, long, long) GetFrameUv(long _frame)
        {
            return (FrameStartU(_frame), FrameEndU(_frame),
                FrameStartV(_frame), FrameEndV(_frame));
        }

        public (float, float, float, float) GetFrameUvProportions(long _frame)
        {
            return (FrameStartUProportion(_frame), FrameStartVProportion(_frame),
                FrameEndUProportion(_frame), FrameEndVProportion(_frame));
        }

        public int FrameWidth()
        {
            return (endU - startU) / countX;
        }

        public int FrameHeight()
        {
            return (endV - startV) / countY;
        }

        public long FrameStartU(long _frameNumber)
        {
            return startU + (endU - startU) / countX * (_frameNumber % countX);
        }

        public long FrameEndU(long _frameNumber)
        {
            return startU + (endU - startU) / countX * (_frameNumber % countX + 1);
        }

        public long FrameStartV(long _frameNumber)
        {
            return startV + (endV - startV) / countY * (_frameNumber / countX);
        }

        public long FrameEndV(long _frameNumber)
        {
            return startV + (endV - startV) / countY * (_frameNumber / countX + 1);
        }

        public float FrameStartUProportion(long _frameNumber)
        {
            return (startU + (endU - startU) / (float) countX * (_frameNumber % countX)) / width;
        }

        public float FrameEndUProportion(long _frameNumber)
        {
            return (startU + (endU - startU) / (float) countX * (_frameNumber % countX + 1)) / width;
        }

        public float FrameStartVProportion(long _frameNumber)
        {
            return (startV + (endV - startV) / (float) countY * (_frameNumber / countX)) / height;
        }

        public float FrameEndVProportion(long _frameNumber)
        {
            return (startV + (endV - startV) / (float) countY * (_frameNumber / countX + 1)) / height;
        }
    }
}