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
}
