using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.API;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Common.Map.Blocks;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    public class BoxRenderer : EntityRenderer
    {
        public ISprite boxSprite;

        public override void Setup(Entity _e)
        {
            var layout = new TextureLayout(SimpleStructPath + "Box.png", 1, 1);
            boxSprite = new LinearSprite(layout, 1, 20).SetFrozen();
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            boxSprite.Render();
            GL.PopMatrix();
        }
    }
}
