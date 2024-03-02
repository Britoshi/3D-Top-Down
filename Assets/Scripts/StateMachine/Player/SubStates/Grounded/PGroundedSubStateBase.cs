
using System;
using UnityEngine;

namespace Game.StateMachine.Player
{
    public abstract class PGroundedSubStateBase : EntityGroundedSubStateBase, IPlayerState
    {
        
        public PStateMachine P_CTX { set; get; }
        public PStateFactory P_Factory { set; get; } 

        //If this is a purformace issue, remake it.
        //protected float CurrentSpeed => Ctx.CurrentSpeed;
        //protected virtual float MaxSpeed => Ctx.MaximumMovementSpeed;

        protected PGroundedSubStateBase(PStateMachine currentContext, PStateFactory entityStateFactory, 
            BaseState superState) : base(currentContext, entityStateFactory, superState)
        {
            P_CTX = currentContext;
            P_Factory = entityStateFactory;
        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            UpdateFacingDirection();
            HandleMovement();
            return true;
        }

        protected override void ChangeAnimation(string name)
        {
            Ctx.LastAnimationChangeAttempt = name + " | Super: " + CurrentSuperState.GetType();
            if (CurrentSuperState == null) return;

            //Debug.Log("This is ran from " + GetType());
            //HMMMMM
            if (!CurrentSuperState.GetType().IsSubclassOf(typeof(EntityAirborneSubStateBase)))
            {
                //print("this is interesting.");
                //Debug.Log("Supposedly this ran" + CurrentSuperState.GetType());
                base.ChangeAnimation(name);
                return;
            }
            
        } 

        protected override void HandleMovement()
        {

        }
    }
}
