using Game.Items;
using Game.Weapons;
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
        private List<Item> items;

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
    public class WeaponSlot : EquipmentSlot
    {
        [HideInInspector]
        public List<WeaponObject> objects;
        [HideInInspector]
        public Weapon weapon;


        public override bool HandleAdd(Item item)
        {
            if (!base.HandleAdd(item)) return false;
            weapon = equipment as Weapon;
            objects = new(weapon.spawnedModels);  
            return true;
        }
        public override bool Remove()
        { 
            weapon = null;
            objects.Clear();
            return base.Remove();
        }
        public override bool HandleRemove(Item item)
        { 
            weapon = null;
            objects.Clear();
            return base.HandleRemove(item);
        }
    }
    [Serializable]
    public class ArmorSlot : EquipmentSlot
    {
        //public List<object> objects;
        [HideInInspector]
        public Gear armor;


        public override bool HandleAdd(Item item)
        {
            if (!base.HandleAdd(item)) return false;
            armor = equipment as Gear;
            //objects = new();
           // foreach (var obj in weapon.spawnedModels)
            //    if (obj.TryGetComponent<WeaponObject>(out var weaponObject))
           //         objects.Add(weaponObject);
            return true;
        }
        public override bool Remove()
        {
            armor = null;
            //objects = null;
            return base.Remove();
        }
        public override bool HandleRemove(Item item)
        {
            armor = null;
            //objects = null;
            return base.HandleRemove(item);
        }
    }

    [Serializable]
    public abstract class EquipmentSlot : ItemContainer, IEnumerable<Item>
    {
        public Equipment equipment;

        public override int Count => equipment == null ? 0 : 1;

        public override bool IsEmpty => equipment == null;

        public bool HasItem() => equipment != null;
        public override void Initialize(Entity owner) 
        {
            base.Initialize(owner);
            if (equipment != null)
            {
                equipment = equipment.Clone();
                equipment.Container = this;
            }
        }

        public override bool HandleAdd(Item item)
        {
            if(equipment == null)
            {
                equipment = item as Equipment; 
                equipment.Container = this;
                equipment.ApplyOnEquip();
                return true;
            }
            return false;
        }
        public virtual bool Remove()
        {
            if (equipment == null)
            {
                throw new System.Exception("Why are you trying to remove an item that doesn't exist?");
            }
            equipment.ApplyOnUnEquip();
            equipment.Container = null;
            equipment = null;
            return true;
        }
        public override bool HandleRemove(Item item)
        {
            if(equipment == null)
            {
                throw new System.Exception("Why are you trying to remove an item that doesn't exist?");
            }
            equipment.ApplyOnUnEquip();
            return true;
        }

        //public override void HandleSort()
        //{
        //    return;
        //}
        // Implementing IEnumerable interface
        public override IEnumerator<Item> GetEnumerator()
        {
            if (equipment != null)
            {
                yield return equipment;
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
        public ItemSortList(ItemSortList list) : base(list)
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