using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PlayerWallState : PlayerRootState
    {
        public PlayerWallState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory) : base(currentContext, entityStateFactory)
        {
        }

        public override bool CheckSwitchStates()
        {
            if (!Ctx.IsWalkingIntoWall) 
            {
                if (Ctx.IsGrounded)
                    return SwitchState(Factory.Grounded());
                else
                    return SwitchState(Factory.Airborne());
            }


            if (Ctx.JumpButtonDown)
            {
                if (Ctx.CanJump())
                    return SwitchState(Factory.Jump());
                else if (Ctx.CanDoubleJump())
                    return SwitchState(new PlayerDoubleJumpState(Ctx, Factory));
            }

            return false;
        }

        public override void EnterState()
        {
            //Reset Velocity
            Ctx.HorizontalVelocity = 0;
            InitializeSubState();
        }

        public override void ExitState()
        {

        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            base.UpdateState();
            return true;
        }
        public override bool FixedUpdateState()
        {
            return true;
        }

        public override void InitializeSubState()
        {
            if (Ctx.IsGrounded)
                SetSubState(new PlayerWallGroundedSubState(Ctx, Factory));
            else
                SetSubState(new PlayerWallAirborneSubState(Ctx, Factory));
        }
    }
}
