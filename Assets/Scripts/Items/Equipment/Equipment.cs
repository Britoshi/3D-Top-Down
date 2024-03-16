
using System;
using System.Collections.Generic;
using Game.Buff;
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
    public enum EquippableArea
    {     
        HEAD,
        BODY,
        HAND,
        BOOTS,
        WEAPON,
    }

    [Serializable]
    public abstract class Equipment : Item
    {
        [Header("Equipment")]
        public EquippableArea targetArea;
        public GameObject[] models;
        public AttributeAffect[] statusModifiersOnEquip;
        public OnHitAffect[] onTargetHit, onGetHit, onKill;
        /// <summary>
        /// Tells which ones to check
        /// </summary>
        
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
        public EquippableArea[] AllAreas()
        { 
            return Enum.GetValues(typeof(EquippableArea)) as EquippableArea[];
        }
        readonly List<GameObject> spawnedModels = new();
        public Transform GetTargetTransform() => owner.GetEquipmentTransform(targetArea);
        public virtual void SetModel()
        {
            if (models.Length == 1)
            {
                var target = GetTargetTransform();
                if (target == null) return;
                spawnedModels.Add(Instantiate(models[0], target));
            }
        }
        public virtual void ClearModel()
        {
            spawnedModels.ForEach(model => Destroy(model));
            spawnedModels.Clear();
        }
        public virtual void ApplyOnEquip()
        {  
            if (Container.isStaticContainer) return; 

            SetModel();
            for (int i = 0; i < statusModifiersOnEquip.Length; i++)
                statusModifiersOnEquip[i]?.Apply(owner, owner);
        }
        public virtual void ApplyOnUnEquip()
        {
            if (Container.isStaticContainer) return;
             
            for (int i = 0; i < statusModifiersOnEquip.Length; i++)
                statusModifiersOnEquip[i]?.Remove(owner);
        }
    }
}