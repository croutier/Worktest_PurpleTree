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

        DrawPropertiesExcluding(serializedObj, new string[] { "levelDuration", "rocksToSpawnCoin", "coinLifespan","coinMinDistanceToHero"});
        myScript.levelDuration[(int)myScript.difficultySetting] = EditorGUILayout.FloatField("Level duration: ", myScript.levelDuration[(int)myScript.difficultySetting]);
        myScript.rocksToSpawnCoin[(int)myScript.difficultySetting] = EditorGUILayout.FloatField("Rocks to spawn a coin: ", myScript.rocksToSpawnCoin[(int)myScript.difficultySetting]);
        myScript.coinLifespan[(int)myScript.difficultySetting] = EditorGUILayout.FloatField("Coin lifespan: ", myScript.coinLifespan[(int)myScript.difficultySetting]);
        myScript.coinMinDistanceToHero[(int)myScript.difficultySetting] = EditorGUILayout.FloatField("Coin min distance to Hero: ", myScript.coinMinDistanceToHero[(int)myScript.difficultySetting]);        
        serializedObj.ApplyModifiedProperties();
    }
}
#endif
