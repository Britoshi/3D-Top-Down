using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    internal class PlayerCrouchState : PlayerMovementBaseSubState
    {
        bool standAlone;
        public PlayerCrouchState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory, PlayerBaseState superState) :
            base(currentContext, entityStateFactory, superState)
        {

        }

        void OnCrouch()
        {

            throw new NotImplementedException();
            //Ctx.Player.EntityCollider.Crouch(); 
        }

        void OnStandUp()
        {

            throw new NotImplementedException();
            //Ctx.Player.EntityCollider.Uncrouch(); 
        }

        public override void EnterState()
        {
            OnCrouch();
            ChangeAnimation("Crouch");
        }

        public override bool UpdateState()
        { 
            if (CheckSwitchStates()) return false;

            UpdateFacingDirection(); 
            HandleMovement();
            
            return true;
        }
        public override bool FixedUpdateState()
        {
            return true;
        }
        public override void ExitState() { }
        public override void InitializeSubState() { }
        public override bool CheckSwitchStates()
        {
            if (Ctx.IsHoldingDown)
            {
                if (Ctx.IsMoving)
                    return SwitchState(Factory.Crawl(CurrentSuperState));
                return false;
            }

            OnStandUp();

            if (Ctx.IsMoving && !Ctx.IsRunning)
                return SwitchState(Factory.Walk(CurrentSuperState));
            else if (Ctx.IsMoving && Ctx.IsRunning)
                return SwitchState(Factory.Run(CurrentSuperState));
            return SwitchState(Factory.Idle(CurrentSuperState)); 
        }

        protected override void HandleMovement()
        {
            /*
            if (CurrentSpeed >= MaxSpeed * 1.5f)
                Ctx.HorizontalVelocity += Time.deltaTime * MaxSpeed * Ctx.MovementForceGravity * -Ctx.VelocityDir;
            else
                Ctx.HorizontalVelocity += Time.deltaTime * Ctx.MovementForceGravity * -Ctx.VelocityDir;

            float stopThreshHold = .1f;
            if (CurrentSpeed <= stopThreshHold)
                Ctx.HorizontalVelocity = 0f;*/

            throw new NotImplementedException();
        }
    }
}