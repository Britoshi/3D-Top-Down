using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class MeleeWeaponObject : WeaponObject, IHasHitBox
    {
        [field: SerializeField] public GameObject HitBox { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}