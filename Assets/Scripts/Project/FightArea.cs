using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class FightArea : MonoBehaviour
{
    [SerializeField] int Level;
    public int TotalSoldierCount;
    
    [SerializeField] TextMeshProUGUI AreaSoldierCountTMP;
    [SerializeField] GameObject RedFlag;
    [SerializeField] GameObject SoldierGroup;
    [SerializeField] bool IsStartFight = false;
    

    public GameObject Build;
    public int Soldier;
    public bool Iscomplete = false;

    SpawnCharacter spawnCharacter;
    void Start()
    {
       // firstSoldierValue();
    }
    public void firstSoldierValue()
    {
        AreaSoldierCountTMP.text = "Level " + (Level+1).ToString() + " " + Soldier + "/" + TotalSoldierCount;
        if (Iscomplete)
        {
            Build.transform.localScale = Vector3.one;
            RedFlag.SetActive(false);
            gameObject.SetActive(false);
            SoldierGroup.SetActive(false);
            AreaSoldierCountTMP.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            SoldierGroup.GetComponent<EnemySpawn>().SpawnSoldierFightArea(TotalSoldierCount-Soldier);
        }
    }
   public void SoldierCount(int soldierLevel)
    {
       if(!Iscomplete)
        {
            Soldier+=soldierLevel;
            AreaSoldierCountTMP.text = "Level " + Level.ToString() + " " + Soldier + "/" + TotalSoldierCount;
            if (Soldier < TotalSoldierCount)
                Save_Area.Instante.SetValue(Level,soldierLevel);
            if (Soldier >= TotalSoldierCount || GetComponentInChildren<EnemySpawn>().Enemys.Count<=0)
            {
                Build.transform.DOScale(Vector3.one, 1);
                 Iscomplete = true;
                Save_Area.Instante.Complete(Level, Build, true);
                RedFlag.gameObject.SetActive(false);
                gameObject.SetActive(false);
                SoldierGroup.SetActive(false);
                SpawnCharacter.Instante.SoldierIdleAnim();
                AreaSoldierCountTMP.transform.parent.gameObject.SetActive(false);
                UIManager.Instante.GoldText(TotalSoldierCount);
            }
        }
        
    }
   
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Commander"))
        {
            
         //StartCoroutine(Fight.Instante.FightStart(transform.GetChild(0),TotalSoldierCount-Soldier));
           if(Soldier<=0)
            Save_Area.Instante.AddArea(Level);
            IsStartFight = true;
        }
        
    }
}
