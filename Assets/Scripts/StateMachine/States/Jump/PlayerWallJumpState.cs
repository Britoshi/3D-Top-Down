using Game; 
using System;
using UnityEngine;

namespace Game
{
    public class PlayerWallJumpState : PlayerRootState
    {
        public PlayerWallJumpState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        { 

        }

        public override void EnterState()
        {
            Ctx.PreventLongJump = true;
            Ctx.ButtonUpFailSafe = true; 
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
        public override void InitializeSubState() { }
        public override bool CheckSwitchStates() => SwitchState(Factory.Airborne()); 
         
    }
}
