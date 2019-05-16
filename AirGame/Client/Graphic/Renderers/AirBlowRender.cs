using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.API;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Client.Graphic.Renderers
{
    internal class AirBlowRenderer : EntityRenderer
    {
        private ISprite _sprite;
        public override void Setup(Entity _e)
        {
            TextureLayout layout = new TextureLayout(@"air_blow.png", 3, 1);
            _sprite = new LinearSprite(layout, 3, 10);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            _sprite.Render();
        }
    }
}
