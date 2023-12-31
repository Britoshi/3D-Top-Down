using Game;
using UnityEngine;

namespace Game
{
    public class PlayerIdleState : PlayerMovementBaseSubState
    {
        bool standAlone;
        public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory, PlayerBaseState superState, bool standAlone) :
            base(currentContext, entityStateFactory, superState)
        {
            this.standAlone = standAlone;
        }
        public override void EnterState()
        {
            if(!standAlone)
                ChangeAnimation("Idle");
        }

        public override bool UpdateState()
        {
            if (!standAlone)
            {
                if (CheckSwitchStates()) return false;
                UpdateFacingDirection();
            }
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
                return SwitchState(Factory.Crouch(CurrentSuperState));
            } 
            if (Ctx.IsMoving && !Ctx.IsRunning)
                return SwitchState(Factory.Walk(CurrentSuperState));
            else if (Ctx.IsMoving && Ctx.IsRunning) 
                return SwitchState(Factory.Run(CurrentSuperState));
            return false;
        }

        protected override void HandleMovement()
        {
            throw new System.Exception();
            /*
            if (CurrentSpeed >= MaxSpeed * 1.5f)
                Ctx.HorizontalVelocity += Time.deltaTime * MaxSpeed * Ctx.MovementForceGravity * -Ctx.VelocityDir;
            else
                Ctx.HorizontalVelocity += Time.deltaTime * Ctx.MovementForceGravity * -Ctx.VelocityDir;

            float stopThreshHold = .1f;
            if (CurrentSpeed <= stopThreshHold)
                Ctx.HorizontalVelocity = 0f;*/
        }
    }
}
