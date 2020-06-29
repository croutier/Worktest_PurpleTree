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
        if(nextSpawnTime<= 0)
        {
            anim.SetTrigger("Throw");
            nextSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        }
        else
        {
            nextSpawnTime -= Time.deltaTime;
        }
    }

    public void SpawnRock()
    {
        float angle = Random.Range(minThrowAngle, maxThrowAngle)*Mathf.Deg2Rad;
        float strength = Random.Range(minStrength, maxStrength);
        //trigonometry, cos*hyp = adj = x , sen * hyp = opp = y
        Vector2 throwVector = new Vector2(Mathf.Cos(angle) * strength, Mathf.Sin(angle) * strength);
        GameObject rock = Instantiate(rockPrefab, rockSpawPoint.position, new Quaternion());
        rock.GetComponent<RockPhysics>().Spawn(throwVector, goal);
    }
    
}
