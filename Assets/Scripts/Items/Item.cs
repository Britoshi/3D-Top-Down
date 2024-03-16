using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.UI.GridLayoutGroup;

namespace Game.Items
{

    [Flags]
    public enum Type
    {
        NONE = 0,
        EQUIPMENT = 1 << 0,
        ARMOR = 1 << 1 | EQUIPMENT,
        WEAPON = 1 << 2 | EQUIPMENT,
        HEAD_GEAR = 1 << 3 | ARMOR,
        FULL_GEAR = 1 << 4 | ARMOR,
        MELEE_WEAPON = 1 << 5 | WEAPON,
        RANGE_WEAPON = 1 << 6 | WEAPON,
    }

    [Serializable]
    //[CreateAssetMenu(fileName = "new item", menuName = "Scriptable Objects/new item")]
    public abstract class Item : ScriptableObject, IComparable<Item>
    {
        public string description; //"&{" and "}&" for using live variables
        public Type type;
        public short weight;
        public int cost;
        public bool isQuestItem = false;
        public Sprite icon;

        public AttributeAffect[] attributeAffect;
        public bool HasAttributeAffector => attributeAffect.Length > 0;   
        public ItemContainer Container { set; get; }
        public Entity owner => Container.Owner;
            
        public int CompareTo(Item other)
        {
            if (Container == null) return name.CompareTo(other.name);
            return Container.SortType switch
            {
                SortType.NAME => name.CompareTo(other.name),
                SortType.TYPE => type.CompareTo(other.type),
                SortType.WEIGHT => weight.CompareTo(other.weight),
                SortType.COST => cost.CompareTo(other.cost),
                SortType.QUEST_ITEM => isQuestItem.CompareTo(other.isQuestItem),
                _ => -1,
            };
        }

        public virtual void ApplyOnPossession()
        {
            if (Container.isStaticContainer) return;

            owner.inventory.CurrentWeight += weight;
            for (int i = 0; i < attributeAffect.Length; i++)
                attributeAffect[i]?.Apply(owner, owner);
        }
        public virtual void ApplyOnDepossession()
        {
            if (Container.isStaticContainer) return; 

            owner.inventory.CurrentWeight -= weight;
            for (int i = 0; i < attributeAffect.Length; i++)
                attributeAffect[i]?.Remove(owner);
        }
    }

    public class Itemt : ScriptableObject, IAdvancedTriggers
    {
        public string description;

        [SerializeField]
        public AttributeAffect[] attributeModifiers;

        [field: SerializeField]
        public OnHitAffect[] OnHitAffects { get; set; }
        [field: SerializeField]
        public OnHitAffect[] OnGetHitAffects { get; set; }
        [field: SerializeField]
        public OnHitAffect[] OnKillAffects { get; set; }

        public Itemt(string name, string description, AttributeAffect[] mods, OnHitAffect[] onHit, OnHitAffect[] onGetHit, OnHitAffect[] onKill)
        {
            this.name = name;
            this.description = description;
            attributeModifiers = mods;
            OnHitAffects = onHit;
            OnGetHitAffects = onGetHit;
            OnKillAffects = onKill;
        }

        /// <summary>
        /// Append an item to the array of items.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="itemObject"></param>
        /// <returns>true if successful, false if inventory is full.</returns>
        public static bool AddToInventory(Inventory inventory, Item itemObject)
        {
            /*
            for (int i = 0; i < inventory.items.Length; i++)
            {
                var item = inventory.items[i];
                if (item != null)
                {
                    inventory.items[i] = itemObject;
                    return true;
                }
            } */
            //if (inventory.IsFull()) return false;
            inventory.storage.Add(itemObject);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="itemObject"></param>
        /// <returns>true if removed successfuly. false if it failed.</returns>
        public static void RemoveFromInventory(Inventory inventory, Item itemObject)
        {
           // if (inventory.items.Count <= 0) throw new Exception("Item was not found?");
            inventory.storage.Remove(itemObject);
        }

        internal bool Apply(Entity entity)
        {
            for (int i = 0; i < attributeModifiers.Length; i++)
                attributeModifiers[i]?.Apply(entity, entity);

            if (this is IAdvancedFunctions)
            {
                var targetItem = this as IAdvancedFunctions;
                targetItem.OnStart();
                Tick.AddFunction(targetItem.OnTick);
            }
            throw new Exception();
            //do this down here
            //return AddToInventory(stats.inventory, this);
        }
        internal void Remove(Entity stats)
        {
            ////for (int i = 0; i < attributeModifiers.Length; i++)
             //   attributeModifiers[i]?.Remove(stats);

            if (this is IAdvancedFunctions)
            {
                var targetItem = this as IAdvancedFunctions;
                Tick.AddFunction(targetItem.OnTick);
            }
            //RemoveFromInventory(stats.inventory, this);
        }

        internal void OnHit(Entity owner, Entity target, float amount) =>
            IAdvancedTriggers.OnHit(this, owner, target, amount, CustomOnHit);
        internal void OnGetHit(Entity owner, Entity source, float amount) =>
            IAdvancedTriggers.OnGetHit(this, owner, source, amount, CustomOnGetHit);
        internal void OnKill(Entity killer, Entity victim) =>
            IAdvancedTriggers.OnKill(this, killer, victim, CustomOnKill);

        protected virtual void CustomOnHit(Entity owner, Entity target, float amount) { }
        protected virtual void CustomOnGetHit(Entity owner, Entity   source, float amount) { }
        protected virtual void CustomOnKill(    Entity killer, Entity victim) { }
    }


}
