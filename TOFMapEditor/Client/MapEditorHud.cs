using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.API.Gui;
using GlLib.Client.Graphic;
using GlLib.Common;
using OpenTK;

namespace TOFMapEditor.Client
{
    internal class MapEditorHud : GuiFrame
    {
        public MapEditorHud()
        {
            AddRectangle(16, 16, 64, 64);
            AddRectangle(16, 20 + 64, 64, 20);
            AddText("HI", 16, 16 + 64, 64, 30);
            AddHorizontalBar(100, 100, 500, 50, new Color(40, 60, 240, 255));
            AddHorizontalBar(100, 150, 500, 50, new Color(240, 60, 40, 255));
            AddNumeric(350, 80, 20, 10);
        }
    }
}
