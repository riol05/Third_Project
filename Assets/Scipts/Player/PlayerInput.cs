using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractInput : MonoBehaviour, IInteractable
{
    private CharacterMovement playerMove;
    bool canPickUp = true;
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
    private void PickUpItem() // e 키에 넣을예정 Input system은 bool 변수가 아닌 int를 사용해서 int가 올라간만큼 아이템 획득
    {
        RaycastHit hit;
        if (canPickUp)
        {
            if (Physics.SphereCast(transform.position, 3f, Vector3.down, out hit, 0f, ItemMask))
            {
                Inventory.instance.CheckTypeForGetItem(hit.transform.GetComponent<DropItem>().ItemSO);
                Destroy(hit.transform); // TODO : 오브젝트 풀링 사용
            }
        }
    }

    public void Interact()
    {

    }

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
