using System; 

namespace Game
{
	[Serializable]
	//[CreateAssetMenu(fileName = "New Default Status", menuName = "ScriptableObjects/New Default Status")] 
	public class StatusBuilder
    { 
        public uint Level = 1, MaxHP = 100;  
		public uint HP = 100; 
		public int Money; 
		public uint Speed;
		public uint Range;
		public uint Offense;
		public uint Defense; 
		public float MovementSpeed = 5f;
		public float AttackSpeed = .8f; 
		public float AttackRange = 5f;
		public uint AttackDamage = 1, AbilityPower;
		public uint FlatArmorPenetration, FlatMagicPenetration;
		public float ArmorPenetration, MagicPenetration; 
		public uint Armor = 0, MagicResistance = 0; 
		public float HPRegen = 1f;
		public uint Tenacity;
		public float LifeSteal, DamageOutputModifier = 1f, HealingModifier = 1f;
	}
}
