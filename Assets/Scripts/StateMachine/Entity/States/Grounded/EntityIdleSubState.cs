namespace Game.StateMachine
{
    public class EntityIdleSubState : EntityGroundedSubStateBase, IHasAnimation
    {
        public virtual string GetAnimationName() => standAlone ? null : "Idle"; 
        public override AnimationParameter GetAnimationParameter() => AnimationParameter.IDLE;
        /// <summary>
        /// This represents a toggle of whether the idle state will only inherit the idle functionalities.
        /// It seems useless right now.
        /// </summary>
        readonly bool standAlone;
        public EntityIdleSubState(StateMachine currentContext, StateFactory entityStateFactory, BaseState superState, bool standAlone = false) :
            base(currentContext, entityStateFactory, superState)
        {
            this.standAlone = standAlone;
        }
        public override void EnterState()
        { 
            base.EnterState();
        }

        public override bool UpdateState()
        {
            if (!standAlone)
            {
                if (CheckSwitchStates()) return false;
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
            /*
            if (Ctx.IsCrouching)
            {
                if (Ctx.IsMoving)
                    return SwitchState(Factory.Crawl(CurrentSuperState));
                return SwitchState(Factory.Crouch(CurrentSuperState));
            }*/
            if (Ctx.IsMoving && !Ctx.IsRunning)
                return SwitchState(Factory.Walk(CurrentSuperState));
            else if (Ctx.IsMoving && Ctx.IsRunning)
                return SwitchState(Factory.Run(CurrentSuperState));
            return false;
        }

        protected override void HandleMovement()
        {
            //throw new System.Exception();
            print("handle movement fix");
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