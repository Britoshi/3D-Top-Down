using UnityEngine; 
using UnityEditor.Playables;
using Game.Abilities;

namespace Game
{
    public class PlayerAbilityState : PlayerRootState
    {
        private BaseAbility _currentAbility;

        public PlayerAbilityState(PlayerStateMachine currentContext, PlayerStateFactory entityStateFactory, BaseAbility ability) : base(currentContext, entityStateFactory)
        {
            _currentAbility = ability;
        }

        public override bool CheckSwitchStates()
        {
            if (_currentAbility.IsHoldAbility())
            {
                throw new System.NotImplementedException();
                //if (!(_currentAbility as HoldAbility).PreventAnimationDelegate && _currentAbility.GetAnimationProgress() > .5f)
                { 
                    //Ctx.ResetAnimation();
                    //return true; 
                } 
                //return false; 
            }

            if (_currentAbility.GetAnimationProgress() > 0.988f)
            { 
                Ctx.ResetAnimation();
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            Ctx.IsBusy = true;
            ChangeAnimation(_currentAbility.GetAnimation());
            _currentAbility.OnStart();
        }

        public override void ExitState()
        {
            base.ExitState(); 
            Ctx.IsBusy = false;
            _currentAbility.OnEnd();
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
            if (CheckSwitchStates()) return false;
            base.UpdateState();
            CheckEnd();
            _currentAbility.OnUpdate();
            return true;
        }

        private void CheckEnd()
        { 

        }
    }
}
