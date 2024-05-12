using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Gun",menuName ="Gun")]
public class Gun : ScriptableObject
{
    public GameObject Bullet;
    public Transform BulletSpawnPos;
    public float ShootTime = 0.5f;
    public int BulletCount = 5;
}
