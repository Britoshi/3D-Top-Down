
namespace Game.StateMachine
{
    public class DeadState : RootState, IHasAnimation
    {
        public DeadState(StateMachine currentContext, StateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {
            _isRootMotion = true;
            LockMovement = true;
            LockJump = true;
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


        }
        public override bool CheckSwitchStates()
        {

            return false;
        }

        public string GetAnimationName()
        {
            return "Dead";
        }
    }
}
