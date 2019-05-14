using System;
using System.Net.Json;
using GlLib.Client.API;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.API;
using GlLib.Common.Events;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class Entity : IJsonSerializable
    {
        private EntityRenderer _renderer = new StandartRenderer();
        public Chunk chunkObj;

        private bool isDead;
        public PlanarVector maxVel = new PlanarVector(0.3, 0.3);

        public NbtTag nbtTag = new NbtTag();

        public bool noClip;
        protected RestrictedVector3D oldPosition = new RestrictedVector3D();
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

        public RestrictedVector3D OldPosition => oldPosition;

        public RestrictedVector3D Position
        {
            get => position;
            set
            {
                oldPosition = position;
                position = value;
                chunkObj = GetProjection(position, worldObj);
            }
        }

        public virtual void LoadFromJsonObject(JsonObject _jsonObject)
        {
            if (_jsonObject is JsonObjectCollection collection)
            {
                Position = RestrictedVector3D.FromString(((JsonStringValue) collection[1]).Value);
                velocity = PlanarVector.FromString(((JsonStringValue) collection[2]).Value);
                maxVel = PlanarVector.FromString(((JsonStringValue) collection[3]).Value);
                worldObj = Proxy.GetServer().GetWorldById((int) ((JsonNumericValue) collection[4]).Value);
                isDead = ((JsonStringValue) collection[5]).Value == "True";
                noClip = ((JsonStringValue) collection[6]).Value == "True";
                nbtTag = NbtTag.FromString(((JsonStringValue) collection[7]).Value);
            }
        }

        public virtual JsonObject CreateJsonObject()
        {
            var jsonObj = new JsonObjectCollection("entity");
//            SidedConsole.WriteLine((this is Player) +"" + GetName());
            jsonObj.Add(new JsonStringValue("entityId", GetName()));
            if (position != null && velocity != null && maxVel != null && worldObj != null)
            {
                jsonObj.Add(new JsonStringValue("Position", Position + ""));
                jsonObj.Add(new JsonStringValue("Velocity", velocity + ""));
                jsonObj.Add(new JsonStringValue("MaxVelocity", maxVel + ""));
                jsonObj.Add(new JsonNumericValue("WorldId", worldObj.worldId));
                jsonObj.Add(new JsonStringValue("IsDead", isDead + ""));
                jsonObj.Add(new JsonStringValue("Noclip", noClip + ""));
                if (nbtTag != null)
                    jsonObj.Add(new JsonStringValue("entityTag", nbtTag + ""));
            }

            return jsonObj;
        }

        public AxisAlignedBb GetAaBb()
        {
            return Position.ToPlanar().ExpandBothTo(100, 100);
        }

        public TerrainBlock GetUnderlyingBlock()
        {
            return chunkObj[Position.Ix % 16, Position.Iy % 16];
        }

        public virtual void Update()
        {
            if (EventBus.OnEntityUpdate(this)) return;

            MoveEntity();
            velocity *= 0.85;
            //TODO select most efficient way of iteration to avoid CME
            var entities = worldObj.GetEntitiesWithinAaBbAndHeight(GetAaBb(), Position.z);
            foreach (var entity in entities)
                OnCollideWith(entity);
        }

        private void MoveEntity()
        {
            var oldPos = Position;
            var oldChunk = chunkObj;
            var accuracy = 20;
            var dvel = velocity / accuracy;
            for (var i = 0; i < accuracy; i++)
            {
                Position = Position + dvel;
                if (chunkObj != null && chunkObj.isLoaded)
                {
                    if (chunkObj != oldChunk) worldObj.ChangeEntityChunk(this, oldChunk, chunkObj);
                }
                else
                {
                    Position = oldPos;
                    velocity = new PlanarVector();
                    break;
                }

                oldPos = Position;
                oldChunk = chunkObj;
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

        public void SetDead(bool _dead = true)
        {
            isDead = _dead;
            lock (worldObj.entityRemoveQueue)
            {
                worldObj.entityRemoveQueue.Add((this, chunkObj));
            }
        }

        public virtual void OnCollideWith(Entity _obj)
        {
        }

        public EntityRenderer GetRenderer()
        {
            return _renderer;
        }

        public void SetCustomRenderer(EntityRenderer _renderer)
        {
            this._renderer = _renderer;
        }

        public virtual string GetName()
        {
            return "entity.null";
        }

        protected bool Equals(Entity _other)
        {
            return Equals(GetName(), _other.GetName()) &&
                   Equals(worldObj, _other.worldObj) &&
                   Equals(Position, _other.Position) &&
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