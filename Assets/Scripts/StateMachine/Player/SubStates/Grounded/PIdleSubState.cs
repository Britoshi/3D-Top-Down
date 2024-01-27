using Game.StateMachine.Player;

namespace Game.StateMachine.Player
{
    public class PIdleSubState : PGroundedSubStateBase, IHasAnimation
    {
        public virtual string GetAnimationName() => standAlone ? null : "Idle";

        /// <summary>
        /// This represents a toggle of whether the idle state will only inherit the idle functionalities.
        /// It seems useless right now.
        /// </summary>
        readonly bool standAlone;
        public PIdleSubState(PStateMachine currentContext, PStateFactory entityStateFactory, BaseState superState, bool standAlone = false) :
            base(currentContext, entityStateFactory, superState)
        {
            this.standAlone = standAlone;
        }
        public override void EnterState()
        {

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
            P_CTX.HandleIdleMovement();
        }
    }
}