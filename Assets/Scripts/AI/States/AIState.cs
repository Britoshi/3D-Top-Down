using UnityEngine;

namespace Game.AI
{
    [System.Serializable]
    public abstract class AIState : BritoObject
    {
        protected AIStateMachine CTX { private set; get; }
        protected AIStateFactory Factory { private set; get; }
        protected AIState SuperState { private set; get; }
        protected AIState SubState {  private set; get; }
        public bool IsRootState => CTX.CurrentState == this;


        public AIState(AIStateMachine currentContext, AIStateFactory aiFactory, AIState superState = null)
        {
            CTX = currentContext;
            Factory = aiFactory;
              
            SubState = null;
            SuperState = superState;
        }
        internal void Enter()
        {
            EnterState();
            InitializeSubState();
        }
        public void Update()
        {
            var switchedState = !UpdateState();

            if (switchedState) return;
            SubState?.Update();
        }
        private void Exit()
        {
            SubState?.ExitState();
            ExitState();
        }
        protected abstract void EnterState();
        protected abstract bool UpdateState();
        protected abstract void ExitState();
        protected abstract void InitializeSubState();
        protected abstract bool CheckSwitchStates();

        protected bool SwitchState(AIState newState)
        { 
            Exit();

            if (IsRootState) CTX.CurrentState = newState;  
            else SuperState.SetSubState(newState, false); 

            newState.Enter();
            return true;
        }

        internal void SetSuperState(AIState newSuperState)
        {
            SuperState = newSuperState;
        }
        internal void SetSubState(AIState newSubState, bool enterState = true)
        {
            SubState = newSubState;
            if (enterState) newSubState.Enter();

            SubState.SetSuperState(this);
        }
    }
}