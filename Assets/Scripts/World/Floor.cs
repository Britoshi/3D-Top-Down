using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Floor
    {
        public const int HEIGHT = 32;
        public SortedSet<Entity> entities;
        public Chunk chunk;
        public short height;
        public Floor(Chunk chunk, short height)
        {
            this.height = height;
            this.chunk = chunk;
            entities = new();
        }
        public void Remove(Entity entity)
        {
            entities.Remove(entity);
            if (entities.Count == 0) chunk.Remove(height);
        }
        public void Register(Entity entity) => entities.Add(entity);

        public Floor GetMakeFloorAdjacent(Vector3Int difference) =>
            GetMakeFloorAdjacent(difference.x, difference.z, difference.y);

        public Floor GetMakeFloorAdjacent(int x, int y, int height)
        {
            if (x == 0 && y == 0 && height != this.height)
                return chunk.GetMakeFloor(height);
            return chunk.group.GetMakeChunk(x, y).GetMakeFloor(height);
        }

        public void Move(Entity entity, Vector3Int dir)
        {
            var targetFloor = GetMakeFloorAdjacent(dir.x, dir.z, dir.y);
            targetFloor.Register(entity);
            Remove(entity);
        }

        public List<Floor> GetFloorsRadius(int radius)
        {
            List<Floor> floors = new();
            for (int i = -radius; i < radius; i++)
                for (int j = -radius; j < radius; j++)
                    for (int k = -radius; k < radius; k++)
                        floors.Add(GetMakeFloorAdjacent(i, j, k));
            return floors;
        }
    }
}