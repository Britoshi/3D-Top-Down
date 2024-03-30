using Game;
using UnityEngine;

namespace Game
{ 
    public class HealthAffect : ResourceAffect
    {
        [SerializeField] public HealthAffectType type;
        public HealthAffect(HealthAffectType type, params ResourceModifier[] affectAmount) : base(ResourceID.HP, affectAmount)
        {  
            this.type = type;
        } 
         
    }
}
