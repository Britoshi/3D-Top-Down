using UnityEngine;

namespace Game
{
    public class PlayerEntity : Entity
    {
        public static PlayerEntity Instance;

        public override void Awake()
        {
            Instance = this;
            base.Awake();
        }
    }
}