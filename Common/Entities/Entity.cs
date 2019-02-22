using System;
using System.Collections.Generic;
using System.Net.Json;
using GlLib.Common.Events;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class Entity
    {
        public World _worldObj;
        public RestrictedVector3D _position;

        public RestrictedVector3D Position
        {
            get => _position;
            set
            {
                _position = value;
                _chunkObj = GetProjection(_position, _worldObj);
            }
        }

        public PlanarVector _velocity = new PlanarVector();
        public PlanarVector _acceleration = new PlanarVector();
        public PlanarVector _maxVel = new PlanarVector(0.07,0.07);
        public Chunk _chunkObj;

        public NbtTag _nbtTag = new NbtTag();

        public bool _isDead = false;

        public bool _noClip = false;


        public Entity(World world, RestrictedVector3D position)
        {
            _worldObj = world;
           Position = position;
        }
        
        public Entity()
        {
        }

        public AxisAlignedBb GetAaBb()
        {
            return Position.ToPlanar().ExpandBothTo(1, 1);
        }

        public TerrainBlock GetUnderlyingBlock()
        {
            return _chunkObj[Position.Ix % 16, Position.Iy % 16];
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
            MoveEntity();

            List<Entity> entities = _worldObj.GetEntitiesWithinAaBbAndHeight(GetAaBb(), _position._z);
            foreach (var entity in entities)
            {
                OnCollideWith(entity);
            }
        }

        private void MoveEntity()
        {
            RestrictedVector3D oldPos = _position;
            //PlanarVector dVelocity = _velocity / (_velocity.Length * 10);

            Position += _velocity;
            Chunk proj = this._chunkObj;
            if (proj != null && proj._isLoaded)
            {
                if (_chunkObj != proj)
                {
                    _worldObj.ChangeEntityChunk(this, proj);
                }
            }
            else
            {
                Position = oldPos;
                _velocity = new PlanarVector();
            }
        }

        public static Chunk GetProjection(RestrictedVector3D vector,World world)
        {
            if (vector.Ix < 0 || vector.Iy < 0)
            {
                return null;
            }
            try
            {
                return world[vector.Ix / 16, vector.Iy / 16];
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

        public static Entity LoadFromJson(JsonObjectCollection collection, World world, Chunk chunk)
        {
            double posX = 0;
            double posY = 0;
            int z = 0;
            double velX = 0;
            double velY = 0;
            string id = "null";
            string rawTag = "";

            foreach (var entityField in collection)
            {
                if (entityField is JsonArrayCollection arr)
                {
                    switch (arr.Name)
                    {
                        case "pos":
                            (posX, posY, z) = (((JsonNumericValue) arr[0]).Value,
                                ((JsonNumericValue) arr[1]).Value,
                                (int) ((JsonNumericValue) arr[2]).Value);
                            break;
                        case "vel":
                            (velX, velY) = (((JsonNumericValue) arr[0]).Value,
                                ((JsonNumericValue) arr[1]).Value);
                            break;
                    }
                }

                if (entityField is JsonStringValue str)
                {
                    switch (str.Name)
                    {
                        case "id":
                            id = str.Value;
                            break;
                        case "tag":
                            rawTag = str.Value;
                            break;
                    }
                }
            }

            Entity entity = 
                GameRegistry.GetEntityFromName(id, world, new RestrictedVector3D(posX, posY, z));
            entity._velocity = new PlanarVector(velX, velY);
            entity._chunkObj = chunk;
            entity._nbtTag = NbtTag.FromString(rawTag);
            entity.LoadFromNbt(entity._nbtTag);
            return entity;
        }

        public JsonObjectCollection CreateJsonObj()
        {
            JsonNumericValue xJs = new JsonNumericValue(_position._x);
            JsonNumericValue yJs = new JsonNumericValue(_position._y);
            JsonNumericValue zJs = new JsonNumericValue(_position._z);
            JsonArrayCollection posColl = new JsonArrayCollection("pos", new[] {xJs, yJs, zJs});
            JsonNumericValue vXJs = new JsonNumericValue(_velocity._x);
            JsonNumericValue vYJs = new JsonNumericValue(_velocity._y);
            JsonArrayCollection velColl = new JsonArrayCollection("vel", new[] {vXJs, vYJs});
            JsonStringValue id = new JsonStringValue("id",GetName());
            SaveToNbt(_nbtTag);
            JsonStringValue tag = new JsonStringValue("tag",_nbtTag.ToString());

            return new JsonObjectCollection($"entity{GetHashCode()}", 
                new JsonObject[] {posColl, velColl, id, tag});
        }

        protected bool Equals(Entity other)
        {
            return Equals(_worldObj, other._worldObj) &&
                   Equals(_position, other._position) &&
                   Equals(_velocity, other._velocity) &&
                   Equals(_acceleration, other._acceleration) &&
                   Equals(_maxVel, other._maxVel) &&
                   Equals(_chunkObj, other._chunkObj) &&
                   Equals(_nbtTag, other._nbtTag) &&
                   _isDead == other._isDead &&
                   _noClip == other._noClip;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_worldObj != null ? _worldObj.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_position != null ? _position.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_velocity != null ? _velocity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_acceleration != null ? _acceleration.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_maxVel != null ? _maxVel.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_chunkObj != null ? _chunkObj.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_nbtTag != null ? _nbtTag.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _isDead.GetHashCode();
                hashCode = (hashCode * 397) ^ _noClip.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}