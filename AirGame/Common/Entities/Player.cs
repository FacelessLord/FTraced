using System;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Client.Api.Sprites;
using GlLib.Client.Graphic;
using GlLib.Common.Api.Inventory;
using GlLib.Common.Items;
using GlLib.Common.Map;
using GlLib.Common.Registries;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Common.Entities
{
    public class Player : Entity
    {
        public PlayerData data;
        public double accelerationValue = 0.2;
        public string nickname = "Player";
        public HashSet<string> usedBinds = new HashSet<string>();
        public PlayerInventory inventory = new PlayerInventory();

        public Player(string _nickname, World _world, RestrictedVector3D _position) : base(_world, _position)
        {
            nickname = _nickname;
        }

        public Player(World _world, RestrictedVector3D _position) : base(_world, _position)
        {
        }

        public Player()
        {
            inventory.AddItemStack(new ItemStack(Proxy.GetClient().items.varia));
            inventory.AddItemStack(new ItemStack(Proxy.GetClient().items.apple));
            inventory.AddItemStack(new ItemStack(Proxy.GetClient().items.sword));
            inventory.AddItemStack(new ItemStack(Proxy.GetClient().items.armor));
            inventory.AddItemStack(new ItemStack(Proxy.GetClient().items.ring));
        }

        public override string GetName()
        {
            return "entity.player";
        }

        public override void Update()
        {
            base.Update();
        }

        public ISprite playerSprite;

        public override void Render(PlanarVector _xAxis, PlanarVector _yAxis)
        {
            GL.PushMatrix();
//            var btexture = Vertexer.LoadTexture("player.png");
//            Vertexer.BindTexture(btexture);
//
//            Vertexer.StartDrawingQuads();
//
//            Vertexer.VertexWithUvAt(10, -10, 1, 0);
//            Vertexer.VertexWithUvAt(10, 10, 1, 1);
//            Vertexer.VertexWithUvAt(-10, 10, 0, 1);
//            Vertexer.VertexWithUvAt(-10, -10, 0, 0);
//
//            Vertexer.Draw();

            playerSprite.Render();
            GL.PopMatrix();
        }

        public override void LoadFromJsonObject(JsonObject _jsonObject)
        {
            base.LoadFromJsonObject(_jsonObject);
            if (_jsonObject is JsonObjectCollection collection)
            {
                nickname = ((JsonStringValue) collection[7]).Value;
                data = PlayerData.LoadFromNbt(NbtTag.FromString(((JsonStringValue) collection[8]).Value));
            }
        }

        public override JsonObject CreateJsonObject()
        {
            JsonObject obj = base.CreateJsonObject();
            if (obj is JsonObjectCollection collection)
            {
                collection.Add(new JsonStringValue("nickName", nickname));
                if (data != null)
                {
                    NbtTag tag = new NbtTag();
                    data.SaveToNbt(tag);
                    collection.Add(new JsonStringValue("tag", tag.ToString()));
                }
            }

            return obj;
        }

        public void CheckVelocity()
        {
            if (Math.Abs(velocity.x) > maxVel.x) velocity.x *= maxVel.x / Math.Abs(velocity.x);
            if (Math.Abs(velocity.y) > maxVel.y) velocity.y *= maxVel.y / Math.Abs(velocity.y);
        }
    }
}