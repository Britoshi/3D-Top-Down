namespace Game
{
    [System.Flags]
    public enum DamageType
    {
        NONE, AUTO_ATTACK, ABILITY, ON_HIT, ITEM
    }
    /// <summary>
    /// <b>Health Point Modification Metadata.</b>
    /// This struct will contain all the information about how the health is affect.
    /// </summary>
    public class HealthModificationData
    {
        public Status source, target;
        public HealthAffect affect;
        public float modifyValue; 
        
        public DamageType type;

        private HealthModificationData(Status source, Status target, HealthAffect affect, DamageType type, float? amount = null)
        {
            this.source =source;
            this.target = target;
            this.affect = affect;
            this.type = type;

            if(amount == null) modifyValue = affect.GetAmount(source, target);  
            else modifyValue = amount.Value;
        }  
         
        public static HealthModificationData AutoAttack(Status source, Status target) =>
            new(source, target, null, DamageType.AUTO_ATTACK, (int)source.AttackDamage);
        public static HealthModificationData Spell(Status source, Status target, HealthAffect affect) =>
            new(source, target, affect, DamageType.ABILITY);
        public static HealthModificationData SpellAutoAttack(Status source, Status target, HealthAffect affect) =>
            new(source, target, affect, DamageType.AUTO_ATTACK | DamageType.ABILITY); 
        public static HealthModificationData OnHit(Status source, Status target, HealthAffect affect) =>
            new(source, target, affect, DamageType.ON_HIT);  
    }
}