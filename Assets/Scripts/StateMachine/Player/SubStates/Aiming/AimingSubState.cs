
using System;
using UnityEngine;

namespace Game.StateMachine.Player
{
    public abstract class AimingSubState : BaseState, IHasAnimation
    {
        public virtual string GetAnimationName() => "Aim";
        //If this is a purformace issue, remake it.
        //protected float CurrentSpeed => Ctx.CurrentSpeed;
        //protected virtual float MaxSpeed => Ctx.MaximumMovementSpeed;

        public HumanoidStateMachine CTX { set; get; }
        public new HumanoidStateFactory Factory { set; get; }

        protected AimingSubState(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory,
            BaseState superState) : base(currentContext, entityStateFactory, superState)
        { 
            CTX = currentContext;
            Factory = entityStateFactory;
        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            HandleMovement();
            return true;
        }
        protected abstract void HandleMovement();
    }
}
