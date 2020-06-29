using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject rockPrefab;
    [SerializeField]
    GameObject goal;

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
    }
    

    private void Update()
    {
        
    }

    public void SpawnRock()
    {

    }
    
}
