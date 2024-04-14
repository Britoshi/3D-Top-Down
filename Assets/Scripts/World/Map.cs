using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : BritoObject, IComparable<Map>, IEquatable<Map>
{
    public string name;

    public GameObject gameObject;
    public Transform transform;

    public Chunk this[Vec2 position] => Chunks[position];

    public SortedDictionary<Vec2, Chunk> Chunks { set; get; }
    public SortedDictionary<Vec2, Terrain> Terrains { set; get; }

    public Map(GameObject target)
    {
        name = target.name;
        gameObject = target;
        transform = target.transform;

        InitializeChunks();
        InitializeTerrains();
    }

    ///This must be used on a processed map
    void InitializeChunks()
    {
        Chunks = new();
        var chunksTransform = transform.Find(Chunk.HOLDER_TRANSFORM_NAME);

        foreach (Transform child in chunksTransform)
        {
            var position = Chunk.Convert(child.name);
            Chunks.Add(position, new Chunk(child, position));  
        }
    }

    void InitializeTerrains()
    {
        Terrains = new();
        var terrainTransform = transform.Find(Terrain.HOLDER_TRANSFORM_NAME);

        foreach (Transform child in terrainTransform)
        {
            var position = new Vec2(child.position);
            Terrains.Add(position,  new Terrain(child, position));
        }
    }

    public int CompareTo(Map other) =>
        name.CompareTo(other.name);

    public bool Equals(Map other) => name.Equals(other.name);

}