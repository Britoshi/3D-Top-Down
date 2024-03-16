using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	[CreateAssetMenu(fileName = "New Default Status", menuName = "Scriptable Objects/New Default Status")] 
	public class StatusBuilder : ScriptableObject
    {
		[Header("Attribute")]
        public float Strength;
        public float Agility;
        public float Intelligence;
        public float Will;
        public float Vitality;
        public float Endurance;
        public float Resistance;

        [Header("Resources")]
        public uint Level = 1, 
			MaxHP = 100, MaxMP = 100, MaxSP = 100;  
		public uint HP = 100, MP = 100, SP = 100; 
		public int Money;

        [Header("Extra Attribute")]
        public uint Speed;
		public uint Range;
		public uint Offense;
		public uint Defense;
		public uint Mobility;

        [Header("Variables")]
        public float MovementSpeed = 1.75f;
        public float JumpForce = 2f;  
		public uint FlatArmorPenetration, FlatMagicPenetration;
		public float ArmorPenetration, MagicPenetration; 
		public uint Armor = 0, MagicResistance = 0;

		[Header("Regeneration")]
		public float HPRegen = 1f;
        public float SPRegen;
        public float MPRegen;

        [Header("Etc")]
        public uint Tenacity;
		public float DamageOutputModifier = 1f, HealingModifier = 1f;
		public float CooldownReduction = 0f;
	
    }
}
