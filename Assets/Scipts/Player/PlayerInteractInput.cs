using System.Collections;
using System.Collections.Generic;
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
    //                // DropItem UI 생성 함수
    //                // cast.transform.Getcomponent<DropItem(). ui생성()
    //                return;
    //            }
    //            //if(cast.transform.GetComponent<NPC>()) // NPC 일경우
    //            //{
    //            //    NPC 접근 UI 생성
    //            //    return;
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        return;
    //    }
    //} // 상호작용 EX

    public void Attack()
    {
        CharacterInput.instance.attack = false;
        isAttack = true;
        ++attackCombo;
        if (playerMove.isground)// 지상공격
        {
            switch (attackCombo)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default: break;
            }
        }
        else                   // 공중콤보
        {
            switch (attackCombo)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default: break;
            }
        }
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
            isAttack = false;
        }
        if(isAttack)
        {
            attackCombo = 0;
        }
    }

    IEnumerator attackColliderOnOff()
    {
        RaycastHit hit;
        yield return new WaitForSeconds(0.2f);
        if(Physics.SphereCast(transform.position,2f,Vector3.forward,out hit))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 2f, Vector3.forward, 0f,monsterMask);
            foreach(var h in hits)
            {
                //h.transform.GetComponent<Monster>().GetDamage(); // monster 모두 데미지 줌
            }
        }
        yield return null;

    }
}
