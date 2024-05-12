using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MoveControll : MonoBehaviour
{

    public static MoveControll Instante;
    public float SweepSpeed = 5f, MoneySweepSpeed = 5f;
    public float ForwardMoveSpeed = 5f;
    public Transform FirstMoney;
    public bool IsMove = true;

    Joystick joystick;
    [SerializeField] CinemachineVirtualCamera FightAreaCam;
    private void Awake()
    {
        Instante = this;

        joystick = FindObjectOfType<Joystick>();
        joystick.gameObject.SetActive(false);
    }
    void Start()
    {
       
    }
    public void FirstMoneyFunc(Transform _money)
    {
        FirstMoney = _money;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && IsMove)
        {
            float PosX = Input.GetAxis("Mouse X") * Time.deltaTime * SweepSpeed;
            float MoneyX = Input.GetAxis("Mouse X") * Time.deltaTime * MoneySweepSpeed;
            if (FirstMoney != null)
            {
                FirstMoney.transform.localPosition += new Vector3(MoneyX, 0, 0);
                FirstMoney.transform.localPosition = new Vector3(Mathf.Clamp(FirstMoney.transform.localPosition.x, -0.5f, 0.5f), FirstMoney.transform.localPosition.y, FirstMoney.transform.localPosition.z);
            }
                

            transform.position += new Vector3(PosX, 0, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.85f,6.7f), transform.position.y, transform.position.z);
            transform.Translate(Vector3.forward * ForwardMoveSpeed * Time.deltaTime);
        }
    }
    //private void FixedUpdate()
    //{
    //    if (Input.GetMouseButton(0) && IsMove)
    //    {
    //        transform.Translate(Vector3.forward * ForwardMoveSpeed * Time.fixedDeltaTime);

    //    }
    //}
    public GameObject finishWall;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            finishWall = other.gameObject;
            joystick.gameObject.SetActive(true);
            GetComponent<JoystickControll>().enabled = true;
            FightAreaCam.Priority = 2;
            SpawnCharacter.Instante.FollowToCommander();
            MoneyStack.Instante.DoneButton.SetActive(true);
            GetComponent<MoveControll>().enabled = false;
        }
    }
}
