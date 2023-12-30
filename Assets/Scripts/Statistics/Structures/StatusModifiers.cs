using System; 
using Game.Buff;

namespace Game
{ 
    public enum AffectTarget { None, Self, Enemy, }
    /// <summary>
    /// If percentage, it'll just multiply the amount by the baseValue of the stat, but if not, then it'll just add/sub
    /// the amount.
    /// </summary> 
    public enum StatAffectType { Additive, Multiplicative, }
    public enum ResourceAffectType { Additive, Percentage, MaximumPercentage, } 
    
    public enum HealthAffectType
    {
        True, Heal,
        Physical = 16, Magical,
    } 
}