namespace Game
{
    [System.Flags]
    public enum DamageType
    {
        NONE, PRIMARY, ABILITY, ON_HIT, ITEM
    }
    /// <summary>
    /// <b>Health Point Modification Metadata.</b>
    /// This struct will contain all the information about how the health is affect.
    /// </summary>
    public class HealthModificationData
    {
        public Entity source, target;
        public HealthAffect affect;
        public float modifyValue; 
        
        public DamageType type;

        private HealthModificationData(Entity source, Entity target, HealthAffect affect, DamageType type, float? amount = null)
        {
            this.source =source;
            this.target = target;
            this.affect = affect;
            this.type = type;

            if(amount == null) modifyValue = affect.GetAmount(source, target);  
            else modifyValue = amount.Value;
        }  
         
        public static HealthModificationData AutoAttack(Entity source, Entity target) =>
            new(source, target, null, DamageType.PRIMARY, (int)source.status.AttackDamage);
        public static HealthModificationData Spell(Entity source, Entity target, HealthAffect affect) =>
            new(source, target, affect, DamageType.ABILITY);
        public static HealthModificationData SpellAutoAttack(Entity source, Entity target, HealthAffect affect) =>
            new(source, target, affect, DamageType.PRIMARY | DamageType.ABILITY); 
        public static HealthModificationData OnHit(Entity source, Entity target, HealthAffect affect) =>
            new(source, target, affect, DamageType.ON_HIT);  
    }
}