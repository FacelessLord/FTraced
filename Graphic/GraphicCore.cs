namespace GlLib.Graphic
{
    public static class GraphicCore
    {
        public static GraphicWindow _window;

        public static void Run()
        {
            using (_window = new GraphicWindow(400, 300, "GLLib"))
            {
                _window.Run(60);
            }
        }
    }
}