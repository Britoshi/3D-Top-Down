using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Game.Abilities.AbilityEvent;

namespace Game.Abilities
{
    public enum CooldownOn
    {
        START, END, CUSTOM
    }

    public delegate void ApplyTargetEntity(Entity target);
    public delegate void VoidFunction();
    public struct AbilityEvent : IComparable
    {
        public delegate void AbilityFunction();
        public float time;
        public AbilityFunction abilityEvent;

        public int CompareTo(object obj) => time.CompareTo(obj); 
    }

    [Serializable]
    public abstract class BaseAbility
    {


        public Entity Owner { private set; get; }
        public Cooldown cooldown { private set; get; }

        [SerializeField] public string Name { private set; get; }

        public virtual AbilityCost Cost { set; get; }

        protected CooldownOn ApplyCooldownOn { private set; get; }

        private List<AbilityEvent> _abilityEvents;
        private List<AbilityEvent> _liveEvents;

        protected bool MovementOverride; 

        protected bool busy;
        protected float animationProgress;

        public BaseAbility(Entity owner, string name,
            CooldownOn cdOn = CooldownOn.START, Cooldown cooldown = null, AbilityCost cost = null)
        {
            Owner = owner;
            Name = name;
            ApplyCooldownOn = cdOn;

            Cost = cost != null ? cost : AbilityCost.None;
            this.cooldown = cooldown != null ? cooldown : Cooldown.Create(0);

            MovementOverride = false;
            animationProgress = 0f;

            _abilityEvents = new List<AbilityEvent>();

            InvokeOnStart = new VoidFunction[0];
            InvokeOnUpdate = new VoidFunction[0];
            InvokeOnEnd = new VoidFunction[0];
        }

        public void AddEvent(float time, AbilityFunction newEvent)
        {
            var e = new AbilityEvent
            { 
                time = time,
                abilityEvent = newEvent
            };
            _abilityEvents.Add(e);
            _abilityEvents.Sort();
        }

        public virtual bool CanCast()
        {
            if (!Cost.CanCast()) return false;
            if (!cooldown.CanCast()) return false;
            return true;
        }

        public virtual BaseAbility GetAbility() => this;

        public virtual void OnCast()
        {
            Cost.Deduct();   
            busy = true;
        }

        public virtual string GetAnimation() => Name;
        public virtual string GetIcon() => Name;
        public virtual float GetProgress()
        {
            if (busy) return animationProgress;
            return cooldown.GetProgress(); 
        }

        public virtual float GetProgress2() => 0f;


        public float GetAnimationProgress()
        {
            return animationProgress;
        }

        public VoidFunction[] InvokeOnStart, InvokeOnUpdate, InvokeOnEnd;

        public virtual void OnStart()
        {
            _liveEvents = new List<AbilityEvent>(_abilityEvents);

            foreach (var function in InvokeOnStart)
                function();

            ApplyCoolDown();

            if (!MovementOverride && Owner is PlayerEntity)
            {
                var stateMachine = (Owner as PlayerEntity).stateMachine;
                stateMachine.CurrentState.SetSubState(stateMachine.Factory.Idle(stateMachine.CurrentState, true));
            }

            busy = true;
            animationProgress = 0;
        }

        public virtual void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        { 
            busy = true;
            animationProgress = 0;
        }

        public virtual void GlobalUpdate()
        {
            cooldown.GlobalUpdate();
        }

        public virtual void OnUpdate()
        {
            if (_liveEvents == null)
                _liveEvents = new List<AbilityEvent>(_abilityEvents); 

            foreach (var function in InvokeOnUpdate)
                function(); 
        }

        public virtual void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            //Memory Intensive, but whatever.
            List<AbilityEvent> removeList = new List<AbilityEvent>();

            foreach (var e in _liveEvents)
            {
                if (e.time <= stateInfo.normalizedTime)
                { 
                    e.abilityEvent();
                    removeList.Add(e);
                }
                else break;
            }

            foreach (var item in removeList)
                _liveEvents.Remove(item);

            animationProgress = stateInfo.normalizedTime;
        }

        public virtual void OnEnd()
        {
            //Fail safe just incase?
            if (_liveEvents == null)
                _liveEvents = new List<AbilityEvent>(_abilityEvents);

            foreach (var function in InvokeOnEnd)
                function();

            ApplyCoolDown();

            foreach (var e in _liveEvents)
                e.abilityEvent();
            _liveEvents = null;

            busy = false;
            animationProgress = 0;
        }
        public virtual void ApplyCoolDown()
        {

            if (ApplyCooldownOn == CooldownOn.END)
                cooldown.ApplyCooldown();
            else if (ApplyCooldownOn == CooldownOn.START)
                cooldown.ApplyCooldown();
        }

        public virtual void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            busy = false;
            animationProgress = 0;
        }


        public bool IsHoldAbility() => throw new NotImplementedException();
           // GetType().IsSubclassOf(typeof(HoldAbility));
    }
}
