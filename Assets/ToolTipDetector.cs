using Game;
using Game.Items;
using Michsky.UI.ModernUIPack;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[Serializable]
public class ItemData : IEquatable<ItemData>
{
    public bool disable =  true;
    public string title, message;
    public Item item;

    public bool Equals(ItemData other)
    {
        return ToString() == other.ToString();
    }
    public void Disable()
    {
        disable = true;
        item = null;
    }
    public void Set(Item item)
    {
        this.item = item;
        title = item.name;
        message = item.description;
        disable = false;
    }

    public override string ToString()
    {
        return title + ": " + message;
    }
}
public class ToolTipDetector : BritoBehavior
{
    [Serializable]
    public struct ToolTipItem
    {
        public string tag;
        public UnityEvent<int, ItemData> func;
    }
    [Serializable]
    public struct ToolTipData
    {
        public string tag;
        public UnityEvent<GameObject> function;
    }

    [SerializeField] RectTransform rightClickMenu;
    [SerializeField] GameObject prefabMenuItem;

    [SerializeField] TMPro.TMP_Text text;

    public List<ToolTipItem> itemDatas;
    public List<ToolTipData> toolTipDatas;
    Vector2 mousePosition;

    public const string TOOLTIP_STRING = "Tool Tip"; // Substring to check for in the tag

    ItemData currentItem;

    
    void DisableToolTip()
    {
        //print("disable");
        currentItem = null;
        tooltipRectTransform.gameObject.SetActive(false);
    }

    void HandleRightClick()
    { 
        if (Input.GetButtonDown("Fire2"))
        {
            if (currentItem == null || currentItem.disable)
            {
                rightClickMenu.gameObject.SetActive(false);
                return;
            }
            var list = currentItem.item.GetItemActions();
            if (list.Count >= 0)
            {

                //Delete previous ones
                int i = 0;
                foreach (Transform child in rightClickMenu)
                {
                    if (i == 0)
                    {
                        i++; continue;
                    }
                    Destroy(child.gameObject);
                    i++;
                }
                foreach (var action in list)
                {
                    var newObj = Instantiate(prefabMenuItem, rightClickMenu);
                    var button = newObj.GetComponent<ButtonManagerBasicWithIcon>();
                    //button.transform
                    button.buttonText = action.name;
                    button.GetComponentInChildren<Button>().onClick.AddListener(action.action.Invoke);

                    // .transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = action.name;

                }
            }
            var title = rightClickMenu.GetChild(0);
            title.GetChild(0).GetComponent<TMPro.TMP_Text>().text = currentItem.title;
            MoveTransform(rightClickMenu, mousePosition);

            var newpos = rightClickMenu.position;
            newpos.y -= (22) * rightClickMenu.childCount;

            rightClickMenu.position = newpos;
            rightClickMenu.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // Check if the mouse is currently over any UI element with a tag containing the specified substring
        if (!GameSystem.Paused) return;
        var targetGameObject = IsMouseOverToolTip();

        if (targetGameObject)
        {
            print("was something");
            string subtag = targetGameObject.tag.Split(':')[1];
            foreach(var data in toolTipDatas)
            {
                if (data.tag == subtag)
                    data.function.Invoke(targetGameObject);
            }

            /*
            return; 
            if(itemDatas.Count == 0)
            {
                Debug.LogError("There are no item datas. Something  Reset");
            }
            foreach (var itemData in itemDatas)
            {
                if (itemData.tag == subtag)
                {

                    if (int.TryParse(targetGameObject.name, out int index))
                    {
                        currentItem ??= new();
                        itemData.func.Invoke(index, currentItem);

                        if (currentItem.disable)
                        {
                            DisableToolTip();
                            return;
                        }
                        break;
                    } 
                }
            }
            if (currentItem != null)
            {
                RefreshToolTip();
                MoveToolTip();
                tooltipRectTransform.gameObject.SetActive(true);
            }*/
            
        }
        else
        {
            //DisableToolTip();
            //Debug.Log("Mouse is not over a UI element with tag containing 'ToolTip'.");
            // You can add your tooltip hiding logic here
        }

        //HandleRightClick();
    }
    public Canvas canvas;
    public RectTransform tooltipRectTransform;
    void RefreshToolTip()
    {
        text.text = currentItem.ToString();
    }

    void MoveTransform(RectTransform transform, Vector2 position, bool scale = true)
    { 
        var test = position;
        if (scale)
        {
            test.x = (position.x / Camera.main.scaledPixelWidth) * 1920;
            test.y = (position.y / Camera.main.scaledPixelHeight) * 1080;
        }
        transform.anchoredPosition = test;
    }
    void MoveToolTip()
    {
        MoveTransform(tooltipRectTransform, mousePosition, true);
    }
    // Method to check if the mouse is over any UI element with a tag containing the specified substring
    private GameObject IsMouseOverToolTip()
    {  
        // Cast a ray from the mouse position and check if it hits any UI element
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Create a list to store the raycast results
        var results = new List<RaycastResult>();

        // Raycast using the current event data
        EventSystem.current.RaycastAll(eventData, results); 
        // Check each raycast result for a matching tag substring
        foreach (var result in results)
        {
            if (result.gameObject.TagContains(TOOLTIP_STRING))
            {
                mousePosition = result.screenPosition;
                return result.gameObject;
            }
        }

        return null;
    }
}
  
