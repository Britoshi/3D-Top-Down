using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIInventory : BritoBehavior
    { 
        [SerializeField] GameObject slotPrefab;
        [SerializeField] Transform container;
        [SerializeField] AutoCellSize autoCellSize;

        [SerializeField] PlayerEntity player;
        [SerializeField] ItemSortList itemList;
        Inventory inventory;
        //[SerializeField] 
         
        [SerializeField] List<Transform> equipments;

        public Type filter;
        public SortType sortType;

        private void ArrangeList()
        {
            var items = inventory.storage.GetFilteredList(filter);
            if (items != null) itemList = new ItemSortList(items); 
            else gameObject.SetActive(false);


            itemList.SortItems(sortType); 
        }
        GameObject CreateItemSlot(Item item, int index)
        {
            //Debug.Log("Creating Item Slot");
            var newSlotGameObject = Instantiate(slotPrefab, container);
            newSlotGameObject.name = index.ToString();
            //newSlotGameObject.tag = slotPrefab.tag;
            var icon = item.icon != null ? item.icon : GameResources.MissingTexture;
            newSlotGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icon;
            return newSlotGameObject;
        }
        void RefreshEquipmentSlot()
        {
            /*
            if (inventory.equipmentSlots == null) return;
            foreach(var item in inventory.equipmentSlots)
            {
                if (item.Value.item == null) continue;
                int index = (int)item.Key;
                var icon = item.Value.item.icon != null ? item.Value.item.icon : GameResources.MissingTexture;
                equipments[index].GetChild(0).GetComponent<Image>().sprite = icon;
            }*/
        }
        public void RefreshUI()
        {  
            //First Destroy All child elements
            foreach (Transform transform in container.transform)
            {
                if (transform == container) continue;
                Destroy(transform.gameObject);
            }
            foreach(var item in equipments)
            {
                item.GetChild(0).GetComponent<Image>().sprite = null;
            }
            int index = 0;
            foreach (var item in itemList) CreateItemSlot(item, index++);
            RefreshEquipmentSlot();
            autoCellSize.AdjustCellSize();
        }
        public void OnOpenTab(Type filter)
        {
            this.filter = filter;
            ArrangeList();
            RefreshUI();
        }
        //public Item GetItem(int index)
        //{

        //} 
        public void GetStorageItem(int index, ItemData tooltip)
        {
            if (itemList.Count == 0) ArrangeList();
            Item item = itemList[index];
            tooltip.Set(item);
        }
        public void GetEquipmentItem(int index, ItemData tooltip)
        {
            /*
            if (itemList.Count == 0) ArrangeList();
            Item item = inventory.equipmentSlots[(EquipmentType)index].item;
            if (item == null)
            {
                tooltip.Disable();
                return;
            }
            tooltip.Set(item);*/
        }

        private void OnEnable()
        {
            inventory = player.inventory;
            if (inventory == null)
            {
                gameObject.SetActive(false);
                return;
            }
            ArrangeList();
            RefreshUI();
        }
        private void OnDisable()
        {
            itemList.Clear();
        }
        private void Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            itemList = new(); 
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}