namespace GlLib.Client.API
{
    public class Layout
    {
        public int width;
        public int height;
        public int startU;
        public int startV;
        public int endU;
        public int endV;
        public int countX;
        public int countY;

        public Layout(int _width, int _height, int _startU, int _startV, int _endU, int _endV, int _countX, int _countY)
        {
            (width, height) = (_width, _height);
            (startU, startV) = (_startU, _startV);
            (endU, endV) = (_endU, _endV);
            countX = _countX;
            countY = _countY;
        }

        public (float, float, float, float) GetFrameUv(int _frame)
        {
            return (FrameStartU(_frame), FrameStartV(_frame),FrameEndU(_frame),FrameEndV(_frame));
        }

        public float FrameStartU(int _frameNumber)
        {
            return (startU + (endU - startU) / countX * (_frameNumber % countX)) / (float) width;
        }

        public float FrameEndU(int _frameNumber)
        {
            return (startU + (endU - startU) / countX * (_frameNumber % countX + 1)) / (float) width;
        }

        public float FrameStartV(int _frameNumber)
        {
            return (startV + (endV - startV) / countY * (_frameNumber / countY)) / (float) height;
        }

        public float FrameEndV(int _frameNumber)
        {
            return (startV + (endV - startV) / countY * (_frameNumber / countY + 1)) / (float) height;
        }
    }
}