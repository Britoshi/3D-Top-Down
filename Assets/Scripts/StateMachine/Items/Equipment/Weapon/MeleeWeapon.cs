
using System;
using UnityEngine;

namespace Game.Items
{
    [Serializable]
    [CreateAssetMenu(fileName = "new melee weapon", menuName = "Scriptable Objects/new melee weapon")]
    public class MeleeWeapon : Weapon
    {
        public override string AnimationPrefix => "Sword";
    }
}