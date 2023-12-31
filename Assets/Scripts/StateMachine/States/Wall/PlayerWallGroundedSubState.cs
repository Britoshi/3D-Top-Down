using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PlayerWallGroundedSubState : PlayerBaseState
    {
        public PlayerWallGroundedSubState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory, PlayerBaseState superState = null) : 
            base(currentContext, entityStateFactory, superState)
        {

        }

        public override bool CheckSwitchStates()
        {
            //if (Ctx.JumpTrigger)
            //    return SwitchState(Factory.Jump());
            if (!Ctx.IsGrounded) return SwitchState(new PlayerWallAirborneSubState(Ctx, Factory));
            return false;
        }

        public override void EnterState()
        {
            ChangeAnimation("Touching Wall Grounded");
            Ctx.DoubleJumpCount = 0;
        }

        public override void ExitState()
        {

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

            Ctx.ResetJumpGracePeriod();
            return true;
        }
    }
}
