
using System;
using UnityEngine;

namespace Game.Items
{
    public enum WeapnHoldType
    {
        RIGHT_HAND, 
        LEFT_HAND, 
        BOTH_HAND
    }
    
    [Serializable]
    public abstract class Weapon : Equipment
    {
        public override EquipmentType EquipType => EquipmentType.WEAPON;

        public WeapnHoldType holdType;
        /// <summary>
        /// This is specification for what kind of default animation a player would get
        /// </summary>
        public abstract string AnimationPrefix { get; } 
        public override void SetModel()
        {
            base.SetModel();
        }
    }
}