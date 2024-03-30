
using Game.Abilities;
using Game.Weapons;
using System;
using System.Collections.Generic;
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

        [Header("Weapon")]
        public string primaryAbilityName;

        public WeapnHoldType holdType;

        internal List<WeaponObject> runtimeObjects = new();

        /// <summary>
        /// This is specification for what kind of default animation a player would get
        /// </summary>
        public abstract string AnimationPrefix { get; }
        public override void SetModel()
        {
            base.SetModel();
        }

        public override void ClearModel()
        {
            base.ClearModel();
            runtimeObjects.Clear();
        }

        public override void RegisterModel(GameObject target)
        {
            var weaponObj = target.GetComponent<WeaponObject>();
            weaponObj.Initialize(owner, this);
            runtimeObjects.Add(weaponObj);
        }
        public override void ApplyOnEquip()
        {
            base.ApplyOnEquip();
            var ability = AbilityFactory.GetAbility(primaryAbilityName, owner);
            owner.abilityController.SetPrimaryAbility(ability);
        }
        public override void ApplyOnUnEquip()
        {
            base.ApplyOnUnEquip();
            owner.abilityController.ClearPrimaryAbility();
        }
    }
}