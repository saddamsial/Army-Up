using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Area : MonoBehaviour
{

    public static Save_Area Instante;
    public List<FightArea> FightAreas = new List<FightArea>();
    public List<Areas> areas=new List<Areas>();
    public Areas[] area;
    public int AreaIndex = 0;

    private void Awake()
    {
        Instante = this;
    }
    void Start()
    {    if(PlayerPrefs.GetInt("AreaIndex")==0)
        {
            for (int i = 0; i < area.Length; i++)
            {
                areas.Add(area[i]);
                PlayerPrefsExtra.SetList<Areas>("Soldier", areas);
            }
        }
        
        LoadArea();
        
        
    }
    public void LoadArea()
    {

        areas= PlayerPrefsExtra.GetList<Areas>("Soldier",new List<Areas>());
        if(areas.Count>0)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                
                FightAreas[i].Soldier = areas[i].Soldier;
                FightAreas[i].Iscomplete = areas[i].IsComplete;
                FightAreas[i].firstSoldierValue();
            }
        }
        
        
       
    }
    public void AddArea(int index)
    {
        
       // areas.Add(area[AreaIndex]);
        
    }
   public void SetValue(int index,int value)
    {
        
        areas[index].Soldier+=value;
        PlayerPrefsExtra.SetList<Areas>("Soldier", areas);
        PlayerPrefs.SetInt("AreaIndex", 1);
    }
    public void Complete(int _index,GameObject _build,bool _IsComplete)
    {
        areas[_index].Build=_build;
        areas[_index].IsComplete = _IsComplete;
        PlayerPrefsExtra.SetList<Areas>("Soldier", areas);
    }
}
[System.Serializable]
public class Areas
{
    public int Soldier;
    public GameObject Build;
    public bool IsComplete;
}
