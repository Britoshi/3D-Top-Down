
using Game.Items;
using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    [System.Serializable]
    public struct ItemRegister
    {
        public GameObject gameObject;
        public Item item;
        public ItemRegister(GameObject gameObject, Item item)
        {
            this.gameObject = gameObject;
            this.item = item;
        }
    }
    public class UIInventoryMenu : BritoBehavior 
    { 
    
        public static UIInventoryMenu Instance;

        public Type filter;
        public SortType sortType;

        [SerializeField] GameObject slotPrefab;
        [SerializeField] GameObject actionMenuItem;
        [SerializeField] GameObject equipmentUI, itemUI;

        [SerializeField] RectTransform weaponEquipmentSlot, armorEquipmentSlot;
        [SerializeField] RectTransform itemContainer;
        [SerializeField] RectTransform weaponContainer, armorContainer;
        [SerializeField] RectTransform selector;


        [SerializeField] UIItemSelectionPanel uiItemSelect;
        [SerializeField] UIEquipmentSelector equipmentSelector;

        SortedDictionary<GameObject, Item> registeredItems;

        Inventory inventory; 
        ItemSortList storage, weapons, armors;

        private void Awake()
        {
            Instance = this;
            storage = new();
            weapons = new();
            armors = new();
            registeredItems = new(new GameObjectComparer()); ;
        }

        public static void ReloadCurrentPage()
        {
            if (Instance.itemUI.activeSelf) 
                DisplayItemPage();  
            else if (Instance.equipmentUI.activeSelf)
                DisplayEquipmentPage();

            //Dont reload when nothin's up
        }

        public static void DisplayEquipmentPage()
        {
            Instance.selector.gameObject.SetActive(false);
            UIItemSelectionPanel.Clear();
            UIEquipmentSelector.Unselect();

            Instance.ReloadItemData();
            Instance.RefreshEquipmentsDisplay();
            Instance.itemUI.SetActive(false);
            Instance.equipmentUI.SetActive(true);
        }

        public static void DisplayItemPage()
        { 
            Instance.ReloadItemData();
            Instance.RefreshItemDisplay();
            Instance.equipmentUI.SetActive(false);
            Instance.itemUI.SetActive(true);
        }
        public static void Close()
        { 
            Instance.equipmentUI.SetActive(false);
            Instance.itemUI.SetActive(false);
        }

        Transform hoveredDisplay, previousDisplay;

        public void DisplayToolTip(GameObject target)
        {
            if (!registeredItems.TryGetValue(target, out Item item)) return;
            //var item = registeredItems[target];
            var menu = target.transform.GetChild(1);
            menu.GetChild(0).GetComponent<TMPro.TMP_Text>().text = item.name;
            hoveredDisplay = target.transform;
            menu.gameObject.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
                SelectItem(target, item);
        }
        RectTransform selectedDisplay;
        void SelectItem(GameObject target, Item item)
        {
            selectedDisplay = hoveredDisplay as RectTransform;
            selector.position = selectedDisplay.position; 
            selector.gameObject.SetActive(true);
            SelectOpenDisplay(target, item);
        }

        void SelectOpenDisplay(GameObject target, Item item)
        {
            if (equipmentUI.activeSelf)
            {
                selector.localScale = Vector3.one* 120/200;
                UIEquipmentSelector.Select(target.transform as RectTransform, item);
            }
            else if (itemUI.activeSelf)
            {
                selector.localScale = Vector3.one;
                UIItemSelectionPanel.SetPanel(selectedDisplay, item);
            }
        }
        void UnselectOpenDisplay()
        {
            if (equipmentUI.activeSelf)
                UIEquipmentSelector.Unselect();
            else if (itemUI.activeSelf)
                UIItemSelectionPanel.Clear();
        }


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        { 
        }
        void DisablePreviousDisplay()
        { 
            var menu = previousDisplay.GetChild(1).gameObject;
            menu.SetActive(false);
            previousDisplay = null;
        }
        void LateUpdate()
        {
            if (previousDisplay != null)
            {
                if (hoveredDisplay == null)
                {
                    DisablePreviousDisplay();
                }
                else
                {
                    if (previousDisplay != hoveredDisplay)
                    {
                        DisablePreviousDisplay();
                    }
                }
            }
            previousDisplay = hoveredDisplay;

            if (Input.GetButtonUp("Fire1"))
            {
                if (hoveredDisplay == null)
                {
                    if (selectedDisplay != null)
                    {
                        //disable
                        selector.gameObject.SetActive(false);
                        UnselectOpenDisplay();
                        selectedDisplay = null;
                    }
                }
            }
            //Finally off.
            hoveredDisplay = null;

        }

        public void ReloadItemData()
        {
            storage.Clear();
            weapons.Clear();
            armors.Clear();
            //This is shitty
            inventory = PlayerEntity.Instance.inventory;
            var items = new ItemSortList(inventory.storage.GetItems());
            items.SortItems(sortType);
            foreach (var item in items)
            {
                if (item.GetType().IsSubclassOf(typeof(Equipment)))
                {
                    if ((item as Equipment).EquipType == EquipmentType.WEAPON)
                        weapons.Add(item);
                    else armors.Add(item);
                }
                if(filter == (filter & item.type)) 
                    storage.Add(item); 
            }
        }

        void ClearEquipmentSlots()
        {
            weaponContainer.DestroyChildren();
            armorContainer.DestroyChildren();

            armorEquipmentSlot.GetChild(0).GetComponent<Image>().sprite = null;
            weaponEquipmentSlot.GetChild(0).GetComponent<Image>().sprite = null;
        }
        
        protected void GenerateItemSlots(ItemSortList items, Transform parent)
        {
            GameObject CreateItemSlot(Item item, Transform parent, int index)
            {
                var newSlotGameObject = Instantiate(slotPrefab, parent);
                newSlotGameObject.name = index.ToString();
                var icon = item.GetIcon();
                newSlotGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icon;
                registeredItems.Add(newSlotGameObject, item);
                return newSlotGameObject;
            }

            for (int i = 0; i < items.Count; i++) 
                CreateItemSlot(items[i], parent, i); 
        }
        protected void SetEquipmentSlot(EquipmentSlot slot, RectTransform display)
        { 
            if (!slot.HasItem()) return;
            var icon = slot.item.GetIcon();
            display.GetChild(0).GetComponent<Image>().sprite = icon; 
            registeredItems.Add(display.gameObject, slot.item);
        }

        public void RefreshEquipmentsDisplay()
        {
            registeredItems.Clear();
            ClearEquipmentSlots();
            GenerateItemSlots(weapons, weaponContainer);
            GenerateItemSlots(armors, armorContainer);
            SetEquipmentSlot(inventory.weapon, weaponEquipmentSlot);
            SetEquipmentSlot(inventory.armor, armorEquipmentSlot);
        }
        public void RefreshItemDisplay()
        {
            UIItemSelectionPanel.Clear();
            registeredItems.Clear();
            itemContainer.DestroyChildren();
            GenerateItemSlots(storage, itemContainer);
        }
    }
    
    // Custom comparer for GameObjects
    public class GameObjectComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            if (x == null || y == null)
            {
                throw new System.ArgumentNullException("Cannot compare null GameObjects");
            }

            return x.GetInstanceID().CompareTo(y.GetInstanceID());
        }
    }
}
