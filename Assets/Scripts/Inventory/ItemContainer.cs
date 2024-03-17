using Game.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //public SortType SortType;

        public Entity Owner { protected set; get; } 
        public virtual void Initialize(Entity owner)
        {
            Owner = owner; 
            isStaticContainer = owner == null;
        }
        //public SortType SortType { get;  protected set; }
        public bool Add(Item item)
        {
            item = item.Clone();
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
        //public abstract void HandleSort();
        //public void Sort(SortType type)
        //{
        //    SortType = type;
        //    HandleSort();
        //}

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
        [SerializeField]
        private List<Item> _initList;
        private SortedSet<Item> items;

        public override int Count => items.Count;

        public override bool IsEmpty => items.Count == 0;

        public override void Initialize(Entity owner) 
        {
            base.Initialize(owner);
            items = new();
            //SortType = SortType.NAME;

            for (int i = 0; i < _initList.Count; i++)
            {
                var itemClone = _initList[i].Clone();
                itemClone.Container = this;
                items.Add(itemClone);
            }
            _initList = null;
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

        //public override void HandleSort()
        //{
        //    items = SortItems(items.ToList());
        //}

        // Implementing IEnumerable interface
        public override IEnumerator<Item> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        internal List<Item> GetItems()
        {
            return items?.ToList();
        }
        internal bool GetItems(out List<Item> items)
        {
            if (this.items == null)
            {
                items = null;
                return false;
            }
            items = this.items?.ToList();
            return true;
        }
        internal List<Item> GetFilteredList(Items.Type type)
        {
            if (type == Items.Type.NONE) return items?.ToList();
            return items?.Where(item => item.type.HasFlag(type)).ToList();
        }

        //private SortedSet<Item> SortItems(List<Item> itemList)
        //{
        //    return SortType switch
        //    {
        //        SortType.NAME => new SortedSet<Item>(itemList, Comparer<Item>.Create((x, y) => x.name.CompareTo(y.name))),
        //        SortType.TYPE => new SortedSet<Item>(itemList, Comparer<Item>.Create((x, y) => x.type.CompareTo(y.type))),
        //        SortType.WEIGHT => new SortedSet<Item>(itemList, Comparer<Item>.Create((x, y) => x.weight.CompareTo(y.weight))),
        //        SortType.COST => new SortedSet<Item>(itemList, Comparer<Item>.Create((x, y) => x.cost.CompareTo(y.cost))),
        //        SortType.QUEST_ITEM => new SortedSet<Item>(itemList, Comparer<Item>.Create((x, y) => x.isQuestItem.CompareTo(y.isQuestItem))),
        //        _ => items // Return the original set if sort type is unknown
        //    };
        //}

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

            if (item != null)
            {
                item = item.Clone();
                item.Container = this;
            }
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

        //public override void HandleSort()
        //{
        //    return;
        //}
        // Implementing IEnumerable interface
        public override IEnumerator<Item> GetEnumerator()
        {
            if (item != null)
            {
                yield return item;
            }
        }
    }

    [Serializable]
    public class ItemSortList : List<Item>
    {
        public SortType SortType { get; set; }

        public ItemSortList(List<Item> list) : base(list)
        {

        }
        public ItemSortList() : base()
        {

        }
        public new void Add(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }
            base.Add(item);
        }

        public void SortItems(SortType sortType)
        {
            SortType = sortType;
            switch (SortType)
            {
                case SortType.NAME:
                    Sort((item1, item2) => item1.name.CompareTo(item2.name));
                    break;
                case SortType.TYPE:
                    Sort((item1, item2) => item1.type.CompareTo(item2.type));
                    break;
                case SortType.WEIGHT:
                    Sort((item1, item2) => item1.weight.CompareTo(item2.weight));
                    break;
                case SortType.COST:
                    Sort((item1, item2) => item1.cost.CompareTo(item2.cost));
                    break;
                case SortType.QUEST_ITEM:
                    Sort((item1, item2) => item1.isQuestItem.CompareTo(item2.isQuestItem));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(SortType), "Invalid sort type.");
            }
        }
    }

}