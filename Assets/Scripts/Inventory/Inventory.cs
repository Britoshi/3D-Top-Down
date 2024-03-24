using System;
using System.Collections.Generic;
using Game.Items;
using System.Linq; 
using UnityEngine;
using Unity.VisualScripting.FullSerializer;
namespace Game
{
    [Serializable]
	public class Inventory
    {
        public Entity Owner { set; get; }
        Status status;

        public const int BASE_CARRY_WEIGHT = 60;
        public float EncumbranceWeight =>
            status.Strength + BASE_CARRY_WEIGHT; 
        public float CurrentWeight { set; get; }

        public ItemList storage;
        public EquipmentSlot armor, weapon;


        public Inventory(Entity owner) => Initialize(owner); 
        public Inventory Initialize(Entity owner)
        {
            void InitializeStorage()
            {
                storage.Initialize(owner);
                if (storage.Count == 0) return; 
                storage.ForEach(item => item.ApplyOnPossession()); 
            }

            void InitializeEquipments(EquipmentSlot slot)
            {
                slot.Initialize(owner);
                if (slot.Count == 0) return; 
                //Only  Apply if they exist
                slot.item.ApplyOnPossession();
                slot.item.ApplyOnEquip();
            }

            Owner = owner;
            status = owner.status;
            CurrentWeight = 0f; 
            InitializeStorage();
            InitializeEquipments(armor);
            InitializeEquipments(weapon);

            return this;
        }
          
        public void AddItem(Item item)
        {
            if (storage.Add(item))
            {
                item.ApplyOnPossession();
            }
            else
            {
                Debug.Log("What the fuck again");
            }
        }

        public void RemoveItem(Item item)
        {
            if (storage.Remove(item))
            { 
                item.ApplyOnDepossession();
            }
        }
        private EquipmentSlot GetEquipmentSlot(Equipment equipment) =>
            equipment.EquipType == EquipmentType.WEAPON ? weapon : armor;

        public void Equip(Equipment equipment)
        {
            Debug.Log("This is pretty inefficient \"Contains\"");
            if (!storage.Contains(equipment)) 
                throw new Exception("You cannot equip something you do not have?");
            storage.Remove(equipment);

            EquipmentSlot slot = GetEquipmentSlot(equipment);
            TryUnequipAt(slot);
            if(slot.Add(equipment))
                equipment.ApplyOnEquip();
            else throw new Exception("How did this fail?");
        }
        public void UnEquip(Equipment equipment)
        { 
            if (TryUnequipAt(GetEquipmentSlot(equipment))) return;
            else throw new Exception("What");
        } 
        public bool TryUnequipAt(EquipmentSlot slot)
        {
            // foreach (var item in equipmentSlots)  Debug.Log("shit dawg" + item.Value.item);
            var item = slot.item;
            if (slot.IsEmpty)
            {
                Debug.Log("dis slot empty   dawhg.");
                return false;
            } 
            slot.item.ApplyOnUnEquip();
            slot.Remove();

            storage.Add(item);
            
            return true; 
        }
    }
}
