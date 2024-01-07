using System;
using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles
namespace Game
{ 
    public class BritoBehavior : MonoBehaviour
    {

        public static void Log(string message) => BritoObject.Log(message);
        public static void print(params object[] messages) => BritoObject.print(messages);
    }
}
#pragma warning restore IDE1006 // Naming Styles