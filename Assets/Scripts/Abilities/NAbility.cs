using System;
using System.Collections.Generic;
using UnityEditor;
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


        protected bool MovementOverride;

        private bool currentlyCasting;
        public bool IsCasting => currentlyCasting;

        protected bool busy;
        protected float animationProgress;  

        public bool canCastMidAir;

        public abstract string GetName();
        public abstract string GetAnimationNodeName();
        public abstract bool GetAnimationRootMotion();
        protected abstract CooldownOn ApplyCooldownOn();

        public Event InvokeOnStart, InvokeOnUpdate, InvokeOnEnd;
        public NAbility(Entity owner, bool canCastMidAir, Cooldown cooldown = null, AbilityResourceCost cost = null)
        {
            Owner = owner; 
            this.canCastMidAir = canCastMidAir;

            //Cost = cost != null ? cost : AbilityResourceCost.None;
            Cost = cost ?? new NoResourceCost();
            this.cooldown = cooldown ?? Cooldown.Create(0);

            currentlyCasting = false;
            MovementOverride = false;
            animationProgress = 0f;

            //_abilityEvents = new List<TimedEvent>();

            InvokeOnStart = new();
            InvokeOnUpdate = new();
            InvokeOnEnd = new();
        }

        public Result CanCast()
        {
            //perhaps check if the animation is able?
            if (currentlyCasting) return Result.Fail("Already Casting");
            else if (!cooldown.CanCast()) return Result.Fail("Ability On Cooldown.");
            else if (!Cost.CanDeduct()) return Result.Fail("Not Enough Resources.");
            return Result.Success();
        }

        public Result TryCast()
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

        #region Animation Functions #Only put animation related function.
        public void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animationProgress = 0;
            if (ApplyCooldownOn() == CooldownOn.START) cooldown.ApplyCooldown();
        }
        public void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animationProgress = stateInfo.normalizedTime % 1; 
        }
        public void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animationProgress = -1;
            if (ApplyCooldownOn() == CooldownOn.END) cooldown.ApplyCooldown();
            Owner.stateMachine.ResetState(); 
        }
        #endregion

        /// <summary>
        /// Called before the animation starts.
        /// </summary>
        public void OnAbilityCast()
        {
            currentlyCasting = true;
        }

        public void OnAbilityUpdate()
        {
            cooldown.GlobalUpdate();

        }

        public void OnAbilityEnd()
        {
            currentlyCasting = false;
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