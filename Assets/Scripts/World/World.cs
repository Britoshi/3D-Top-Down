using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class World : BritoBehavior
    {
        public static World Instance { set; get; }

        [SerializeField]
        Transform worldHolder;

        public static SortedSet<Chunk> Chunks { set; get; }

        void Awake()
        {
            Instance = this;
            InitializeWorld();
        }

        void Start()
        {

        }

        public static void InitializeWorld()
        {
            if (Instance.worldHolder == null)
                throw new System.Exception("World instance must have a worldHolder transform attached!");
            Chunks = new SortedSet<Chunk>();

            foreach (Transform child in Instance.worldHolder)
                Chunks.Add(new Chunk(child));
        }
    }
}