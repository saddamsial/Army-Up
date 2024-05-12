using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Fight : MonoBehaviour
{
    public static Fight Instante;

    public List<GameObject> Soldiers = new List<GameObject>();
    private void Awake()
    {
        Instante = this;
    }
    void Start()
    {
        
    }

    public IEnumerator FightStart(Transform area,int soldiercount)
    {
        
        int count = soldiercount/ Soldiers[0].GetComponent<CharacterFollow>().level;
        if (count * Soldiers[0].GetComponent<CharacterFollow>().level < soldiercount)
            count++;
        for (int i = 0; i < count; i++)
        {  
            if(Soldiers[i]!=null)
            {
                GameObject soldier = Soldiers[i];
                //soldier.transform.DOMove(area.position,1f).OnComplete(()=>Destroy(soldier));
                soldier.GetComponent<CharacterFollow>().Character = area;
               
                soldier.GetComponent<CharacterFollow>().AttackAnim();
                yield return null;
            }
            
            
        }
    }
}
