using System.Collections.Generic;
using GlLib.Entities;
using GlLib.Utils;

namespace GlLib.Map
{
    public class World
    {
        public Chunk[,] _chunks;


        public Chunk this[int i, int j]
        {
            get
            {
                if (i + _width / 2 < 0 || i + _width / 2 > _width)
                    return null;
                if (j + _height / 2 < 0 || j + _height / 2 > _height)
                    return null;
                return _chunks[i + _width / 2, j + _height / 2];
            }
            set
            {
                if (i + _width / 2 < 0 || i + _width / 2 > _width)
                    return;
                if (j + _height / 2 < 0 || j + _height / 2 > _height)
                    return;
                _chunks[i + _width / 2, j + _height / 2] = value;
            }
        }

        public int _width;
        public int _height;

        public World(int width, int height)
        {
            _chunks = new Chunk[width, height];
            _width = width;
            _height = height;
        }

        public List<Entity> GetEntitiesWithinAaBb(AxisAlignedBb aabb)
        {
            List<Entity> entities = new List<Entity>();

            List<Chunk> chunks = new List<Chunk>();

            int chkStartX = aabb.StartXI / 16;
            int chkStartY = aabb.StartYI / 16;
            int chkEndX = aabb.EndXI / 16;
            int chkEndY = aabb.EndYI / 16;

            for (int i = chkStartX; i <= chkEndX; i++)
            for (int j = chkStartY; j <= chkEndY; j++)
            {
                Chunk chk = this[i, j];
                if (chk != null)
                    chunks.Add(chk);
            }

            foreach (var chk in chunks)
            {
                foreach (var height in chk._entities)
                {
                    foreach (var entity in height)
                    {
                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }

        public List<Entity> GetEntitiesWithinAaBbAndHeight(AxisAlignedBb aabb, int height)
        {
            List<Entity> entities = new List<Entity>();

            List<Chunk> chunks = new List<Chunk>();

            int chkStartX = aabb.StartXI / 16;
            int chkStartY = aabb.StartYI / 16;
            int chkEndX = aabb.EndXI / 16;
            int chkEndY = aabb.EndYI / 16;

            for (int i = chkStartX; i <= chkEndX; i++)
            for (int j = chkStartY; j <= chkEndY; j++)
            {
                Chunk chk = this[i, j];
                if (chk != null)
                    chunks.Add(chk);
            }

            foreach (var chk in chunks)
            {
                List<Entity> chkEntities = chk._entities[height];
                foreach (var entity in chkEntities)
                {
                    if(entity.GetAaBb().IntersectsWith(aabb))
                        entities.Add(entity);
                }
            }

            return entities;
        }
    }
}