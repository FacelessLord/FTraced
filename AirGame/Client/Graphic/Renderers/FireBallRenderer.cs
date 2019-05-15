using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.API;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;

namespace GlLib.Client.Graphic.Renderers
{
    internal class FireBallRenderer : EntityRenderer
    {
        private ISprite _sprite;
        public override void Setup(Entity _e)
        {
            TextureLayout layout = new TextureLayout(@"12_nebula_spritesheet.png", 8, 8);
            _sprite = new LinearSprite(layout, 8*7+5, 6);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            _sprite.Render();
        }
    }
}
