using UnityEngine;

namespace Game.AI
{
    [System.Serializable]
    public abstract class AIState : BritoObject
    {
        public Entity entity;
        protected AIStateMachine CTX { private set; get; }
        protected AIStateFactory Factory { private set; get; }
        protected AIState SuperState { private set; get; }
        protected AIState SubState {  private set; get; }
        public bool IsRootState => CTX.CurrentState == this;


        public AIState(AIStateMachine currentContext, AIStateFactory aiFactory, AIState superState = null)
        {
            CTX = currentContext;
            Factory = aiFactory;
            entity = currentContext.entity;  

            SubState = null;
            SuperState = superState;

            switchTrigger = false;
        }
        internal void Enter()
        {
            OnEnterState();
            InitializeSubState();
        }
        public void Update()
        {
            if (_CheckSwitchStates()) return;
            OnUpdate();
            SubState?.Update();
        }
        public void FixedUpdate()
        {
            if (_CheckSwitchStatesFixed()) return;
            OnFixedUpdate();
            SubState?.FixedUpdate();
        }
        private void Exit()
        {
            SubState?.Exit();
            OnExitState();
        }
        protected abstract void OnEnterState();
        protected abstract void OnUpdate();
        protected virtual void OnFixedUpdate() { }
        protected abstract void OnExitState();
        protected abstract void InitializeSubState();
        protected abstract bool CheckSwitchStates();
        bool switchTrigger;
        bool _CheckSwitchStates()
        {
            if (switchTrigger) return true;
            else if (CheckSwitchStates())
            {
                switchTrigger = true;
                return true;
            }
            return false;
        }
        protected virtual bool CheckSwitchStatesFixed() => false;
        bool _CheckSwitchStatesFixed()
        {
            if (switchTrigger) return true;
            else if (CheckSwitchStatesFixed())
            {
                switchTrigger = true;
                return true;
            }
            return false;
        }

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