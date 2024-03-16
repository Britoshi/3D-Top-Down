using UnityEngine;
using System;
using Unity.VisualScripting;


namespace Game
{
    public static class Util
    {
        public static void Spawn(object obj, Vector3 position)
        {
            throw new NotImplementedException();
        }
        public static void SpawnOnHitObject(OnHitAffect affect, Vector3 position)
        {

            throw new NotImplementedException();
        }

        public static Array EnumToArray(Type e)
        {
            return Enum.GetValues(e);
        }
        public static T Clone<T>(this T self) where T : ScriptableObject
        {
            return UnityEngine.GameObject.Instantiate(self);
        }
    }

    
}