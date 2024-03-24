using System;
using System.Collections.Generic;

namespace Game.Abilities
{
    public delegate NAbility AbilityConstructor(string key);
    public class AbilityFactory : BritoBehavior
    {
        public static AbilityFactory Instance;

        public static SortedDictionary<string, Type> Abilities;
        public void Awake()
        {
            Instance = this;
            if (Abilities == null)
                InstantiateAbilities();
        }

        public static void InstantiateAbilities()
        { 
            Abilities = new()
            {
                { "sword combo", typeof(SwordCombo) },
                { "magic", typeof(MagicCastTest) },
            };
        }

        public static NAbility GetAbility(string name, Entity entity)
        {
            if (Abilities == null) InstantiateAbilities();

            if (Abilities.TryGetValue(name, out var abilityType))
            { 
                var constructor = abilityType.GetConstructor(new[] { typeof(Entity) });
                if (constructor != null) 
                    return (NAbility)constructor.Invoke(new object[] { entity }); 
                else print("Constructor with int parameter not found for type", abilityType); 
            }
            return null;
        }
    }
}