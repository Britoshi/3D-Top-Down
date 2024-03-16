using Game.Buff; 

namespace Game
{

    public enum HealthAffectType
    {
        TRUE, HEAL,
        PHYSICAL = 18, MAGICAL
    }

    public enum StatID
    {
        NONE,
        STRENGTH,AGILITY,INTELLIGENCE,WILL,VITALITY,ENDURANCE, RESISTANCE,
        LEVEL,
        MAX_HP, MAX_MP, MAX_SP,
        SPEED, RANGE, DEFENSE, OFFENSE, MOBILITY,

        MOVEMENT_SPEED, JUMP_FORCE, 
        FLAT_ARMOR_PENETRATION, FLAT_MAGIC_PENETRATION,
        ARMOR_PENETRATION, MAGIC_PENETRATION,

        //This should never change.
        ARMOR, MAGIC_RESISTANCE,

        HP_REGEN, MP_REGEN, SP_REGEN, TENACITY,
        DAMAGE_OUTPUT_MODIFIER, HEALING_MODIFIER, COOLDOWN_REDUCTION,

        //From here is just Resources
        HP, MP, SP, Money, 
    }

    public enum AttributeID
    {
        NONE,
        STRENGTH, AGILITY, INTELLIGENCE, WILL, VITALITY, ENDURANCE, RESISTANCE,
        LEVEL,
        MAX_HP, MAX_MP, MAX_SP,
        SPEED, RANGE, DEFENSE, OFFENSE, MOBILITY,

        MOVEMENT_SPEED, JUMP_FORCE,
        FLAT_ARMOR_PENETRATION, FLAT_MAGIC_PENETRATION,
        ARMOR_PENETRATION, MAGIC_PENETRATION,

        //This should never change.
        ARMOR, MAGIC_RESISTANCE,

        HP_REGEN, MP_REGEN, SP_REGEN, TENACITY,
        DAMAGE_OUTPUT_MODIFIER, HEALING_MODIFIER, COOLDOWN_REDUCTION,

    }

    public enum ResourceID
    {
        NONE = 0,
        HP = 28,
        MP,
        SP,  
        MONEY,
    }

    public delegate void EntityTriggeredFunction(Entity other);
    public delegate void EntityTriggeredValueFunction(Entity source, float value);

    [System.Serializable]
    public class AttributeAffect
    {
        public AttributeID attributeID;
        public StatAffectType affectType;
        public StatusModule[] statusModules;

        public float GetAmount(Entity entitySource)
        {
            Status source = entitySource.status;
            float total = 0f;
            foreach (var amountItem in statusModules)
            {
                //If none, it has to be just additive
                if (amountItem.statID == StatID.NONE)
                    total += amountItem.amount;
                else
                {
                    if (source == null) continue;
                    Stat stat = source[amountItem.statID];
                    switch (amountItem.statAffectType)
                    {
                        case ResourceAffectType.Additive: total += amountItem.amount; break;
                        case ResourceAffectType.Percentage: total += stat * amountItem.amount; break;
                        case ResourceAffectType.MaximumPercentage: total += stat.GetMaximumValue() * amountItem.amount; break;
                    }
                }
            }
            return total;
        }
        public AttributeAffect(AttributeID id, StatAffectType affectType, StatusModule[] affectAmount)
        {
            attributeID = id;
            this.affectType = affectType;
            this.statusModules = affectAmount;
        }

        
        public void Remove(Entity entitySource)
        {
            if (attributeID == 0) return;

            var targetStat = attributeID;
            float amount = GetAmount(entitySource);

            switch (affectType)
            {
                case StatAffectType.Additive:
                    entitySource[targetStat].Subtract(amount);
                    break;
                case StatAffectType.Multiplicative:
                    entitySource[targetStat].Divide(amount);
                    break;
            }
        } 

        public void Apply(Entity source, Entity targetStats)
        {
            if (attributeID == 0) return; 
            var targetStat = attributeID; 
            float amount = GetAmount(source);

            switch (affectType)
            {
                case StatAffectType.Additive:
                    targetStats[targetStat].Add(amount);
                    break;
                case StatAffectType.Multiplicative:
                    targetStats[targetStat].Multiply(amount);
                    break;
            }
        } 

        public static AttributeAffect Additive(AttributeID attributeID, float affect) =>
            Additive(attributeID, StatusModule.Additive(affect));
        public static AttributeAffect Additive(AttributeID attributeID, params StatusModule[] affects) =>
            new(attributeID, StatAffectType.Additive, affects); 
        public static AttributeAffect Multiplicative(AttributeID attributeID, float affect) =>
            Multiplicative(attributeID, StatusModule.Additive(affect));
        public static AttributeAffect Multiplicative(AttributeID attributeID, params StatusModule[] affects) =>
            new(attributeID, StatAffectType.Multiplicative, affects);
    } 
}