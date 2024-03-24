using Game.StateMachine.Player;

namespace Game.StateMachine.Player
{
    public class HumanoidIdleSubState : HumanoidGroundedSubStateBase
    { 
        /// <summary>
        /// This represents a toggle of whether the idle state will only inherit the idle functionalities.
        /// It seems useless right now.
        /// </summary>
        readonly bool standAlone;
        public HumanoidIdleSubState(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory, BaseState superState, bool standAlone = false) :
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
            if (H_CTX.IsAiming)
                return SwitchState(H_Factory.Aiming(CurrentSuperState));
            else if (Ctx.IsMoving && !Ctx.IsRunning)
                return SwitchState(Factory.Walk(CurrentSuperState));
            else if (Ctx.IsMoving && Ctx.IsRunning)
                return SwitchState(Factory.Run(CurrentSuperState));
            return false;
        }

        protected override void HandleMovement()
        {
            H_CTX.HandleIdleMovement();
        }
    }
}