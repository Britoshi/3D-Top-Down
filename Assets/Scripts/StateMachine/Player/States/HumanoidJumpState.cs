using UnityEngine;

namespace Game.StateMachine.Player
{
    public class HumanoidJumpState : EntityJumpState
    {
        public HumanoidJumpState(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {

        }
    }
}