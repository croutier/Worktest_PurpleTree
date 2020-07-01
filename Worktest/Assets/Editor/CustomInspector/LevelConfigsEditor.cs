using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LevelConfigs))]
public class LevelConfigsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelConfigs myScript = (LevelConfigs)target;
        SerializedObject serializedObj = new SerializedObject(myScript);

        DrawPropertiesExcluding(serializedObj, new string[] { "levelDuration", "rocksToSpawnCoin", "coinLifespan"});
        EditorGUILayout.FloatField("Level duration: ", myScript.levelDuration[(int)myScript.difficultySetting]);
        EditorGUILayout.FloatField("Rocks to spawn a coin: ", myScript.rocksToSpawnCoin[(int)myScript.difficultySetting]);
        EditorGUILayout.FloatField("Coin lifespan: ", myScript.coinLifespan[(int)myScript.difficultySetting]);

        serializedObj.ApplyModifiedProperties();
    }
}
#endif
