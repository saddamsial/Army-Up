using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class Door : MonoBehaviour
{
    [SerializeField] int Index = 0;
   
    [SerializeField] int BlockIndex = 0;
    [SerializeField] Material GreenMat;
    [SerializeField] Material GreyMat;
    public MainDoor mainDoor;
    public bool IsTextChange = false;
    public int ChildIndex = 1;
    int GunDoorCollisionCount = 0;
    int HealtyDoorCollisionCount = 0;

    [Header("Gun Sprite")]
    [SerializeField] Sprite[] GunSprite;
    [SerializeField] Image GunImage;
    [SerializeField] Material[] GunSpriteMat;
    [SerializeField] int GunSpriteIndex = 0;

    
    public enum WhichDoor
    {
        Count,
        Healty,
        Gun
    }
    public WhichDoor DoorEnum;
    void Start()
    {
        DoorTextValue();
    }

   
   public void DoorTextValue()
    {
        for (int i = 0; i < transform.childCount-3; i++)
        {
            GameObject Block = transform.GetChild(i).gameObject;

            Block.GetComponentInChildren<TextMeshProUGUI>().text = (ChildIndex).ToString();
            Block.GetComponent<MeshRenderer>().material = GreyMat;
            ChildIndex++;
        }
        
    }
    public void GunDoorSpriteChange()
    {
        Debug.Log("KAPI SPRÝTE DEÐÝÞTÝRÝLDÝ");
        
            GunDoorCollisionCount = 0;
            GunImage.sprite = GunSprite[GunSpriteIndex + 1];
            GunImage.material = GunSpriteMat[GunSpriteIndex + 1];
            GunSpriteIndex++;
           


        
    }
    public void AllBlockColorChange()
    {
        IsTextChange = true;
        mainDoor.ChangeDoorColor(Index);
       // DoorLevel();
        if (DoorEnum==WhichDoor.Count)
        {
            SpawnCharacter.Instante.CharacterSpawn();
           
        }
       else if(DoorEnum==WhichDoor.Gun)
        {
            GunDoorCollisionCount++;
            if (GunDoorCollisionCount >= 3)
            {
               
                SpawnCharacter.Instante.SoldierGunChange();
                mainDoor.WeaponDoorSpriteChange();

            }
            
        }
        else if (DoorEnum == WhichDoor.Healty)
        {
            HealtyDoorCollisionCount++;
            if (HealtyDoorCollisionCount >= 3)
            {
                HealtyDoorCollisionCount = 0;
                SpawnCharacter.Instante.SoldierHealtChange();
            }
        }
    }
    public void ChangeBlockColor()
    {

        GameObject Block = transform.GetChild(BlockIndex).gameObject;

        Block.GetComponent<MeshRenderer>().material = GreenMat;
        BlockIndex++;
        if (BlockIndex >= transform.childCount-3)
        {
           
            BlockIndex=0;

           
            
            if (IsTextChange == true)
            {
                mainDoor.IsDoorBlockTextChange();
                transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).OnComplete(() =>
                {
                    mainDoor.AgainTextValue(Index);
                    transform.DOScale(Vector3.one, 0.2f);

                    

                });
                
            }
            
            
                
        }
            
    }
    void DoorLevel()
    {
        //MoneyStack.Instante.CharacterLevelText(DoorLevelValue);
    }
}

