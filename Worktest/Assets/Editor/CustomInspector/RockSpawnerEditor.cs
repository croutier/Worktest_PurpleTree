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
        
        EditorGUILayout.LabelField("Min:", myScript.minThrowAngle[(int)myScript.difficultySetting].ToString("#.#") +"°");
        EditorGUILayout.LabelField("Max:", myScript.maxThrowAngle[(int)myScript.difficultySetting].ToString("#.#") + "°");
        EditorGUILayout.MinMaxSlider(ref myScript.minThrowAngle[(int)myScript.difficultySetting], ref myScript.maxThrowAngle[(int)myScript.difficultySetting], 15.0f, 75.0f);

        EditorGUILayout.LabelField("Strength", style: customGUIStyle);
        EditorGUILayout.LabelField("Min:", myScript.minStrength[(int)myScript.difficultySetting].ToString("#.#"));
        EditorGUILayout.LabelField("Max:", myScript.maxStrength[(int)myScript.difficultySetting].ToString("#.#"));
        EditorGUILayout.MinMaxSlider(ref myScript.minStrength[(int)myScript.difficultySetting], ref myScript.maxStrength[(int)myScript.difficultySetting], 1, 20);

        EditorGUILayout.LabelField("Time between spawns", style: customGUIStyle);
        EditorGUILayout.LabelField("Min:", myScript.minTimeBetweenSpawns[(int)myScript.difficultySetting].ToString("#.#") +" s");
        EditorGUILayout.LabelField("Max:", myScript.maxTimeBetweenSpawns[(int)myScript.difficultySetting].ToString("#.#") + " s");
        EditorGUILayout.MinMaxSlider(ref myScript.minTimeBetweenSpawns[(int)myScript.difficultySetting], ref myScript.maxTimeBetweenSpawns[(int)myScript.difficultySetting], 1, 30);

        serializedObj.ApplyModifiedProperties();
    }
}
#endif