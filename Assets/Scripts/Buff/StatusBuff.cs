using System; 
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buff
{
    public class StatusBuff : IComparable<StatusBuff>
    {
        public Status source, target;

        /// <summary>
        /// {targetAttribute} = AttributeAffect.affectedStatName. 
        /// {affectValue} = Affect.GetAmount()
        /// Use {stat_name} to obtain 
        /// </summary>
        [SerializeField] public string name;
        [SerializeField] protected string descriptionFormula;
        public string Description { private set; get; }
        [SerializeField] public bool isPermanent;
        [SerializeField] public float durationInSeconds;
        public float durationCountdown;
        public float DurationRatio => durationCountdown / durationInSeconds;
        [SerializeField] public bool isDebuff;
        [SerializeField] public AttributeAffect[] attributeAffect;

        public StatusBuff(string name, string descriptionFormula, bool isPermanent, float durationInSeconds, bool isDebuff, AttributeAffect[] attributeAffect)
        {
            this.name = name;
            this.descriptionFormula = descriptionFormula;
            this.isPermanent = isPermanent;
            this.durationInSeconds = durationInSeconds;
            this.isDebuff = isDebuff;
            this.attributeAffect = attributeAffect;
        }

        public virtual StatusBuff Clone()
        {
            var clone = new StatusBuff(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect)
            {
                source = source,
                target = target
            };
            return clone;
        }

        public override bool Equals(object other)
        {
            return other is StatusBuff ? CompareTo(other as StatusBuff) == 0 :
                base.Equals(other);
        }
        public int CompareTo(StatusBuff other)
        {
            return name.CompareTo(other.name);
            /*
            int compareValue = name.CompareTo(other.name);
            if (compareValue != 0) return compareValue;
            return isStackable.CompareTo(other.isStackable);*/
        }

        protected virtual void OnUpdate() { }
        public void Update()
        {
            OnUpdate();
            if (isPermanent) return;
            durationCountdown -= (float)Time.deltaTime;
            if (durationCountdown <= 0) OnTimeExpire();
        }

        protected void ApplyAttributeToTarget()
        {
            foreach (var affect in attributeAffect)
			{ 
                affect?.Apply(source, target); 
			}
        }

        protected void RemoveAttributeFromTarget()
        {
            foreach (var affect in attributeAffect)
			{ 
                affect?.Remove(target); 
			}
        }

        internal virtual void OnReapply()
        {  
            ResetTime();
        }
        internal virtual void OnApply()
        { 
            ApplyAttributeToTarget();
            if (isPermanent) return;
            ResetTime();
        }

        public virtual void Apply(Status source, Status target)
        {
            var buffSet = target.appliedBuffs;
            if (buffSet.TryGetValue(this, out StatusBuff existingBuff))
            {
                existingBuff.target = target;
                existingBuff.source = source;
                existingBuff.OnReapply();
            }
            else
            {
                var newInstance = Clone();
                newInstance.name = name;

                buffSet.Add(newInstance);
                newInstance.source = source;
                newInstance.target = target;
                newInstance.OnApply();
            }
        }
        protected void ResetTime()
        {
            if (!isPermanent)
                durationCountdown = durationInSeconds;
        }
        public virtual void OnTimeExpire()
        { 
            RemoveAttributeFromTarget();
            target.appliedBuffs.Remove(this);
        }
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();
    } 
}