using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPhysics : MonoBehaviour
{
    [SerializeField]
    float gravity = 9.8f;
    [SerializeField]
    GameObject indicatorPrefab;
    
    GameObject indicator;
    GameObject goal;
    Vector2 moveVector;
    float minSpeed = 0.1f;
    public bool move = true;
    CustomPhysics physics;

    
    public void Spawn(GameObject goal)
    {
        this.goal = goal; 
        indicator = Instantiate(indicatorPrefab);
        physics = CustomPhysics.Instance;
    }

    public void Throw(Vector3 spawnPos , Vector2 mov)
    {
        transform.position = spawnPos;
        move = true;
        moveVector = mov;
    }

    // Update is called once per frame
    void Update()
    {
        bool gointUp = false;
        if (moveVector.y > 0)
        {
            gointUp = true;
            if (moveVector.y < 0)
            {
                print(transform.position.y);
            }
        }
        if (move)
        {

            moveVector += gravity*Time.deltaTime * Vector2.down;
            
            if (moveVector.y < 0 && gointUp)
            {
                //print(transform.position.y);
            }
            CheckCollisions();
            transform.position = transform.position + (new Vector3(moveVector.x, moveVector.y, 0) * Time.deltaTime);
        }        
    }

    private void CheckCollisions()
    {
        Vector2 center = GetComponent<BoxCollider2D>().bounds.center;
        Vector2 offset = GetComponent<BoxCollider2D>().bounds.size;
        //Vector2[] horizontalOrigns = new Vector2[3];
        Vector2[] verticalOrigns = new Vector2[3];

        for (int i = 0; i < 3; i++)
        {
            //horizontalOrigns[i] = center + new Vector2(((moveVector.x > 0 ? offset.x : -offset.x) / 2), (offset.y / 3 - (offset.y / 3) * i));
            verticalOrigns[i] = center + new Vector2((offset.x / 3 - (offset.x / 3) * i), (moveVector.y > 0 ? offset.y : -offset.y) / 2);
        }        
        
        //BoxCollider2D horizontalCollision = physics.CustomLinearRaycast2D(horizontalOrigns, moveVector * Vector2.right * Time.deltaTime);
        BoxCollider2D verticalCollision = physics.CustomLinearRaycast2D(verticalOrigns, moveVector * Vector2.up * Time.deltaTime);
        /*if(horizontalCollision!= null)
        {
            //Debug.Log("horizontal collision with " + horizontalCollision.gameObject.name);
            moveVector.x = moveVector.x*-1f;            
            if(moveVector.magnitude< minSpeed)
            {
                move = false;
            }
        }*/
        if (verticalCollision != null)
        {
            if(verticalCollision.TryGetComponent(out BouncingBox box))
            {
                BouncingOnTheBox(box);                
            }
            else if(verticalCollision.TryGetComponent(out ScoringZone scoreZone))
            {
                scoreZone.ScorePoint();
                move = false;
                Deactivate();
            }
            else            
            {
                moveVector.y = moveVector.y * -1;
                //Debug.Log("vertical collision with " + verticalCollision.gameObject.name);
                moveVector *= 0.3f;
                indicator.SetActive(false);
                if (moveVector.magnitude < minSpeed)                                                                         
                {                                                                                                            
                    move = false;                                                                                            
                    Deactivate();  
                }                                                                                                            
            }                                                                                                                
        }                                                                                                                    
    }
    private void Deactivate()                                                                                                
    {
        gameObject.SetActive(false);
        indicator.SetActive(false);
    }

    private void BouncingOnTheBox(BouncingBox box)
    {
        moveVector.y = moveVector.y * -box.verticalBounciness;
        moveVector.x = moveVector.x * box.horizontalBounciness;
        float nextBounce = GetNextBouncingSpot();
        if(transform.position.x + nextBounce > goal.transform.position.x - 1)
        {
            indicator.SetActive(false);
            AutoAim();
        }
        else
        {
            indicator.SetActive(true);
            indicator.transform.position = new Vector2(transform.position.x + nextBounce, indicator.transform.position.y);
        }
    }   

    //asuming that the current pos in y is equal to where it has to bounce again and that the moveVector y is going upwards
    public float GetNextBouncingSpot()
    {
        float initialSpeed = moveVector.y;
        //time = speed/acceleration*2 because of its constant acceleration
        float time = (initialSpeed / (gravity))*2;        
        //so the x distansce = time * moveVector.x because it didnt have acceleration
        return time * moveVector.x;
    }
    private void AutoAim()
    {
        float initialSpeed = moveVector.y;
        //same as in next bounce
        float goingUpTime = (initialSpeed / (gravity));
        //get max hight using promedy velocity
        float maxY = transform.position.y +((initialSpeed/2) * goingUpTime);
        if(maxY < goal.transform.position.y + 0.5f)
        {
            maxY = goal.transform.position.y + 0.5f;
            float yDif = maxY - transform.position.y;
            goingUpTime = Mathf.Sqrt(yDif/  (gravity/2));
            initialSpeed = gravity * goingUpTime;
        }
        float goingDownTime = Mathf.Sqrt((maxY - goal.transform.position.y) / (gravity / 2));
        float totalTime = goingDownTime + goingUpTime;
        float xDistance = goal.transform.position.x - transform.position.x;        
        moveVector = new Vector2(xDistance / totalTime, initialSpeed);
    }
}
