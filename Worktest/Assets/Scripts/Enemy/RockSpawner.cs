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

    [SerializeField]
    BoxCollider2D bounceBoxCol;
    [SerializeField]
    bool DrawHint = true;

    List<GameObject> rockPool;
    int poolSize = 5;

    float bounceBoxTopY = 0.0f;

    public float minThrowAngle = 30.0f;    
    public float maxThrowAngle = 50.0f;
    public float minStrength = 1;
    public float maxStrength = 8;
    public float minTimeBetweenSpawns = 5;
    public float maxTimeBetweenSpawns = 15;

    float nextSpawnTime = 2.0f;

    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        bounceBoxTopY = bounceBoxCol.bounds.center.y + bounceBoxCol.bounds.size.y / 2;
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
        RockPhysics rock = GetARock();
        float landingX = PredictLandingX(throwVector, rock.Gravity);
        rock.Throw(rockSpawPoint.position,throwVector, landingX);
    }

    private float PredictLandingX(Vector2 throwVector, float gravity)
    {
        float landing = rockSpawPoint.position.x;

        float initialSpeed = throwVector.y;        
        float goingUpTime = (initialSpeed / (gravity));
        float maxY = rockSpawPoint.position.y + ((initialSpeed / 2) * goingUpTime);
        float goingDownTime = Mathf.Sqrt((maxY - bounceBoxTopY) / (gravity / 2));
        float totalTime = goingDownTime + goingUpTime;
        landing += throwVector.x*totalTime;
        return landing;
    }

    private void OnDrawGizmos()
    {
        if (DrawHint)
        {
            float realisticScale = 0.5f;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(rockSpawPoint.position, rockSpawPoint.position + new Vector3(Mathf.Cos(minThrowAngle * Mathf.Deg2Rad), Mathf.Sin(minThrowAngle * Mathf.Deg2Rad), 0) * maxStrength * realisticScale);
            Gizmos.DrawLine(rockSpawPoint.position, rockSpawPoint.position + new Vector3(Mathf.Cos(maxThrowAngle * Mathf.Deg2Rad), Mathf.Sin(maxThrowAngle * Mathf.Deg2Rad), 0) * maxStrength * realisticScale);

            // -5 , 0
            bounceBoxTopY = bounceBoxCol.bounds.center.y + bounceBoxCol.bounds.size.y / 2;
            float gravity = rockPrefab.GetComponent<RockPhysics>().Gravity;
            float minLandX = PredictLandingX(new Vector2(Mathf.Cos(minThrowAngle * Mathf.Deg2Rad), Mathf.Sin(minThrowAngle * Mathf.Deg2Rad)) * minStrength, gravity);
            float maxLandX = PredictLandingX(new Vector2(Mathf.Cos(maxThrowAngle * Mathf.Deg2Rad), Mathf.Sin(maxThrowAngle * Mathf.Deg2Rad)) * maxStrength, gravity);
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector2(minLandX, -5), new Vector2(minLandX, 0));
            Gizmos.DrawLine(new Vector2(maxLandX, -5), new Vector2(maxLandX, 0));

            Vector2 lastPos = rockSpawPoint.position;
            Vector2 promedyVector = new Vector2(Mathf.Cos((minThrowAngle + ((maxThrowAngle - minThrowAngle) / 2)) * Mathf.Deg2Rad), Mathf.Sin((minThrowAngle + ((maxThrowAngle - minThrowAngle) / 2)) * Mathf.Deg2Rad)) * (minStrength + ((maxStrength - minStrength) / 2));
            float goingUpTime = (promedyVector.y / (gravity));            
            float maxY = lastPos.y + ((promedyVector.y / 2) * goingUpTime);
            print(maxY);
            float goingDownTime = Mathf.Sqrt((maxY - -4.5f) / (gravity / 2));
            print(goingDownTime);
            print((maxY - -4.5f));
            float totalTime = goingUpTime + goingDownTime;            
            for (int i = 0; i < 10; i++)
            {
                float currentTime = (totalTime / 10) * (i+1);
                Vector2 currentPos = new Vector2(rockSpawPoint.position.x + promedyVector.x* currentTime, rockSpawPoint.position.y + (promedyVector.y * currentTime - (gravity/2 * Mathf.Pow(currentTime,2))));
                Gizmos.DrawLine(lastPos, currentPos);  
                lastPos = currentPos;
            }
        }    
    }


}
