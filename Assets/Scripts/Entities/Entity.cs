using UnityEngine;

namespace Game
{

    [RequireComponent(typeof(Status))]
    public class Entity : BritoBehavior
    {
        public string ID { set; get; }
        public Status status;
        public Inventory inventory;
        public StateMachine stateMachine;
        public new Rigidbody rigidbody; 

        public void Awake()
        {
            InitializeComponents(); 
            status.owner = this;
        }

        private void InitializeComponents()
        {
            rigidbody = GetComponent<Rigidbody>(); 

            status = status != null ? status : GetComponent<Status>();
            status.Initialize(this);

            inventory ??= new();
            inventory.Initialize(this);

            stateMachine ??= GetComponent<StateMachine>();
            stateMachine.Initialize(this);
        }
    }
}