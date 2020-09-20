using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(World))]
[CanEditMultipleObjects]
public class WorldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        World world = (World)target;
        if (GUILayout.Button("Generate World")) {
            world.GenerateWorld();
        }
        if (GUILayout.Button("Clear World"))
        {
            world.ClearWorld();
        }
    }


}
