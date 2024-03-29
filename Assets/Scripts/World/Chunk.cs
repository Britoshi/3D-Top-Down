using System.Collections.Generic;
using System; 
using UnityEngine;

namespace Game
{
    public class Chunk : IComparable<Chunk>, IEquatable<Chunk>
    {
        public Vec2 position;
        public SortedSet<Floor> floors;
        public GameObject gameObject;

        public Chunk(Transform transform)
        {
            floors = new SortedSet<Floor>();
            gameObject = transform.gameObject;
            //This depends on the gameObject having the right naming!
            var token = gameObject.name.Split('x');
            int x = int.Parse(token[0]);
            int y = int.Parse(token[1]);
            position = new Vec2(x, y);

            //now initialize children
            foreach (Transform child in gameObject.transform)
                floors.Add(new Floor(this, child));
        }

        public bool TryGetFloor(int height, out Floor floor) =>
            floors.TryGetValue(Floor.Comparer(height), out floor);

        public int CompareTo(Chunk other) => position.CompareTo(other.position);
        public bool Equals(Chunk other) => position.Equals(other.position);
    }    
}