using UnityEngine;

namespace Game.Weapons
{
    internal interface IHasHitBox
    {
        public bool HitBoxScan { set; get; }
        public Collider HitBox { set; get; }

        public void Toggle(bool state, object[] args);
    }
}