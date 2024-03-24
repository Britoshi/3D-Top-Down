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
    public struct ToolTipData
    {
        public string tag;
        public UnityEvent<GameObject> function;
        //public UnityEvent<GameObject> clickFunction;
    }

    [SerializeField] RectTransform rightClickMenu;
    [SerializeField] GameObject prefabMenuItem;

    [SerializeField] TMPro.TMP_Text text;

    public List<ToolTipData> toolTipDatas;
    Vector2 mousePosition;

    public const string TOOLTIP_STRING = "Tool Tip"; // Substring to check for in the tag
     

    private void Update()
    {
        // Check if the mouse is currently over any UI element with a tag containing the specified substring
        if (!GameSystem.Paused) return;
        var targetGameObject = IsMouseOverToolTip();

        if (targetGameObject)
        {

            string subtag = targetGameObject.tag.Split(':')[1];
            foreach(var data in toolTipDatas)
            {
                if (data.tag == subtag)
                {
                    data.function.Invoke(targetGameObject);
                    //if (Input.GetButtonDown("Fire1"))
                    //    data.clickFunction.Invoke(targetGameObject);
                }
            }
        }
        else
        {
            //DisableToolTip();
            //Debug.Log("Mouse is not over a UI element with tag containing 'ToolTip'.");
            // You can add your tooltip hiding logic here
        }

        //HandleRightClick();
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
  
