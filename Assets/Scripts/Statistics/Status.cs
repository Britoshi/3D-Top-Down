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
        public IntegerAttribute Level, MaxHP;
        //Root Attributes
        public IntegerAttribute Speed, Range, Offense, Defense;
        //Child Attributes
        public FloatAttribute MovementSpeed;
        public FloatAttribute AttackSpeed;
        //Offense
        public FloatAttribute AttackRange;
        public IntegerAttribute AttackDamage, AbilityPower;
        public IntegerAttribute FlatArmorPenetration, FlatMagicPenetration;
        public FloatAttribute ArmorPenetration, MagicPenetration;
        //Defense
        public IntegerAttribute Armor, MagicResistance;
        //None 
        public FloatAttribute HPRegen;
        public IntegerAttribute Tenacity;
        public FloatAttribute LifeSteal, DamageOutputModifier, HealingModifier;

        public CappedResource HP;
        public SignedResource Money;

        public FloatAttribute CooldownReduction => throw new NotImplementedException();

        #endregion

        private Stat[] _stat_array;
        //Marker to make sure there are no lingering ticks.
        private bool freedTasks;

        internal SortedSet<StatusBuff> appliedBuffs;
        internal SortedSet<AdvancedStatusBuff> appliedAdvancedBuffs;
        internal Entity owner;

        #region Initializers 
        public void Initialize()
        {
            dead = false;
            appliedBuffs = new();
            appliedAdvancedBuffs = new();
			/*
            var builderArray = builder.Call("initialize_Status");

            Level = new((uint)builderArray[0]);
            MaxHP = new((uint)builderArray[1]);
            Speed = new((uint)builderArray[2]);
            Range = new((uint)builderArray[3]);
            Offense = new((uint)builderArray[4]);
            Defense = new((uint)builderArray[5]);

            MovementSpeed = new((float)builderArray[6], parent: Speed);
            AttackSpeed = new((float)builderArray[7], parent: Speed);

            AttackRange = new((int)builderArray[8], parent: Range);
            AttackDamage = new((uint)builderArray[9], parent: Offense);
            AbilityPower = new((uint)builderArray[10], parent: Offense);

            FlatArmorPenetration = new((uint)builderArray[11], parent: Offense);
            FlatMagicPenetration = new((uint)builderArray[12], parent: Offense);
            ArmorPenetration = new((float)builderArray[13], parent: Offense);
            MagicPenetration = new((float)builderArray[14], parent: Offense);

            Armor = new((uint)builderArray[15], parent: Defense);
            MagicResistance = new((uint)builderArray[16], parent: Defense);

            HPRegen = new((float)builderArray[17]);
            Tenacity = new((uint)builderArray[18]);
            LifeSteal = new((float)builderArray[19]);
            DamageOutputModifier = new((float)builderArray[20]);
            HealingModifier = new((float)builderArray[21]);

            HP = new((uint)builderArray[22], MaxHP);
            Money = new((int)builderArray[23]);

            _stat_array = new Stat[24]
            {
                                Level, MaxHP, Speed, Range, Offense, Defense, MovementSpeed, AttackSpeed, AttackRange,
                                AttackDamage, AbilityPower, FlatArmorPenetration,FlatMagicPenetration, ArmorPenetration,
                                MagicPenetration ,Armor, MagicResistance, HPRegen, Tenacity, LifeSteal, DamageOutputModifier,
                                HealingModifier, HP, Money,
            };

            onKilled += OnDeathEssential;

            inventory = new();
   		 Debug.Error("Bro Fix this");         
			//builder.Call("initialize_inventory", this);
			*/
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
            Initialize();
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
					{ 
                        buff?.Update();
					}
                }
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
            {
                if (item.OnHitAffects != null)
                    item.OnHit(this, target, amount);
            }
            foreach (var buff in appliedAdvancedBuffs)
			{ 
                if (buff?.OnHitAffects != null) buff.OnHit(this, target, amount);
  	      }
		}

        public void ApplyGetHitAffects(Status source, int amount)
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

        public bool AddItem(Item item)
        {
            item.Apply(this);
            return true;
        }

        public void RemoveItem(Item item)
        {
            item.Remove(this);
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