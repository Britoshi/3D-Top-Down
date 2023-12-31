using UnityEngine;

namespace Game
{
    public class PlayerEntity : Entity
    {
        public Rigidbody rb;
        public Status Stats;

        public static PlayerEntity Instance;
    }
}