using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class PlayerGroundedState : PlayerRootState
    {
        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory) : 
            base(currentContext, entityStateFactory)
        {
        } 

        public override void EnterState()
        {
            InitializeSubState();
            Ctx.DoubleJumpCount = 0; 
        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            base.UpdateState();
            Ctx.ResetJumpGracePeriod();
            return true;
        }
        public override bool FixedUpdateState()
        { 
            return true;
        }

        public override void ExitState() 
        {

        }
        public override void InitializeSubState()
        {
            if (Ctx.IsHoldingDown)
            {
                if (Ctx.IsMoving)
                    SetSubState(Factory.Crawl(this));
                else
                    SetSubState(Factory.Crouch(this));
            }
            else
            {
                if (!Ctx.IsMoving && !Ctx.IsRunning)
                    SetSubState(Factory.Idle(this));
                else if (Ctx.IsMoving && !Ctx.IsRunning) 
                    SetSubState(Factory.Walk(this));  
                else 
                    SetSubState(Factory.Run(this));  
            }
        }
        public override bool CheckSwitchStates()
        {
            if (Ctx.JumpTrigger)
                return SwitchState(Factory.Jump());
            else if (!Ctx.IsGrounded)
                return SwitchState(Factory.Airborne());
            else if (Ctx.IsWalkingIntoWall)
                return SwitchState(Factory.Wall());
            
            return false;
        } 
    }
}
