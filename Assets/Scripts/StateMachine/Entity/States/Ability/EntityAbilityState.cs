using Game.Abilities;

namespace Game.StateMachine
{
    public class EntityAbilityState : RootState
    {
        protected NAbility ability;


        public EntityAbilityState(NAbility ability, StateMachine currentContext, StateFactory entityStateFactory) :
            base(currentContext, entityStateFactory)
        {
            this.ability = ability;
            _isRootMotion = ability.GetAnimationRootMotion();

            LockMovement = ability.LockMovement;
            LockJump = ability.LockJump;
        }

        public override void EnterState()
        {
            ability.OnAbilityCast();
            InitializeSubState();
            int layer = LockMovement ? 2 : 3;
            ChangeAnimation(ability.GetAnimationNodeName(), true, layer);
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
            ability.OnAbilityEnd();
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
            
            if (LockMovement) return;

            if (Ctx.GetType().IsSubclassOf(typeof(HumanoidStateMachine)))
            {
                var hCtx = Ctx as HumanoidStateMachine;
                if (hCtx.IsAiming)
                {
                    SetSubState((Factory as HumanoidStateFactory).Aiming(this));
                    return;
                }
            } 
            if (!Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Idle(this));
            else if (Ctx.IsMoving && !Ctx.IsRunning)
                SetSubState(Factory.Walk(this));
            else
                SetSubState(Factory.Run(this));

        }
        public override bool CheckSwitchStates()
        {
            //if (!Ctx.IsGrounded)
           //     return SwitchState(Factory.Airborne());
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
