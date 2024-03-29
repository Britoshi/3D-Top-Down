using UnityEngine;

namespace Game.StateMachine
{

    public class Stagger : RootState, IHasAnimation
    {
        public string GetAnimationName() => "Stagger"; 
        
        public float countdown;
        public const float DURATION = 1.4f;
        public Stagger(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
        {
            _isRootMotion = true;
        }
        public override void EnterState()
        {
            base.EnterState();
            countdown = 0;
        }
        public override bool CheckSwitchStates()
        {
            countdown += Time.deltaTime;
            if(countdown > DURATION)  
                return SwitchState(Factory.Default()); 
            return false;
        }

        public override bool FixedUpdateState()
        {
            return true;
        }


        public override void InitializeSubState()
        {

        }

        public override bool UpdateState()
        {
            base.UpdateState();
            if (CheckSwitchStates()) return false; 
            return true;
        }
    }
}