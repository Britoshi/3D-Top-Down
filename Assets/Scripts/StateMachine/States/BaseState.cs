using UnityEngine;

namespace Game
{
    public abstract class BaseState
    {
        private bool _isRootState = false;
        private StateMachine _ctx;
        private StateFactory _factory;
        private BaseState _currentSuperState;
        private BaseState _currentSubState;

        protected bool IsRootState { set { _isRootState = value; } get => _isRootState; }
        protected StateMachine Ctx { get { return _ctx; } }
        protected StateFactory Factory { get { return _factory; } }
        protected BaseState CurrentSuperState => _currentSuperState;
        protected BaseState CurrentSubState => _currentSubState;

        public BaseState(StateMachine currentContext, StateFactory entityStateFactory, BaseState superState = null)
        {
            _ctx = currentContext;
            _factory = entityStateFactory;

            _currentSubState = null;
            _currentSuperState = superState;
        }

        public abstract void EnterState();
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


        protected virtual void ChangeAnimation(string name)
        {
            Ctx.animator.Play(name);
            Ctx.CurrentAnimation = name;
        }

        protected void ConfirmAnimationChange(string name)
        {
            if (Ctx.CurrentAnimation != name)
            {
                Debug.Log("The animation did not match what it is suppsoed to! Switching from: \n" + Ctx.CurrentAnimation + " to " + name);

                Ctx.CurrentAnimation = name;
                ChangeAnimation(name);
            }
        }
    }
}
