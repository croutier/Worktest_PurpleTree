using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    ScoringZone scoring;

    int rockScore = 0;

    private void Start()
    {
        scoring.Score += RockScore;
    }

    private void RockScore()
    {
        Debug.Log("POINTO!!");

    }
}
