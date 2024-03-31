
namespace Game.StateMachine
{
    /// <summary>
    /// Filler animations MUST HAVE FillerAnimationNode Attached!
    /// </summary>
    public class FillerState : RootState, IHasAnimation
    {
        public string animationName;
        public bool inturruptable;
        public FillerState(StateMachine currentContext, StateFactory playerStateFactory, string animationName, bool inturruptable,  bool rootMotion) :
            base(currentContext, playerStateFactory)
        {
            this.animationName = animationName;
            this.inturruptable = inturruptable;
            _isRootMotion = rootMotion; 
            if(_isRootMotion)
            {
                if (!inturruptable)
                {
                    LockMovement = true;
                    LockJump = true;
                }
            }
        }

        public override bool CheckSwitchStates()
        {
            if(inturruptable && Ctx.IsMoving) return SwitchState(Factory.Default());
            return false;
        }

        public override bool FixedUpdateState()
        {
            return true;
        }

        public string GetAnimationName()
        {

            return animationName;
        }

        public override void InitializeSubState()
        {

        }
        public override void ExitState()
        {
            base.ExitState();
        }
    }
}