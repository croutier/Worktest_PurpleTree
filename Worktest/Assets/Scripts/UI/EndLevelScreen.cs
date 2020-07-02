using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndLevelScreen : MonoBehaviour
{
    [SerializeField]
    Text rockCounter;
    [SerializeField]
    Text coinCounter;
    public void SetScree(int rockScore, int coinScore)
    {
        rockCounter.text = "x " + rockScore.ToString("00");
        coinCounter.text = "x " + coinScore.ToString("00");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
