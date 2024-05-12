using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level", menuName = "Creat Level/New Level", order = 1)]
public class LevelScriptTable : ScriptableObject
{
    public List<Enemys> enemys = new List<Enemys>();
    public GameObject Doors;
    public List<GameObject> EnemyGroup=new List<GameObject>();
    public List<Moneys> moneys = new List<Moneys>();
    public List<Enviroment> enviroments = new List<Enviroment>();
}
[System.Serializable]
public class Enemys
{
    public int EnemyCount=0;
    public int ArmorValue=0;
    public int WeaponValue=0;
}
[System.Serializable]
public class Moneys
{
    public GameObject MoneyPrefab;
    public Vector3 Position;
    public Vector3 Rotation;
}
[System.Serializable]
public class Enviroment
{
    public int EnviromentPrefabIndex;
    public Vector3 Position;
    public Vector3 Rotation;
}
