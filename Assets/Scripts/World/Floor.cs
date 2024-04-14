using Game;
using System;
using UnityEngine;

public class Floor : IComparable<Floor>
{
    public const int HEIGHT = 64;

    public int height;
    public Chunk chunk;
    public GameObject gameObject;
    public Transform transform;

    ///Dummy Object for comparison
    private Floor(int height) => this.height = height;
    public static Floor Comparer(int height) => new Floor(height);

    public Floor(Chunk chunk, Transform transform)
    {
        this.chunk = chunk;
        this.transform = transform;
        gameObject = transform.gameObject;
        //This depends on the gameObject having the right naming!
        height = int.Parse(gameObject.name);
    }

    public Vec2 GetPosition() => chunk.position;

    public int CompareTo(Floor other) => height.CompareTo(other.height);
}