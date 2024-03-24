using Game; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.StateMachine.Player
{
    public class HumanoidRunSubState : HumanoidGroundedSubStateBase
    {
        public HumanoidRunSubState(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory, BaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {
        }
        public override void EnterState()
        {
            base.EnterState(); 
        }
        public override bool UpdateState()
        {
            return base.UpdateState(); 
        }
        public override bool FixedUpdateState()
        {
            return true;
        }
        public override void ExitState() { }
        public override void InitializeSubState() { }
        public override bool CheckSwitchStates()
        {
            if (H_CTX.IsAiming)
                return SwitchState(H_Factory.Aiming(CurrentSuperState));
            else if (!Ctx.IsMoving)
                return SwitchState(Factory.Idle(CurrentSuperState));
            else if (!Ctx.IsRunning)
                return SwitchState(Factory.Walk(CurrentSuperState)); 
            return false;
        }

        protected override void HandleMovement()
        {
            H_CTX.HandleRunMovement();
        }
    }
}
