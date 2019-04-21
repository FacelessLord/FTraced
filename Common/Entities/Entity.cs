using System;
using System.Net.Json;
using GlLib.Common.Events;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class Entity
    {
        public Chunk chunkObj;

        public bool isDead;
        public PlanarVector maxVel = new PlanarVector(0.1, 0.2);

        public NbtTag nbtTag = new NbtTag();

        public bool noClip;
        protected RestrictedVector3D position;

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

        public virtual void SaveToNbt(NbtTag _tag)
        {
            _tag.SetString("EntityId", GetName());
            if (position != null && velocity != null && maxVel != null && worldObj != null)
            {
                _tag.SetString("Position", position + "");
                _tag.SetString("Velocity", velocity + "");
                _tag.SetString("MaxVelocity", maxVel + "");
                _tag.SetInt("WorldId", worldObj.worldId);
                _tag.SetBool("IsDead", isDead);
                _tag.SetBool("noclip", noClip);
                if (nbtTag != null)
                    _tag.AppendTag(nbtTag, "EntityTag");
            }
        }

        public virtual void LoadFromNbt(NbtTag _tag)
        {
            position = RestrictedVector3D.FromString(_tag.GetString("Position"));
            velocity = PlanarVector.FromString(_tag.GetString("Velocity"));
            maxVel = PlanarVector.FromString(_tag.GetString("MaxVelocity"));
            worldObj = Proxy.GetServer().GetWorldById(_tag.GetInt("WorldId"));
            isDead = _tag.GetBool("IsDead");
            noClip = _tag.GetBool("noclip");
            if (_tag.CanRetrieveTag("EntityTag"))
                nbtTag = _tag.RetrieveTag("EntityTag");
        }

        public static Entity LoadFromJson(JsonStringValue _rawTag, Chunk _chunk)
        {
            var entityTag = NbtTag.FromString(_rawTag.Value);

            var entity = Proxy.GetRegistry().GetEntityFromName(entityTag.GetString("EntityId"));
            entity.LoadFromNbt(entityTag);
            return entity;
        }

        public JsonStringValue CreateJsonObj()
        {
            var tag = new NbtTag();
            SaveToNbt(tag);
            return new JsonStringValue("Entity" + GetHashCode(), tag.ToString());
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