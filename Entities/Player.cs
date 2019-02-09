using GlLib.Graphic;
using GlLib.Map;
using GlLib.Registries;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Entities
{
    public class Player : Entity
    {
        public string _nickname = "Player";
        public Player(string nickname,World world, RestrictedVector3D position) : base(world, position)
        {
            _nickname = nickname;
        }
        public Player(World world, RestrictedVector3D position) : base(world, position)
        {
        }

        public override string GetName()
        {
            return "entity.player";
        }

        public override void Update()
        {
            _velocity = Core.Core.playerSpeed;
            base.Update();
        }

        public override void Render(PlanarVector xAxis, PlanarVector yAxis)
        {
            GL.PushMatrix();
            Texture btexture = Blocks.AutumnGrassStone.GetTexture(_position.Ix, _position.Iy);
            Vertexer.BindTexture(btexture);
            //Vertexer.DrawTexturedModalRect(btexture,0, 0, 0, 0, btexture.width, btexture.height);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(20, 0, 1, 0);
            Vertexer.VertexWithUvAt(20, 20, 1, 1);
            Vertexer.VertexWithUvAt(0, 20, 0, 1);
            Vertexer.VertexWithUvAt(0, 0, 0, 0);

            Vertexer.Draw();
            GL.PopMatrix();
        }
    }
}