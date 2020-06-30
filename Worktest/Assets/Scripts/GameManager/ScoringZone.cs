using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringZone : MonoBehaviour
{
    public delegate void ScoreDelegate();
    public ScoreDelegate Score;
    
    public void ScorePoint()
    {
        if(Score!= null)
            Score();
    }
    private void OnDestroy()
    {
        if(Score!= null)
        {
            CleanDelegate();
        }
    }
    private void CleanDelegate()
    {
        Delegate[] functions = Score.GetInvocationList();
        for (int i = 0; i < functions.Length; i++)
        {
            Score -= (ScoreDelegate)functions[i];
        }
    }
}
