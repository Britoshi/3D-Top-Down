
namespace Game.StateMachine
{
    public class HumanoidGroundedState : RootState, IHasAnimation
    {
        public virtual string GetAnimationName() => "Default";

        public HumanoidStateMachine H_CTX { set; get; }
        public HumanoidStateFactory H_Factory { set; get; }
        public HumanoidGroundedState(HumanoidStateMachine currentContext, HumanoidStateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {
            H_CTX = currentContext;
            H_Factory = entityStateFactory;
        }

        public override void EnterState()
        {
            base.EnterState();
            InitializeSubState();
            //ChangeAnimation(GetAnimationName());
        }

        public override bool UpdateState()
        {
            if (CheckSwitchStates()) return false;
            base.UpdateState();
            //Ctx.ResetJumpGracePeriod();
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
            /*
            
            if (Ctx.IsCrouching)
            {
                if (Ctx.IsMoving)
                    SetSubState(Factory.Crawl(this));
                else
                    SetSubState(Factory.Crouch(this));
            }
            else*/

            if (H_CTX.IsAiming) 
                SetSubState(H_Factory.Aiming(this));
            else if (!Ctx.IsMoving && !Ctx.IsRunning)
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
            /*
            if (Ctx.JumpTrigger)
                return SwitchState(Factory.Jump());
            else if (Ctx.IsWalkingIntoWall)
                return SwitchState(Factory.Wall());
            */
            return false;
        }

    }
}
