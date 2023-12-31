/*
using UnityEngine;

namespace Game.Abilities
{

    public class ExampleChainAttackNode1 : ChainNode
    {
        public ExampleChainAttackNode1(EntityBase owner, ChainedAbility master, float castGrace) : 
            base(owner, master, castGrace, AbilityCost.None)
        {
            AddEvent(.5f, SpawnHitbox);
        }

        void DamageEnemy(EntityBase target)
        {
            target.InflictDamage(DamageType.Normal, 100);
        }

        void SpawnHitbox()
        {
            var hitbox = Resources.InstantiateHitbox(Owner, Name);
            hitbox.Init(DamageEnemy, .1f);
        }
    }
    public class ExampleChainAttackNode2 : ChainNode
    {
        public ExampleChainAttackNode2(Entity owner, ChainedAbility master, float castGrace) :
            base(owner, master, castGrace, AbilityCost.None)
        { 
            AddEvent(.5f, SpawnHitbox);
        }

        void DamageEnemy(Entity target)
        {
            target.InflictDamage(DamageType.NONE, 100);
        }

        void SpawnHitbox()
        {
            var hitbox = Resources.InstantiateHitbox(Owner, Name);
            hitbox.Init(DamageEnemy, .1f);

            Debug.Log("SPAWNED");
        }
    }
    public class ExampleChainAttackNode3 : ChainNode
    {
        public ExampleChainAttackNode3(EntityBase owner, ChainedAbility master, float castGrace) :
            base(owner, master, castGrace, AbilityCost.None)
        {
            AddEvent(.5f, SpawnHitbox);
        }

        void DamageEnemy(EntityBase target)
        {
            target.InflictDamage(DamageType.Normal, 100);
        }

        void SpawnHitbox()
        {
            var hitbox = Resources.InstantiateHitbox(Owner, Name);
            hitbox.Init(DamageEnemy, .1f);
        }
    }

    public class ExampleChainAttack : ChainedAbility
    {
        public ExampleChainAttack(EntityBase owner) : 
            base(owner, "Chain Attack", Cooldown.Create(5f))
        {
            new ExampleChainAttackNode1(owner, this, 3f);
            new ExampleChainAttackNode2(owner, this, 3f);
            new ExampleChainAttackNode3(owner, this, 3f);
            MovementOverride = false;
        } 

        public override void GlobalUpdate()
        {
            base.GlobalUpdate(); 
        }
    }
}
*/