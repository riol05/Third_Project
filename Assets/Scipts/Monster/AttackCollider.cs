using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Queue<Transform> targets = new Queue<Transform>();
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
    private void OnCollisionEnter(Collision collision)
    {
        //target.Add(collision.collider.transform);
        targets.Enqueue(collision.collider.transform);
    }
}