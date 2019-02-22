using GlLib.Client.Graphic;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Entities
{
    public class Player : Entity
    {
        public string _nickname = "Player";
        public double _accelValue = 0.0005;

        private PlayerData _playerData;

        public PlayerData Data
        {
            get => _playerData;
            set
            {
                _playerData = value;
                _worldObj = value._world;
                _position = value._position;
            }
        }

        public Player(string nickname, World world, RestrictedVector3D position) : base(world, position)
        {
            _nickname = nickname;
        }

        public Player(World world, RestrictedVector3D position) : base(world, position)
        {
        }

        public Player()
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
            Texture btexture = Vertexer.LoadTexture("player.png");
            Vertexer.BindTexture(btexture);
            //Vertexer.DrawTexturedModalRect(btexture,0, 0, 0, 0, btexture.width, btexture.height);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(10, -10, 1, 0);
            Vertexer.VertexWithUvAt(10, 10, 1, 1);
            Vertexer.VertexWithUvAt(-10, 10, 0, 1);
            Vertexer.VertexWithUvAt(-10, -10, 0, 0);

            Vertexer.Draw();
            GL.PopMatrix();
        }

        public override void LoadFromNbt(NbtTag tag, World world)
        {
            _nickname = tag.GetString("Name");
            Data = PlayerData.LoadFromNbt(tag);
            base.LoadFromNbt(tag, world);
        }

        public override void SaveToNbt(NbtTag tag)
        {
            tag.SetString("Name", _nickname);
            if (Data != null)
                Data.SaveToNbt(tag);
            base.SaveToNbt(tag);
        }
    }
}