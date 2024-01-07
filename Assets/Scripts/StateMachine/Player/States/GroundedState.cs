using UnityEngine;

namespace Game.StateMachine.Player
{
    public class GroundedState : EntityGroundedState
    {
        public GroundedState(PStateMachine currentContext, PStateFactory entityStateFactory) : 
            base(currentContext, entityStateFactory)
        {

        }
    }
}