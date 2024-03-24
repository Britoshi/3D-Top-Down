
using System;
using UnityEngine;

namespace Game.StateMachine.Player
{
    public abstract class HumanoidGroundedSubStateBase : EntityGroundedSubStateBase, IHumanoidState, IHasAnimation
    { 
        public virtual string GetAnimationName() => "Default";
        public HumanoidStateMachine H_CTX { set; get; }
        public HumanoidStateFactory H_Factory { set; get; } 

        //If this is a purformace issue, remake it.
        //protected float CurrentSpeed => Ctx.CurrentSpeed;
        //protected virtual float MaxSpeed => Ctx.MaximumMovementSpeed;

        protected HumanoidGroundedSubStateBase(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory, 
            BaseState superState) : base(currentContext, entityStateFactory, superState)
        {
            H_CTX = currentContext;
            H_Factory = entityStateFactory;
        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            HandleMovement();
            return true;
        }
         

        protected override void HandleMovement()
        {

        }
    }
}
