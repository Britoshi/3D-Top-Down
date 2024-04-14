
using System;
using UnityEngine;

namespace Game.Items
{
    [Serializable]
    public abstract class Gear : Equipment
    {
        public override EquipmentType EquipType => EquipmentType.ARMOR;
    }
}