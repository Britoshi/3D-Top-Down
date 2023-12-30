using System;
using System.Collections.Generic; 

namespace Game
{
    [Serializable]
    public class Item : IAdvancedTriggers
    {

        public string name;
        public string description;

        public AttributeAffect[] attributeModifiers;
        public OnHitAffect[] OnHitAffects { get; set; }
        public OnHitAffect[] OnGetHitAffects { get; set; }
        public OnHitAffect[] OnKillAffects { get; set; }

        public Item(string name, string description, AttributeAffect[] mods, OnHitAffect[] onHit, OnHitAffect[] onGetHit, OnHitAffect[] onKill)
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
            if (inventory.IsFull()) return false;
            inventory.items.Add(itemObject);
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
            if(inventory.items.Count <= 0) throw new Exception("Item was not found?");
            inventory.items.Remove(itemObject);
        }

        internal bool Apply(Status stats)
        {
            for (int i = 0; i < attributeModifiers.Length; i++) 
                attributeModifiers[i]?.Apply(stats, stats); 

            if (this is IAdvancedFunctions)
            {
                var targetItem = this as IAdvancedFunctions;
                targetItem.OnStart();
                Tick.AddFunction(targetItem.OnTick);
            } 
            return AddToInventory(stats.inventory, this); 
        }
        internal void Remove(Status stats)
        {
            for (int i = 0; i < attributeModifiers.Length; i++) 
                attributeModifiers[i]?.Remove(stats); 

            if (this is IAdvancedFunctions)
            {
                var targetItem = this as IAdvancedFunctions;
                Tick.AddFunction(targetItem.OnTick);
            }
            RemoveFromInventory(stats.inventory, this); 
        }

        internal void OnHit(Status owner, Status target, float amount) =>
            IAdvancedTriggers.OnHit(this, owner, target, amount, CustomOnHit);
        internal void OnGetHit(Status owner, Status source, float amount) =>
            IAdvancedTriggers.OnGetHit(this, owner, source, amount, CustomOnGetHit); 
        internal void OnKill(Status killer, Status victim) =>
            IAdvancedTriggers.OnKill(this, killer, victim, CustomOnKill);

        protected virtual void CustomOnHit(Status owner, Status target, float amount) { }
        protected virtual void CustomOnGetHit(Status owner, Status source, float amount) { }
        protected virtual void CustomOnKill(Status killer, Status victim) { }
    }


}
