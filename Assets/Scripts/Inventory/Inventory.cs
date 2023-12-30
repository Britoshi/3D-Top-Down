using System;
using System.Collections.Generic;
using System.Linq; 
//using UnityEngine;

namespace Game
{

	public class Inventory
    {
        public const int MAX_ITEMS = 6;
        public List<Item> items;

        public Inventory()
        {
            items = new ();
        }

        public bool IsFull() => items.Count >= MAX_ITEMS; 
    }
}
