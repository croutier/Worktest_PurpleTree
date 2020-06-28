using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsColision : MonoBehaviour
{
    [SerializeField]
    BoxCollider2D colision;

    [SerializeField]
    string collisionID = "";

    private void Start()
    {
        CustomPhysics.Instance.UpdateCollision(collisionID, colision);
    }
}
