using UnityEngine;

namespace Game.StateMachine.Player
{

    public class AirborneState : EntityAirborneState
    {
        PStateMachine context;
        public AirborneState(PStateMachine currentContext, PStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            context = currentContext;
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