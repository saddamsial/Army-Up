using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    public static Commander Instante;

    Animator animator;
    public bool IsStart = false,Attack=false;
    private void Awake()
    {
        Instante = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !Attack)
        {
            IsStart = true;
            animator.Play("Run");
        }
        if(Input.GetMouseButtonUp(0) && !Attack)
        {
            animator.Play("Idle");
            //SoldierIdleAnim();
        }
    }
    public void CommandAnim()
    {
        Attack = true;
        animator.Play("Attack");
    }
    void SoldierIdleAnim()
    {
        for (int i = 0; i < SpawnCharacter.Instante.Soldiers.Count; i++)
        {
            SpawnCharacter.Instante.Soldiers[i].GetComponent<CharacterFollow>().IdleAnim();
        }
    }
    public void Die()
    {
        animator.Play("Die");
    }
}
