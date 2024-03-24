
using System;
using UnityEngine;

namespace Game.StateMachine
{
    public abstract class EntityGroundedSubStateBase : BaseState
    {
        //If this is a purformace issue, remake it.
        //protected float CurrentSpeed => Ctx.CurrentSpeed;
        //protected virtual float MaxSpeed => Ctx.MaximumMovementSpeed;

        protected EntityGroundedSubStateBase(StateMachine currentContext, StateFactory entityStateFactory, 
            BaseState superState) : base(currentContext, entityStateFactory, superState)
        {

        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false; 
            HandleMovement();
            return true;
        }
         

        protected virtual void HandleMovement()
        {

        }
    }
}
