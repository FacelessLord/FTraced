using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.API;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    internal class FireBallRenderer : EntityRenderer
    {
        private double rotation;
        private ISprite _sprite;

        public FireBallRenderer(PlanarVector _aim, Direction direction)
        {
            if (_aim == new PlanarVector(0, 0))
                switch (direction)
                {
                    case Direction.Right:
                        rotation = 0;
                        break;
                    case Direction.Left:
                        rotation = Math.PI;
                        break;
                }
            else rotation = _aim.Angle;
        }
        public override void Setup(Entity _e)
        {
            TextureLayout layout = new TextureLayout(@"12_nebula_spritesheet.png", 8, 8);
            _sprite = new LinearSprite(layout, 8*7+5, 6);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            GL.Rotate((180 / Math.PI) * rotation + 270, OpenTK.Vector3d.UnitZ);
            _sprite.Render();
            GL.PopMatrix();
        }
    }
}
