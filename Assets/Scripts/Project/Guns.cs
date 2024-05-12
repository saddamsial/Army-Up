using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    
    public int GunIndex = 0;
    public bool IsShoot = true;
    [Header("Spawn Bullet")]
    public List<GameObject> _Bullets = new List<GameObject>();
    public  GameObject Bullet;
    public Transform BulletSpawnPos;
    public float ShootTime;
    public int BulletCount = 5;
    [SerializeField] bool IsArrow = false;
    void Start()
    {
        SpawnBullet();
    }
    public void SelectGun()
    {
       
        SpawnBullet();
    }
    public void SpawnBullet()
    {
        for (int i = 0; i < BulletCount; i++)
        {
            GameObject _bullet = Instantiate(Bullet, BulletSpawnPos.position, Quaternion.identity, BulletSpawnPos.transform.parent);
            _bullet.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _bullet.GetComponent<Bullet>().ParentGun = BulletSpawnPos.transform;
            if (IsArrow)
            {
                _bullet.GetComponent<Bullet>().Arrow = IsArrow;
            }
            _Bullets.Add(_bullet);
            _bullet.SetActive(false);


        }
        //StartCoroutine(Shoot());
    }
    public Coroutine shooting;
    public IEnumerator Shoot()
    {
       
        for (int i = 0; i < _Bullets.Count; i++)
        {
            if (IsShoot)
            {
                
                
                _Bullets[i].transform.parent = null;
                _Bullets[i].SetActive(true);
                _Bullets[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                // _Bullets[i].GetComponent<Rigidbody>().AddForce(Vector3.forward * 250, ForceMode.Acceleration);
                yield return new WaitForSeconds(ShootTime);
            }
            else
                break;
            
        }
        AgainShoot();
    }
   void AgainShoot()
    { 
        for (int i = 0; i < _Bullets.Count; i++)
        {
            _Bullets[i].SetActive(false);
            _Bullets[i].transform.parent = BulletSpawnPos.parent;
            _Bullets[i].transform.localPosition =BulletSpawnPos.localPosition;
            _Bullets[i].GetComponent<Rigidbody>().velocity=Vector3.zero;
        }
        if(IsShoot)
        StartCoroutine(Shoot());
    }
    // Update is called once per frame
  
}
