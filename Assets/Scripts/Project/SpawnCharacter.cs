using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnCharacter : MonoBehaviour
{
    public static SpawnCharacter Instante;
    
    public GameObject Soldier;
    public List<GameObject> Soldiers = new List<GameObject>();
    public Transform[] SpawnPoint;
    public int Index = 0;
   
    [Header("King")]
    public GameObject King;
    Rigidbody rigidbody;
    private void Awake()
    {
        if (Instante == null)
            Instante = this;
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
   
    public void CharacterSpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject _Ch = Instantiate(Soldier, SpawnPoint[Index].transform.position, Quaternion.identity, transform);
            Soldiers.Add(_Ch);
            if (GunIndex>0)
            _Ch.GetComponent<CharacterFollow>().AnimAndGunChange(GunIndex);
            if (healtyIndex > 0)
                _Ch.GetComponent<CharacterFollow>().HealtyChange(healtyIndex,true);
            Index++;

            
        }
       

    }
    public void FollowToCommander()
    {
        King.GetComponent<Collider>().isTrigger = false;
        for (int i = 0; i < Soldiers.Count; i++)
        {
            //Soldiers[i].GetComponent<JoystickControll>().enabled=true;
            Soldiers[i].transform.parent = null;
            Soldiers[i].GetComponent<CharacterFollow>().Character2 = transform;
            Soldiers[i].GetComponent<CharacterFollow>().Character = null;
            Soldiers[i].GetComponent<CharacterFollow>().PlatformFight = false;
            Soldiers[i].GetComponent<CapsuleCollider>().radius = 0.75f;
        }
        Fight.Instante.Soldiers = Soldiers;
    }
    public void StopFollow(bool _follow)
    {
        for (int i = 0; i < Soldiers.Count; i++)
        {
            
            Soldiers[i].GetComponent<CharacterFollow>().Follow = _follow;
           
        }
    }
   public IEnumerator SoldierGotoPoint()
    {

        Index = 0;
        for (int i = 0; i < Soldiers.Count; i++)
        {

            //Soldiers[i].transform.DOMove(SpawnPoint[i].position, 0.5f).OnComplete(()=>
            //{
            //    DOTween.Kill(Soldiers[i]);
            //});
            Soldiers[i].GetComponent<CharacterFollow>().PointFollowSpeed = 20f;
            Soldiers[i].GetComponent<CharacterFollow>().Character = SpawnPoint[i].transform;
            Index++;
            //yield return null;
            yield return new WaitForSeconds(0.01f);
        }
        //EnviromentDamageMove();
        StopShoot();
        for (int i = 0; i < Soldiers.Count; i++)
        {

            Soldiers[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
            
            
        }
        //Index = Soldiers.Count;
        yield return new WaitForSeconds(0.5f);
        MoveControll.Instante.IsMove = true;
        Commander.Instante.IsStart = false;
        Commander.Instante.Attack = false;
        
        
    }
    public void EnviromentDamageMove()
    {
        Index = 0;
        for (int i = 0; i < Soldiers.Count; i++)
        {
            Debug.Log("askerler dizliyor");
            Soldiers[i].GetComponent<CharacterFollow>().PointFollowSpeed = 10f;
            Soldiers[i].GetComponent<CharacterFollow>().Character=SpawnPoint[i].transform;
           // Soldiers[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
            Index++;
            //yield return new WaitForSeconds(0.1f);
        }
        //StopShoot();
        
    }
   public void StopShoot()
    {
        CharacterFollow characterFollow;
        for (int i = 0; i < Soldiers.Count; i++)
        {
            characterFollow = Soldiers[i].GetComponent<CharacterFollow>();
            characterFollow.Attack = false;
            characterFollow.IdleAnim();
            if (characterFollow.ActiveGun!=null)
                characterFollow.ActiveGun.IsShoot = false;
           
        }
        IsShoot = false;
    }
    public List<GameObject> AttackSoldier = new List<GameObject>();
   
    bool IsShoot = false;
    public void Attack2(List<GameObject> enemys)
    {
        rigidbody.isKinematic = true;
        if(GunIndex<3)
        {
            for (int i = 0; i < Soldiers.Count; i++)
            {
                for (int j = 0; j < enemys.Count; j++)
                {
                    Soldiers[i].GetComponent<CharacterFollow>().PointFollowSpeed = 10f;
                    Soldiers[i].GetComponent<CharacterFollow>().Character = enemys[j].transform;
                    Soldiers[i].GetComponent<CharacterFollow>().AttackAnim();
                }

            }
        }
        else
        {
           if(!IsShoot)
            {  
                IsShoot = true;
                int enemyindex = 0;
                for (int j = 0; j < Soldiers.Count; j++)
                {
                    Soldiers[j].GetComponent<CharacterFollow>().AttackAnim();
                    if (enemys.Count < enemyindex)
                        enemyindex = 0;
                        
                    Soldiers[j].GetComponent<CharacterFollow>().GunShoot(enemys[enemyindex].transform);
                    
                    enemyindex++;
                    
                }
            }
            
        }
    }
    
    public int GunIndex = 0;
    public void SoldierGunChange()
    {
        GunIndex++;
        
        for (int i = 0; i < Soldiers.Count; i++)
        {
            Soldiers[i].GetComponent<CharacterFollow>().AnimAndGunChange(GunIndex);
        }
       // King.GetComponent<CharacterFollow>().AnimAndGunChange(GunIndex);
        
    }
    public int healtyIndex = 0;
    public void SoldierHealtChange()
    {  
        healtyIndex++;
        if (healtyIndex > 3)
            healtyIndex = 3;
        

            for (int i = 0; i < Soldiers.Count; i++)
            {
                Soldiers[i].GetComponent<CharacterFollow>().HealtyChange(healtyIndex,false);
            }
        
        
        
    }
    public void SoldierIdleAnim()
    {
        JoystickControll.Instante.joystick.gameObject.SetActive(true);
        
        GetComponent<JoystickControll>().enabled = true;
        King.GetComponent<Commander>().Attack = false;
        for (int i = 0; i < Soldiers.Count; i++)
        {
            Soldiers[i].GetComponent<CharacterFollow>().Attack = false;
            Soldiers[i].GetComponent<CharacterFollow>().IdleAnim();
        }
        StopShoot();
    }
    Vector3 Spawnpos()
    {
        Vector3 pos = Random.insideUnitSphere*2;
        Vector3 newpos = transform.position + pos;
        newpos.y = transform.position.y;
        newpos.z = transform.position.z-2;
        return newpos;
    }
    
}
