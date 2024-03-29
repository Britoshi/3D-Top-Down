using UnityEngine.AI;

namespace Game.AI
{
    public class AIChaseEnemy : AIState
    {
        NavMeshAgent agent;
        Entity target;
        public AIChaseEnemy(AIStateMachine currentContext, AIStateFactory aiFactory, Entity target, AIState superState = null) : base(currentContext, aiFactory, superState)
        {
            this.target = target;
            agent = CTX.navMesh;
        }

        protected override bool CheckSwitchStates()
        {
            return false;
        }

        protected override void OnEnterState()
        {
            entity.stateMachine.movementOverride = true;
            entity.stateMachine.rotationOverride = true;
            entity.stateMachine.IsMoving = true;
            //entity.stateMachine.IsRunning = true;
        }

        protected override void OnExitState()
        {  
            entity.stateMachine.movementOverride = false;
            entity.stateMachine.rotationOverride = false;
            entity.stateMachine.IsMoving = false;
            //entity.stateMachine.IsRunning = false;
        }

        protected override void InitializeSubState()
        {

        }

        protected override void OnUpdate()
        {
            var x = entity.transform.forward.x;
            var y = entity.transform.forward.z;
            entity.stateMachine.AssertInput(x, y);
        }

        protected override void OnFixedUpdate()
        {
            agent.destination = target.transform.position;
        }
    }
}