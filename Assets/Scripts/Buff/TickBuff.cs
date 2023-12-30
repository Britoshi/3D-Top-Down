//using UnityEngine;



namespace Game.Buff
{
    //[CreateAssetMenu(fileName = "New Tick Buff", menuName = "ScriptableObjects/Buff/New Tick Buff")]
    public class TickBuff : StatusBuff, IHasOnTick
    {

        public ResourceAffect affect;
        protected float damageToDealWithDeltaTime;

        public TickBuff(string name, string descriptionFormula, bool isPermanent, float durationInSeconds, bool isDebuff, AttributeAffect[] attributeAffect, ResourceAffect affect) : base(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect)
        {
            this.affect = affect;
        }

        public override StatusBuff Clone()
        {
            var clone = new TickBuff(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect, affect)
            {
                source = source,
                target = target,
            };
            return clone;
        }

        public void OnTick()
        {
            target.ApplyResourceAffect(source, affect, damageToDealWithDeltaTime);
        }

        protected void CalculateDamage()
        {
            int totalTicks = (int)(durationInSeconds / Tick.TICK_INTERVAL);
            var affectAmount = affect.GetAmount(source, target);
            damageToDealWithDeltaTime = affectAmount / totalTicks;
        }

        internal override void OnApply()
        {
            ApplyAttributeToTarget(); 
            CalculateDamage();

            Tick.AddFunction(OnTick);
            if (isPermanent) return;
            ResetTime();
        }

        internal override void OnReapply()
        {
            ResetTime();
            CalculateDamage();
        }

        protected override void OnUpdate() { }

        public override void OnTimeExpire()
        {
            RemoveAttributeFromTarget();
            target.appliedBuffs.Remove(this);
            Tick.RemoveFunction(OnTick);
        }
    }

}