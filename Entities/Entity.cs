using System.Collections.Generic;
using DiggerLib;
using GlLib.Events;
using GlLib.Map;
using GlLib.Utils;

namespace GlLib.Entities
{
    public class Entity
    {
        public World _worldObj;
        public RestrictedVector3D _position;
        public PlanarVector _velocity = new PlanarVector();
        public Chunk _chunkObj;

        public bool _isDead = false;

        public bool _noClip = false;

        public Entity(World world, RestrictedVector3D position, int height)
        {
            _worldObj = world;
            _position = position;
            _chunkObj = world._chunks[position.Ix / 16, position.Iy / 16];
        }

        public AxisAlignedBb GetAaBb()
        {
            return _position.ToPlanar().ExpandBothTo(1,1);
        }

        public TerrainBlock GetUnderlyingBlock()
        {
            return _chunkObj[_position.Ix % 16, _position.Iy % 16];
        }

        public virtual void Update()
        {
            if (EventBus.OnEntityUpdate(this)) return;
            _position += _velocity;
            if (_position.Ix / 16 != _chunkObj._chunkX || _position.Iy / 16 != _chunkObj._chunkY)
            {
                if (EventBus.OnEntityLeftChunk(this) || EventBus.OnEntityEnteredChunk(this))
                    _position -= _velocity;
                    _chunkObj = _worldObj._chunks[_position.Ix / 16, _position.Iy / 16];
            }

            List<Entity> entities = _worldObj.GetEntitiesWithinAaBbAndHeight(GetAaBb(), _position._z);
            foreach (var entity in entities)
            {
                OnCollideWith(entity);
            }
        }

        public void SetDead(bool dead)
        {
            _isDead = dead;
        }

        public virtual void OnCollideWith(Entity obj)
        {
            
        }
        
        public virtual void Render()
        {
            
        }
    }
}