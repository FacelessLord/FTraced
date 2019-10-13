using System;
using GlLib.Client.Api.Renderers;
using GlLib.Client.Api.Sprites;
using GlLib.Common.Entities;
using GlLib.Utils;
using GlLib.Utils.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Client.Graphic.Renderers
{
    internal class FireBallRenderer : EntityRenderer
    {
        private LinearSprite _sprite;
        private readonly double rotation;

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

        protected override void Setup(Entity _e)
        {
            var layout = new TextureLayout(Textures.fireBall, 8, 8);
            _sprite = new LinearSprite(layout, 61, 6);
            _sprite.Scale(3f, 2);
            var box = _e.AaBb;
            _sprite.Scale((float) box.Width, (float) box.Height);
        }

        public override void Render(Entity _e, PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            GL.Rotate(180 / Math.PI * rotation + 270, Vector3d.UnitZ);
            _sprite.Render();
            GL.PopMatrix();
        }
    }
}