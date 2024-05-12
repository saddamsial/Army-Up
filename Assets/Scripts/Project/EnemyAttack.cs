using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    Rigidbody rigidbody;
    public Transform Soldier;
    bool IsDead = false;

    EnemySpawn enemySpawn;
    Animator animator;
    public int AnimIndex = 0;
    public string[] AnimNames;

    [Header("Gun")]
    public List<GameObject> Gun = new List<GameObject>();
    public Guns ActiveGun;


    [Header("Healty")]
    public List<GameObject> Healty = new List<GameObject>();
    public int DamageCount = 0;
    public bool Attack = false;
    [SerializeField] ParticleSystem BloodParticle;
    [SerializeField] GameObject Splash_Img;
    public int Level = 1;
    public int GunLevel = 1;
    public int HealtyLevel = 1;
    void Start()
    {
        animator = GetComponent<Animator>();
        enemySpawn = GetComponentInParent<EnemySpawn>();
        rigidbody = GetComponent<Rigidbody>();
    }
    public void AnimAndGunChange(int index)
    {
        animator = GetComponent<Animator>();
        animator.Play(AnimNames[index]+"_Idle");
        for (int i = 0; i < Gun.Count; i++)
        {
            Gun[i].SetActive(false);
        }
        Gun[index].SetActive(true);
        GunLevel = index;
        AnimIndex = index;
        Level +=GunLevel;
        if (index >= 3)
            ActiveGun = Gun[index].GetComponent<Guns>();
        GetComponentInParent<EnemySpawn>().LevelValueText();
    }
    public void AttackRunAnim(int index)
    {
        Attack = true;
        animator.Play(AnimNames[index]);
    }
    public void HealtyChange(int index)
    {
        
        for (int i = 0; i <index; i++)
        {
            Healty[i].SetActive(true);
        }
        HealtyLevel = index;

        Level += HealtyLevel;
        GetComponentInParent<EnemySpawn>().LevelValueText();
        DamageCount++;
    }
    public void GetDamage()
    {
        DamageCount--;
        //MoneyStack.Instante.CharacterLevelText(-1);
        if (DamageCount < 0)
        {
            
            Death();

        }


    }
    public void GunShoot()
    {
        if (ActiveGun != null)
        {

            ActiveGun.IsShoot = true;
            StartCoroutine(ActiveGun.Shoot());
        }

    }
    public void IdleAnim()
    {
        animator.Play(AnimNames[AnimIndex] + "_Idle");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Soldier!=null && GunLevel<3)
        {
            transform.position = Vector3.MoveTowards(transform.position, Soldier.position, Time.fixedDeltaTime * 10);
        }
        else if(Soldier != null && GunLevel >= 3)
        {
          ActiveGun.transform.LookAt(Soldier);
        }
        else if(Soldier != null && GunLevel >= 3)
        {
            int randomsoldier = Random.Range(0, SpawnCharacter.Instante.Soldiers.Count);
            Soldier = SpawnCharacter.Instante.Soldiers[randomsoldier].transform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !IsDead && Attack && !enemySpawn.IsAreaFight)
        {
           
            
            rigidbody.AddForce(Vector3.forward * 10, ForceMode.Impulse);
            CharacterFollow characterFollow = collision.gameObject.GetComponent<CharacterFollow>();


            if (characterFollow.level >= Level)
            {
                characterFollow.GetDamage(Level);
                Death();
            }

            else
            {
                characterFollow.GetDamage(characterFollow.level);
                Level -= characterFollow.level;
                enemySpawn.AgainSoldierAlign();

            }
        }
        if (collision.gameObject.CompareTag("Player") && !IsDead && Attack && enemySpawn.IsAreaFight)
        {

            enemySpawn.FightAreaAttack();
           
        }
        else if (collision.gameObject.CompareTag("Bullet") && !IsDead && Attack)
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.SetActive(false);
            //enemySpawn.DestroyEnemys();
            Death();
        }
        
    }
    public void Death()
    {
        IsDead = true;
        BloodParticle.transform.parent = null;
        BloodParticle.Play();
        Instantiate(Splash_Img, transform.position + Vector3.up * 0.2f, Quaternion.Euler(90, 0, 0));
        enemySpawn.Enemys.Remove(gameObject);
        if (!enemySpawn.IsAreaFight)
            enemySpawn.AgainSoldierAlign();
        if (enemySpawn.IsAreaFight)
        {
            enemySpawn.GetComponentInParent<FightArea>().SoldierCount(Level);
        }
        Vibration.Vibrate(15);
        Destroy(gameObject);
        
    }
    public void DeathArea()
    {
        IsDead = true;
        BloodParticle.transform.parent = null;
        BloodParticle.Play();
        Instantiate(Splash_Img, transform.position + Vector3.up * 0.2f, Quaternion.Euler(90, 0, 0));
       
        
       enemySpawn.GetComponentInParent<FightArea>().SoldierCount(Level);
        
        Vibration.Vibrate(15);
        Destroy(gameObject);
    }
    public void BulletDeath()
    {
        enemySpawn.Enemys.Remove(gameObject);
        IsDead = true;
        BloodParticle.transform.parent = null;
        BloodParticle.Play();
        Instantiate(Splash_Img, transform.position + Vector3.up * 0.2f, Quaternion.Euler(90, 0, 0));
        
       

        Vibration.Vibrate(15);
        Destroy(gameObject);
    }
}
