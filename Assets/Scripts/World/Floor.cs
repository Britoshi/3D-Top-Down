using System;
using UnityEngine;

namespace Game
{
    public class Floor : IComparable<Floor>
    {
        public int height;
        public Chunk chunk;
        public GameObject gameObject;

        ///Dummy Object for comparison
        private Floor(int height) => this.height = height;
        public static Floor Comparer(int height) => new(height);

        public Floor(Chunk chunk, Transform transform)
        {
            this.chunk = chunk;
            gameObject = transform.gameObject;
            //This depends on the gameObject having the right naming!
            height = int.Parse(gameObject.name);
        }

        public Vec2 GetPosition() => chunk.position;

        public int CompareTo(Floor other) => height.CompareTo(other.height);
    }
}