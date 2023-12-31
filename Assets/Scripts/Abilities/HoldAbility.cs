/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Abilities
{
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }
    public abstract class HoldAbility : BaseAbility
    { 
        private float _castingTimer;
        private float _castingDuration;

        public bool PreventAnimationDelegate;

        //public AbilityDrainCost DrainCost { set; get; }

        public HoldAbility(Entity owner, string name, float castDuration, CooldownOn cooldownType, AbilityDrainCost cost, Cooldown cooldown = null) :
            base(owner, name, cdOn: cooldownType, cooldown: cooldown, cost: cost)
        {
            //DrainCost = cost;
            _castingDuration = castDuration;
            _castingTimer = 0f;
            PreventAnimationDelegate = false;
        }

        public override float GetProgress()
        {
            return base.GetProgress();
        }

        public override float GetProgress2()
        {
            if (!busy) return 0f;
            
            if (_castingDuration == 0f) return 1f;

            return _castingTimer / _castingDuration;
        }


        public virtual bool CanHold()
        {
            //if(!DrainCost.CanHold()) return false;
            throw new NotImplementedException();
            if(_castingDuration != 0f)
                if (_castingTimer >= _castingDuration) return false;

            return true;
        }

        public override bool CanCast()
        {

            if (busy) return false;
            if (!DrainCost.CanCast()) return false;
            if (!cooldown.CanCast()) return false;

            return true;
        }

        //AbilityEventController EventController;

        /// <summary>
        /// This is the core mechanic. This needs to be set from the input manager.
        /// </summary>
        protected bool buttonReader;

        public void SetButtonReading(bool state)
        { 
            buttonReader = state;
        }
        void OnTickChecker(object sender, OnTickEventArgs e)
        {
            if(buttonReader && CanHold())
            {
                OnHold(); 
                if (!CanHold()) StopHold();
            }
            else
            {
                StopHold();
            }
        }


        public override void OnCast()
        {
            busy = true; 
            DrainCost.Deduct();
            _castingTimer = 0;
            //EventController = Tick.AddCustomTick(OnTickChecker);
            CustomApplyCoolDown();
            buttonReader = true; 
            PreventAnimationDelegate = false;
            throw new NotImplementedException();
        }

        public virtual void OnHold()
        {
            //_castingTimer += EventController.CustomEvent.deltaTime;
            DrainCost.Drain();
            throw new NotImplementedException();
        }

        public virtual void StopHold()
        {
            //EventController.RemoveEvent();
            //EventController = null;
            CustomApplyCoolDown();
            PreventAnimationDelegate = false;
            throw new NotImplementedException();
        }


        public override void ApplyCoolDown()
        {

        }

        public void CustomApplyCoolDown()
        { 
            if (ApplyCooldownOn == CooldownOn.END)
                cooldown.ApplyCooldown();
            else if (ApplyCooldownOn == CooldownOn.START)
                cooldown.ApplyCooldown();
        }


        public override void GlobalUpdate()
        {
            cooldown.GlobalUpdate();
        }
        public override void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (PreventAnimationDelegate) return;
            PreventAnimationDelegate = true;
            base.OnAnimationStart(animator, stateInfo, layerIndex);

        }

        public override void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (PreventAnimationDelegate) return;
            base.OnAnimationEnd(animator, stateInfo, layerIndex);
        }
    }
}
*/