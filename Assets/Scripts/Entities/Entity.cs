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
            status ??= GetComponent<Status>();
            status.owner = this;
        }


        public void AutoAttack(Entity other)
        {
            EntityUtil.AutoAttack(status, other.status);
        }
    }
}