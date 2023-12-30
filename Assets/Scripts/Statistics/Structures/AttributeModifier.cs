using UnityEngine;

namespace Game
{ 
    public class AttributeModifier
    {
        [SerializeField] public StatID statID;
        [SerializeField] public StatAffectType statAffectType;
        [SerializeField] public float amount;
        private AttributeModifier(StatAffectType statAffectType, StatID id, float amount)
        {
            this.statAffectType = statAffectType;
            statID = id;
            this.amount = amount;
        }
        public static AttributeModifier Additive(StatID id, float amount) => new(StatAffectType.Additive, id, amount);
        public static AttributeModifier Multiplicative(StatID id, float amount) => new(StatAffectType.Multiplicative, id, amount);
    }
}
