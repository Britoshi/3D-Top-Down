using Game.Abilities;
using UnityEngine;

namespace Game
{
    public class Ability
    {
        public Entity owner;
        public Cooldown cooldown;
        [SerializeField] public string Name { private set; get; }

        public virtual AbilityCost Cost { set; get; }

        protected CooldownOn ApplyCooldownOn { private set; get; }
    }
}