using System;
using System.Net.Json;
using GlLib.Common.Events;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class Entity
    {
        public PlanarVector acceleration = new PlanarVector();
        public Chunk chunkObj;

        public bool isDead;
        public PlanarVector maxVel = new PlanarVector(0.07, 0.07);

        public NbtTag nbtTag = new NbtTag();

        public bool noClip;
        protected RestrictedVector3D position;

        public PlanarVector velocity = new PlanarVector();
        public World worldObj;

        public Entity(World world, RestrictedVector3D position)
        {
            worldObj = world;
            Position = position;
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

            if (acceleration.y == 0 && acceleration.x == 0) velocity *= 0.95;
            velocity += acceleration;
            if (Math.Abs(velocity.x) > maxVel.x) velocity.x *= maxVel.x / Math.Abs(velocity.x);
            if (Math.Abs(velocity.y) > maxVel.y) velocity.y *= maxVel.y / Math.Abs(velocity.y);
            MoveEntity();

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

        public static Chunk GetProjection(RestrictedVector3D vector, World world)
        {
            if (vector.Ix < 0 || vector.Iy < 0) return null;
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
            isDead = dead;
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
            tag.SetString("EntityId", GetName());
            if (position != null && velocity != null && maxVel != null && acceleration != null && worldObj != null)
            {
                tag.SetString("Position", position + "");
                tag.SetString("Velocity", velocity + "");
                tag.SetString("MaxVelocity", maxVel + "");
                tag.SetString("Acceleration", acceleration + "");
                tag.SetInt("WorldId", worldObj.worldId);
                tag.SetBool("IsDead", isDead);
                tag.SetBool("noclip", noClip);
                if (nbtTag != null)
                    tag.AppendTag(nbtTag, "EntityTag");
            }
        }

        public virtual void LoadFromNbt(NbtTag tag)
        {
            position = RestrictedVector3D.FromString(tag.GetString("Position"));
            velocity = PlanarVector.FromString(tag.GetString("Velocity"));
            maxVel = PlanarVector.FromString(tag.GetString("MaxVelocity"));
            acceleration = PlanarVector.FromString(tag.GetString("Acceleration"));
            worldObj = Proxy.GetServer().GetWorldById(tag.GetInt("WorldId"));
            isDead = tag.GetBool("IsDead");
            noClip = tag.GetBool("noclip");
            if (tag.CanRetrieveTag("EntityTag"))
                nbtTag = tag.RetrieveTag("EntityTag");
        }

        public static Entity LoadFromJson(JsonStringValue rawTag, Chunk chunk)
        {
            var entityTag = NbtTag.FromString(rawTag.Value);

            var entity = Proxy.GetSideRegistry().GetEntityFromName(entityTag.GetString("EntityId"));
            entity.LoadFromNbt(entityTag);
            return entity;
        }

        public JsonStringValue CreateJsonObj()
        {
            var tag = new NbtTag();
            SaveToNbt(tag);
            return new JsonStringValue("Entity" + GetHashCode(), tag.ToString());
        }

        protected bool Equals(Entity other)
        {
            return Equals(GetName(), other.GetName()) &&
                   Equals(worldObj, other.worldObj) &&
                   Equals(position, other.position) &&
                   Equals(velocity, other.velocity) &&
                   Equals(acceleration, other.acceleration) &&
                   Equals(maxVel, other.maxVel) &&
                   Equals(chunkObj, other.chunkObj) &&
                   Equals(nbtTag, other.nbtTag) &&
                   isDead == other.isDead &&
                   noClip == other.noClip;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = worldObj != null ? worldObj.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (position != null ? position.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (velocity != null ? velocity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (acceleration != null ? acceleration.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (maxVel != null ? maxVel.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (chunkObj != null ? chunkObj.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (nbtTag != null ? nbtTag.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ isDead.GetHashCode();
                hashCode = (hashCode * 397) ^ noClip.GetHashCode();
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