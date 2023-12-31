using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class PlayerAscendingSubState : PlayerAirborneSubStateBase
    { 
        public PlayerAscendingSubState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) :
            base(currentContext, playerStateFactory)
        {
            ChangeAnimation("Air Ascend"); 
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
            if (Ctx.Player.rb.velocity.y < Ctx.AirborneApexThreshHold)
                return SwitchState(Factory.AirborneDescend());
            else if (Ctx.Player.rb.velocity.y < Ctx.AirborneApexThreshHoldFromAscent)
                return SwitchState(Factory.AirborneApex());
            return false;
        }

    }
}
