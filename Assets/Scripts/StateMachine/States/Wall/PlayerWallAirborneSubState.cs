using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class PlayerWallAirborneSubState : PlayerBaseState
    { 
        public PlayerWallAirborneSubState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory, PlayerBaseState superState = null) : 
            base(currentContext, entityStateFactory, superState)
        {

        }

        public override bool CheckSwitchStates()
        {  
            //if (Ctx.IsGrounded) 
            //    return SwitchState(new PlayerWallGroundedSubState(Ctx, Factory));
            //else if(Ctx.IsTouchingWallGrip) 
            //    return SwitchState(new PlayerWallGripSubState(Ctx, Factory));
            return false;
        }

        public override void EnterState()
        {
            ChangeAnimation("Touching Wall Airborne");
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
             
            Ctx.HandleJump(); 

            return true;
        }

    }
}
