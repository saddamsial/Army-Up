using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollow : MonoBehaviour
{
    public Transform Character;
    public Transform Character2;
    public bool Commander = false;
    Animator animator;
    public string[] AnimNames;
    public int animIndex = 0;
    public bool Attack = false;
    public bool Follow = false;
    [Header("Gun")]
    public List<GameObject> Gun = new List<GameObject>();
    public Guns ActiveGun;

    [Header("Healty")]
    public List<GameObject> Healty = new List<GameObject>();
    public int DamageCount = 0;

    [SerializeField] ParticleSystem BloodEffect;
    [SerializeField] ParticleSystem UpdateEffect;
    [SerializeField] GameObject Splash_Img;
    [Header("Soldier Level Value")]
    public int level = 1;
    public int GunLevel = 1;
    public int HealtyLevel = 1;
    void Start()
    {
        animator = GetComponent<Animator>();
        if(!Commander)
        MoneyStack.Instante.CharacterLevelIncrease(level);
    }
    public void AnimAndGunChange(int index)
    {
        
        if(index<=7)
        {
            level = 1;
            animIndex = index;
            animator = GetComponent<Animator>();
            animator.Play(AnimNames[index]);
            for (int i = 0; i < Gun.Count; i++)
            {
                Gun[i].SetActive(false);
            }
            Gun[index].SetActive(true);
            if(animIndex>=1)
            {
                GunLevel=index;
                level ++;
            }
            
            level = HealtyLevel + GunLevel;
            MoneyStack.Instante.CharacterLevelIncrease(level);
            if (index>=3)
            ActiveGun = Gun[index].GetComponent<Guns>();
            UpdateEffect.Play();
        }
        
    }
    bool LookAt = false;
    Transform Enemy;
    public void GunShoot(Transform _enemy)
    {  
        if(ActiveGun!=null)
        {
            LookAt = true;
            ActiveGun.IsShoot = true;
            Enemy = _enemy;
            StartCoroutine(ActiveGun.Shoot());
        }
        
    }
    public void IdleAnim()
    {
        animator.Play(AnimNames[animIndex]+"_Idle");
    }
    public void RunAnim()
    {
        animator.Play(AnimNames[animIndex]);
    }
    public void HealtyChange(int index,bool firstSpawn)
    {
        
        //Healty[0].SetActive(false);
        if(index<=Healty.Count)
        {
            level = 1;
            for (int i = 0; i < index; i++)
            {
                Healty[i].SetActive(true);
            }
            HealtyLevel=index;
            
            level = (HealtyLevel+1)+GunLevel;
            MoneyStack.Instante.CharacterLevelIncrease(level);
               
           
            DamageCount =level;
            UpdateEffect.Play();
        }
        
    }
    public bool PlatformFight = true;
    public void GetDamage(int levelvalue)
    {
        DamageCount-=levelvalue;
        
        if (!Commander && DamageCount<=0 && PlatformFight)
        {
            DeathEffect();
            gameObject.SetActive(false);
            SpawnCharacter.Instante.Soldiers.Remove(gameObject);
            MoneyStack.Instante.CharacterLevelDecrease(level,true);
            Destroy(gameObject);
         
        }
        if (!Commander && !PlatformFight)
        {
            DeathEffect();
            gameObject.SetActive(false);
            SpawnCharacter.Instante.Soldiers.Remove(gameObject);
            //MoneyStack.Instante.CharacterLevelDecrease(level, false);
            Destroy(gameObject);
        }

    }
    public void DestroySoldier()
    {

        DeathEffect();
        gameObject.SetActive(false);
        SpawnCharacter.Instante.Soldiers.Remove(gameObject);
        MoneyStack.Instante.CharacterLevelDecrease(level,false);
        Fight.Instante.Soldiers.Remove(gameObject);
        //Character.GetComponentInParent<FightArea>().SoldierCount(level);
        Destroy(gameObject);
    }
    void EnviromentDamage()
    {
        DeathEffect();
        
        SpawnCharacter.Instante.Soldiers.Remove(gameObject);
        SpawnCharacter.Instante.EnviromentDamageMove();
        MoneyStack.Instante.CharacterLevelDecrease(level, true);
        
        Destroy(gameObject);
    }
    void DeathEffect()
    {
        BloodEffect.transform.parent = null;
        BloodEffect.Play();
        transform.parent = null;
        Instantiate(Splash_Img, transform.position+Vector3.up*0.2f, Quaternion.Euler(90,0,0));
    }
    public void AttackAnim()
    {
        Attack = true;
        
        animator.Play(AnimNames[animIndex]);
    }
    // Update is called once per frame
    public float PointFollowSpeed = 10f;
    private void Update()
    { 
        if(!Commander)
        {
            if ((Input.GetMouseButtonDown(0)|| Input.GetMouseButton(0)) && !Attack)
            {
                RunAnim();
            }
            if (Input.GetMouseButtonUp(0) && !Attack)
            {
                IdleAnim();
            }
        }
        if (Character != null)
        {

            transform.position = Vector3.MoveTowards(transform.position, Character.position, Time.deltaTime * PointFollowSpeed);
            Vector3 direction = transform.position - Character.transform.position;
            Quaternion LookPos = Quaternion.LookRotation(-direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LookPos, Time.deltaTime * 350);
        }
        else if (Character2 != null && Follow)
        {

            Vector3 direction = transform.position - Character2.transform.position;
            Quaternion LookPos = Quaternion.LookRotation(Character2.forward, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LookPos, Time.deltaTime * 350);
           
           transform.position = Vector3.MoveTowards(transform.position, Character2.position, Time.deltaTime * 20);
            
        }
        if(LookAt && Enemy!=null)
        {
            transform.LookAt(Enemy);
        }

    }

  
    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.CompareTag("AreaPoint") && !Commander && Attack)
        //{
        //    DestroySoldier();
        //}
        if (other.gameObject.CompareTag("Engel") && !Commander)
        {
            
            EnviromentDamage();
         
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet") && !Commander)
        {
            collision.gameObject.SetActive(false);
            GetDamage(level);
        }
    }
}
