
namespace Game.StateMachine
{
    public class EntityGroundedState : RootState
    {
        public EntityGroundedState(StateMachine currentContext, StateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {
        }

        public override void EnterState()
        {
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
            if (!Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Idle(this));
            else if (Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Walk(this));
            else
                SetSubState(Factory.Run(this));
           
        }
        public override bool CheckSwitchStates()
        {
            if (!Ctx.IsGrounded)
                return SwitchState(Factory.Airborne());
            return false;
        }
    }
}
