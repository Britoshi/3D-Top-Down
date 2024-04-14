using UnityEngine;
using UnityEditor;
using Game;

[CustomEditor(typeof(MapProcessor))]
public class MapProcessorEditor : Editor
{
    //private SerializedProperty mapSizeProp;
    //private SerializedProperty processButtonProp;



    private void OnEnable()
    {
        // Fetch the serialized properties
        //mapSizeProp = serializedObject.FindProperty("mapSize");
        //processButtonProp = serializedObject.FindProperty("processButton");
    }

    public override void OnInspectorGUI()
    {

        // Update the serialized object
        serializedObject.Update();

        // Display default inspector property fields
        //EditorGUILayout.PropertyField(mapSizeProp);

        // Display a button to process the map
        if (GUILayout.Button("Process Map"))
        {
            // Call the ProcessMap method of the target script
            MapProcessor mapProcessor = (MapProcessor)target;
            mapProcessor.ProcessMap();
        }
        if (GUILayout.Button("Register Map"))
        {
            // Call the ProcessMap method of the target script
            MapProcessor mapProcessor = (MapProcessor)target;
            World.RegisterMap(mapProcessor.processedMap);
        }
        if (GUILayout.Button("Map Dump"))
        {
            // Call the ProcessMap method of the target script
            MapProcessor mapProcessor = (MapProcessor)target;
            World.Dump();
        }

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}