using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    GameObject objectToFollow;
    [SerializeField]
    Camera targetCamera;
    [SerializeField]
    float lerpSpeed;

    float cameraHeight;
    float cameraWidth;

    [System.Serializable]
    public struct CameraBounds
    {      
        public float top;
        public float right;
        public float bottom;
        public float left;
    }

    [SerializeField]
    CameraBounds boundaries;
    public CameraBounds Boundaries { get { return boundaries; } }

    private void Start()
    {
        cameraHeight = targetCamera.orthographicSize * 2f;
        cameraWidth = cameraHeight * targetCamera.aspect;
    }
    void Update()
    {
        Follow(objectToFollow);
    }

    private void Follow(GameObject target)
    {
        Vector3 startPos = targetCamera.transform.position;
        Vector3 targetPos = new Vector3(target.transform.position.x,target.transform.position.y,targetCamera.transform.position.z);

        targetCamera.transform.position = Vector3.Lerp(startPos, targetPos, lerpSpeed * Time.deltaTime);

        targetCamera.transform.position = new Vector3
            (
                Mathf.Clamp(targetCamera.transform.position.x, boundaries.left + cameraWidth / 2, boundaries.right - cameraWidth / 2),
                Mathf.Clamp(targetCamera.transform.position.y, boundaries.bottom + cameraHeight/2, boundaries.top - cameraHeight / 2),
                targetCamera.transform.position.z
            );
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(boundaries.left, boundaries.top), new Vector2(boundaries.right, boundaries.top));
        Gizmos.DrawLine(new Vector2(boundaries.right, boundaries.top), new Vector2(boundaries.right, boundaries.bottom));
        Gizmos.DrawLine(new Vector2(boundaries.left, boundaries.bottom), new Vector2(boundaries.right, boundaries.bottom));
        Gizmos.DrawLine(new Vector2(boundaries.left, boundaries.top), new Vector2(boundaries.left, boundaries.bottom));
    }
}
