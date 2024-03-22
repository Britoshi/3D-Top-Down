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
            var clone = UnityEngine.Object.Instantiate(self);
            clone.name = self.name;
            return clone;
        }
        public static bool TagContains(this GameObject gameObject, string substring)
        {
                return  gameObject.tag.Contains(substring);
        }

        public static void DestroyChildren(this Transform transform)
        {
            if (transform.childCount == 0) return;
            foreach(Transform child in transform)
                UnityEngine.Object.Destroy(child.gameObject); 
        }
        public static void DestroyChildren(this RectTransform transform)
        {
            if (transform.childCount == 0) return;
            foreach (Transform child in transform)
                UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    
}