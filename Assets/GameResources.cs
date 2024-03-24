using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct SpriteResource : IComparable<SpriteResource>
    {
        public string name;
        public Sprite image;

        public readonly int CompareTo(SpriteResource other) =>
            name.CompareTo(other.name); 
    }
    public class GameResources : BritoBehavior
    {
        [SerializeField] Sprite missingTexture;
        public static GameResources Instance;

        [SerializeField] List<SpriteResource> _images;
        SortedDictionary<string, Sprite> images;
        private void Awake()
        {
            Instance = this;
            images = new();
            foreach (var image in _images) images.Add(image.name.ToLower(), image.image); 
        }
        public void Start()
        {
            
        }
        internal static Sprite GetIcon(string name)
        {
            name = name.ToLower();
            if(Instance.images.TryGetValue(name, out var image) ) return image;
            return MissingTexture;
        }

        public static Sprite MissingTexture => Instance.missingTexture;
    }
}