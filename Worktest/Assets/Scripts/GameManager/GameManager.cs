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

    [SerializeField]
    Text timeText;
    [SerializeField]
    GameObject rockDisplay;
    [SerializeField]
    GameObject coinDisplay;
    [SerializeField]
    GameObject endScreen;

    [SerializeField]
    HeroMovementController heroRef;
    [SerializeField]
    RockSpawner enemyRef;

    LevelConfigs configs;

    GameObject coinContainer;
    List<GameObject> coinPool;
    [SerializeField]
    int coinPoolSize;

    float curentTime = 99f;
    int rockScore = 0;
    int coinScore = 0;

    bool gameEnded = false;

    LevelDifficulty levelDiff;
    private void Start()
    {
        levelDiff = LevelDifficulty.Instance;
        configs = GetComponent<LevelConfigs>();
        scoringZone.Score += RockScore;
        curentTime = configs.levelDuration[levelDiff.Difficulty];
        StartCoinPool();
    }
    private void Update()
    {
        if (!gameEnded)
        {
            if (curentTime > 0)
            {
                curentTime -= Time.deltaTime;
                timeText.text = curentTime.ToString("00");
            }
            else
            {
                EndLevel();
            }
        }        
    }

    private void EndLevel()
    {
        enemyRef.gameEnded = true;
        heroRef.gameEnded = true;
        gameEnded = true;

        rockDisplay.SetActive(false);
        coinDisplay.SetActive(false);
        timeText.transform.parent.gameObject.SetActive(false);

        endScreen.SetActive(true);
        endScreen.GetComponent<EndLevelScreen>().SetScree(rockScore, coinScore);


    }

    private void StartCoinPool()
    {
        coinContainer = new GameObject();
        coinContainer.name = "CoinContainer";
        coinPool = new List<GameObject>();
        for (int i = 0; i < coinPoolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab, coinContainer.transform);
            coin.SetActive(false);
            coin.GetComponent<Coin>().Spawn(coinDisplay.GetComponentInChildren<Image>().gameObject);
            coin.GetComponent<Coin>().Score += CoinScore;
            coinPool.Add(coin);
        }
    }  
    private Coin GetACoin()
    {
        foreach (GameObject coin in coinPool)
        {
            if (!coin.activeInHierarchy)
            {
                coin.SetActive(true);
                return coin.GetComponent<Coin>();
            }
        }
        GameObject newCoin = Instantiate(coinPrefab, coinContainer.transform);
        newCoin.GetComponent<Coin>().Spawn(coinDisplay.GetComponentInChildren<Image>().gameObject);
        coinPool.Add(newCoin);
        newCoin.GetComponent<Coin>().Score += CoinScore;
        newCoin.SetActive(true);
        return newCoin.GetComponent<Coin>();
    }
    private void CoinScore()
    {
        coinScore ++;
        coinDisplay.GetComponentInChildren<Text>().text = "x " + coinScore.ToString("00");
    }
    private void RockScore()
    {
        rockScore++;
        rockDisplay.GetComponentInChildren<Text>().text = "x " + rockScore.ToString("00");
        if(rockScore%configs.rocksToSpawnCoin[levelDiff.Difficulty] == 0)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        float spawnX = -15.0f;
        do
        {
            if(heroRef.transform.position.x + configs.coinMinDistanceToHero[levelDiff.Difficulty]> heroRef.RightLimit-1 && 
                heroRef.transform.position.x - configs.coinMinDistanceToHero[levelDiff.Difficulty] < heroRef.LeftLimit + 1)
            {
                Debug.LogError("Cannot Spawn coin because the min spawn distance is too high");
                return;
            }
            spawnX = UnityEngine.Random.Range(heroRef.LeftLimit + 1, heroRef.RightLimit - 1);

        } while(spawnX < heroRef.transform.position.x + configs.coinMinDistanceToHero[levelDiff.Difficulty] &&
                spawnX > heroRef.transform.position.x - configs.coinMinDistanceToHero[levelDiff.Difficulty]);

        Coin coin = GetACoin();
        coin.Respawn(spawnX, configs.coinLifespan[levelDiff.Difficulty]);
    }

}
