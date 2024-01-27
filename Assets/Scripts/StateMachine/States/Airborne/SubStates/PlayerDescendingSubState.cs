using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PlayerDescendingSubState : PlayerAirborneSubStateBase
    { 
        public PlayerDescendingSubState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :
            base(currentContext, playerStateFactory)
        {
            ChangeAnimation("Air Descend");
        }
        public override void EnterState()
        {

        }
        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            return true;
        }
        public override bool FixedUpdateState()
        {
            return true;
        }
        public override void ExitState() { }
        public override void InitializeSubState()
        {
            base.InitializeSubState();
        }
        public override bool CheckSwitchStates()
        {
            //if (Ctx.Player.rb.velocity.y > 0)
            //    return SwitchState(Factory.AirborneAscend()); 
            //else if (Ctx.Player.rb.velocity.y > Ctx.AirborneApexThreshHold)
            //    return SwitchState(Factory.AirborneApex());
            return false;
        }

    }
}
