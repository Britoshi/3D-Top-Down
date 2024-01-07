using Game; 
using System; 
using UnityEngine; 

namespace Game.StateMachine
{
    public class EntityAirborneState : RootState
    {
        public EntityAirborneState(StateMachine currentContext, StateFactory playerStateFactory): base(currentContext, playerStateFactory) 
        { 
            InitializeSubState();
        }

        public override bool CheckSwitchStates()
        {   
            /*
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
            */ 
            if (Ctx.IsGrounded)
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
            Ctx.AirborneBehavior();
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
            SetSubState(Factory.Airborne(this));

            /*
            if(Ctx.rigidbody.velocity.y > 0) 
                SetSubState(Factory.AirborneAscend());
            else if (Ctx.rigidbody.velocity.y < Ctx.AirborneApexThreshHold)
                SetSubState(Factory.AirborneDescend());
            else if (Ctx.rigidbody.velocity.y > Ctx.AirborneApexThreshHold)
                SetSubState(Factory.AirborneApex()); */
        }  
    }
}
