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

        [SerializeField] List<Item> _display;

        public Type filter;
        public SortType sortType;

        private void ArrangeList()
        {
            var items = inventory.storage.GetFilteredList(filter);
            if (items != null) itemList = new ItemSortList(items); 
            else gameObject.SetActive(false);
            itemList.SortItems(sortType);
            _display = itemList;
        }
        void CreateItemSlot(Item item)
        {
            Debug.Log("Creating Item Slot");
            var newSlotGameObject = Instantiate(slotPrefab, container);
            var icon = item.icon != null ? item.icon : GameResources.MissingTexture;
            newSlotGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        }
        public void RefreshUI()
        {  
            //First Destroy All child elements
            foreach (Transform transform in container.transform)
            {
                if (transform == container) continue;
                Destroy(transform.gameObject);
            }

            foreach (var item in itemList) CreateItemSlot(item);
            autoCellSize.AdjustCellSize();
        }
        public void OnOpenTab(Type filter)
        {
            this.filter = filter;
            ArrangeList();
            RefreshUI();
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