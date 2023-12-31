using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class PlayerWallGripSubState : PlayerBaseState
    { 
        public PlayerWallGripSubState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory, PlayerBaseState superState = null) : 
            base(currentContext, entityStateFactory, superState)
        {

        }

        public override bool CheckSwitchStates()
        {
            if (Ctx.IsGrounded)
                return SwitchState(new PlayerWallGroundedSubState(Ctx, Factory));
            else if (!Ctx.IsTouchingWallGrip)
                return SwitchState(new PlayerWallAirborneSubState(Ctx, Factory));
            else if (Ctx.JumpButtonDown) 
                return SwitchState(new PlayerWallJumpState(Ctx, Factory));
            return false;
        } 

        public override void EnterState()
        {
            ChangeAnimation("Touching Wall Airborne");
        }

        public override void ExitState()
        { 
            Ctx.ResetWallJumpGracePeriod();
        }

        public override bool FixedUpdateState()
        {
            return true;
        }

        public override void InitializeSubState()
        { 

        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
             

            float divider = .5f;
            float dragVelocity = (Physics2D.gravity.y / divider) * Time.deltaTime;

            if (Ctx.rb.velocity.y <= dragVelocity)
                Ctx.SetVelocityY(dragVelocity);
            else
                Ctx.HandleJump();
             
            Ctx.ResetWallJumpGracePeriod();

            return true;
        }

    }
}
