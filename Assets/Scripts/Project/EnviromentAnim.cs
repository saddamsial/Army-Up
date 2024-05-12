using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnviromentAnim : MonoBehaviour
{
    [SerializeField] Vector3 RotateValue;
    [SerializeField] float MovePos;
    [SerializeField] bool IsMove = false;
    void Start()
    {   if(IsMove)
        transform.DOLocalMoveX(MovePos, 3f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);

       transform.DOLocalRotate(RotateValue,0.7f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.Rotate(RotateValue * Time.deltaTime);
    }
}
