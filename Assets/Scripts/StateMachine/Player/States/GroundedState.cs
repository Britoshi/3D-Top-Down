using UnityEngine;

namespace Game.StateMachine.Player
{
    public class GroundedState : EntityGroundedState
    {
        protected HumanoidStateMachine hCtx;
        protected HumanoidStateFactory hFactory;
        public GroundedState(HumanoidStateMachine currentContext, HumanoidStateFactory playerStateFactory) : 
            base(currentContext, playerStateFactory)
        {
            hCtx = currentContext;
            hFactory = playerStateFactory;
        }

        public override bool CheckSwitchStates()
        {
            if (!Ctx.IsGrounded) return SwitchState(Factory.Airborne()); 
            //If grounded and aiming
            //else if (hCtx.IsAiming) return SwitchState(hFactory.Aiming());

            return false;
        }
    }
}