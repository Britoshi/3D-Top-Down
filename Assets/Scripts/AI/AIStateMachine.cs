using Game.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.AI
{
    [RequireComponent(typeof(Entity))]
    public class AIStateMachine : BritoBehavior
    {
        public Entity entity;
        public NavMeshAgent navMesh;
        public AIState CurrentState { get; internal set; } 
        public AIStateFactory Factory { get; private set; }

        public virtual void AssignFactory() => Factory = new AIStateFactory(this);

        public virtual void Start()
        {
            entity = GetComponent<Entity>();
            navMesh = GetComponent<NavMeshAgent>();
            AssignFactory();
            ChangeState(Factory.Default());
        }

        public virtual void Update()
        {
            CurrentState.Update();
        }
        public virtual void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }

        public void ChangeState(AIState state)
        {
            CurrentState = state;
            state.Enter();
        }

    }
}