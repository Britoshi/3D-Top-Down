using Game;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Rendering;
using UnityEngine;

//Singleton https://onlinesequencer.net/3918696
public class MapProcessor : BritoBehavior
{
    struct ChunkCache
    {
        public GameObject obj;
        public SortedDictionary<int, GameObject> floors;

        public ChunkCache(GameObject obj)
        {
            this.obj = obj;
            floors = new();
        }

        public readonly GameObject this[int value] => floors[value];
    }

    // Tagging System =>
    // 1. Default -> just shove em inside the Flooring
    // 2. MAP: Terrain -> new system to load in terrains //Name(int) = load distance
    // 3. MAP: Group -> does not go through the children and append each;

    //public static MapProcessor Instance { set; get; }

    public const string TAG_TERRAIN = "MAP: TERRAIN";
    public const string TAG_GROUP = "MAP: GROUP";
    public const string TAG_LIGHT= "MAP: LIGHT";

    public GameObject targetMap;
    public GameObject processedMap;

    private Transform chunkParent, terrainParent;

    private SortedDictionary<Vec2, ChunkCache> chunkCache; 

    void ProcessTerrain(Transform target)
    {
        float renderDistance = 128;
        if (target.TryGetComponent<Renderer>(out var renderer))
            renderDistance = renderer.bounds.size.x > renderer.bounds.size.y ? renderer.bounds.size.x : renderer.bounds.size.y;
        else if (target.TryGetComponent<Collider>(out var collider))
            renderDistance = collider.bounds.size.x > collider.bounds.size.y ? collider.bounds.size.x : collider.bounds.size.y;
        else Debug.LogError("Couldn't find any measurement of the render distance. What the fuck");

        renderDistance /= Chunk.SIZE;
        renderDistance++;

        var clone = Instantiate(target.gameObject, target.position,target.rotation, terrainParent);
        clone.transform.localScale = target.lossyScale;
        clone.name = $"{Terrain.RENDER_DISTANCE_TAG}:{renderDistance}:in chunks";
    }

    void ProcessObject(Transform target)
    {
        int x = (int)Mathf.Floor(target.position.x / Chunk.SIZE);
        int y = (int)Mathf.Floor(target.position.y / Floor.HEIGHT);
        int z = (int)Mathf.Floor(target.position.z / Chunk.SIZE);
        var position = new Vec2(x, z);

        //Fetch Chunk
        if (!chunkCache.TryGetValue(position, out var chunk))
        {
            var chunkHolder = new GameObject(x + "x" + z);
            chunkHolder.transform.position = (position * Chunk.SIZE).ToVector3TopDown();
            chunkHolder.transform.parent = chunkParent.transform;
            chunk = new ChunkCache(chunkHolder);
            chunkCache.Add(position, chunk);
        }

        //Fetch Floor
        if (!chunk.floors.TryGetValue(y, out GameObject floor))
        {
            floor = new GameObject(y.ToString());
            floor.transform.position = new (position.x, y, position.y);
            floor.transform.parent = chunk.obj.transform;
            chunk.floors.Add(y, floor);
        }

        //Now Append new shitter;
        var newObject = Instantiate(target.gameObject, target.position, target.rotation, floor.transform);
        newObject.transform.localScale = target.lossyScale;
        newObject.name = target.name;
        //Done?
    }

    void Process(Transform target)
    {
        switch (target.tag)
        {
            case TAG_TERRAIN:
                ProcessTerrain(target);
                return;
            case TAG_GROUP:
                if (target.childCount == 0) Debug.Log(target.name + "(Group Object) does not have any child. Might wanna fix that.");
                ProcessObject(target);
                return;
            case TAG_LIGHT:
                Debug.Log("NOT  WORKING YET");
                return;
        }
        if(target.childCount == 0)  
            ProcessObject(target);
        else foreach (Transform child in target)
            Process(child);
    }

    public void ProcessMap()
    {
        if (!targetMap)
        {
            Debug.Log("You need a map to process?");
            return;
        }
        ProcessMap(targetMap);
    }

    public void ProcessMap(GameObject target)
    {

        processedMap = new GameObject("PROCESSED: " + target.name);
        processedMap.tag = "MAP";

        chunkParent = new GameObject(Chunk.HOLDER_TRANSFORM_NAME).transform;
        chunkParent.parent = processedMap.transform;

        terrainParent = new GameObject("TERRAINS").transform;
        terrainParent.parent = processedMap.transform;

        chunkCache = new();
        Process(target.transform);
    }
}
