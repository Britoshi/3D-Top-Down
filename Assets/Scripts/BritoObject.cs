using System;

public class BritoObject : object
{
    public static void Log(string message)
    {
        //Debug.Log(message);
        UnityEngine.Debug.Log(message);
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