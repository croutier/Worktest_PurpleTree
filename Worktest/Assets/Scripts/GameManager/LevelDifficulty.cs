using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Medim,
    Hard
}
public class LevelDifficulty : MonoBehaviour
{
    private static LevelDifficulty instance = null;
    public static LevelDifficulty Instance { get { return instance; } }
    [SerializeField]
    Difficulty difficulty;
    public int Difficulty { get { return (int)difficulty; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(Difficulty dif)
    {
        difficulty = dif;
    }

}
