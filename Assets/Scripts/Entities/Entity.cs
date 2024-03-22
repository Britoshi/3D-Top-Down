using Game.Abilities;
using Game.Items;
using UnityEngine;
namespace Game
{  
    [RequireComponent(typeof(Status))]
    public class Entity : BritoBehavior
    {
        public string ID { set; get; }
        public Status status;  
        public Inventory inventory;
        public StateMachine.StateMachine stateMachine;
        public EntityAbilityController abilityController;
        public new Rigidbody rigidbody;
        public Animator animator;
        public TransformData transforms;

        public virtual void Awake()
        {
            InitializeComponents(); 
            status.owner = this;
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

        public Stat this[byte id] => status[id];
        public Stat this[int id] => status[id];
        public Stat this[StatID id] => status[id];
        public Attribute this[AttributeID id] => status[id];
        public Resource this[ResourceID id] => status[id];
        public Stat this[HealthAffectType id] => status[id];
    }
}