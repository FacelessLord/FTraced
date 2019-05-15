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

        public (int, int, int, int) GetFrameUv(int _frame)
        {
            return (FrameStartU(_frame), FrameEndU(_frame),
                FrameStartV(_frame), FrameEndV(_frame));
        }

        public (float, float, float, float) GetFrameUvProportions(int _frame)
        {
            return (FrameStartUProportion(_frame), FrameStartVProportion(_frame),
                FrameEndUProportion(_frame), FrameEndVProportion(_frame));
        }

        public int FrameStartU(int _frameNumber)
        {
            return startU + (endU - startU) / countX * (_frameNumber % countX);
        }

        public int FrameEndU(int _frameNumber)
        {
            return startU + (endU - startU) / countX * (_frameNumber % countX + 1);
        }

        public int FrameStartV(int _frameNumber)
        {
            return startV + (endV - startV) / countY * (_frameNumber / countX);
        }

        public int FrameEndV(int _frameNumber)
        {
            return startV + (endV - startV) / countY * (_frameNumber / countX + 1);
        }

        public float FrameStartUProportion(int _frameNumber)
        {
            return (startU + (endU - startU) / (float) countX * (_frameNumber % countX)) / width;
        }

        public float FrameEndUProportion(int _frameNumber)
        {
            return (startU + (endU - startU) / (float) countX * (_frameNumber % countX + 1)) / width;
        }

        public float FrameStartVProportion(int _frameNumber)
        {
            return (startV + (endV - startV) / (float) countY * (_frameNumber / countX)) / height;
        }

        public float FrameEndVProportion(int _frameNumber)
        {
            return (startV + (endV - startV) / (float) countY * (_frameNumber / countX + 1)) / height;
        }
    }
}