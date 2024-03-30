using UnityEngine;

namespace Game
{
    public class EntityUtil
    {
        public static void Modify(Entity self, HealthModificationData metadata)
        { 
            float amount = metadata.modifyValue; 
            if (amount < 0) Damage(self, metadata);
            else if (amount > 0) Heal(self, metadata);
        }


        static float CalculateDamage(HealthModificationData data)
        {
            float damage = Mathf.Abs(data.modifyValue);

            //if (data.type == DamageType.PRIMARY)
            //    damage -= damage - damage * (100f / (100f + data.target.Armor));
            if (data.affect.type != HealthAffectType.TRUE)
                damage -= damage - damage * (100f / (100f + data.target[data.affect.type]));

            if (data.source != null) damage *= data.source.status.DamageOutputModifier;
            return damage;
        }

        public static void Damage(Entity owner, int damage)
        {
            if (owner.status.dead) return; 
            owner.status.HP.Subtract(damage);
            if (!VitalCheck(owner)) Death(owner, null);
        } 

        public static void Damage(Entity victim, Entity source, int damage)
        {
            victim?.status.ApplyGetHitAffects(source, damage);
            damage = Mathf.Abs(damage);
            Damage(victim, damage); 
        }

        public static void Damage(Entity victim, HealthModificationData damageData)
        {  
            if (victim.status.dead) return;

            Entity attacker = damageData.source;
            float damage = CalculateDamage(damageData);
            attacker?.status.onDealDamage?.Invoke(victim, damage);

            victim.status.HP.Subtract(damage);

            if (damageData.type == DamageType.ABILITY) 
                attacker?.status.ApplyOnHitAffects(victim, damage);  
            else if (damageData.type == DamageType.ON_HIT)
            {

            } 
            victim?.status.ApplyGetHitAffects(attacker, damage); 

            if (damageData.type == DamageType.ABILITY) Debug.Log("Lmao Spell aint supported yet"); 

            victim.status.onTakeDamage?.Invoke(attacker, damage);
            if (!VitalCheck(victim)) Death(victim, attacker);
        }

        public static void Heal(Entity owner, HealthModificationData metadata)
        {
            if (owner.status.dead) return;
            Entity healer = metadata.source;
            float totalHealAmount = owner.status.HealingModifier * metadata.modifyValue;
            owner.status.HP.Add(totalHealAmount);
        }
        public static void Heal(Entity owner, int amount)
        {
            if (owner.status.dead) return; 
            float totalHealAmount = owner.status.HealingModifier * amount;
            owner.status.HP.Add(totalHealAmount);
        }

        /// <summary>
        /// See if the entity should be alive or not.
        /// </summary>
        /// <returns>true, if it should be alive. false if it should be dead</returns>
        public static bool VitalCheck(Entity self) => self.status.HP > 0;

        public static void Death(Entity self, Entity killer)
        {
            self.status.dead = true;  
            killer?.status.ApplyOnKillAffects(self);  
            self.status.onKilled?.Invoke(killer);
        } 
    }
}
