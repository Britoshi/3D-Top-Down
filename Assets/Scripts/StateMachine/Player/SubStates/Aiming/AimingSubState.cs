
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

        public PStateMachine P_CTX { set; get; }
        public PStateFactory P_Factory { set; get; }

        protected AimingSubState(PStateMachine currentContext, PStateFactory entityStateFactory,
            BaseState superState) : base(currentContext, entityStateFactory, superState)
        { 
            P_CTX = currentContext;
            P_Factory = entityStateFactory;
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
