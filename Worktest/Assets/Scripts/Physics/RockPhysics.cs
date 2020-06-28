using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPhysics : MonoBehaviour
{
    Vector2 moveVector;
    [SerializeField]
    float gravity = 9.8f;

    float minSpeed = 0.1f;
    public bool move = true;
    // Start is called before the first frame update
    CustomPhysics physics;

    private void Start()
    {
        physics = CustomPhysics.Instance;
        //moveVector = new Vector2(0,-1);
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            moveVector += gravity/100 * Vector2.down;
            CheckCollisions();
            transform.position = transform.position + (new Vector3(moveVector.x, moveVector.y, 0) * Time.deltaTime);
        }        
    }

    private void CheckCollisions()
    {
        Vector2 center = GetComponent<BoxCollider2D>().bounds.center;
        Vector2 offset = GetComponent<BoxCollider2D>().bounds.size;
        Vector2[] horizontalOrigns = new Vector2[3];
        Vector2[] verticalOrigns = new Vector2[3];

        for (int i = 0; i < 3; i++)
        {
            horizontalOrigns[i] = center + new Vector2(((moveVector.x > 0 ? offset.x : -offset.x) / 2), (offset.y / 3 - (offset.y / 3) * i));
            verticalOrigns[i] = center + new Vector2((offset.x / 3 - (offset.x / 3) * i), (moveVector.y > 0 ? offset.y : -offset.y) / 2);
        }
        Debug.DrawLine(horizontalOrigns[0], horizontalOrigns[2]);
        Debug.DrawLine(verticalOrigns[0], verticalOrigns[2]);
        BoxCollider2D horizontalCollision = physics.CustomLinearRaycast2D(horizontalOrigns, moveVector * Vector2.right * Time.deltaTime);
        BoxCollider2D verticalCollision = physics.CustomLinearRaycast2D(verticalOrigns, moveVector * Vector2.up * Time.deltaTime);
        if(horizontalCollision!= null)
        {
            Debug.Log("horizontal collision with " + horizontalCollision.gameObject.name);
            moveVector.x = moveVector.x*-1;
            moveVector *= 0.3f;
            if(moveVector.magnitude< minSpeed)
            {
                move = false;
            }
        }
        if (verticalCollision != null)
        {
            moveVector.y = moveVector.y * -1;
            Debug.Log("vertical collision with " + verticalCollision.gameObject.name);
            moveVector *= 0.3f; 
            if (moveVector.magnitude < minSpeed)
            {
                move = false;
            }
        }
    }
}
