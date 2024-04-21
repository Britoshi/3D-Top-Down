
using Game;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer : BritoBehavior
{
    public static SortedDictionary<int, GameObject> renderedChunks;

    public void Awake()
    {
        renderedChunks = new();
    }

    public static void RenderMap(string mapName) { }

    public static void UnrenderMap(string mapName)
    {
        //This probably will be removed.
    }

    public static void StackRender(Transform target)
    {
        string[] token = target.name.Split(':');
         int renderStack = int.Parse(token[1]) + 1;
        if (renderStack == 1)
        {
            if (target.CompareTag(Map.TAG))
                RenderMap(token[0]);
            else StackRender(target.parent);
        }
    }

    public static void UnstackRender(Transform target)
    {
        string[] token = target.name.Split(':');
        int renderStack = int.Parse(token[1]) - 1;
        if (renderStack == 0)
        {
            if (target.CompareTag(Map.TAG))
                UnrenderMap(token[0]);
            else UnstackRender(target.parent);
        }
        target.name = $"{token[0]}:{renderStack}";
    }

    public static void Render(GameObject target)
    {

        StackRender(target.transform.parent);
    }

    public static void Unrender(GameObject target)
    {

        UnstackRender(target.transform.parent);
    }
}