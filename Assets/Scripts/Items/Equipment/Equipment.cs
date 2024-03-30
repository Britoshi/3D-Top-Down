
using System;
using System.Collections.Generic;
using Game.Buff;
using Game.Weapons;
using UnityEngine; 

namespace Game.Items
{
    [Flags]
    public enum EquipmentOnHitAffect
    {
        NONE = 0,
        ON_HIT = 1,
        ON_GET_HIT = 2,
        ON_KILL = 4
    }

    //Gonna be further expanded. 
    [Flags]
    public enum EquipmentType
    {      
        ARMOR = 1,
        WEAPON = 2,
    }
    public enum EquipmentLocation
    { 
        LEFT_HAND,
        RIGHT_HAND,
        BOTH_HANDS,
        HEAD,
        BODY,
        PANTS,
        SHOES,
    }

    [Serializable]
    public abstract class Equipment : Item
    {
        [Header("Equipment")]
        public EquipmentLocation targetArea;
        public GameObject[] models;
        public AttributeAffect[] statusModifiersOnEquip;
        public OnHitAffect[] onTargetHit, onGetHit, onKill;
        /// <summary>
        /// Tells which ones to check
        /// </summary>

        public bool isEquipped;

        public abstract EquipmentType EquipType { get; }
        public EquipmentOnHitAffect OnHits { set; get; }

        private void OnValidate()
        {
            OnHits = 0;
            if (onTargetHit.Length > 0)
                OnHits |= EquipmentOnHitAffect.ON_HIT;
            if (onGetHit.Length > 0)
                OnHits |= EquipmentOnHitAffect.ON_GET_HIT;
            if (onKill.Length > 0)
                OnHits |= EquipmentOnHitAffect.ON_KILL;
        } 
        public readonly List<WeaponObject> spawnedModels = new();
        public Transform GetTargetTransform() => owner.GetEquipmentTransform(targetArea);
        public virtual void SetModel()
        {    
            if (models.Length == 1)
            {
                var target = GetTargetTransform();
                if (target == null) return;
                spawnedModels.Add(Instantiate(models[0], target).GetComponent<WeaponObject>());
            }
        }
        public virtual void ClearModel()
        {
            spawnedModels.ForEach(model => Destroy(model.gameObject));
            spawnedModels.Clear();
        }
        public virtual void ApplyOnEquip()
        {
            //if (Container.isStaticContainer) return; 
            if (owner == null) throw new Exception("WhAT");
            isEquipped = true;
            SetModel();
            for (int i = 0; i < statusModifiersOnEquip.Length; i++)
                statusModifiersOnEquip[i]?.Apply(owner, owner);
        }
        public virtual void ApplyOnUnEquip()
        {
            //if (Container.isStaticContainer) return;
            isEquipped = false;
            ClearModel();
            for (int i = 0; i < statusModifiersOnEquip.Length; i++)
                statusModifiersOnEquip[i]?.Remove(owner);
        }

        public virtual void Equip()
        {
            owner.inventory.Equip(this);
        }
        public virtual void Unequip()
        {
            if (!owner.inventory.TryUnequipAt(Container as EquipmentSlot))
                throw new Exception("What the fuck");
        }

        public override List<ItemAction> GetItemActions()
        {
            var ba = base.GetItemActions();
            ba.Add(
                isEquipped ? new("Unequip", Unequip) :
                new( "Equip", Equip)
                );
            return ba;
        } 
    }
}