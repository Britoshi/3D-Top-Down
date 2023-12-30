using UnityEngine;

namespace Game
{ 
    public class ResourceModifier
    {
        /// <summary>
        /// If this variable is empty or null, it means that it is a flat number. 
        /// Otherwise, the amountOrPercentage will become percentage of the given stat name.
        /// </summary> 
        [SerializeField] public ResourceAffectType affectType;
        [SerializeField] public StatID statID;
        [SerializeField] public float flatAmount;
        [SerializeField] public float statScalePercentage;
        public ResourceModifier(float flatAmount, ResourceAffectType type, StatID statID = StatID.NONE, float scaling = 0f)
        {
            affectType = type;
            this.statID = statID;
            this.flatAmount = flatAmount;
            statScalePercentage = scaling;
        }
        public static ResourceModifier Additive(float value) => new(value, ResourceAffectType.Additive);
        public static ResourceModifier Additive(StatID statID, float scaling) => new(0, ResourceAffectType.Additive, statID, scaling);
        public static ResourceModifier Multiplicative(float value) => new(value, ResourceAffectType.Percentage);
        public static ResourceModifier Multiplicative(StatID statID, float scaling) => new(0, ResourceAffectType.Percentage, statID, scaling);
        public static ResourceModifier MaximumValueMultiplicative(float value) => new(value, ResourceAffectType.MaximumPercentage);
        public static ResourceModifier MaximumValueMultiplicative(StatID statID, float scaling) => new(0, ResourceAffectType.MaximumPercentage, statID, scaling);
    }
}
