
using System;
using UnityEngine;

namespace Game.StateMachine
{
    public abstract class EntityGroundedSubStateBase : BaseState
    {
        //If this is a purformace issue, remake it.
        //protected float CurrentSpeed => Ctx.CurrentSpeed;
        //protected virtual float MaxSpeed => Ctx.MaximumMovementSpeed;

        public override BasicStateIndex GetBasicStateIndex() => BasicStateIndex.MOVEMENT;

        protected EntityGroundedSubStateBase(StateMachine currentContext, StateFactory entityStateFactory, 
            BaseState superState) : base(currentContext, entityStateFactory, superState)
        {

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
            //HMMMMM  this is for airborne?
            if (!CurrentSuperState.GetType().IsSubclassOf(typeof(EntityAirborneSubStateBase)))
            {
                //print("this is interesting.");
                //Debug.Log("Supposedly this ran" + CurrentSuperState.GetType());
                base.ChangeAnimation(name);
                return;
            }
            
        }

        protected void UpdateFacingDirection()
        {
            //if (!Ctx.IsMoving) return;

            //Ctx.Player.FacingDir = Ctx.RawInputDir;
            //Ctx.spriteRenderer.flipX = Ctx.Player.FacingDir < 0;
        }

        protected virtual void HandleMovement()
        {
            throw new NotImplementedException();
            /*
            float sensitivity = Ctx.MovementSpeedGainSensitivty;
            float oppositeGravity = Ctx.MovementOppositeForceGravity;
            int inputDir = Ctx.RawInputDir;

            //Flip X setter later 
            if (CurrentSpeed <= 0.1f)
                Ctx.HorizontalVelocity += Time.deltaTime * MaxSpeed * sensitivity * inputDir;
            else if (Ctx.VelocityDir != inputDir)
            {
                if (Ctx.IsGrounded)
                    Ctx.HorizontalVelocity += Time.deltaTime * Mathf.Sqrt(CurrentSpeed) * oppositeGravity * inputDir;
                else
                    Ctx.HorizontalVelocity += Time.deltaTime * MaxSpeed * oppositeGravity * inputDir;
            }
            else
            {
                if (CurrentSpeed < MaxSpeed)
                    Ctx.HorizontalVelocity += Time.deltaTime * MaxSpeed * sensitivity * inputDir;
                else if (CurrentSpeed > MaxSpeed)
                {
                    //If you're going way faster than you're supposed to.
                    var theoraticalMaximumSpeedMultipliar = 2f;
                    if (Ctx.IsGrounded && CurrentSpeed >= MaxSpeed * theoraticalMaximumSpeedMultipliar)
                        Ctx.HorizontalVelocity += -Time.deltaTime * CurrentSpeed * theoraticalMaximumSpeedMultipliar * inputDir;
                    else
                        Ctx.HorizontalVelocity += -Time.deltaTime * CurrentSpeed * theoraticalMaximumSpeedMultipliar * inputDir;
                }
                else
                    Ctx.HorizontalVelocity += Time.deltaTime * MaxSpeed * sensitivity * inputDir;

                //Just hard set the velocity if it is close enough.
                float threshhold = Ctx.IsRunning ? .5f : .25f;
                if (Mathf.Abs(CurrentSpeed - MaxSpeed) <= threshhold)
                    Ctx.HorizontalVelocity = MaxSpeed * inputDir;
            }*/
        }
    }
}
