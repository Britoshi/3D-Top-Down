using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatusModule
    {
        /// <summary>
        /// This should be NONE if you are not using it as a derivative, such as this value will contribute 15% of armor of the wearer.
        /// </summary>
        [Tooltip("This should be NONE if you are not using it as a derivative")]
        [SerializeField] public StatID statID;
        [SerializeField] public ResourceAffectType statAffectType;
        [SerializeField] public float amount;
        public StatusModule(ResourceAffectType statAffectType, StatID id, float amount)
        {
            this.statAffectType = statAffectType;
            statID = id;
            this.amount = amount;
        }
        public static StatusModule Additive(float amount) => new(ResourceAffectType.Additive, 0, amount);
        public static StatusModule Additive(StatID id, float amount) => new(ResourceAffectType.Additive, id, amount);
        public static StatusModule Multiplicative(StatID id, float amount) => new(ResourceAffectType.Percentage, id, amount);
        public static StatusModule MaximumMultiplicative(StatID id, float amount) => new(ResourceAffectType.MaximumPercentage, id, amount);
    }
}
