using System.Drawing;
using UnityEngine;

namespace Game.AI
{
    public class AIWanderAround : AIState
    {
        public const float DEFAULT_DETECTION_RADIUS = 10f;

        public AIWanderAround(AIStateMachine currentContext, AIStateFactory aiFactory, AIState superState = null) : base(currentContext, aiFactory, superState)
        {

        }

        protected override bool CheckSwitchStates() => false;

        protected override bool CheckSwitchStatesFixed()
        {
            if (DetectHostile()) return true;
            return false;
        }

        protected override void OnEnterState()
        {

        }

        protected override void OnExitState()
        {

        }

        protected override void InitializeSubState()
        {

        }

        protected override void OnUpdate()
        {

        }

        protected bool DetectHostile()
        {
            Collider[] colliders = Physics.OverlapSphere(entity.transform.position, DEFAULT_DETECTION_RADIUS, 1 << 16); // Adjust the radius as needed
            Entity nearest = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                float distance = Vector3.Distance(entity.transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    if (collider == entity.hitBox) continue;
                    var other = collider.GetComponentInParent<Entity>();
                    if (other == null) throw new System.Exception("How does an entity not have a entity script attached?");
                    if (!entity.IsHostileTo(other)) continue; 
                    minDistance = distance;
                    nearest = other;
                }
            } 
            return nearest != null ? SwitchState(Factory.Chase(nearest)) : false;
        }

        protected override void OnFixedUpdate()
        {

        }
    }
}