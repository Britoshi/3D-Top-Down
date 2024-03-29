using Game.Abilities;
using Game.Items;
using System;
using System.ComponentModel;
using UnityEngine;
namespace Game
{

    [RequireComponent(typeof(Status))]
    public class Entity : BritoBehavior
    {
        public enum Faction
        {
            NEUTRAL,
            PLAYER,
            HOSTILE,
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

        public void TriggerWeaponHitBox(int index)
        {

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