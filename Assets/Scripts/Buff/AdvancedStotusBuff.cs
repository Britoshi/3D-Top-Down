﻿
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buff
{
    //[CreateAssetMenu(fileName = "New Advanced Buff", menuName = "ScriptableObjects/Buff/New Advanced Buff")]
    public class AdvancedStatusBuff : StatusBuff, IAdvancedTriggers
    {
        public AdvancedStatusBuff(string name, string descriptionFormula, bool isPermanent, float durationInSeconds, bool isDebuff, AttributeAffect[] attributeAffect, OnHitAffect[] OnHitAffects, OnHitAffect[] OnGetHitAffects, OnHitAffect[] OnKillAffects) : base(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect)
        {

            this.OnHitAffects = OnHitAffects;
            this.OnGetHitAffects = OnGetHitAffects;
            this.OnKillAffects = OnKillAffects;
        }

        public override StatusBuff Clone()
        {
            var clone = new AdvancedStatusBuff(name, descriptionFormula, isPermanent, durationInSeconds, isDebuff, attributeAffect, OnHitAffects, OnGetHitAffects, OnKillAffects)
            {
                source = source,
                target = target,
            };
            return clone;
        }

        [SerializeField] public OnHitAffect[] OnHitAffects { get; set; }
        [SerializeField] public OnHitAffect[] OnGetHitAffects { get; set; }
        [SerializeField] public OnHitAffect[] OnKillAffects { get; set; }
        public override void Apply(Status source, Status target)
        {
            SortedSet<AdvancedStatusBuff> buffSet = target.appliedAdvancedBuffs;
            if (buffSet.TryGetValue(this, out AdvancedStatusBuff existingBuff))
            {
                existingBuff.target = target;
                existingBuff.source = source;
                existingBuff.OnReapply();
            }
            else
            {
                var newInstance = (AdvancedStatusBuff)Clone();

                buffSet.Add(newInstance);
                target.appliedBuffs.Add(newInstance);

                newInstance.source = source;
                newInstance.target = target;

                newInstance.OnApply();
            }
        }
        internal override void OnApply()
        {
            ApplyAttributeToTarget();
            if (isPermanent) return;
            ResetTime();
        }

        internal override void OnReapply()
        {
            ResetTime();
        }

        protected override void OnUpdate()
        {

        }

        public override void OnTimeExpire()
        {
            RemoveAttributeFromTarget();
            target.appliedBuffs.Remove(this);
            target.appliedAdvancedBuffs.Remove(this);
        }

        internal void OnHit(Status owner, Status target, float amount) =>
            IAdvancedTriggers.OnHit(this, owner, target, amount, CustomOnHit);
        internal void OnGetHit(Status owner, Status source, float amount) =>
            IAdvancedTriggers.OnGetHit(this, owner, source, amount, CustomOnGetHit);
        internal void OnKill(Status killer, Status victim) =>
            IAdvancedTriggers.OnKill(this, killer, victim, CustomOnKill);

        public virtual void CustomOnHit(Status owner, Status target, float amount) { }
        public virtual void CustomOnGetHit(Status owner, Status source, float amount) { }
        public virtual void CustomOnKill(Status killer, Status victim) { }
    }
}