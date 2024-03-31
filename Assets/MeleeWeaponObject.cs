using Game.Abilities; 
using System.Collections;
using System.Collections.Generic; 
using UnityEngine; 

namespace Game.Weapons
{
    public enum HitBoxType
    {
        BOX, SPHERE, CAPSULE
    }
    public class MeleeWeaponObject : WeaponObject, IHasHitBox
    { 
        public bool HitBoxScan { set; get; }

        public GameObject slashFX;
        [field: SerializeField] public Collider HitBox { get; set; }
        public FactionTarget targetLayer;

        SortedSet<Entity> contacted, affected;
        object[] currentArgs;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            HitBoxScan = false;
            HitBox.enabled = false;
            contacted = new();
            affected = new();
            Physics.IgnoreCollision(entity.hitBox, HitBox);
        }

        public override void SpawnFX()
        {
            base.SpawnFX();
            var rot = transform.rotation;
            var g = Instantiate(slashFX, entity.transform.position, rot);
            g.transform.LookAt(transform.right + transform.position);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            
            

            if (HitBoxScan)
            {
                //if (HitBox is BoxCollider)
                //{
                //    var colliders = Physics.OverlapBox(HitBox.transform.position + HitBox.bounds.center, HitBox.bounds.size, HitBox.transform.rotation);
                //    foreach (var col in colliders)
                //    {
                //        OnDetectCollision(col);
                //    }
                //}
                //else
                //{
                //    throw new System.Exception("Not supported collider type");
                //}

                foreach (var contact in contacted)
                {
                    if (affected.Contains(contact)) continue; 
                    ability.Affect(contact, currentArgs); 
                    affected.Add(contact);
                    Physics.IgnoreCollision(HitBox, contact.hitBox);
                }
            }
            else
            {
            }
        }
        NAbility ability;
        protected virtual void TurnOn(object[] args)
        {
            contacted = new SortedSet<Entity>();
            affected = new SortedSet<Entity>();
            ability = entity.abilityController.CurrentAbility;
            currentArgs = args;
        }
        protected virtual void TurnOff()
        {
            foreach(var affected in affected)
            {
                Physics.IgnoreCollision(HitBox, affected.hitBox,  false);
            }
            contacted = null;
            affected = null;
        }
        public void Toggle(bool state, object[] args)
        {

            HitBoxScan = state;
            HitBox.enabled = state;
            if (state) TurnOn(args); else TurnOff();
        }

        void  OnDetectCollision(Collider other)
        {
            if ((other.gameObject.layer & 1 << 15) == 1 << 15) return;
            if (other == entity.hitBox) return;
            var targetEntity = other.GetComponentInParent<Entity>();
            if (!targetEntity) return;

            if ((targetLayer & FactionTarget.HOSTILE) == FactionTarget.HOSTILE)
            {
                if (!entity.IsFriendlyTo(targetEntity))
                    contacted.Add(targetEntity);
            }
            if ((targetLayer & FactionTarget.FRIENDLY) == FactionTarget.FRIENDLY)
            {
                if (!entity.IsHostileTo(targetEntity))
                    contacted.Add(targetEntity);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnDetectCollision(collision.collider);
        }

    }
}