using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] float Speed = 5f;
    public Transform ParentGun;

    public bool Arrow = false;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        rigidbody.velocity = Vector3.zero;
        GetComponent<TrailRenderer>().enabled = true;
    }
    private void OnDisable()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ParentGun != null)
        {
            if (!Arrow)
            {
                rigidbody.velocity = (ParentGun.transform.forward * Speed);
            }
            else if (Arrow)
            {
                rigidbody.velocity = (ParentGun.transform.up * Speed);
            }
        }
    }
}
