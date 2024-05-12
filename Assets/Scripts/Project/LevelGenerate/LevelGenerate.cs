using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class LevelGenerate : MonoBehaviour
{
    [SerializeField] int levelIndex = 1;
    public LevelScriptTable levelScript;
   public LevelScriptTable ID;
    
    [Header("Enviroment Script")]
    [SerializeField] MainDoor mainDoorScript;
    [SerializeField] GameObject _newLevel;
   
    [Header("Level Enviroment")]
    [SerializeField] string LevelName = "";
    [SerializeField] GameObject _MainDoor,_newdoor;
    [SerializeField] GameObject Doors;

    [Header("Enemy")]
    [SerializeField] List<EnemyGenerate> enemies = new List<EnemyGenerate>();
    [SerializeField] GameObject EnemyGroup;
    [SerializeField] GameObject _enemy;

    [Header("Money")]
    [SerializeField] int MoneySpawnCount = 0;
    [SerializeField] GameObject MoneyGroupPrefab;
    [SerializeField] GameObject MoneyGroupParent;
    [SerializeField] List<GameObject> MoneyList = new List<GameObject>();

    [Header("Enviroment")]
    [SerializeField] GameObject[] Enviroment;
    [SerializeField] GameObject EnviromentParent;
    [SerializeField] List<int> EnviromentIndexList;
    [SerializeField] List<GameObject> EnviromentList;
    [SerializeField] int EnviromentCount=0;
   
    void Start()
    {
        levelScript= (LevelScriptTable)Resources.Load("Levels/MainLevels/Level " + levelIndex.ToString());
        ImportLevel();
    }
    void ImportLevel()
    {
        ParentObjectCreat();
        
        CreateDoor();

        CreatEnviroment();
    }
    float PositionZ = 0;
    void CreateDoor()
    {
        _newLevel.name = levelScript.name;
        _newdoor.AddComponent<MainDoor>();
        mainDoorScript = _newdoor.GetComponent<MainDoor>();
        MoneyCreat();
        for (int i = 0; i < 4; i++)
        {
            GameObject _door = Instantiate(Doors, _newdoor.transform.position + new Vector3(0, 0, PositionZ), Quaternion.identity, _newdoor.transform);
           
            if (i < levelScript.EnemyGroup.Count)
                CreatEnemyGroup(i,_door.transform);
            PositionZ += 75f;


        }
        for (int i = 0; i < 3; i++)
        {
            mainDoorScript._door.Add(new DoorName());
            mainDoorScript._door[i].doors = new GameObject[_newdoor.transform.childCount];
            for (int j = 0; j < _newdoor.transform.childCount; j++)
            {

                mainDoorScript._door[i].doors[j] = _newdoor.transform.GetChild(j).transform.GetChild(i).gameObject;
                _newdoor.transform.GetChild(j).transform.GetChild(i).GetComponent<Door>().mainDoor = mainDoorScript;
            }
        }
       
    }
    void MoneyCreat()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject MoneySpawned = Instantiate(levelScript.moneys[i].MoneyPrefab, levelScript.moneys[i].Position, Quaternion.identity, MoneyGroupParent.transform);
            MoneySpawned.transform.localPosition = levelScript.moneys[i].Position;
            MoneySpawned.transform.localEulerAngles = levelScript.moneys[i].Rotation;
        }
        

    }
    void CreatEnemyGroup(int i,Transform _doorPos)
    {
        
            GameObject _enemygroup = Instantiate(EnemyGroup, _doorPos.position+new Vector3(0,0,20), Quaternion.identity, _enemy.transform);
            _enemygroup.GetComponent<EnemySpawn>().EnemyCount = levelScript.enemys[i].EnemyCount;
            _enemygroup.GetComponent<EnemySpawn>().HealtyValue = levelScript.enemys[i].ArmorValue;
            _enemygroup.GetComponent<EnemySpawn>().GunValue = levelScript.enemys[i].WeaponValue;
            //_enemygroup.GetComponent<EnemySpawn>().EnemySpawned();
        
    }
    void CreatEnviroment()
    {
        for (int i = 0; i <levelScript.enviroments.Count; i++)
        {
            
            GameObject _enviroment = Instantiate(Enviroment[levelScript.enviroments[i].EnviromentPrefabIndex], EnviromentParent.transform.position, Quaternion.Euler(-90, 0, 0), EnviromentParent.transform);
            _enviroment.transform.localPosition = levelScript.enviroments[i].Position;
            _enviroment.transform.localEulerAngles = levelScript.enviroments[i].Rotation;


        }
    }
    #region Level_Create
    public void LevelCreate()
    {
        ParentObjectCreat();
        SpawnDoor();
        EnviromentSpawn();
        
    }
    void ParentObjectCreat()
    {
        ID = ScriptableObject.CreateInstance<LevelScriptTable>();
        ID.name = LevelName;
        _newLevel = new GameObject();
        _newLevel.name = LevelName;
        _newLevel.transform.position =Vector3.zero;

        _newdoor = new GameObject("Main Door");
        _newdoor.transform.parent = _newLevel.transform;
        _newdoor.transform.localPosition = new Vector3(0,0,75);

        _enemy = new GameObject("EnemyGroup");
        _enemy.transform.parent = _newLevel.transform;
        _enemy.transform.localPosition = new Vector3(0, 0, 75);

        MoneyGroupParent = new GameObject("MoneyGroup");
        MoneyGroupParent.transform.parent = _newLevel.transform;
        MoneyGroupParent.transform.localPosition = Vector3.zero;

        EnviromentParent = new GameObject("Enviroment");
        EnviromentParent.transform.parent = _newLevel.transform;
        EnviromentParent.transform.localPosition = Vector3.zero;
    }
    void SpawnDoor()
    {
        _newdoor.AddComponent<MainDoor>();
        mainDoorScript = _newdoor.GetComponent<MainDoor>();
        float PositionZ = 0;
        MoneySpawn(MoneyGroupParent.transform);
        for (int i = 0; i < 4; i++)
        {
            GameObject _door = Instantiate(Doors, _newdoor.transform.position+new Vector3(0,0,PositionZ), Quaternion.identity, _newdoor.transform);
            if (i <enemies.Count)
                SpawnEnemyGroup(i, _door.transform);

            
            PositionZ += 75f;




        }
       // MoneySpawn(_enemygroup.transform);
        for (int i = 0; i < 3; i++)
        {
            mainDoorScript._door.Add(new DoorName());
            mainDoorScript._door[i].doors = new GameObject[_newdoor.transform.childCount];
            for (int j = 0; j < _newdoor.transform.childCount; j++)
            {
                
                mainDoorScript._door[i].doors[j] = _newdoor.transform.GetChild(j).transform.GetChild(i).gameObject;
                _newdoor.transform.GetChild(j).transform.GetChild(i).GetComponent<Door>().mainDoor = mainDoorScript;
            }
        }
    }
    GameObject _enemygroup;
    void SpawnEnemyGroup(int i,Transform _doorPosition)
    {
        
             _enemygroup = Instantiate(EnemyGroup, _doorPosition.transform.position + new Vector3(0, 0, 15), Quaternion.identity, _enemy.transform);
            _enemygroup.GetComponent<EnemySpawn>().EnemyCount = enemies[i].EnemyCount;
            _enemygroup.GetComponent<EnemySpawn>().HealtyValue = enemies[i].ArmorValue;
            _enemygroup.GetComponent<EnemySpawn>().GunValue = enemies[i].WeaponValue;
        
        MoneySpawn(_enemygroup.transform);
    }
    float MoneyPosZ = 0;
    void MoneySpawn(Transform _EnemyPos)
    {
        MoneyPosZ = 0;
        for (int i = 0; i < 2; i++)
        {
            GameObject MoneySpawned = Instantiate(MoneyGroupPrefab, (_EnemyPos.position+ new Vector3(0,0,10)) + new Vector3(Random.Range(-8, 8.4f), 0.25f, MoneyPosZ), Quaternion.identity, MoneyGroupParent.transform);
            MoneyList.Add(MoneySpawned);
            MoneyPosZ += MoneyGroupPrefab.GetComponent<BoxCollider>().size.z+10;
        }
        
                  
    }
    float EnviromentSpawnPosZ = 90;
    void EnviromentSpawn()
    {
        for (int i = 0; i < EnviromentCount; i++)
        {   int randomenviroment = Random.Range(0, Enviroment.Length);
            GameObject _enviroment = Instantiate(Enviroment[randomenviroment], EnviromentParent.transform.position+new Vector3(Random.Range(-8, 8), 0.3f, EnviromentSpawnPosZ), Quaternion.Euler(-90,0,0), EnviromentParent.transform);
            EnviromentList.Add(_enviroment);
            EnviromentIndexList.Add(randomenviroment);
            EnviromentSpawnPosZ +=Random.Range(30,90);
        }
    }
    #endregion
#if UNITY_EDITOR
    public void levelSave()
    {
        ID.Doors = _MainDoor;
        for (int i = 0; i < enemies.Count; i++)
        {
            ID.EnemyGroup.Add(EnemyGroup);
            ID.enemys.Add(new Enemys());
            ID.enemys[i].EnemyCount = enemies[i].EnemyCount;
            ID.enemys[i].ArmorValue = enemies[i].ArmorValue;
            ID.enemys[i].WeaponValue = enemies[i].WeaponValue;
            Debug.Log("düþmanlar kaydedildi");
        }
        for (int i = 0; i < MoneySpawnCount*2; i++)
        {
            ID.moneys.Add(new Moneys());
            ID.moneys[i].MoneyPrefab = MoneyGroupPrefab;
            ID.moneys[i].Position = MoneyList[i].transform.localPosition;
            ID.moneys[i].Rotation = MoneyList[i].transform.localEulerAngles;

        }
        for (int i = 0; i < EnviromentList.Count; i++)
        {
            ID.enviroments.Add(new Enviroment());
            ID.enviroments[i].EnviromentPrefabIndex = EnviromentIndexList[i];
            ID.enviroments[i].Position = EnviromentList[i].transform.localPosition;
            ID.enviroments[i].Rotation = EnviromentList[i].transform.localEulerAngles;
        }
        AssetDatabase.CreateAsset(ID, "Assets/Resources/Levels/MainLevels/" + LevelName + ".asset");
        AssetDatabase.SaveAssets();
    }
    public void LevelRemove()
    {
        AssetDatabase.DeleteAsset("Assets/Resources/Levels/MainLevels/" + LevelName + ".asset");
        AssetDatabase.SaveAssets();

    }
#endif
    public void LevelDelete()
    {
        MoneyList.Clear();
        MoneyPosZ = 0;
        PositionZ = 0;
        EnviromentCount = 0;
        EnviromentList.Clear();
        EnviromentIndexList.Clear();
        EnviromentSpawnPosZ = 90;
        
        DestroyImmediate(_newLevel);
    }
}
[System.Serializable]
public class EnemyGenerate
{
    public int EnemyCount = 0;
    public int ArmorValue = 0;
    public int WeaponValue = 0;
}
