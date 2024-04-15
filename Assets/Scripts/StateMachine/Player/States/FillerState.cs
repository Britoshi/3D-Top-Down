
namespace Game.StateMachine
{
    /// <summary>
    /// Filler animations MUST HAVE FillerAnimationNode Attached!
    /// </summary>
    public class FillerState : RootState, IAnimationLayerOverride
    { 
        public int animationLayer;
        public bool inturruptable;
        public bool interrupted;
        public FillerState(StateMachine currentContext, StateFactory playerStateFactory, bool inturruptable,  bool rootMotion, int animationLayer) :
            base(currentContext, playerStateFactory)
        { 
            this.inturruptable = inturruptable;
            this.animationLayer = animationLayer;

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

        public override void InitializeSubState()
        {

        }
        public override void ExitState()
        {
            base.ExitState();
        }

        public int GetLayerIndex()
        {
            return animationLayer;
        }
    }
    public class AnimState : RootState, IHasAnimation, IAnimationLayerOverride
    {
        public string animName;
        public int animationLayer;
        public bool inturruptable;
        public AnimState(StateMachine currentContext, StateFactory playerStateFactory, string animName, bool inturruptable, bool rootMotion, int animationLayer) :
            base(currentContext, playerStateFactory)
        {
            this.animName = animName;
            this.inturruptable = inturruptable;
            this.animationLayer = animationLayer;

            _isRootMotion = rootMotion;
            if (_isRootMotion)
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
            if (inturruptable && Ctx.IsMoving) return SwitchState(Factory.Default());
            return false;
        }

        public override bool FixedUpdateState()
        {
            return true;
        }

        public override void InitializeSubState()
        {

        }
        public override void ExitState()
        {
            base.ExitState();
        }

        public int GetLayerIndex()
        {
            return animationLayer;
        }

        public string GetAnimationName()
        {
            return animName;
        }
    }
}