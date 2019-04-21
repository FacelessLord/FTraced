using System;
using System.Collections.Generic;
using GlLib.Client.Graphic;
using GlLib.Common.Map;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Entities
{
    public class Player : Entity
    {
        private PlayerData _playerData;
        public double accelerationValue = 0.05;
        public string nickname = "Player";
        public HashSet<string> usedBinds = new HashSet<string>();

        public Player(string _nickname, World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            this.nickname = _nickname;
        }

        public Player(World _world, RestrictedVector3D _position) : base(_world, _position)
        {
        }

        public Player()
        {
        }

        public PlayerData Data
        {
            get => _playerData;
            set
            {
                _playerData = value;
                position = value.position;
                worldObj = Proxy.GetServer().GetWorldById(value.worldId);
            }
        }

        public override string GetName()
        {
            return "entity.player";
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Render(PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
            var btexture = Vertexer.LoadTexture("player.png");
            Vertexer.BindTexture(btexture);

            Vertexer.StartDrawingQuads();

            Vertexer.VertexWithUvAt(10, -10, 1, 0);
            Vertexer.VertexWithUvAt(10, 10, 1, 1);
            Vertexer.VertexWithUvAt(-10, 10, 0, 1);
            Vertexer.VertexWithUvAt(-10, -10, 0, 0);

            Vertexer.Draw();
            GL.PopMatrix();
        }

        public override void LoadFromNbt(NbtTag _tag)
        {
            nickname = _tag.GetString("name");
            Data = PlayerData.LoadFromNbt(_tag);
            base.LoadFromNbt(_tag);
        }

        public override void SaveToNbt(NbtTag _tag)
        {
            _tag.SetString("name", nickname);
            if (Data != null)
                Data.SaveToNbt(_tag);
            base.SaveToNbt(_tag);
        }

        public void CheckVelocity()
        {
            if (velocity.Length > maxVel.Length) velocity *= maxVel.Length / velocity.Length;
        }
    }
}