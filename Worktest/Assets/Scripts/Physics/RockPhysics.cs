using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPhysics : MonoBehaviour
{
    Vector2 moveVector;
    [SerializeField]
    float gravity = 9.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = new Vector2();
        moveVector += gravity * Vector2.down;
        transform.position= transform.position + (new Vector3(moveVector.x , moveVector.y,0) * Time.deltaTime);
    }
}
