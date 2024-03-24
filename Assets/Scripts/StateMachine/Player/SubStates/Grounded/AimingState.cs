
using Game.StateMachine.Player;

namespace Game.StateMachine
{
    public class AimingState : RootState
    {
        protected PStateMachine pStateMachine;
        protected PStateFactory pFactory;
        public AimingState(StateMachine currentContext, StateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {
            pStateMachine = currentContext as PStateMachine;
            pFactory = Factory as PStateFactory;
        }

        public override void EnterState()
        {
            base.EnterState();
            InitializeSubState();
        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            base.UpdateState();
            pStateMachine.HandleAimingBehavior();
            return true;
        }
        public override bool FixedUpdateState()
        {
            return true;
        }

        public override void ExitState()
        {

        }
         
        public override void InitializeSubState()
        {
            if (!Ctx.IsMoving)
                SetSubState(pFactory.AimIdle(this)); 
            else
                SetSubState(pFactory.AimMove(this));  
        }
        public override bool CheckSwitchStates()
        {
            if (!Ctx.IsGrounded)
                return SwitchState(Factory.Airborne());
            else if (!pStateMachine.IsAiming)
                return SwitchState(Factory.Grounded());
            return false;
        }
    }
}
