using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn Instante;
    
    [SerializeField] GameObject Enemy;
    
    [SerializeField] float Distance;
    public List<GameObject> Enemys = new List<GameObject>();
    [Header("Level Value")]
    public int EnemyCount;
    public int HealtyValue;
    public int GunValue;
    [SerializeField] int LevelValue;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] GameObject CircleImg;
    public int healtyMod = 0;

    [Header("Area Fight")]
    public bool IsAreaFight = false;
    private void Awake()
    {
        Instante = this;
    }
    void Start()
    {
        //LevelValueText(EnemyCount);
        if(!IsAreaFight)
        EnemySpawned();

        
    }
    public void SpawnSoldierFightArea(int soldierValue)
    {
        
            LevelValue = 1 + HealtyValue + GunValue;
            EnemyCount = soldierValue / LevelValue;
            EnemySpawned();
        
    }
    public void LevelValueText()
    {

        LevelValue=Enemys.Count*Enemys[0].gameObject.GetComponent<EnemyAttack>().Level;
        //LevelValue += value;
        LevelText.text = "Lvl " + LevelValue.ToString();

    }
   public void EnemySpawned()
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            GameObject _enemy=Instantiate(Enemy, Spawnpos(), Quaternion.Euler(0,180,0), transform);
            Enemys.Add(_enemy);
            if (GunValue>0)
            _enemy.GetComponent<EnemyAttack>().AnimAndGunChange(GunValue);
            if(HealtyValue > 0)
            _enemy.GetComponent<EnemyAttack>().HealtyChange(HealtyValue);
            
           
        }
        //LevelValueText();
    }
    Vector3 Spawnpos()
    {
        Vector3 pos = Random.insideUnitSphere*Distance;
        Vector3 newpos = transform.position + pos;
        newpos.y = transform.position.y;
        return newpos;
    }
    bool IsShooting = false;
    void Attack()
    {
        MoveControll.Instante.IsMove = false;
        Commander.Instante.CommandAnim();
        if(Enemys.Count>0)
        {  //Attack
            if(GunValue<3)
            {
                for (int i = 0; i < Enemys.Count; i++)
                {
                    Transform enemy = Enemys[i].transform;
                    enemy.GetComponent<EnemyAttack>().AttackRunAnim(GunValue);

                    if (SpawnCharacter.Instante.Soldiers.Count > i && Enemys.Count > 0)

                    {

                        Enemys[i].GetComponent<EnemyAttack>().Soldier = SpawnCharacter.Instante.Soldiers[i].transform;


                    }
                    else if (Enemys.Count > 0)
                    {
                        AttackToKing(enemy.gameObject);
                    }
                    else
                    {

                        AgainSoldierAlign();
                    }
                }
            }
         //Gun Shoot   
         else
            {
                for (int i = 0; i < Enemys.Count; i++)
                {
                    Transform enemy = Enemys[i].transform;
                    enemy.GetComponent<EnemyAttack>().AttackRunAnim(GunValue);

                    

                    
                        if (!IsShooting)
                        {
                            IsShooting = true;
                            
                            for (int j = 0; j < Enemys.Count; j++)
                            {
                                Enemys[j].GetComponent<EnemyAttack>().GunShoot();
                                if (SpawnCharacter.Instante.Soldiers.Count > j && Enemys.Count > 0)
                                    Enemys[j].GetComponent<EnemyAttack>().Soldier = SpawnCharacter.Instante.Soldiers[j].transform;
                        }
                        }
                        


                    
                   
                    
                }
            }
        }
        else
        {
            
            AgainSoldierAlign();
        }
        SpawnCharacter.Instante.Attack2(Enemys);
        

    }
   public void StopShoot()
    {
        Debug.Log("ATEÞ DURDURULDU");
        if (Enemys.Count > 0)
        {

            EnemyAttack EnemySoldier;
            for (int j = 0; j < Enemys.Count; j++)
            {
                EnemySoldier = Enemys[j].GetComponent<EnemyAttack>();
                
                EnemySoldier.IdleAnim();
                if (EnemySoldier.ActiveGun != null)
                    EnemySoldier.ActiveGun.IsShoot = false;

            }
            IsShoot = false;

        }
        else
        {

            AgainSoldierAlign();
        }
    }
    public bool IsShoot = false;
    public int count = 0;
    
    
   
   
  
    void AttackToKing(GameObject childEnemy)
    {


        childEnemy.GetComponent<EnemyAttack>().Soldier = SpawnCharacter.Instante.King.transform;
        
                

            
            

        
    }
    public void AgainSoldierAlign()
    {
        Debug.Log("Düþman öldü: " + gameObject.name);
        if (transform.childCount <=1 || Enemys.Count<=0)
        {
            Debug.Log("düþmanlar bitti");
            transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(SpawnCharacter.Instante.SoldierGotoPoint());
           // StartCoroutine(BoolenControll());
            
        }
        else
        {
            GetComponent<Collider>().enabled = true;
            Attack();
        }

    }
    int CommanderLevel = 0;
    bool IsFigtStart = false;
   public void FightAreaAttack()
    {
        if(!IsFigtStart)
        {
            IsFigtStart = true;
            if (CommanderLevel >= LevelValue)
            {
                int DeathSoldierCount = LevelValue / SpawnCharacter.Instante.Soldiers[0].GetComponent<CharacterFollow>().level;
                StartCoroutine(DestroyRedSoldier(Enemys.Count));
                StartCoroutine(DestroyBlueSoldier(DeathSoldierCount));
            }
           else if (CommanderLevel < LevelValue)
            {
                int DeathSoldierCount = CommanderLevel / Enemys[0].GetComponent<EnemyAttack>().Level;
                Debug.Log("MAVÝ ASKER SAYISI: " + DeathSoldierCount);
                StartCoroutine(DestroyRedSoldier(DeathSoldierCount));
                StartCoroutine(DestroyBlueSoldier(SpawnCharacter.Instante.Soldiers.Count));
            }
        }
        
    }
    IEnumerator DestroyBlueSoldier(int soldiercount)
    {
        Debug.Log("ÖLECEK OLAN MAVÝ ASKER SAYISI************: " + soldiercount);
        for (int i = 0; i < soldiercount; i++)
        {
            Debug.Log("ÖLEN MAVÝ ASKER SAYISI************: " + (i+1));
            SpawnCharacter.Instante.Soldiers[0].GetComponent<CharacterFollow>().DestroySoldier();
            
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator DestroyRedSoldier(int count)
    {
        Debug.Log("ÖLECEK OLAN ASKER SAYISI: " + count);
        for (int i = 0; i < count; i++)
        {
            Enemys[i].GetComponent<EnemyAttack>().DeathArea();
            Debug.Log("ÖLEN OLAN ASKER SAYISI: " + (i + 1));
            yield return new WaitForSeconds(0.05f);
        }
        Enemys.RemoveRange(0, count-1);
    }
   
    IEnumerator BoolenControll()
    {
        yield return new WaitForSeconds(3f);
        while (Commander.Instante.Attack==true || MoveControll.Instante.IsMove==false)
        {
            Debug.Log("deðerler deðiþtirildi");
            Commander.Instante.Attack = false;
            MoveControll.Instante.IsMove = true;
            StartCoroutine(SpawnCharacter.Instante.SoldierGotoPoint());
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Commander") && transform.childCount>1 && !IsAreaFight)
        {   
            GetComponent<Collider>().enabled = false;
            CircleImg.SetActive(false);
            other.GetComponent<MoneyStack>().enemySpawn = this;
            Attack();
        }
        else if (other.gameObject.CompareTag("Commander") && transform.childCount > 1 && IsAreaFight)
        {
            GetComponent<Collider>().enabled = false;
            CircleImg.SetActive(false);
            JoystickControll.Instante.joystick.gameObject.SetActive(false);
           
            other.GetComponent<JoystickControll>().enabled = false;
            
            SpawnCharacter.Instante.StopFollow(false);
            CommanderLevel = other.GetComponent<MoneyStack>().LevelValue;
            
            Attack();
        }
    }
}
