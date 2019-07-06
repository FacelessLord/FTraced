using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK.Graphics;

namespace GlLib.Client.Graphic.Renderers
{
    internal class BatRenderer : EntityRenderer
    {
        public LinearSprite batSprite;

        public override void Setup(Entity _p)
        {
            var layout = new TextureLayout(Textures.bat, 6, 1);
            batSprite = new LinearSprite(layout, 6, 6);
            var box = _p.AaBb;
            batSprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            batSprite.Render();
        }
    }
}