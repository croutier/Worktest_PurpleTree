using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovementController : MonoBehaviour
{
    enum MoveState
    {
        movingRight,
        movingLeft,
        idle
    }
    [SerializeField]
    GameObject smokePrefab;
    Smoke smoke;
    [SerializeField]
    float smokeOffset = 0.4f;

    [SerializeField] 
    float topSpeed = 10.0f;
    [SerializeField]
    bool inertia = false;
    [SerializeField]
    float inertiaAcceleration = 2.0f;
    float currentSpeed = 0.0f;

    int touchInput = 0; // -1 Left , 1 Right , 0 none
    [SerializeField]
    float moveLimitOffset = 2.0f;
    float rightLimit = 0.0f;
    public float RightLimit { get { return rightLimit; } }
    float leftLimit = 0.0f;
    public float LeftLimit { get { return leftLimit; } }
    MoveState currentMoveState = MoveState.idle;

    Animator anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        //I use the camera bouds here so its easier to scale the lvl,but I dont like too much this this solution
        CameraFollow.CameraBounds bounds = Camera.main.gameObject.GetComponent<CameraFollow>().Boundaries;
        rightLimit = bounds.right - moveLimitOffset;
        leftLimit = bounds.left + moveLimitOffset;
        smoke = Instantiate(smokePrefab, transform).GetComponent<Smoke>();
    }

    void Update()
    {
        DetectTouch();
        MoveLogic();        
    }

    private void MoveLogic()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || touchInput == 1)
        {
            if (currentMoveState != MoveState.movingRight)
            {
                anim.SetBool("Moving", true);
                anim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                SpawnSmoke(-1);
            }
            currentMoveState = MoveState.movingRight;
            if (inertia)
            {
                currentSpeed += inertiaAcceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -topSpeed, topSpeed);
            }
            else
            {
                currentSpeed = topSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.A) || touchInput == -1)
        {
            if (currentMoveState != MoveState.movingLeft)
            {
                anim.SetBool("Moving", true);
                anim.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                SpawnSmoke(1);
            }
            currentMoveState = MoveState.movingLeft;
            if (inertia)
            {
                currentSpeed -= inertiaAcceleration * Time.deltaTime;
                currentSpeed =  Mathf.Clamp(currentSpeed, -topSpeed, topSpeed);
            }
            else
            {
                currentSpeed = -topSpeed;
            }
        }
        else
        {
            if (inertia)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, inertiaAcceleration * Time.deltaTime);
                currentSpeed = Mathf.Clamp(currentSpeed, -topSpeed, topSpeed);
            }
            else
            {
                currentSpeed = 0;
            }
            if (currentSpeed == 0)
            {
                anim.speed = 0.5f;
                anim.SetBool("Moving", false);
                currentMoveState = MoveState.idle;
            }
        }
        Move();
    }

    private void SpawnSmoke(int direction)
    {
        smoke.Spawn(smokeOffset * direction,(direction>0));
    }

    private void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            Vector2 pos = Input.GetTouch(0).position;
            if (pos.x > Screen.width / 2)
            {
                touchInput =  1;
            }
            else
            {
                touchInput = -1;
            }

        }
        else
        {
            touchInput = 0;
        }
    }

    private void Move()
    {
        if (currentSpeed != 0)
        {
            anim.speed = Mathf.Clamp(Math.Abs(currentSpeed)/ topSpeed,0.2f,1);
            transform.position = new Vector3(transform.position.x + currentSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            transform.position = new Vector3
               (
                   Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                   transform.position.y,
                   transform.position.z
               );
        }        
    }
}
