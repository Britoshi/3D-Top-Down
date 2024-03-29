using UnityEngine;

namespace Game.Weapons
{
    internal interface IHasHitBox
    { 
        public GameObject HitBox { set; get; }
    }
}