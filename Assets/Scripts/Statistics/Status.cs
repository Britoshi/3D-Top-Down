using Game.Buff;
using System.Collections.Generic;
using System;
using Game.Utilities;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Entity))]
    [Serializable]
    public class Status : BritoBehavior, IHasOnTick
    {
        public StatusBuilder builder;

        public EntityTriggeredFunction onKilled;
        public EntityTriggeredValueFunction onDealDamage, onTakeDamage;

        public Inventory inventory;
        public bool dead;

        #region Status Layout  
        [HideInInspector]
        public IntegerAttribute Level, MaxHP, MaxMP, MaxSP;
        //Root Attributes
        [HideInInspector]
        public IntegerAttribute Speed, Range, Offense, Defense;
        //Child Attributes
        [HideInInspector]
        public IntegerAttribute Mobility;

        [HideInInspector]
        public FloatAttribute MovementSpeed, JumpForce;
        [HideInInspector]
        public FloatAttribute AttackSpeed;
        [HideInInspector]
        //Offense
        public FloatAttribute AttackRange;
        [HideInInspector]
        public IntegerAttribute AttackDamage, AbilityPower;
        [HideInInspector]
        public IntegerAttribute FlatArmorPenetration, FlatMagicPenetration;
        [HideInInspector]
        public FloatAttribute ArmorPenetration, MagicPenetration;
        [HideInInspector]
        //Defense
        public IntegerAttribute Armor, MagicResistance;
        [HideInInspector]
        //None 
        public FloatAttribute HPRegen;
        [HideInInspector]
        public IntegerAttribute Tenacity;
        [HideInInspector]
        public FloatAttribute LifeSteal, DamageOutputModifier, HealingModifier;
        [HideInInspector]

        public CappedResource HP, MP, SP;
        [HideInInspector]
        public SignedResource Money;
        [HideInInspector]

        public FloatAttribute CooldownReduction;

        #endregion

        private Stat[] _stat_array;

        //Marker to make sure there are no lingering ticks.
        private bool freedTasks;

        internal SortedSet<StatusBuff> appliedBuffs;
        internal SortedSet<AdvancedStatusBuff> appliedAdvancedBuffs;
        internal Entity owner;

        #region Initializers 
        internal void Initialize(Entity owner)
        {
            this.owner = owner;
            Initialize();
        }

        private void Initialize()
        {
            dead = false;
            appliedBuffs = new();
            appliedAdvancedBuffs = new();

            builder =
            builder != null ? builder : ScriptableObject.CreateInstance<StatusBuilder>();

            Level = new(builder.Level);
            MaxHP = new(builder.MaxHP);
            MaxMP = new(builder.MaxMP);
            MaxSP = new(builder.MaxSP);
            Speed = new(builder.Speed);
            Range = new(builder.Range);
            Offense = new(builder.Offense);
            Defense = new(builder.Defense);

            Mobility = new(builder.Mobility);
            MovementSpeed = new(builder.MovementSpeed, parent: Mobility);
            JumpForce = new(builder.JumpForce, parent: Mobility);

            AttackSpeed = new(builder.AttackSpeed, parent: Speed);

            AttackRange = new((int)builder.AttackRange, parent: Range);
            AttackDamage = new(builder.AttackDamage, parent: Offense);
            AbilityPower = new(builder.AbilityPower, parent: Offense);

            FlatArmorPenetration = new(builder.FlatArmorPenetration, parent: Offense);
            FlatMagicPenetration = new(builder.FlatMagicPenetration, parent: Offense);
            ArmorPenetration = new(builder.ArmorPenetration, parent: Offense);
            MagicPenetration = new(builder.MagicPenetration, parent: Offense);

            Armor = new(builder.Armor, parent: Defense);
            MagicResistance = new(builder.MagicResistance, parent: Defense);

            HPRegen = new(builder.HPRegen);
            Tenacity = new(builder.Tenacity);
            LifeSteal = new(builder.LifeSteal);
            DamageOutputModifier = new(builder.DamageOutputModifier);
            HealingModifier = new(builder.HealingModifier);

            HP = new(builder.HP, MaxHP);
            MP = new(builder.MP, MaxMP);
            SP = new(builder.SP, MaxSP);
            Money = new(builder.Money);

            CooldownReduction = new(builder.CooldownReduction);

            _stat_array = new Stat[31]
            {
                Level, MaxHP, MaxMP, MaxSP, Speed, Range, Offense, Defense, Mobility, MovementSpeed, JumpForce, AttackSpeed, AttackRange,
                AttackDamage, AbilityPower, FlatArmorPenetration,FlatMagicPenetration, ArmorPenetration,
                MagicPenetration ,Armor, MagicResistance, HPRegen, Tenacity, LifeSteal, DamageOutputModifier,
                HealingModifier, CooldownReduction, HP, MP, SP, Money,
            };

            onKilled += OnDeathEssential;

            print(name, "status successfully initialized.");
        }
        #endregion

        #region Getters
        public Stat this[byte id] => _stat_array[id];
        public Stat this[int id] => _stat_array[id];
        public Stat this[StatID id] => _stat_array[(int)id - 1];
        public Attribute this[AttributeID id] => _stat_array[(int)id - 1] as Attribute;
        public Resource this[ResourceID id] => _stat_array[(int)id - 1] as Resource;
        public Stat this[HealthAffectType id] => _stat_array[(int)id - 1];
        public Stat Get(HealthAffectType id) => this[id];
        #endregion

        #region Node Functions
        void Awake()
        {

        }
        void Start()
        {
            Tick.AddFunction(OnTick);
            freedTasks = false;
        }

        void Update()
        {
            if (appliedBuffs.Count > 0)
            {
                try
                {
                    foreach (var buff in appliedBuffs) 
                        buff?.Update(); 
                }
                //Stupid foreach.
                catch (InvalidOperationException)
                {

                }
            }
            if (HP.GetValue() < MaxHP.GetValue()) HP.Add((int)(HPRegen.GetValue() * Time.deltaTime));
        }

        void OnDestroy()
        {
            OnDeathEssential(null);
        }
        #endregion

        #region Essential Function
        public void OnTick()
        {
            //Regen
            HP.Add(HPRegen.GetValue() * (float)Tick.TrueDeltaTime);
        }

        public void ApplyResourceAffect(Status source, ResourceAffect resourceAffect, float? amount = null)
        {
            if (resourceAffect.id == 0) return;
            amount ??= resourceAffect.GetAmount(source, this);
            this[resourceAffect.id].Add(amount.Value);
        }

        public void ApplyOnHitAffects(Status target, float amount)
        {
            //Eventually want to transition to event system.
            foreach (var item in inventory.items)
                if (item.OnHitAffects != null)
                    item.OnHit(this, target, amount);

            foreach (var buff in appliedAdvancedBuffs)
                if (buff?.OnHitAffects != null) buff.OnHit(this, target, amount);

        }

        public void ApplyGetHitAffects(Status source, float amount)
        {
            foreach (var item in inventory.items) 
                if (item?.OnGetHitAffects != null) item.OnGetHit(this, source, amount);

            foreach (var buff in appliedAdvancedBuffs) 
                if (buff?.OnGetHitAffects != null) buff.OnGetHit(this, source, amount);

        }

        public void ApplyOnKillAffects(Status victim)
        {
            foreach (var item in inventory.items){ 
                if (item?.OnKillAffects != null) item.OnKill(this, victim);
            }
			foreach (var buff in appliedAdvancedBuffs){ 
                if (buff?.OnKillAffects != null) buff.OnKill(this, victim);
        	}
		}

        public void OnDeathEssential(Status _)
        {
            if (freedTasks) return;
            foreach (var buff in appliedBuffs)
            {
                if (buff is IHasOnTick tickBuff)
                    Tick.RemoveFunction(tickBuff.OnTick);
            }
            Tick.RemoveFunction(OnTick);
            freedTasks = true;
        } 
		
		/*
        public void AddNewOnDeathFunction(Node node, string function)
        {
            void F(Status killer)
            {
                node.Call(function);
            }
            onKilled += F;
        }

        public void AddNewOnTakeDamageFunction(Node node, string function)
        {
            void F(Stat Stat, float value)
            {
                node.Call(function, (int)value);
            }
            //onTakeDamage += F; 
            HP.AddCallBack(F);
        }*/

        void DoVitalCheck(Stat stat, int change, Status affecter)
        {
            if (stat.GetValue() <= 0) EntityUtil.Death(this, affecter);
        }

        public void InvokeOnKill(Status killer) => onKilled?.Invoke(killer);
        public void InvokeOnDealDamage(Status target, int amount) => onDealDamage?.Invoke(target, amount);
        public void InvokeOnTakeDamage(Status source, int amount) => onTakeDamage?.Invoke(source, amount);
        //Temporary solution
        public HealthModificationData AutoAttackData(Status other) => HealthModificationData.AutoAttack(this, other);
        #endregion
    }
}