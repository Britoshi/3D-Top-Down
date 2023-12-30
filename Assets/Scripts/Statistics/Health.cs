using UnityEngine;

namespace Game
{
    public class Health
    {
        public static void Modify(Status self, HPModData metadata)
        { 
            float amount = metadata.modifyValue; 
            if (amount < 0) Damage(self, metadata);
            else if (amount > 0) Heal(self, metadata);
        }

        public static void AutoAttack(Status source, Status target)
        {
            var metadata = HPModData.AutoAttack(source, target);
            Damage(target, metadata); 
        }

        static int CalculateDamage(HPModData data)
        {
            float damage = Mathf.Abs(data.modifyValue);

            if (data.isAutoAttack)
                damage -= damage - damage * (100f / (100f + data.target.Armor));
            else if (data.affect.type != HealthAffectType.True)
                damage -= damage - damage * (100f / (100f + data.target[data.affect.type]));

            if (data.source != null) damage *= data.source.DamageOutputModifier;
            return (int)damage;
        }

        public static void Damage(Status owner, int damage)
        {
            if (owner.dead) return; 
            owner.HP.Subtract(damage);
            if (!VitalCheck(owner)) Death(owner, null);
        } 

        public static void Damage(Status victim, Status source, int damage)
        {
            victim?.ApplyGetHitAffects(source, damage);
            damage = Mathf.Abs(damage);
            Damage(victim, damage); 
        }

        public static void Damage(Status victim, HPModData damageData)
        {  
            if (victim.dead) return;

            Status attacker = damageData.source;
            int damage = CalculateDamage(damageData);
            attacker?.onDealDamage?.Invoke(victim, damage);

            victim.HP.Subtract(damage);

            if (damageData.isAutoAttack) 
                attacker?.ApplyOnHitAffects(victim, damage);  
            else if (damageData.isOnHit)
            {

            } 
            victim?.ApplyGetHitAffects(attacker, damage); 

            if (damageData.isSpell) Debug.Log("Lmao Spell aint supported yet"); 

            victim.onTakeDamage?.Invoke(attacker, damage);
            if (!VitalCheck(victim)) Death(victim, attacker);
        }

        public static void Heal(Status owner, HPModData metadata)
        {
            if (owner.dead) return;
            Status healer = metadata.source;
            float totalHealAmount = owner.HealingModifier * metadata.modifyValue;
            owner.HP.Add(totalHealAmount);
        }
        public static void Heal(Status owner, int amount)
        {
            if (owner.dead) return; 
            float totalHealAmount = owner.HealingModifier * amount;
            owner.HP.Add(totalHealAmount);
        }

        /// <summary>
        /// See if the entity should be alive or not.
        /// </summary>
        /// <returns>true, if it should be alive. false if it should be dead</returns>
        public static bool VitalCheck(Status self) => self.HP > 0;

        public static void Death(Status self, Status killer)
        {
            self.dead = true;  
            killer?.ApplyOnKillAffects(self);  
            self.onKilled?.Invoke(killer);
        } 
    }
}
