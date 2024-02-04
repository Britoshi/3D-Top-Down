using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Abilities
{
    public class Cooldown
    {
        private float _time;
        private float _cooldownTime;

        public float GetProgress()
        {
            if (_cooldownTime == 0) return 1; 
            return 1 - (_time / _cooldownTime);
        }

        //Must be called from mother script so it doesn't stop after stop casting.
        public void GlobalUpdate()
        {
            if(!CanCast())
                _time -= Time.deltaTime;
        }

        public bool CanCast() => _time <= 0;

        public void ApplyCooldown(float? cooldownReductionPercent = null) =>
            _time = cooldownReductionPercent != null ? _cooldownTime * (1 - cooldownReductionPercent.Value) : _cooldownTime; 

        private Cooldown(float cooldown) =>
            _cooldownTime = cooldown; 

        public static Cooldown Create(float cooldown) { return new Cooldown(cooldown); } 
    }
}
