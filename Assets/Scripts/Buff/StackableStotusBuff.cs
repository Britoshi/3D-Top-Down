using UnityEngine;

namespace Game.Buff
{
    //[CreateAssetMenu(fileName = "New Stackable Status Buff", menuName = "ScriptableObjects/Buff/New Stackable Status Buff")]
    public class StackableStatusBuff : StatusBuff
    {
        public int maximumStackCount = 6;
        public int stackCount;

        /// <summary>
        /// How fast does it decay.
        /// </summary>
        public float fadeAwayTimeInSeconds = .15f;

        public StackableStatusBuff(string name, string descriptionFormula, bool isPermanent, float durationInSeconds, bool isDebuff, AttributeAffect[] attributeAffect, int maxCount, float fadeAwayTimeInSeconds) : base(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect)
        {
            maximumStackCount = maxCount;
            this.fadeAwayTimeInSeconds = fadeAwayTimeInSeconds;
        }

        public override StatusBuff Clone()
        {
            var clone = new StackableStatusBuff(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect, maximumStackCount, fadeAwayTimeInSeconds)
            {
                source = this.source,
                target = this.target
            };
            return clone;
        }

        internal override void OnApply()
        {
            stackCount = 1;
            base.OnApply();
        }

        internal override void OnReapply()
        {
            if (stackCount < maximumStackCount)
            {
                ApplyAttributeToTarget();
                stackCount++;
            }

            if (!isPermanent) ResetTime();
        }

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
    			Debug.LogError("Supposed to Free?");            
				//Free();
            }
        }
    }
}