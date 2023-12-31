using Game;
using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Game
{
    public class PlayerJumpState : PlayerRootState
    {
        float deadtime = 0;

        public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {
            
        }

        public override void EnterState()
        {
            deadtime = 0;
            Ctx.JumpTrigger = false;
            ApplyInitialJump();
            Ctx.PurgeJumpGracePeriod();
            ChangeAnimation("Air Ascend");
        }

        public override bool UpdateState()
        {
            if (deadtime > 0.1f)
            {
                if (CheckSwitchStates()) return false;
            }
            else
            {
                deadtime += Time.deltaTime;
                Ctx.HandleJump();
            }
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
            if (Ctx.IsWalkingIntoWall) return SwitchState(new PlayerWallState(Ctx, Factory));
            return SwitchState(Factory.Airborne()); 
        }

        public void ApplyInitialJump()
        {
            //Ctx.JumpTargetLong = Ctx.transform.position.y + Ctx.Player.Stats[Stat.LONG_JUMP_HEIGHT].GetValue();
            //Ctx.JumpTargetShort = Ctx.transform.position.y + Ctx.Player.Stats[Stat.SHORT_JUMP_HEIGHT].GetValue();
            Ctx.PreventLongJump = false;
            Ctx.ButtonUpFailSafe = false;

            Ctx.UpdateJumpVariables();

            Ctx.SetVelocityY(Ctx.JumpVelocityShort);

            throw new NotImplementedException();
        }
         
    }
}
