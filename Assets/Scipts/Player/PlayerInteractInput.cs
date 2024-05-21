using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerInteractInput : MonoBehaviour, IDamageable
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
                Attack();
            else
                CharacterInput.instance.attack = false;
        }
    }

    bool isComboNow;
    Coroutine Attackroutine = null;
    public void Attack()
    {
        Attackroutine = StartCoroutine(AttackRoutine());
    }
    IEnumerator AttackRoutine()
    {
        CharacterInput.instance.attack = false;
        isAttack = true;
        attackCombo++;
        if (playerMove.isground)// 지상공격
        {
            attackCombo = Mathf.Clamp(attackCombo,0,3);
            if(attackCombo == 3)
            {
                attackCombo = 0;
            }
            if(isComboNow)
            switch (attackCombo)
            {
                case 0:
                    attackTimeCD = 0.3f;
                    // TODO : 캐릭터 move에서addforce로 움직임  메서드를 가져와야할듯??
                    StartCoroutine(attackColliderOnOff(0.1f));
                    playerMove.ani.SetTrigger("Attack1");
                        yield return null;

                        break;
                case 1:
                    attackTimeCD = 0.3f;
                    StartCoroutine(attackColliderOnOff(0.1f));
                    playerMove.ani.SetTrigger("Attack2");
                        yield return null;

                        break;
                case 2:
                    attackTimeCD = 0.6f;
                    StartCoroutine(attackColliderOnOff(0.4f));
                    playerMove.ani.SetTrigger("Attack3");
                        yield return null;
                        break;
                default: break;
                        
            }
        }
        else
        {

        }
        Attackroutine = null;
        yield return null;
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
            attackTime -= Time.deltaTime;

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
    
    [SerializeField]
    private int damage;
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
                    else   throw new NullReferenceException("monster component is not there");
                }
            }
            ACollider.gameObject.SetActive(false);
            CharacterInput.instance.freeze = false;
        }
        yield return null;
    }


    public int FullHp;
    private int curHp;
    public void GetDamage(int i)
    {
        curHp -= i;
        if (curHp <= 0) Death();

        playerMove.ani.SetTrigger("Hit");
        // 데미지 관련
        StartCoroutine(HitRoutine());
    }
    public void Death()
    {
        playerMove.ani.SetTrigger("Death");
        playerMove.freeze = true;
        GameManager.Instance.GameOver();
    }

    public void GiveDamage(int i, Monster monster)
    {
        monster.GetDamage(damage);
    }

    IEnumerator HitRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        yield return null;
        playerMove.FreezeStop();
    }

    public void GiveDamage(int i)
    {
        throw new NotImplementedException();
    }
}
