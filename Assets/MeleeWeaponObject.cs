using Game.Abilities; 
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine; 

namespace Game.Weapons
{
    public class MeleeWeaponObject : WeaponObject, IHasHitBox
    {
        public bool HitBoxScan { set; get; }
        [field: SerializeField] public Collider HitBox { get; set; }
        public FactionTarget targetLayer;

        SortedSet<Entity> contacted, affected;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            HitBoxScan = false;
            HitBox.enabled = false;
            contacted = new();
            affected = new();
        }
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();


            if (HitBoxScan)
            {
                foreach(var contact in contacted)
                {
                    if (affected.Contains(contact)) continue; 
                    ability.Affect(contact); 
                    affected.Add(contact);
                }
            }
            else
            {
                contacted = null;
                affected = null;
            }
        }
        NAbility ability;
        void TurnOn()
        {
            contacted = new SortedSet<Entity>();
            affected = new SortedSet<Entity>();
            ability = entity.abilityController.CurrentAbility;
        }
        void TurnOff()
        {

        }
        public void Toggle(bool state)
        {
            HitBoxScan = state;
            HitBox.enabled = state;
            if (state) TurnOn(); else TurnOff();
        }

        private void OnTriggerEnter(Collider other)
        {
            var targetEntity = other.GetComponentInParent<Entity>();
            if (!targetEntity) throw new System.Exception("How does a collider not have an entity script!?");

            if((targetLayer & FactionTarget.HOSTILE) == FactionTarget.HOSTILE)
            {
                if(!entity.IsFriendlyTo(targetEntity))
                    contacted.Add(targetEntity);
            }
            if((targetLayer & FactionTarget.FRIENDLY) == FactionTarget.FRIENDLY)
            {
                if (!entity.IsHostileTo(targetEntity))
                    contacted.Add(targetEntity);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            
        }

    }
}