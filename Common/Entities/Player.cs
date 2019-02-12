using GlLib.Client.Graphic;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Entities
{
    public class Player : Entity
    {
        public string _nickname = "Player";
        public double _accelValue = 0.005;

        public Player(string nickname, World world, RestrictedVector3D position) : base(world, position)
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
            base.Update();
        }

        public override void Render(PlanarVector xAxis, PlanarVector yAxis)
        {
            GL.PushMatrix();
            Texture btexture = Vertexer.LoadTexture("one_and_a_half_sword.png");
            Vertexer.BindTexture(btexture);
            //Vertexer.DrawTexturedModalRect(btexture,0, 0, 0, 0, btexture.width, btexture.height);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(60, -60, 1, 0);
            Vertexer.VertexWithUvAt(60, 60, 1, 1);
            Vertexer.VertexWithUvAt(-60, 60, 0, 1);
            Vertexer.VertexWithUvAt(-60, -60, 0, 0);

            Vertexer.Draw();
            GL.PopMatrix();
        }

        public override void LoadFromNbt(NbtTag tag)
        {
            _nickname = tag.GetString("Name");
            base.LoadFromNbt(tag);
        }
    }
}