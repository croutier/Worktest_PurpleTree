using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfigs : MonoBehaviour
{
    public Difficulty difficultySetting;

    public float[] levelDuration = new float[] { 0, 0, 0 };
    public float[] rocksToSpawnCoin = new float[] { 0, 0, 0 };
    public float[] coinLifespan = new float[] { 0, 0, 0 };
}
