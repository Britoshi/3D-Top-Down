using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text text;
    public static DebugText instance;

    private void Awake()
    { 
        instance = this;
        
    }

    public static void Log(params object[] msgs)
    {
        string output = msgs[0].ToString();
        for (int i = 1; i < msgs.Length; i++)
        {
            output += " " + msgs[i].ToString();
        }
        output += ".\n";
        instance.text.text = output;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
