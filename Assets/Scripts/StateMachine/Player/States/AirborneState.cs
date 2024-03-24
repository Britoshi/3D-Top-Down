using UnityEngine;

namespace Game.StateMachine.Player
{

    public class AirborneState : EntityAirborneState
    {
        protected HumanoidStateMachine hCtx;
        protected HumanoidStateFactory hFactory;
        public AirborneState(HumanoidStateMachine currentContext, HumanoidStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            hCtx = currentContext;
            hFactory = playerStateFactory;
        }
        public override bool CheckSwitchStates()
        {
            if (Ctx.IsGrounded)
            { 
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