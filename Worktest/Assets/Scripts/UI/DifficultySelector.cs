using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField]
    Image target;
    [SerializeField]
    Text difText;

    [SerializeField]
    Color easyColor;
    [SerializeField]
    Color mediumColor;
    [SerializeField]
    Color hardColor;

    int currentDif = 0;
    LevelDifficulty difficuty;    
    private void Start()
    {
        difficuty = LevelDifficulty.Instance;
    }

    public void ChangeDifficulty()
    {
        currentDif++;
        currentDif = currentDif % 3;
        switch (currentDif)
        {
            case 0:
                target.color = easyColor;
                difText.text = "Easy";
                difficuty.SetDifficulty(Difficulty.Easy);
                break;
               
            case 1:
                target.color = mediumColor;
                difText.text = "Medium";
                difficuty.SetDifficulty(Difficulty.Medim);
                break;
                
            case 2:
                target.color = hardColor;
                difText.text = "Hard";
                difficuty.SetDifficulty(Difficulty.Hard);
                break;
        }
    }
}
