using Game; 
using System; 
using UnityEngine; 

namespace Game
{
    public class PlayerAirborneState : PlayerRootState
    {
        public PlayerAirborneState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory): base(currentContext, playerStateFactory) 
        { 
            InitializeSubState();
        }

        public override bool CheckSwitchStates()
        {   
            if (Ctx.JumpButtonDown) 
            {
                if (Ctx.CanWallJump())
                    return SwitchState(new PlayerWallJumpState(Ctx, Factory));
                else if (Ctx.CanJump())
                    return SwitchState(Factory.Jump());
                else if (Ctx.CanDoubleJump())
                    return SwitchState(new PlayerDoubleJumpState(Ctx, Factory));
            }
            else if (Ctx.IsWalkingIntoWall)
                return SwitchState(Factory.Wall());
            else if (Ctx.IsGrounded)
            {
                return SwitchState(Factory.Grounded());
            }
            return false;
        }

        public override void EnterState()
        {

        }

        public override bool UpdateState()
        {
            base.UpdateState();
            if (CheckSwitchStates()) return false;
            Ctx.HandleJump();
            return true;
        }

        public override bool FixedUpdateState()
        {
            return true;
        }

        public override void ExitState()
        { 
            base.ExitState();
        }

        public override void InitializeSubState()
        { 
            //if(Ctx.Player.rb.velocity.y > 0) 
            //    SetSubState(Factory.AirborneAscend());
            //else if (Ctx.Player.rb.velocity.y < Ctx.AirborneApexThreshHold)
            //    SetSubState(Factory.AirborneDescend());
            //else if (Ctx.Player.rb.velocity.y > Ctx.AirborneApexThreshHold)
            //    SetSubState(Factory.AirborneApex()); 
        }

        public void HandleHorizontalPhysics()
        {
            throw new NotImplementedException();
        }

    }
}
