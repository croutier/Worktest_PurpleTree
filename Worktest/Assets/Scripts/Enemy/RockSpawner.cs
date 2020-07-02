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

    LevelDifficulty levelDiff;

    public Difficulty difficultySetting;

    GameObject rockContainer;
    List<GameObject> rockPool;
    int poolSize = 5;

    [HideInInspector]
    public bool gameEnded;

    float bounceBoxTopY = 0.0f;

    public float[] minThrowAngle = new float[]{0, 0,0};    
    public float[] maxThrowAngle = new float[] { 0, 0, 0 };
    public float[] minStrength = new float[] { 0, 0, 0 };
    public float[] maxStrength = new float[] { 0, 0, 0 };
    public float[] minTimeBetweenSpawns = new float[] { 0, 0, 0 };
    public float[] maxTimeBetweenSpawns = new float[] { 0, 0, 0 };

    float nextSpawnTime = 2.0f;
    

    Animator anim;
    private void Start()
    {
        levelDiff = LevelDifficulty.Instance;
        anim = GetComponent<Animator>();
        bounceBoxTopY = bounceBoxCol.bounds.center.y + bounceBoxCol.bounds.size.y / 2;
        GeneratePool();
        nextSpawnTime = UnityEngine.Random.Range(minTimeBetweenSpawns[levelDiff.Difficulty], maxTimeBetweenSpawns[levelDiff.Difficulty]);
    }
    private void Update()
    {
        if (!gameEnded)
        {
            if (nextSpawnTime <= 0)
            {
                anim.SetTrigger("Throw");
                nextSpawnTime = UnityEngine.Random.Range(minTimeBetweenSpawns[levelDiff.Difficulty], maxTimeBetweenSpawns[levelDiff.Difficulty]);
            }
            else
            {
                nextSpawnTime -= Time.deltaTime;
            }
        }
       
    }

    private void GeneratePool()
    {
        rockContainer = new GameObject();
        rockContainer.name = "RockContainer";
        rockPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rock = Instantiate(rockPrefab, rockContainer.transform);
            rock.SetActive(false);
            rock.GetComponent<RockPhysics>().Spawn(goal, rockContainer.transform);
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
        GameObject newRock = Instantiate(rockPrefab, rockContainer.transform);
        newRock.GetComponent<RockPhysics>().Spawn(goal, rockContainer.transform);        
        rockPool.Add(newRock);
        newRock.SetActive(true);
        return newRock.GetComponent<RockPhysics>();
    }
    
    public void SpawnRock()
    {
        float angle = UnityEngine.Random.Range(minThrowAngle[levelDiff.Difficulty], maxThrowAngle[levelDiff.Difficulty]) *Mathf.Deg2Rad;
        float strength = UnityEngine.Random.Range(minStrength[levelDiff.Difficulty], maxStrength[levelDiff.Difficulty]);
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
            Gizmos.DrawLine(rockSpawPoint.position, rockSpawPoint.position + new Vector3(Mathf.Cos(minThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad), Mathf.Sin(minThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad), 0) * maxStrength[(int)difficultySetting] * realisticScale);
            Gizmos.DrawLine(rockSpawPoint.position, rockSpawPoint.position + new Vector3(Mathf.Cos(maxThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad), Mathf.Sin(maxThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad), 0) * maxStrength[(int)difficultySetting] * realisticScale);

            // -5 , 0
            bounceBoxTopY = bounceBoxCol.bounds.center.y + bounceBoxCol.bounds.size.y / 2;
            float gravity = rockPrefab.GetComponent<RockPhysics>().Gravity;
            float minLandX = PredictLandingX(new Vector2(Mathf.Cos(minThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad), Mathf.Sin(minThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad)) * maxStrength[(int)difficultySetting], gravity);
            float maxLandX = PredictLandingX(new Vector2(Mathf.Cos(maxThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad), Mathf.Sin(maxThrowAngle[(int)difficultySetting] * Mathf.Deg2Rad)) * minStrength[(int)difficultySetting], gravity);
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector2(minLandX, -5), new Vector2(minLandX, 0));
            Gizmos.DrawLine(new Vector2(maxLandX, -5), new Vector2(maxLandX, 0));

            Vector2 lastPos = rockSpawPoint.position;
            Vector2 promedyVector = new Vector2(Mathf.Cos((minThrowAngle[(int)difficultySetting] + ((maxThrowAngle[(int)difficultySetting] - minThrowAngle[(int)difficultySetting]) / 2)) * Mathf.Deg2Rad), Mathf.Sin((minThrowAngle[(int)difficultySetting] + ((maxThrowAngle[(int)difficultySetting] - minThrowAngle[(int)difficultySetting]) / 2)) * Mathf.Deg2Rad)) * (minStrength[(int)difficultySetting] + ((maxStrength[(int)difficultySetting] - minStrength[(int)difficultySetting]) / 2));
            float goingUpTime = (promedyVector.y / (gravity));            
            float maxY = lastPos.y + ((promedyVector.y / 2) * goingUpTime);

            float goingDownTime = Mathf.Sqrt((maxY - -4.5f) / (gravity / 2));
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
