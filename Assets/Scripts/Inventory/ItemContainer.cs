using Game.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum SortType
    {
        NAME,
        TYPE,
        WEIGHT,
        COST,
        QUEST_ITEM,
    }
    [Serializable]
    public abstract class ItemContainer : BritoObject, IEnumerable<Item>
    {
        /// <summary>
        /// Like a chest or a object container.
        /// </summary>
        [HideInInspector]
        public bool isStaticContainer = true;
        public abstract int Count { get; }
        public abstract bool IsEmpty { get; }

        public Entity Owner { protected set; get; } 
        public virtual void Initialize(Entity owner)
        {
            Owner = owner; 
            isStaticContainer = owner == null;
            foreach (var item in this) 
                if(item != null) 
                    item.Container = this;
        }
        public SortType SortType { get;  protected set; }
        public bool Add(Item item)
        {
            if (HandleAdd(item))
            {
                item.Container = this;
                return true;
            }
            return false;
        }
        public abstract bool HandleAdd(Item item);
        public bool Remove(Item item)
        {
            if (HandleRemove(item))
            {
                item.Container = null;
                return true;
            }
            return false;
        }
        public abstract bool HandleRemove(Item item);
        public abstract void HandleSort();
        public void Sort(SortType type)
        {
            SortType = type;
            HandleSort();
        }

        // Implementing IEnumerable interface
        public abstract IEnumerator<Item> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Implementing ForEach method
        public void ForEach(Action<Item> action)
        {
            foreach (var item in this) 
                action(item); 
        }

    }
    [Serializable]
    public class ItemList : ItemContainer, IEnumerable<Item>
    {
        public List<Item> items;

        public override int Count => items.Count;

        public override bool IsEmpty => items.Count == 0;

        public override void Initialize(Entity owner) 
        {
            base.Initialize(owner);
            items ??= new();
            SortType = SortType.NAME;
        }
         
        public override bool HandleAdd(Item item)
        {
            items.Add(item);
            return true;
        }

        public override bool HandleRemove(Item item)
        {
            items.Remove(item);
            return true;
        }

        public override void HandleSort()
        {
            items.Sort();
        }

        // Implementing IEnumerable interface
        public override IEnumerator<Item> GetEnumerator()
        {
            return items.GetEnumerator();
        }

    }
    [Serializable]
    public class EquipmentSlot : ItemContainer, IEnumerable<Item>
    {
        public Equipment item;

        public override int Count => item == null ? 0 : 1;

        public override bool IsEmpty => item == null;

        public bool HasItem() => item != null;
        public override void Initialize(Entity owner) 
        {
            base.Initialize(owner);
            //if(item != null) item.Container = this;
        }

        public override bool HandleAdd(Item item)
        {
            if(this.item == null)
            {
                this.item = item as Equipment;
                return true;
            }
            return false;
        }
        public bool Remove()
        {
            if (item == null)
            {
                throw new System.Exception("Why are you trying to remove an item that doesn't exist?");
            }
            item = null;
            return true;
        }
        public override bool HandleRemove(Item item)
        {
            if(this.item == null)
            {
                throw new System.Exception("Why are you trying to remove an item that doesn't exist?");
            }
            return true;
        }

        public override void HandleSort()
        {
            return;
        }
        // Implementing IEnumerable interface
        public override IEnumerator<Item> GetEnumerator()
        {
            if (item != null)
            {
                yield return item;
            }
        } 
    }
}