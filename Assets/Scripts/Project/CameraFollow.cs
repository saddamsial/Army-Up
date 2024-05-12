using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed = 5f;
    Vector3 Distance, Offset;
    void Start()
    {
        Offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Distance = target.position + Offset;
        transform.position = Vector3.MoveTowards(transform.position, Distance, Time.deltaTime * FollowSpeed);
    }
}
