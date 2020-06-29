using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

[CustomEditor(typeof(RockSpawner))]
public class RockSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RockSpawner myScript = (RockSpawner)target;
        SerializedObject serializedObj = new SerializedObject(myScript);
        GUIStyle customGUIStyle = new GUIStyle();
        customGUIStyle.fontSize = 16;

        DrawPropertiesExcluding(serializedObj, new string[] { "minThrowAngle", "maxThrowAngle", "minStrength", "maxStrength", "minTimeBetweenSpawns", "maxTimeBetweenSpawns" });
        GUILayout.Label("");
        EditorGUILayout.LabelField("Angle",style: customGUIStyle);       

        EditorGUILayout.LabelField("Min:", myScript.minThrowAngle.ToString("#.#") +"°");
        EditorGUILayout.LabelField("Max:", myScript.maxThrowAngle.ToString("#.#") + "°");
        EditorGUILayout.MinMaxSlider(ref myScript.minThrowAngle, ref myScript.maxThrowAngle, 15.0f, 75.0f);

        EditorGUILayout.LabelField("Strength", style: customGUIStyle);
        EditorGUILayout.LabelField("Min:", myScript.minStrength.ToString("#.#"));
        EditorGUILayout.LabelField("Max:", myScript.maxStrength.ToString("#.#"));
        EditorGUILayout.MinMaxSlider(ref myScript.minStrength, ref myScript.maxStrength, 1, 10);

        EditorGUILayout.LabelField("Time between spawns", style: customGUIStyle);
        EditorGUILayout.LabelField("Min:", myScript.minTimeBetweenSpawns.ToString("#.#") +" s");
        EditorGUILayout.LabelField("Max:", myScript.maxTimeBetweenSpawns.ToString("#.#") + " s");
        EditorGUILayout.MinMaxSlider(ref myScript.minTimeBetweenSpawns, ref myScript.maxTimeBetweenSpawns, 1, 30);

        serializedObj.ApplyModifiedProperties();
    }
}
#endif