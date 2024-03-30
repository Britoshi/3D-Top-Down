using Game.Abilities;
using Game.Items;
using System;
using System.ComponentModel;
using UnityEngine;
namespace Game
{

    [RequireComponent(typeof(Status))]
    public class Entity : BritoBehavior, IComparable<Entity>
    {
        [Flags]
        public enum FactionLayer
        {
            DEFAULT = 0,
            NEUTRAL = 1 << Faction.NEUTRAL,
            PLAYER = 1 << Faction.PLAYER,
            HOSTILE = 1 << Faction.HOSTILE, 
        }
        public enum Faction
        {
            NEUTRAL = 0,
            PLAYER = 1,
            HOSTILE = 2,
        }

        public int ID => GetInstanceID();

        [Header("Parameter")]
        public Faction faction;
        [Header("Entity Component")]
        public Status status;  
        public Inventory inventory;
        public StateMachine.StateMachine stateMachine;
        public EntityAbilityController abilityController;
        [Header("Unity Component")]
        public new Rigidbody rigidbody;
        public Animator animator;
        public TransformData transforms;
        public Collider hitBox;


        public virtual void Awake()
        {
            InitializeComponents(); 
            status.owner = this;
        }

        public virtual void Start()
        {
            if (hitBox == null) Debug.LogError("Entity must have hitbox assigned");
        }

        private void InitializeComponents()
        {
            rigidbody = GetComponent<Rigidbody>(); 
            transforms.Initialize(this);

            status = status != null ? status : GetComponent<Status>();
            status.Initialize(this);

            inventory.Initialize(this);


            stateMachine =
            stateMachine != null ? stateMachine : GetComponent<StateMachine.StateMachine>();
            stateMachine.Initialize(this);

            abilityController =
            abilityController != null ? abilityController : GetComponent<EntityAbilityController>();
            abilityController.Initialize(this);
        }

        internal Transform GetEquipmentTransform(EquipmentLocation targetArea)
        {  
            transforms.targets.TryGetValue(targetArea, out Transform targetTransform);
            return targetTransform;  
        }

        #region FACTION
        public bool IsHostileTo(Entity other)
        {
            if (other.faction == 0 || faction == 0) return false;
            else if (faction != other.faction) return true;
            return false;
        }
        public bool IsFriendlyTo(Entity other)
        {
            if (faction == other.faction) return true;
            return false;
        }
        #endregion

        public void PrimaryAttack(Entity other, float amount)
        {
            var affect = new HealthAffect(HealthAffectType.PHYSICAL, new ResourceModifier(amount, ResourceAffectType.Additive));
            var data = HealthModificationData.AutoAttack(this, other, affect);
            float calculatedDamage = data.modifyValue;

            other.status.HP.Subtract(calculatedDamage);
            other.stateMachine.Interrupt(other.stateMachine.Factory.Stagger());
            //other.stateMachine.PlayHurtAnimation();
            print("lol hurt");
        }

        public void EnableWeaponHitBox()
        { 
            var weaponSlot = inventory.weaponSlot;
            if (!weaponSlot.HasItem()) return; 
            if (weaponSlot.weapon.runtimeObjects.Count == 0) return; 
            foreach (var obj in weaponSlot.weapon.runtimeObjects)
                obj.SetHitBox(true); 
        }
        public void DisableWeaponHitBox()
        { 
            //Precontext that the disable should be called with enable.
            foreach (var obj in inventory.weaponSlot.weapon.runtimeObjects)
                obj.SetHitBox(true);
        }

        public int CompareTo(Entity other)
        {
            return GetInstanceID().CompareTo(other.GetInstanceID());
        }

        #region Getter
        public Stat this[byte id] => status[id];
        public Stat this[int id] => status[id];
        public Stat this[StatID id] => status[id];
        public Attribute this[AttributeID id] => status[id];
        public Resource this[ResourceID id] => status[id];
        public Stat this[HealthAffectType id] => status[id];
        #endregion
    }
}