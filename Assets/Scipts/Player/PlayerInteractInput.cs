using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerInteractInput : MonoBehaviour
{
    private CharacterMovement playerMove;
    public LayerMask ItemMask;

    private void Awake()
    {
        playerMove = GetComponent<CharacterMovement>();
    }
    private void Update()
    {
        AttackState();
        if(CharacterInput.instance.attack)
        {
            if (playerMove.attackAble)
            {
                Attack();
            }
        }
    }

    //public void Interact()
    //{
    //    RaycastHit[] InteractThing = Physics.SphereCastAll(transform.position, 3f, Vector3.zero);
    //    if (InteractThing != null)
    //    {
    //        foreach (RaycastHit cast in InteractThing)
    //        {
    //            if (cast.transform.GetComponent<DropItem>())
    //            {
    //                // DropItem UI ���� �Լ�
    //                // cast.transform.Getcomponent<DropItem(). ui����()
    //                return;
    //            }
    //            //if(cast.transform.GetComponent<NPC>()) // NPC �ϰ��
    //            //{
    //            //    NPC ���� UI ����
    //            //    return;
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        return;
    //    }
    //} // ��ȣ�ۿ� EX

    public void Attack()
    {
        CharacterInput.instance.attack = false;
        isAttack = true;
        attackCombo++;
        if (playerMove.isground)// �������
        {
            switch (attackCombo)
            {
                case 0:
                    attackTimeCD = 0.3f;
                    // TODO : ĳ���� move����addforce�� ������  �޼��带 �����;��ҵ�??
                    StartCoroutine(attackColliderOnOff(0.1f));
                    // TODO :  animation blend�� animation ����? ����?
                    break;
                case 1:
                    attackTimeCD = 0.3f;
                    StartCoroutine(attackColliderOnOff(0.1f));
                    break;
                case 2:
                    attackTimeCD = 0.3f;
                    StartCoroutine(attackColliderOnOff(0.1f));
                    break;
                case 3:
                    attackTimeCD = 0.4f;
                    StartCoroutine(attackColliderOnOff(0.2f));
                    break;
                case 4:
                    attackTimeCD = 0.6f;
                    StartCoroutine(attackColliderOnOff(0.4f));
                    break;
                default: break;
            }
        }
        else              // �����޺� ���� x
        { }
    }
    public bool isAttack;
    public int attackCombo;
    private float attackTime;
    public float attackTimeCD = 0.4f;

    private float exitAttackTime;
    public float exitAttackTimeCD = 1.3f;

    [SerializeField]
    private LayerMask monsterMask;
    private void AttackState()
    {
        if (CharacterInput.instance.attack)
        {
            attackTime = attackTimeCD;
            exitAttackTime = exitAttackTimeCD;
        }


        if (attackTime > 0 )
        {
            attackTime -= Time.deltaTime;
        }
        else if (attackTime <= 0)
        {
            exitAttackTime -= Time.deltaTime;
            StopAttack();
        }
    }
    private void StopAttack()
    {
        isAttack = false;
        attackCombo = 0;
    }

    public AttackCollider ACollider;
    public int damage;
    IEnumerator attackColliderOnOff(float f)
    {
        RaycastHit hit;
        CharacterInput.instance.freeze = true;
        yield return new WaitForSeconds(0.1f);
        if(Physics.SphereCast(transform.position,2f, transform.forward,out hit))
        {
            //RaycastHit[] hits = Physics.SphereCastAll(transform.position, 2f, transform.forward, 0f,monsterMask);
            ACollider.gameObject.SetActive(true);
            yield return new WaitForSeconds(f);
            
            if (ACollider.targets.Count > 0)
            {
                Transform t;
                while (ACollider.targets.TryDequeue(out t))
                {
                    Monster monster = t.GetComponent<Monster>();
                    if (monster != null)  monster.GetDamage(damage);
                    
                    else    throw new NullReferenceException("monster component is not there");
                }
            }
            ACollider.gameObject.SetActive(false);
            CharacterInput.instance.freeze = false;
        }
        yield return null;
    }
}
