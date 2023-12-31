using Game;
using System;
using UnityEngine;

namespace Game
{
    public class PlayerDoubleJumpState : PlayerRootState
    {
        public PlayerDoubleJumpState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {
            
        }

        public override void EnterState()
        {
            Ctx.PurgeJumpGracePeriod();
            ApplyJumpVelocity();
            Ctx.DoubleJumpCount++;
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

        }
        public override bool CheckSwitchStates()
        { 
            return SwitchState(Factory.Airborne()); 
        }

        public void ApplyJumpVelocity()
        {
            throw new NotImplementedException();
            /*
            var jumpTarget = Ctx.transform.position.y + Ctx.Player.Stats[(int)Stat.DOUBLE_JUMP_HEIGHT]; 
            Ctx.PreventLongJump = true;
            Ctx.ButtonUpFailSafe = true;

            var distanceToJump = jumpTarget - Ctx.transform.position.y; 
            var jumpVelocity = Mathf.Sqrt(Ctx.JUMP_GRAVITY_CONSTANT * distanceToJump);  
            Ctx.SetVelocityY(jumpVelocity);*/
        }
         
    }
}
