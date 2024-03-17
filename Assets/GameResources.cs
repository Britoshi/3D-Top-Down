using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameResources : BritoBehavior
    {
        [SerializeField] Sprite missingTexture;
        public static GameResources Instance;
        private void Awake()
        {
            Instance = this;
        }
        public static Sprite MissingTexture => Instance.missingTexture;
    }
}