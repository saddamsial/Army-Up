using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Base.UI;
using UnityEngine.SceneManagement;
public class MoneyStack : MonoBehaviour
{
    public static MoneyStack Instante;
    
    public float MoneyCount =3f;
    public float MoneyForwerdSpeed = 5f;
    public List<GameObject> MoneyList = new List<GameObject>();

    
    [Header("Level Text")]
    [SerializeField] TextMeshProUGUI LevelTMP;
    [SerializeField] Transform LevelTextParent;
    public int LevelValue = 1;
    Camera mainCam;

    [SerializeField] UI_Gameover _Gameover;
    public EnemySpawn enemySpawn;

    public GameObject DoneButton;
    private void Awake()
    {
        Instante = this;
    }
    void Start()
    {
        mainCam = Camera.main;
        
        //CharacterLevelIncrease(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
           MoneyMoveEffect();
         
        }
        if(Input.GetMouseButtonUp(0))
        {
            MoneyMoveCenter();
        }
        LevelTextParent.LookAt(mainCam.transform.position);
    }
   
    void MoneyMoveEffect()
    {
        Debug.Log("Hareket ediyor");
        for (int i = 1; i < MoneyList.Count; i++)
        {
           
            Vector3 _MoneyPos = MoneyList[i].transform.localPosition;
            _MoneyPos.x = MoneyList[i - 1].transform.localPosition.x;
            //MoneyList[i].transform.localPosition = Vector3.Lerp(MoneyList[i].transform.localPosition, _MoneyPos, 5* Time.deltaTime);
             MoneyList[i].transform.DOLocalMove(_MoneyPos,0.25f);




        }
    }
    void MoneyMoveCenter()
    {
        for (int i = 0; i < MoneyList.Count; i++)
        {

            Vector3 _MoneyPos = MoneyList[i].transform.localPosition;
            _MoneyPos.x = 0;

            MoneyList[i].transform.DOLocalMove(_MoneyPos, 0.35f);




        }
    }
    IEnumerator MoneyScaleAnim()
    {
        for (int i = MoneyList.Count-1; i >=0; i--)
        {
            GameObject _money = MoneyList[i];
            _money.transform.DOScale(new Vector3(150,150,150),0.3f).OnComplete(()=>
            {
                _money.transform.DOScale(new Vector3(100,100,100), 0.3f);
            });
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void CharacterLevelIncrease(int value)
    {
        LevelValue=SpawnCharacter.Instante.Soldiers.Count* SpawnCharacter.Instante.Soldiers[0].gameObject.GetComponent<CharacterFollow>().level;
        Debug.Log("Gelen deðer: " + value);
        
        
        
        LevelTMP.text = "Lvl " + (LevelValue).ToString();


    }
    public void CharacterLevelDecrease(int value,bool die)
    {
        LevelValue-=value;

        
        if (LevelValue < 1 && SpawnCharacter.Instante.Soldiers.Count<=0)
        {
            LevelValue = 1;
            _Gameover.gameObject.SetActive(true);

            if (!die)
                DoneButton.SetActive(true);
           else if (die)
            {
                _Gameover.EnableOverUI(false);
                Commander.Instante.Die();
            }
            enemySpawn.StopShoot();
        }
        else
        {
            CharacterLevelIncrease(0);
        }
        LevelTMP.text = "Lvl " + (LevelValue).ToString();


    }
    public void DoneButtonClick()
    {
        _Gameover.gameObject.SetActive(true);
        _Gameover.EnableOverUI(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Money"))
        {
            other.gameObject.tag = "Untagged";
            other.gameObject.transform.parent = transform;
            MoneyCount +=1.5f;
             other.transform.localPosition = new Vector3(0, other.transform.localPosition.y, MoneyCount);
            other.transform.localRotation = Quaternion.Euler(-90, 0, 90);
            //other.transform.DOLocalMove(new Vector3(0, other.transform.localPosition.y, MoneyCount), 0.3f);
            MoneyList.Add(other.gameObject);
            
            if (MoneyList.Count == 1)
                GetComponent<MoveControll>().FirstMoneyFunc(other.transform);
            StartCoroutine(MoneyScaleAnim());
            

        }
        else if(other.gameObject.CompareTag("Door"))
        {
            if(MoneyList.Count>=1)
            {
                
                DOTween.Kill(MoneyList[MoneyList.Count - 1]);
                Destroy(MoneyList[MoneyList.Count - 1]);
                MoneyList.Remove(MoneyList[MoneyList.Count - 1]);
                other.GetComponent<Door>().AllBlockColorChange();
                MoneyCount--;
                
                Vibration.Vibrate(25);
                 if(MoneyList.Count<=0)
                {
                    MoneyCount = 2;
                    other.GetComponent<Door>().mainDoor.IsDoorBlockTextChange();
                    other.transform.parent.gameObject.SetActive(false);
                }
            }
           
            
        }
    }
}
