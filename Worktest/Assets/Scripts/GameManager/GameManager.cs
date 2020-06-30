using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    Easy,
    Medim,
    Hard
}

public class GameManager : MonoBehaviour
{
    public static Difficulty CurrentDificiulty;
    [SerializeField]
    Difficulty difficulty = Difficulty.Medim;
    [SerializeField]
    ScoringZone scoring;
    [SerializeField]
    GameObject coinPrefab;


    int rockScore = 0;
    int coin = 0;

    private void Awake()
    {
        CurrentDificiulty = difficulty;
    }
    private void Start()
    {
        scoring.Score += RockScore;
    }
    
    public void ChangeDifficulty(Difficulty newDificulty)
    {
        CurrentDificiulty = newDificulty;
    }

    private void RockScore()
    {
        Debug.Log("POINTO!!");

    }
    private void OnDestroy()
    {
        scoring.Score -= RockScore;
    }
}
