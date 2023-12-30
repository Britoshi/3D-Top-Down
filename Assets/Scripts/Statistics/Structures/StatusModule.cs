using UnityEngine;

namespace Game
{ 
    public class StatusModule
    {
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
