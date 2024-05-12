using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour
{
    public List<DoorName> _door = new List<DoorName>();
   
    void Start()
    {
        
    }

    public void ChangeDoorColor(int index)
    {
        for (int i = 0; i < _door[0].doors.Length; i++)
        {
            _door[index].doors[i].GetComponent<Door>().ChangeBlockColor();
        }
        
    }
    public void WeaponDoorSpriteChange()
    {
        for (int i = 0; i < _door[0].doors.Length; i++)
        {
            _door[2].doors[i].GetComponent<Door>().GunDoorSpriteChange();
        }
    }
    public void IsDoorBlockTextChange()
    {
        for (int x = 0; x < _door.Count; x++)
        {
            for (int i = 0; i < _door[0].doors.Length; i++)
            {
                _door[x].doors[i].GetComponent<Door>().IsTextChange=false;
            }
        }
        

    }
    public void AgainTextValue(int index)
    {
       
        for (int i = 0; i < _door[0].doors.Length; i++)
        {
            _door[index].doors[i].GetComponent<Door>().DoorTextValue();
        }
    }
}
[System.Serializable]
public class DoorName
{
    
    public GameObject[] doors;
    
    
}