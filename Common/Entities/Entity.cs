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
        public RestrictedVector3D position;

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
                if (chunkObj != proj) worldObj.ChangeEntityChunk(this, proj);
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

        public virtual void LoadFromNbt(NbtTag tag, World world)
        {
            position = RestrictedVector3D.FromString(tag.GetString("Position"));
            velocity = PlanarVector.FromString(tag.GetString("Velocity"));
            maxVel = PlanarVector.FromString(tag.GetString("MaxVelocity"));
            acceleration = PlanarVector.FromString(tag.GetString("Acceleration"));
            worldObj = world.worldId == tag.GetInt("WorldId") ? world : null;
            isDead = tag.GetBool("IsDead");
            noClip = tag.GetBool("noclip");
            if (tag.CanRetrieveTag("EntityTag"))
                nbtTag = tag.RetrieveTag("EntityTag");
        }

        public static Entity LoadFromJson(JsonObjectCollection collection, World world, Chunk chunk)
        {
            double posX = 0;
            double posY = 0;
            var z = 0;
            double velX = 0;
            double velY = 0;
            var id = "null";
            var rawTag = "";

            foreach (var entityField in collection)
            {
                if (entityField is JsonArrayCollection arr)
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

                if (entityField is JsonStringValue str)
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

            var entity =
                Proxy.GetSideRegistry().GetEntityFromName(id, world, new RestrictedVector3D(posX, posY, z));
            entity.velocity = new PlanarVector(velX, velY);
            entity.chunkObj = chunk;
            entity.nbtTag = NbtTag.FromString(rawTag);
            entity.LoadFromNbt(entity.nbtTag, world);
            return entity;
        }

        public JsonObjectCollection CreateJsonObj()
        {
            var xJs = new JsonNumericValue(position.x);
            var yJs = new JsonNumericValue(position.y);
            var zJs = new JsonNumericValue(position.z);
            var posColl = new JsonArrayCollection("pos", new[] {xJs, yJs, zJs});
            var vXJs = new JsonNumericValue(velocity.x);
            var vYJs = new JsonNumericValue(velocity.y);
            var velColl = new JsonArrayCollection("vel", new[] {vXJs, vYJs});
            var id = new JsonStringValue("id", GetName());
            SaveToNbt(nbtTag);
            var tag = new JsonStringValue("tag", nbtTag.ToString());

            return new JsonObjectCollection($"entity{GetHashCode()}",
                new JsonObject[] {posColl, velColl, id, tag});
        }

        protected bool Equals(Entity other)
        {
            return Equals(worldObj, other.worldObj) &&
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