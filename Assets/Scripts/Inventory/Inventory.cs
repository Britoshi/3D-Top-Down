using System;
using System.Collections.Generic;
using Game.Items;
using System.Linq; 
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;

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
        public EquipmentSlot head, body, hands, feet, weapon;
        public Dictionary<EquippableArea, EquipmentSlot> equipmentSlots;


        public Inventory(Entity owner) => Initialize(owner); 
        public Inventory Initialize(Entity owner)
        {
            void InitializeStorage()
            {
                storage.Initialize(owner);
                if (storage.Count == 0) return; 
                storage.ForEach(item => item.ApplyOnPossession()); 
            }

            void InitializeEquipments()
            {
                equipmentSlots ??= new()
                {
                    [EquippableArea.HEAD] = head,
                    [EquippableArea.BODY] = body,
                    [EquippableArea.HAND] = hands,
                    [EquippableArea.BOOTS] = feet,
                    [EquippableArea.WEAPON] = weapon
                };
                foreach (var iter in equipmentSlots)
                {
                    var slot = iter.Value; 
                    slot.Initialize(owner);
                    if (slot.Count == 0) continue; 
                    //Only  Apply if they exist
                    slot.item.ApplyOnPossession();
                    slot.item.ApplyOnEquip();
                }
            }

            Owner = owner;
            status = owner.status;
            CurrentWeight = 0f; 
            InitializeStorage();
            InitializeEquipments();

            return this;
        }
          
        public void AddItem(Item item)
        {
            if (storage.Add(item))
            {
                item.ApplyOnPossession();
            }
        }

        public void RemoveItem(Item item)
        {
            if (storage.Remove(item))
            { 
                item.ApplyOnDepossession();
            }
        }

        public void Equip(Equipment equipment)
        {
            Debug.Log("This is pretty inefficient \"Contains\"");
            if (!storage.Contains(equipment)) 
                throw new Exception("You cannot equip something you do not have?");
             
            var slot = equipmentSlots[equipment.targetArea];
            TryUnequipAt(slot);
            slot.Add(equipment);
            equipment.ApplyOnEquip(); 
        }
        public void UnEquip(Equipment equipment)
        {

        } 
        public bool TryUnequipAt(EquipmentSlot slot)
        {  
            if (slot.IsEmpty)
            {
                Debug.Log("dis slot empty   dawhg.");
                return false;
            } 
            slot.item.ApplyOnUnEquip();
            slot.Remove();
            return true; 
        }
    }
}
