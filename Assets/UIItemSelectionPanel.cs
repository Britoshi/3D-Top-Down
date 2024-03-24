using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game.Items;
using Michsky.UI.ModernUIPack;
using Game.UI;
namespace Game
{
    public class UIItemSelectionPanel : BritoBehavior
    {
        public static UIItemSelectionPanel Instance;

        public GameObject toggle;
        public Item currentItem;

        [SerializeField] TMP_Text itemName, description;
        [SerializeField] Image icon;
        [SerializeField] RectTransform scrollContent, actionHolder;

        [SerializeField] RectTransform selector;

        [Header("Prefab")]
        [SerializeField] GameObject actionButton;

        public void Awake()
        {
            Instance = this;
        }
        public void Start()
        {

            Instance = this;
        }
        void CreateActionButton(Item.ItemAction action)
        {
            var newButton = Instantiate(actionButton, actionHolder);
            var button = newButton.GetComponent<ButtonManagerBasicIcon>();
            button.buttonIcon = GameResources.GetIcon(action.name);
            button.clickEvent.AddListener(action.action.Invoke); 
            button.clickEvent.AddListener(UIInventoryMenu.ReloadCurrentPage);
        }
        void ClearPanel()
        {
            actionHolder.DestroyChildren(); 
        }
        void RefreshUI()
        {
            ClearPanel();
            if (currentItem != null)
            {
                itemName.text = currentItem.name;
                description.text = currentItem.description;
                icon.sprite = currentItem.GetIcon();

                foreach (var action in currentItem.GetItemActions())
                    CreateActionButton(action);
            }
            toggle.SetActive(currentItem != null);
        }
        public static void SetPanel(RectTransform target, Item item)
        {
            Instance.selector.gameObject.SetActive(true);

            Instance.currentItem = item;
            Instance.RefreshUI();
        }
        public static void Clear()
        { 
            Instance.selector.gameObject.SetActive(false);

            Instance.currentItem = null;
            Instance.RefreshUI();
        }
    }
}