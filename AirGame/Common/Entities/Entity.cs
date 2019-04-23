using System;
using System.Net.Json;
using GlLib.Common.API;
using GlLib.Common.Events;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class Entity : IJsonSerializable
    {
        public Chunk chunkObj;

        public bool isDead;
        public PlanarVector maxVel = new PlanarVector(0.1, 0.2);

        public NbtTag nbtTag = new NbtTag();

        public bool noClip;
        protected RestrictedVector3D position = new RestrictedVector3D();

        public PlanarVector velocity = new PlanarVector();
        public World worldObj;

        public Entity(World _world, RestrictedVector3D _position)
        {
            worldObj = _world;
            Position = _position;
        }

        public Entity()
        {
        }

        public RestrictedVector3D Position
        {
            get => position;
            set
            {
                position = value;
                chunkObj = GetProjection(position, worldObj);
            }
        }

        public AxisAlignedBb GetAaBb()
        {
            return Position.ToPlanar().ExpandBothTo(1, 1);
        }

        public TerrainBlock GetUnderlyingBlock()
        {
            return chunkObj[Position.Ix % 16, Position.Iy % 16];
        }

        public virtual void Update()
        {
            if (EventBus.OnEntityUpdate(this)) return;

            if (velocity.Length > maxVel.Length) velocity *= maxVel.Length / velocity.Length;
            MoveEntity();
            velocity *= 0.85;
            var entities = worldObj.GetEntitiesWithinAaBbAndHeight(GetAaBb(), position.z);
            foreach (var entity in entities) OnCollideWith(entity);
        }

        private void MoveEntity()
        {
            var oldPos = position;
            //PlanarVector dVelocity = _velocity / (_velocity.Length * 10);

            Position += velocity;
            var proj = chunkObj;
            if (proj != null && proj.isLoaded)
            {
                if (chunkObj != proj) ((ServerWorld) worldObj).ChangeEntityChunk(this, proj);
            }
            else
            {
                Position = oldPos;
                velocity = new PlanarVector();
            }
        }

        public static Chunk GetProjection(RestrictedVector3D _vector, World _world)
        {
            if (_vector.Ix < 0 || _vector.Iy < 0) return null;
            try
            {
                return _world[_vector.Ix / 16, _vector.Iy / 16];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetDead(bool _dead)
        {
            isDead = _dead;
        }

        public virtual void OnCollideWith(Entity _obj)
        {
        }

        public virtual void Render(PlanarVector _xAxis, PlanarVector _yAxis)
        {
        }

        public virtual string GetName()
        {
            return "entity.null";
        }

        public virtual void LoadFromJsonObject(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonObjectCollection collection)
            {
                Position = RestrictedVector3D.FromString(((JsonStringValue)collection[1]).Value);
                velocity = PlanarVector.FromString(((JsonStringValue)collection[2]).Value);
                maxVel = PlanarVector.FromString(((JsonStringValue)collection[3]).Value);
                worldObj = Proxy.GetServer().GetWorldById((int)((JsonNumericValue)collection[4]).Value);
                isDead = ((JsonLiteralValue) collection[5]).Value == JsonAllowedLiteralValues.True;
                noClip = ((JsonLiteralValue) collection[6]).Value == JsonAllowedLiteralValues.True;
                if (collection.Count > 7)
                    nbtTag = NbtTag.FromString(((JsonStringValue)collection[collection.Count-1]).Value);
            }
        }

        public virtual JsonObject CreateJsonObject()
        {
            JsonObjectCollection jsonObj = new JsonObjectCollection("entity");
            jsonObj.Add(new JsonStringValue("entityId", GetName()));
            if (position != null && velocity != null && maxVel != null && worldObj != null)
            {
                jsonObj.Add(new JsonStringValue("Position", Position + ""));
                jsonObj.Add(new JsonStringValue("Velocity", velocity + ""));
                jsonObj.Add(new JsonStringValue("MaxVelocity", maxVel + ""));
                jsonObj.Add(new JsonNumericValue("WorldId", worldObj.worldId));
                jsonObj.Add(new JsonLiteralValue(isDead+""));
                jsonObj.Add(new JsonLiteralValue(noClip+""));
                if (nbtTag != null)
                    jsonObj.Add(new JsonStringValue("entityTag", nbtTag+""));
            }
            return jsonObj;
        }

        protected bool Equals(Entity _other)
        {
            return Equals(GetName(), _other.GetName()) &&
                   Equals(worldObj, _other.worldObj) &&
                   Equals(position, _other.position) &&
                   Equals(velocity, _other.velocity) &&
                   Equals(maxVel, _other.maxVel) &&
                   Equals(chunkObj, _other.chunkObj) &&
                   Equals(nbtTag, _other.nbtTag) &&
                   isDead == _other.isDead &&
                   noClip == _other.noClip;
        }

        public override bool Equals(object _obj)
        {
            if (ReferenceEquals(null, _obj)) return false;
            if (ReferenceEquals(this, _obj)) return true;
            if (_obj.GetType() != GetType()) return false;
            return Equals((Entity) _obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = worldObj != null ? worldObj.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (position != null ? position.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (velocity != null ? velocity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (maxVel != null ? maxVel.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (chunkObj != null ? chunkObj.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (nbtTag != null ? nbtTag.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ isDead.GetHashCode();
                hashCode = (hashCode * 397) ^ noClip.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Entity _left, Entity _right)
        {
            return Equals(_left, _right);
        }

        public static bool operator !=(Entity _left, Entity _right)
        {
            return !Equals(_left, _right);
        }
    }
}