using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;


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

        public static Vector3Int ToChunkCoordinate(this Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x / Chunk.UNIT_SIZE);
            int y = Mathf.FloorToInt(position.y / Floor.HEIGHT);
            int z = Mathf.FloorToInt(position.z / Chunk.UNIT_SIZE);
            return new(x, y, z);
        }
        public static void DestroyChildren(this RectTransform transform)
        {
            if (transform.childCount == 0) return;
            foreach (Transform child in transform)
                UnityEngine.Object.Destroy(child.gameObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasState(this Animator anim, string stateName, int layer = 0)
        { 
            int stateID = Animator.StringToHash(stateName);
            return anim.HasState(layer, stateID);

        }
    }

    
}