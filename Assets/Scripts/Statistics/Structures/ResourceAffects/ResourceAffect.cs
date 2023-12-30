
using UnityEngine;
namespace Game
{

    [System.Serializable]
    public class ResourceAffect
    {
        [SerializeField] public ResourceID id;
        [SerializeField] public ResourceModifier[] resourceModifiers;

        public float GetAmount(Status source, Status target)
        {
            //GD.Print($"Calculating amount from {source} to {target}");
            float total = 0f;
            var ID = id;
            //GD.Print($"MOD:" + resourceModifiers.Count);
            void ProcessModifier(ResourceModifier modifier)
            {
                if (source == null || target == null) return;

                Resource resource = target[ID];

                if (modifier.statID == 0)
                {
                    //GD.Print("This work?");
                    switch (modifier.affectType)
                    {
                        case ResourceAffectType.Additive: total += modifier.flatAmount; break;
                        case ResourceAffectType.Percentage: total += resource * modifier.flatAmount; break;
                        case ResourceAffectType.MaximumPercentage:
                            total += resource.GetMaximumValue() * modifier.flatAmount;
                            break;
                    }
                    return;
                }
                else
                {
                    var ratio = source[modifier.statID] * modifier.statScalePercentage;
                    switch (modifier.affectType)
                    {
                        case ResourceAffectType.Additive: total += ratio; break;
                        case ResourceAffectType.Percentage: total += resource * ratio; break;
                        case ResourceAffectType.MaximumPercentage:
                            total += resource.GetMaximumValue() * ratio;
                            break;
                    }
                    total += modifier.flatAmount;
                }
            }

            if (resourceModifiers != null)
                foreach (var affect in resourceModifiers)
				{ 
                    ProcessModifier(affect);
					}
            return total;
        }
        public ResourceAffect(ResourceID id, ResourceModifier[] affectAmount)
        {
            //GD.Print("So... what 1?" + affectAmount.Count);
            this.id = id;
            resourceModifiers = affectAmount;
            //GD.Print("So... what? 2" + resourceModifiers.Count);
        }
         
    }
}
