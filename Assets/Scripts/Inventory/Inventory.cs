using System;
using System.Collections.Generic;
using System.Linq; 
//using UnityEngine;

namespace Game
{

	public class Inventory
    {
        public Entity owner;

        public const int MAX_ITEMS = 6;
        public List<Item> items;

        public Inventory()
        {
            items = new ();
        }

        internal void Initialize(Entity owner)
        {
            this.owner = owner;
        }

        public bool IsFull() => items.Count >= MAX_ITEMS; 
    }
}
