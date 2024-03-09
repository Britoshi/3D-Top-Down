using UnityEngine;

namespace Game.StateMachine.Player
{
    public class GroundedState : EntityGroundedState
    {
        protected PStateMachine pCtx;
        protected PStateFactory pFactory;
        public GroundedState(PStateMachine currentContext, PStateFactory playerStateFactory) : 
            base(currentContext, playerStateFactory)
        {
            pCtx = currentContext;
            pFactory = playerStateFactory;
        }

        public override bool CheckSwitchStates()
        {
            if (!Ctx.IsGrounded) return SwitchState(Factory.Airborne()); 
            //If grounded and aiming
            else if (pCtx.IsAiming) return SwitchState(pFactory.Aiming());

            return false;
        }
    }
}