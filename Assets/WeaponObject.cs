using Game.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.Entity;

namespace Game.Weapons
{
    [Flags]
    public enum FactionTarget
    {
        DEFAULT = 0,
        FRIENDLY = 1,
        HOSTILE = 1 << 1,
    }
    public class WeaponObject : BritoBehavior
    {
        public Weapon weapon;
        internal Entity entity;

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        public virtual void Initialize(Entity entity, Weapon weapon)
        {
            this.entity = entity;
            this.weapon = weapon;
        }

        public void SetHitBox(bool state, params object[] args)
        {
            if (this is not IHasHitBox)
            {
                print("No hitbox, skip");
                return;
            }

            var hitbox = (IHasHitBox)this;
            hitbox.Toggle(state, args); 
        }

        public virtual void SpawnFX()
        {

        }
    }
}