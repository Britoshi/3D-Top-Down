using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponObject : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetHitBox(bool state)
        {
            if (this is not IHasHitBox)
            {
                print("No hitbox, skip");
                return;
            } 
            var hitbox = (IHasHitBox)this;
            hitbox.HitBox.SetActive(state);
        }
    }
}