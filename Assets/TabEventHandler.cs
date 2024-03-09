using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.EventSystems;

public class TapEventHandler : MonoBehaviour
{
    

    void HandleTap(Vector2 touchPosition)
    {
        // Convert the touch position to world coordinates if needed
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        // Do something with the tap position
        DebugText.Log(worldPosition);
    }
}