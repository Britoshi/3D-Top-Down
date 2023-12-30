namespace Game
{
    /// <summary>
    /// <b>Health Point Modification Metadata.</b>
    /// This struct will contain all the information about how the health is affect.
    /// </summary>
    public class HPModData
    {
        public Status source, target;
        public HealthAffect affect;
        public float modifyValue; 
        
        public bool isAutoAttack;
        public bool isSpell;
        public bool isOnHit; 

        private HPModData(Status source, Status target, HealthAffect affect, bool auto, bool spell, bool onHit, float? amount = null)
        {
            this.source =source;
            this.target = target;
            this.affect = affect;
            isAutoAttack = auto;
            isSpell = spell;
            isOnHit = onHit; 

            if(amount == null) modifyValue = affect.GetAmount(source, target);  
            else modifyValue = amount.Value;
        }  
         
        public static HPModData AutoAttack(Status source, Status target) =>
            new(source, target, null, true, false, false, (int)source.AttackDamage);
        public static HPModData Spell(Status source, Status target, HealthAffect affect) =>
            new(source, target, affect, false, true, false);
        public static HPModData SpellAutoAttack(Status source, Status target, HealthAffect affect) =>
            new(source, target, affect, true, true, false); 
        public static HPModData OnHit(Status source, Status target, HealthAffect affect) =>
            new(source, target, affect, false, false, true); 
        public static HPModData Heal(Status source, Status target, HealthAffect affect) =>
            new(source, target, affect, false, false, false); 
    }
}