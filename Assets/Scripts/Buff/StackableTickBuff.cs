using System;

using UnityEngine;

namespace Game.Buff
{
    //[CreateAssetMenu(fileName = "New Stackable Tick Buff", menuName = "ScriptableObjects/Buff/New Stackable Tick Buff")]
    public class StackableTickBuff : StatusBuff, IHasOnTick
    {
        public ResourceAffect affect;
        protected float damageToDealWithDeltaTime;

        public int maximumStackCount = 6;
        public int stackCount;
        /// <summary>
        /// How fast does it decay.
        /// </summary>
        public float fadeAwayTimeInSeconds = .15f;

        public StackableTickBuff(string name, string descriptionFormula, bool isPermanent, float durationInSeconds, bool isDebuff, AttributeAffect[] attributeAffect, int maxCount, ResourceAffect affect, float fadeAwayTimeInSeconds) : base(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect)
        {
            maximumStackCount = maxCount; 
            this.affect = affect;
            this.fadeAwayTimeInSeconds = fadeAwayTimeInSeconds;
        }

        public override StatusBuff Clone()
        {
            var clone = new StackableTickBuff(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect, maximumStackCount, affect, fadeAwayTimeInSeconds)
            {
                source = this.source,
                target = this.target,
            };
            return clone;
        }

        public void OnTick()
        {
            if (stackCount == 0)
                throw new Exception("How is tick running with 0 stack?");
            if (affect is HealthAffect)
            {
                var tickDamage = damageToDealWithDeltaTime * stackCount; 
                if (tickDamage < 0)
                    EntityUtil.Damage(target, source, (int)MathF.Round(tickDamage));
                else if(tickDamage > 0)
                    EntityUtil.Heal(target, (int)MathF.Round(tickDamage));
            }
            else target.status.ApplyResourceAffect(source, affect, damageToDealWithDeltaTime * stackCount);
        }

        internal void CalculateDamage()
        {
            int totalTicks = (int)(durationInSeconds / Tick.TICK_INTERVAL);
            var affectAmount = affect.GetAmount(source, target); 
            damageToDealWithDeltaTime = affectAmount / totalTicks; 
        }

        internal override void OnApply()
        {
            stackCount = 1;

            ApplyAttributeToTarget(); 
            CalculateDamage();

            Tick.AddFunction(OnTick);
            ResetTime();
        }

        internal override void OnReapply()
        {
            ResetTime();
            CalculateDamage();

            if (stackCount < maximumStackCount)
            {
                ApplyAttributeToTarget();
                stackCount++;
            }
        }

        protected override void OnUpdate() { }

        public override void OnTimeExpire()
        {
            if (durationCountdown == 0f)
            {
                for (int i = 0; i < stackCount; i++)
                    RemoveAttributeFromTarget();
                stackCount = 0;
            }
            else
            {
                RemoveAttributeFromTarget();
                durationCountdown = fadeAwayTimeInSeconds;
                stackCount--;
            }

            if (stackCount <= 0)
            {
                target.status.appliedBuffs.Remove(this);
                Tick.RemoveFunction(OnTick);
                
				Debug.LogError("Free is supposed to happen, forgot what it does tho.");
				//Free();
            }
        }
    }
}