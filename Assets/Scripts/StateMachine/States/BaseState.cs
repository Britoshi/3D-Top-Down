namespace Game.StateMachine
{
    public enum AnimationParameter
    {
        NONE, IDLE, MOVEMENT,
        WALK = 10,
        RUN = 11,
    }

    public abstract class BaseState : BritoObject
    {
        private bool _isRootState = false;
        private StateMachine _ctx;
        private StateFactory _factory;
        private BaseState _currentSuperState;
        private BaseState _currentSubState;

        public virtual AnimationParameter GetAnimationParameter() => AnimationParameter.NONE;

        protected bool IsRootState { set { _isRootState = value; } get => _isRootState; }
        protected StateMachine Ctx { get { return _ctx; } }
        protected StateFactory Factory { get { return _factory; } }
        protected BaseState CurrentSuperState => _currentSuperState;
        protected BaseState CurrentSubState => _currentSubState;

        protected bool _isRootMotion;
        public bool GetIsRootMotion() => _isRootMotion;

        public bool LockJump;
        public bool LockMovement;

        public BaseState GetRootState()
        {
            int i = 0;
            var curr = this;
           // print("I am potato.", this.GetType());
            while (curr != null)
            {
                //print("I am potato.", this.GetType(), " ", i);
                if (i > 100) throw new System.Exception("This ran 100 times, this shouldn't be a thing.");
                if(curr.IsRootState)
                {
                    return curr;
                }
                if (curr.CurrentSuperState == null) 
                    throw new System.Exception("How are you not root and dont have a super?");

                curr = curr.CurrentSuperState;
                i++;
            }
            
            throw new System.Exception("There was no rootstate???");
        }

        public BaseState(StateMachine currentContext, StateFactory entityStateFactory, BaseState superState = null)
        {
            _ctx = currentContext;
            _factory = entityStateFactory;

            _isRootMotion = false;
            _currentSubState = null;
            _currentSuperState = superState;
            LockMovement = false;
            LockJump = false;  
                
        }

        public virtual void EnterState()
        {
            if (this is IHasAnimation) ChangeAnimation((this as IHasAnimation).GetAnimationName());
        }
        public abstract bool FixedUpdateState();
        /// <summary>
        /// Updates on every frame
        /// </summary>
        /// <returns>true if successful, false if a swap occured.</returns>
        public abstract bool UpdateState();
        public virtual void ExitState()
        {
            if (CurrentSubState != null) CurrentSubState.ExitState();
        }
        public abstract bool CheckSwitchStates();
        public abstract void InitializeSubState();

        /// <summary>
        /// This should be only used outside state.
        /// </summary>
        /// <param name="newState"></param>
        public void TriggerState(BaseState newState)
        {
            SwitchState(newState);
        }

        protected bool SwitchState(BaseState newState)
        {

            ExitState();

            if (_isRootState)
            {
                //if(Ctx.CurrentState._currentSubState != null) 
                //    newState.SetSuperState()
                _ctx.currentState = newState;
            }
            else
            {
                //Debug.Log("Switching Sub State to " + newState);
                _currentSuperState.SetSubState(newState, false);
                //Debug.Log("Switched Sub State to " + _currentSuperState._currentSubState);
            }

            newState.EnterState();
            return true;
        }

        internal void SetSuperState(BaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }
        internal void SetSubState(BaseState newSubState, bool enterState = true)
        {
            _currentSubState = newSubState;
            if (enterState) newSubState.EnterState();

            _currentSubState.SetSuperState(this);
        }

        public void UpdateStates()
        {
            var switchedState = !UpdateState();

            if (switchedState) return;

            if (_currentSubState != null)
                _currentSubState.UpdateStates();
        }

        public void FixedUpdateStates()
        {
            var switchedState = !UpdateState();
            if (switchedState) return;
            if (_currentSubState != null)
                _currentSubState.FixedUpdateStates();
        }

        public override string ToString()
        {
            var output = this.GetType().ToString();
            var substate = _currentSubState;
            if (substate != null)
            {
                output += "\n";

                output += substate.CurrentSuperState.GetType() + " -> ";
                output += substate.ToString();
            }
            return output;
        }


        protected virtual void ChangeAnimation(string name, bool instant = false, int layer = 0)
        {
            var anim = Ctx.GetAnimator();
            if (anim.HasState(Ctx.GetBasicAnimationNamePrefix() + name))
                name = Ctx.GetBasicAnimationNamePrefix() + name;

            if (Ctx.CurrentAnimation == name) return;
                if (instant) anim.Play(name , layer);
            else Ctx.GetAnimator().CrossFadeInFixedTime(name, 0.2f, layer);
            Ctx.CurrentAnimation = name;
        }

    }
}
