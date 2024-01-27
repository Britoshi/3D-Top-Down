using UnityEngine;

namespace Game.StateMachine.Player
{
    public class PJumpState : EntityJumpState
    {
        public PJumpState(PStateMachine currentContext, PStateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {

        }
    }
}