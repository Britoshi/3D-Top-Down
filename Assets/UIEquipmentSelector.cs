using Game.Items;
using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIEquipmentSelector : BritoBehavior
    {
        public static UIEquipmentSelector Instance;
        public GameObject toggle;
        [SerializeField] GameObject actionButtonPrefab;

        [SerializeField] TMPro.TMP_Text name, description;
        [SerializeField] RectTransform actionButtons;
        private void Awake()
        {
            Instance = this;
        }

        void CreateActionButton(Item.ItemAction action)
        {
            var newButton = Instantiate(actionButtonPrefab, actionButtons);
            var button = newButton.GetComponent<ButtonManagerBasicIcon>();
            button.buttonIcon = GameResources.GetIcon(action.name);
            button.clickEvent.AddListener(action.action.Invoke);
            button.clickEvent.AddListener(UIInventoryMenu.ReloadCurrentPage);
        }

        public void Clear()
        {
            actionButtons.DestroyChildren();
        }
        void MoveSelector(RectTransform target)
        {
            toggle.transform.position = target.position;
            var targetWidth = target.rect.width;
            var togTran = (toggle.transform as RectTransform);
            var offset = targetWidth + togTran.rect.width / 2;
            var pos = togTran.anchoredPosition;
            pos.x +=  offset;
            togTran.anchoredPosition = pos;

        }
        public void _Select(RectTransform target, Item item)
        {
            Clear();
            name.text = item.name;
            description.text = item.description;
            foreach (var action in item.GetItemActions())
                CreateActionButton(action);
            MoveSelector(target);
            toggle.SetActive(true);
        }
        public void _Unselect()
        {
            toggle.SetActive(false);
            Clear();  
        }
        public static void Select(RectTransform target, Item item) => Instance._Select(target, item);
        public static void Unselect() => Instance._Unselect();
    }
}
