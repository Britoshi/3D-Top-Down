using System.Collections.Generic;
using System;
using Game;
using UnityEngine;

public class Chunk : IComparable<Chunk>, IEquatable<Chunk>
{
    public const int SIZE = 32;
    public const string HOLDER_TRANSFORM_NAME = "CHUNKS";

    public Vec2 position;
    public SortedSet<Floor> floors;
    public GameObject gameObject;
    public Transform transform;

    public static Vec2 Convert(string name)
    { 
        var token = name.Split('x');
        int x = int.Parse(token[0]);
        int y = int.Parse(token[1]);
        return new Vec2(x, y);
    }

    public Chunk(Transform transform, Vec2 position)
    {
        floors = new SortedSet<Floor>();
        this.transform = transform;
        gameObject = transform.gameObject;
        this.position = position;

        //now initialize children
        foreach (Transform child in transform)
            floors.Add(new Floor(this, child));
    }

    public bool TryGetFloor(int height, out Floor floor) =>
        floors.TryGetValue(Floor.Comparer(height), out floor);

    public int CompareTo(Chunk other) => position.CompareTo(other.position);
    public bool Equals(Chunk other) => position.Equals(other.position);
}