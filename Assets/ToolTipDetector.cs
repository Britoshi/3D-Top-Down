using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[Serializable]
public class ToolTip : IEquatable<ToolTip>
{
    public bool disable =  true;
    public string title, message;

    public bool Equals(ToolTip other)
    {
        return ToString() == other.ToString();
    }
    public void Disable() => disable = true;
    public void Set(string title, string message)
    {
        this.title = title;
        this.message = message;
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
        public UnityEvent<int, ToolTip> func;
    }

    [SerializeField] TMPro.TMP_Text text;

    public List<ToolTipItem> tooltips;
    Vector2 mousePosition;

    public const string TOOLTIP_STRING = "ToolTip"; // Substring to check for in the tag

    ToolTip current;

    
    void DisableToolTip()
    { 
        current = null;
        tooltipRectTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        // Check if the mouse is currently over any UI element with a tag containing the specified substring
        if (!GameSystem.Paused) return;
        var isMouseOver = IsMouseOverToolTip();

        if (isMouseOver)
        {
            var prevTT = current;
            string subtag = isMouseOver.tag.Split(':')[1];
            foreach (var tooltip in tooltips)
            {
                if(tooltip.tag == subtag)
                {
                    if(int.TryParse(isMouseOver.name,  out int index))
                    {
                        current ??= new();
                        tooltip.func.Invoke(index, current);
                        if (current.disable)
                        {
                            DisableToolTip();
                            return;
                        }
                        break;
                    } 
                }
            }
            if(current != null && prevTT != null)
            {
                RefreshToolTip();
                MoveToolTip();
                tooltipRectTransform.gameObject.SetActive(true);
            }
            
        }
        else
        {
            DisableToolTip();
            //Debug.Log("Mouse is not over a UI element with tag containing 'ToolTip'.");
            // You can add your tooltip hiding logic here
        }
    }
    public Canvas canvas;
    public RectTransform tooltipRectTransform;
    void RefreshToolTip()
    {
        text.text = current.ToString();
    }
    void MoveToolTip()
    {
        var test = mousePosition;
        test.x = (mousePosition.x / Screen.width) * 1920;
        test.y = (mousePosition.y / Screen.height) * 1080;
        tooltipRectTransform.anchoredPosition = test;
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
  
