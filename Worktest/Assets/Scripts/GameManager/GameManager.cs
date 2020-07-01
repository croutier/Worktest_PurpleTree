using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(LevelConfigs))]
public class GameManager : MonoBehaviour
{    
    [SerializeField]
    ScoringZone scoringZone;
    [SerializeField]
    GameObject coinPrefab;

    LevelConfigs configs;


    List<GameObject> coinPool;
    [SerializeField]
    int coinPoolLength;

    float curentTime = 99f;
    int rockScore = 0;
    int coinScore = 0;
    
    private void Start()
    {
        configs = GetComponent<LevelConfigs>();
        scoringZone.Score += RockScore;
        curentTime = configs.levelDuration[LevelDifficulty.Instance.Difficulty];
        StartCoinPool();
    }

    private void StartCoinPool()
    {
        throw new NotImplementedException();
    }
    private void CoinScore()
    {

    }
    private void RockScore()
    {
        Debug.Log("POINTO!!");

    }
    private void OnDestroy()
    {
        scoringZone.Score -= RockScore;
    }
}
