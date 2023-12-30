 
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public static class ItemMaster
    {
        /*
        public static List<Item> ItemList = new();
        static SortedDictionary<string, Item> _itemDictionary;
        public static SortedDictionary<string, Item> ItemDictionary
        {
            get
            {
                if (_itemDictionary == null)
                    ImportDictionary();
                return _itemDictionary;
            }
        }

        //[Command("give", MonoTargetType.All)]
        public static void GiveItemToEntity(string entityName, string itemName)
        {
            var targetEntity = EntityMaster.GetEntity(entityName);

            if (targetEntity == null)
            {
                GD.Print($"Entity with a given name \"{entityName}\" does not exist.");
                return;
            }

            var targetItem = GetItem(itemName);

            if (targetItem == null)
            {
                GD.Print($"Item with a given name \"{itemName}\" does not exist.");
                return;
            }

            targetEntity.inventory.AddItem(targetItem);
            GD.Print($"Giving {entityName} {itemName}");
        }

        public static Item GetItem(string entityName)
        {
            if (ItemDictionary.TryGetValue(entityName, out var item)) return item;
            return null;
        }

        static void ImportDictionary()
        {
            _itemDictionary = new();
            ImportItems();
            ItemList.ForEach(item => _itemDictionary.Add(item.Name, item));
        }

        public static void ImportItems()
        {
            //Item[] allItems = Resources.LoadAll<Item>("Items");
            //ItemList = allItems.AsReadOnlyList().ToList();
            //Debug.Log("Item Import Complete!");
            throw new NotImplementedException();
        }
    }*/
    }
}