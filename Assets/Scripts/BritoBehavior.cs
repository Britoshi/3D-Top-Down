using System;
using UnityEngine;

#pragma warning disable IDE1006 // Naming Styles
namespace Game
{ 
    public class BritoBehavior : MonoBehaviour
    {

        public static void Log(string message)
        {
            //Debug.Log(message);
			Console.WriteLine(message);
        }
        public static void print(params object[] messages)
        {
            string output = "";
            foreach (object message in messages)
            {
                output += message != null ? message.ToString() : "NULL";
                output += " ";
            }
            Log(output);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles