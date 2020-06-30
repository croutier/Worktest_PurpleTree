using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject rockPrefab;
    [SerializeField]
    Transform rockSpawPoint;
    [SerializeField]
    GameObject goal;

    List<GameObject> rockPool;
    int poolSize = 5;

    public float minThrowAngle = 30.0f;    
    public float maxThrowAngle = 50.0f;
    public float minStrength = 1;
    public float maxStrength = 8;
    public float minTimeBetweenSpawns = 5;
    public float maxTimeBetweenSpawns = 15;

    float nextSpawnTime = 3.0f;

    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        GeneratePool();
    }

    private void GeneratePool()
    {
        rockPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rock = Instantiate(rockPrefab);
            rock.GetComponent<RockPhysics>().Spawn(goal);
            rockPool.Add(rock);
        }
    }

    private RockPhysics GetARock()
    {
        foreach(GameObject rock in rockPool)
        {
            if (!rock.activeInHierarchy)
            {
                rock.SetActive(true);
                return rock.GetComponent<RockPhysics>();
            }
        }
        GameObject newRock = Instantiate(rockPrefab);
        newRock.GetComponent<RockPhysics>().Spawn(goal);        
        rockPool.Add(newRock);
        newRock.SetActive(true);
        return newRock.GetComponent<RockPhysics>();
    }

    private void Update()
    {
        if(nextSpawnTime<= 0)
        {
            anim.SetTrigger("Throw");
            nextSpawnTime = UnityEngine.Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        }
        else
        {
            nextSpawnTime -= Time.deltaTime;
        }
    }
    //this could use a pool, but the spawn ratio is too low
    public void SpawnRock()
    {
        float angle = UnityEngine.Random.Range(minThrowAngle, maxThrowAngle)*Mathf.Deg2Rad;
        float strength = UnityEngine.Random.Range(minStrength, maxStrength);
        //trigonometry, cos*hyp = adj = x , sen * hyp = opp = y
        Vector2 throwVector = new Vector2(Mathf.Cos(angle) * strength, Mathf.Sin(angle) * strength);        
        GetARock().Throw(rockSpawPoint.position,throwVector);
    }
    
}
