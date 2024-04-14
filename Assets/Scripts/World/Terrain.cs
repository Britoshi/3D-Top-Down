using System.Collections.Generic;
using System;
using Game;
using UnityEngine;

public class Terrain : IComparable<Terrain>, IEquatable<Terrain>
{
    public const string HOLDER_TRANSFORM_NAME = "TERRAINS";
    public const string RENDER_DISTANCE_TAG = "Render Distance";

    public Vec2 position;

    public GameObject gameObject;
    public Transform transform;

    public float renderDistanceInChunks;

    public Terrain(Transform transform, Vec2 position)
    {
        this.transform = transform;
        gameObject = transform.gameObject;
        //This depends on the gameObject having the right naming!

        var token = gameObject.name.Split(':');
        if (token[0] != RENDER_DISTANCE_TAG)
            throw new System.Exception("How the fuck does this terrain not have a correct render distance tag?");

        renderDistanceInChunks = float.Parse(token[1]); 
        this.position = position;

    }

    public int CompareTo(Terrain other) => position.CompareTo(other.position);
    public bool Equals(Terrain other) => position.Equals(other.position);
}