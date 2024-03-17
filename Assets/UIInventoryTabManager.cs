using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public enum TabIndex
    {
        ALL, GEAR, WEAPON, CONSUMABLE, MISC
    }
    [Serializable]
    public struct Tab : IEquatable<Tab>
    { 
        public Button button; 
        public UnityEvent onTabClick;
        public Items.Type type;

        public readonly bool Equals(Tab other) =>
            button.Equals(other.button);
    }
    public class UIInventoryTabManager : BritoBehavior
    {
        public Tab? currentTab;
        [SerializeField] Sprite selectedSprite, defaultSprite; 
        [SerializeField] Tab[] tabs;

        [SerializeField] UIInventory uiInventory;
        // Start is called before the first frame update
        void Start()
        {
            foreach (var tab in tabs)
            {
                tab.button.onClick.AddListener(() => Switch(tab));
                tab.button.onClick.AddListener(tab.onTabClick.Invoke);
                tab.button.onClick.AddListener(() => uiInventory.OnOpenTab(tab.type));
            }
        }

        // Update is called once per frame
        void Update()
        {
            //buttomBehaviors.
        }

        private void OnEnable()
        {
            if(currentTab == null)
                Switch(tabs[0]);
        }

        void Switch(Tab tab)
        {
            if (currentTab.Equals(tab)) return;

            if(currentTab != null) 
                currentTab.Value.button.image.sprite = defaultSprite; 

            currentTab = tab;
            tab.button.image.sprite = selectedSprite;

        }
    }
}