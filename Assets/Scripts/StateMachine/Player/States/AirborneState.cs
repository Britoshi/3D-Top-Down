using UnityEngine;

namespace Game.StateMachine.Player
{

    public class AirborneState : EntityAirborneState
    {
        protected PStateMachine pCtx;
        protected PStateFactory pFactory;
        public AirborneState(PStateMachine currentContext, PStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            pCtx = currentContext;
            pFactory = playerStateFactory;
        }
        public override bool CheckSwitchStates()
        {
            if (Ctx.IsGrounded)
            {
                if (pCtx.IsAiming)
                    return SwitchState(pFactory.Aiming());
                else 
                    return SwitchState(Factory.Grounded());
            }
            return false;
        }

        public override bool UpdateState()
        {
            base.UpdateState();
            if (CheckSwitchStates()) return false;
            Ctx.AirborneBehavior();
            return true;
        }
    }
}