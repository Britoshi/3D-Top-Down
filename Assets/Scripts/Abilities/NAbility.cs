using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine; 

namespace Game.Abilities
{
    public enum CooldownOn
    {
        START, END, CUSTOM
    }
    public struct TimedEvent : IComparable
    {
        public float time;
        public Action abilityEvent;

        public int CompareTo(object obj) => time.CompareTo(obj);
    }
    [Serializable]
    public abstract class NAbility : BritoObject
    {
        public Entity Owner { private set; get; }
        public Cooldown cooldown { private set; get; } 
        public virtual AbilityResourceCost Cost { set; get; } 

        public bool LockJump { protected set; get; }
        public bool LockMovement { protected set; get; } 

        private bool currentlyCasting;
        public bool IsCasting => currentlyCasting;

        protected bool busy;
        protected float animationProgress;

        /// <summary>
        /// The animation will be able to change once it reaches this progression. 
        /// Only if there is a change queue such as moving/casting ability
        /// </summary>
        protected float preemptiveAnimationCancelThreshHold = 1f;

        public bool isCastableAirborne;

        public abstract string GetName();
        public abstract string GetAnimationNodeName();
        public abstract bool GetAnimationRootMotion();
        protected abstract CooldownOn ApplyCooldownOn();

        public Event InvokeOnStart, InvokeOnUpdate, InvokeOnEnd;

        void RegisterSelfToAbilityController()
        {
            Owner.abilityController.RegisterAbility(this);
        }
        public NAbility(Entity owner, bool isCastableAirborne, bool lockMovement, bool lockJump, Cooldown cooldown = null, AbilityResourceCost cost = null)
        {
            Owner = owner; 
            this.isCastableAirborne = isCastableAirborne;

            //Cost = cost != null ? cost : AbilityResourceCost.None;
            Cost = cost ?? new NoResourceCost();
            this.cooldown = cooldown ?? Cooldown.Create(0);

            currentlyCasting = false; 
            animationProgress = 0f;

            //_abilityEvents = new List<TimedEvent>();

            InvokeOnStart = new();
            InvokeOnUpdate = new();
            InvokeOnEnd = new();

            LockJump = lockJump;
            LockMovement = lockMovement;

            RegisterSelfToAbilityController();
        }
        bool endedPreemptively;
        public virtual Result CanCast()
        {
            //perhaps check if the animation is able? 
            if (Owner.abilityController.IsCasting)
            {
                //var currAbility = Owner.abilityController.currentAbility;
                //if(currAbility.animationProgress <= currAbility.preemptiveAnimationCancelThreshHold) 
                    return Result.Fail("Already Casting");
                //print("Trying to cast a queued ability");
            }
            if (!cooldown.CanCast()) return Result.Fail("Ability On Cooldown.");
            else if (!Cost.CanDeduct()) return Result.Fail("Not Enough Resources.");
            return Result.Success();
        }

        public virtual Result TryCast()
        {
            var check = CanCast();
            if (check.result == ResultType.SUCCESS)
            {
                SetStateMachine();
                Cost?.Deduct();
            }
            return check;
        } 

        void SetStateMachine() =>
            Owner.stateMachine.currentState.TriggerState(Owner.stateMachine.Factory.Ability(this));

        protected virtual void HandlePreemptiveCancelling()
        {   void Trigger()
            {
                endedPreemptively = true;

            }
            if (Owner.abilityController.TryCastAbilityQueue())
            {
                Trigger();
                
                return;
            }
            else if (Owner.stateMachine.TryEnterNoneIdleState())
            {
                Trigger();
                return;
            }
        }

        #region Animation Functions #Only put animation related function.
        public virtual void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            print("Anim Start", GetName());
            animationProgress = 0;
            if (ApplyCooldownOn() == CooldownOn.START) cooldown.ApplyCooldown();
            endedPreemptively = false;
        }
        public  virtual void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (endedPreemptively) return;

            animationProgress = stateInfo.normalizedTime % 1; 
            //Check for preemptive ending
            if(preemptiveAnimationCancelThreshHold < 1f)
            {
               // if (animationProgress > preemptiveAnimationCancelThreshHold)
                    //HandlePreemptiveCancelling();
            }
        }
        public virtual void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            print("Ability Animation Ended", GetName());
            if (ApplyCooldownOn() == CooldownOn.END) cooldown.ApplyCooldown(); 

            if (!endedPreemptively)
            {
                print("Ability Animation ended normally", GetName());
                Owner.stateMachine.ResetState();
                Owner.abilityController.currentAbility = null;
            }
            animationProgress = -1;
            endedPreemptively = false;
        }
        #endregion
        /// <summary>
        /// Update regardless of any state.
        /// </summary>
        public virtual  void PassiveUpdate()
        { 
            cooldown.GlobalUpdate();
        }

        /// <summary>
        /// Called before the animation starts.
        /// </summary>
        public virtual void OnAbilityCast()
        {
            currentlyCasting = true;
            print("Cast On", GetName());
        }

        public virtual void OnAbilityUpdate()
        {

        }

        public virtual void OnAbilityEnd()
        {
            currentlyCasting = false;
            print("ability ended", GetName());
        }
    }
    
    public class NoResourceCost : AbilityResourceCost
    {
        public NoResourceCost() : base(null, null)
        {

        }

        public override void CalculateCost()
        {
            return;
        }
        public override bool CanDeduct()
        {
            return true;
        }
        public override void Deduct()
        {
            return;
        }
    }

    public class AbilityResourceCost
    {
        public enum CostType
        {
            Addtive, Percentage, MaximumPercentage, MissingPercentage
        }

        public class ResourceCost
        {
            public Entity owner;
            public CostType type;
            public ResourceID resourceID;
            public float flatAmount;
            public float value;

            public float? calculatedCost;

            public ResourceCost(Entity owner, CostType type, ResourceID resourceID, float flatAmount, float value)
            {
                this.owner = owner;
                this.type = type;
                this.resourceID = resourceID;
                this.flatAmount = flatAmount;
                this.value = value;
                calculatedCost = null;
            }

            public static ResourceCost Additive(Entity owner, ResourceID resourceID, float flatAmount) =>
                new(owner, CostType.Addtive, resourceID, flatAmount, 0);
            public static ResourceCost Percentage(Entity owner, ResourceID resourceID, float value, float flatAmount = 0) =>
                new(owner, CostType.Percentage, resourceID, flatAmount, value);
            public static ResourceCost MaximumPercentage(Entity owner, ResourceID resourceID, float value, float flatAmount = 0) =>
                new(owner, CostType.MaximumPercentage, resourceID, flatAmount, value);

            public float CalculateCost()
            {
                calculatedCost = flatAmount;
                switch (type)
                {
                    case CostType.Addtive:
                        calculatedCost += value;
                        break;
                    case CostType.Percentage:
                        calculatedCost += owner.status[resourceID] * value;
                        break;
                    case CostType.MaximumPercentage:
                        calculatedCost += owner.status[resourceID].GetMaximumValue() * value;
                        break;
                    default:
                        throw new NotImplementedException();
                } 
                return calculatedCost.Value;
            }

            public bool CanDeduct()
            {
                if(calculatedCost == null) 
                    CalculateCost();
                return owner.status[resourceID].GetValue() >= calculatedCost;
            }

            public void Deduct()
            {
                if(calculatedCost == null) throw new Exception("Calculate Cost first before deducting?");
                else if (!CanDeduct()) throw new Exception("This is impossible.");
                else if (calculatedCost < 0) throw new Exception("The deduction value should not be negative in this usage case.");

                owner.status[resourceID].Add(-calculatedCost.Value);
            }
        }

        protected Entity Owner;
        protected List<ResourceCost> Costs;

        public AbilityResourceCost(Entity owner, List<ResourceCost> costs)
        {
            Owner = owner;
            Costs = costs; 
        }


        public virtual void CalculateCost()
        {
            Costs.ForEach(cost => cost.CalculateCost());
        }
        public virtual bool CanDeduct()
        {
            CalculateCost();
            foreach (var cost in Costs) 
                if (!cost.CanDeduct()) return false; 
            return true;
        }
        public virtual void Deduct()
        {
            Costs.ForEach(cost => cost.Deduct());
        }
    }
}