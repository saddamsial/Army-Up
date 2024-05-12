using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControll : MonoBehaviour
{
    public static JoystickControll Instante;

     public Joystick joystick;
    Rigidbody rigidbody;
    [SerializeField] float MoveSpeed = 15f, RotateSpeed = 350f;
    private void Awake()
    {  
        Instante = this;
        rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        joystick = FindObjectOfType<Joystick>();
        joystick.gameObject.SetActive(true);
        joystick.input = Vector2.zero;
        joystick.handle.anchoredPosition = Vector2.zero;
        FirstClick = true;
        Touching = true;
        GetComponent<CapsuleCollider>().isTrigger = false;
    }
    private void OnDisable()
    {
        joystick.gameObject.SetActive(false);
    }
    void Start()
    {
        //joystick.gameObject.SetActive(true);
    }
    bool Touching = false,FirstClick=true;
   
    // Update is called once per frame
    void Update()
    {
       
        if(Input.GetMouseButton(0) &&(Mathf.Abs(joystick.Horizontal)>=0.1f ||Mathf.Abs( joystick.Vertical)>=0.1f))
        {
            Vector3 rotate= new Vector3(joystick.Horizontal, 0, joystick.Vertical) * Time.deltaTime * RotateSpeed;
            Quaternion LookPos = Quaternion.LookRotation(rotate, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LookPos, Time.deltaTime * RotateSpeed);
            rigidbody.MovePosition(transform.position+transform.forward * Time.deltaTime * MoveSpeed);
            // transform.position+=transform.forward * Time.deltaTime * MoveSpeed;
            rigidbody.isKinematic = false;
            Touching = true;
            if(FirstClick)
            {
                FirstClick = false;
                SpawnCharacter.Instante.StopFollow(Touching);
            }
            

        }
        if(Input.GetMouseButtonUp(0))
        {
            rigidbody.isKinematic = true;
            Touching = false;
            FirstClick = true;
            SpawnCharacter.Instante.StopFollow(Touching);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            other.gameObject.SetActive(false);
            MoveControll.Instante.finishWall.GetComponent<BoxCollider>().isTrigger = false;

        }
    }
}
