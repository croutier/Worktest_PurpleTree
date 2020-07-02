using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public void Spawn(float spawnX, bool rotated)
    {
        transform.localPosition = new Vector3(spawnX, 0, 0);
        GetComponent<SpriteRenderer>().flipX = rotated;
        GetComponent<Animator>().SetTrigger("reset");
    }    
}
