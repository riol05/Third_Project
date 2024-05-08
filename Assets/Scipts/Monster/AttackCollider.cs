using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField]
    Collider boxForAttack1;
    
    [SerializeField]
    Collider boxForAttack2;

    public float attackTime = 0.5f;

    public void Attack(int i)
    {
        if (i < 0 && i > 3)   return;
        
        else
            StartCoroutine(AttackColliderOn(i));
    }
    IEnumerator AttackColliderOn(int i)
    {
        if (i == 1)
        {
            
        }
        if( i == 2)
        {

        }
        yield return null;
        yield return new WaitForSeconds(attackTime);

    }
}