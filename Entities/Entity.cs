using System;
using System.Collections.Generic;
using DiggerLib;
using GlLib.Events;
using GlLib.Graphic;
using GlLib.Map;
using GlLib.Registries;
using GlLib.Utils;
using OpenTK.Graphics.OpenGL;

namespace GlLib.Entities
{
    public class Entity
    {
        public World _worldObj;
        public RestrictedVector3D _position;
        public PlanarVector _velocity = new PlanarVector();
        public PlanarVector _acceleration = new PlanarVector();
        public PlanarVector _maxVel = new PlanarVector(0.3,0.3);
        public Chunk _chunkObj;

        public NbtTag _nbtTag = new NbtTag();

        public bool _isDead = false;

        public bool _noClip = false;

        
        public Entity(World world, RestrictedVector3D position)
        {
            _worldObj = world;
            _position = position;
            _chunkObj = world._chunks[position.Ix / 16, position.Iy / 16];
        }

        public AxisAlignedBb GetAaBb()
        {
            return _position.ToPlanar().ExpandBothTo(1, 1);
        }

        public TerrainBlock GetUnderlyingBlock()
        {
            return _chunkObj[_position.Ix % 16, _position.Iy % 16];
        }

        public virtual void Update()
        {
            if (EventBus.OnEntityUpdate(this)) return;
            
            if (_acceleration._y == 0 && _acceleration._x == 0)
            {
                _velocity *= 0.95;
            }
            _velocity += _acceleration;
            if (Math.Abs(_velocity._x) > _maxVel._x)
            {
                _velocity._x *= _maxVel._x / Math.Abs(_velocity._x);
            }
            if (Math.Abs(_velocity._y) > _maxVel._y)
            {
                _velocity._y *= _maxVel._y / Math.Abs(_velocity._y);
            }
            MoveEntity(this,_position,_velocity);

            List<Entity> entities = _worldObj.GetEntitiesWithinAaBbAndHeight(GetAaBb(), _position._z);
            foreach (var entity in entities)
            {
                OnCollideWith(entity);
            }
        }

        private void MoveEntity(Entity entity, RestrictedVector3D position, PlanarVector velocity)
        {
            RestrictedVector3D oldPos = _position;
            //PlanarVector dVelocity = _velocity / (_velocity.Length * 10);
            
            _position += _velocity;
            Chunk proj = GetProjection(_position);
            if (proj != null && proj._isLoaded)
            {
                if (_chunkObj != proj)
                {
                    Core.Core.World.ChangeEntityChunk(this, proj);
                }
            }
            else
            {
                _position = oldPos;
            }
        }

        public static Chunk GetProjection(RestrictedVector3D vector)
        {
            if (vector.Ix < 0 || vector.Iy < 0)
            {
                return null;
            }
            try
            {
                return Core.Core.World[vector.Ix / 16, vector.Iy / 16];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void SetDead(bool dead)
        {
            _isDead = dead;
        }

        public virtual void OnCollideWith(Entity obj)
        {
    
        }

        public virtual void Render(PlanarVector xAxis, PlanarVector yAxis)
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

        public virtual string GetName()
        {
            return "entity.null";
        }

        public virtual void SaveToNbt(NbtTag tag)
        {
        }

        public virtual void LoadFromNbt(NbtTag tag)
        {
        }
    }
}