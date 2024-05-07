using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public enum MonsterState
    {
        Idle,
        Hit,
        Attack,
        Patrol,
        Chase,
        BackHome,
        Die
    }

    private Vector3 HomePos;
    public List<Item> dropTable;
    public Item dropItem;
    private MonsterState stateMon;
    private Dictionary<MonsterState, Action> StateMachineDic;


    public int curHp;
    private int fullHp;

    private float Speed;
    public float moveSpeed;
    private float comeBackSpeed;
    public float maxPatrolRadius;
    public float maxChaseRadius;

    public LayerMask playerLayer;
    private void Awake()
    {
        StateMachineDic = new Dictionary<MonsterState, Action>()
        {
            {MonsterState.Idle, Idle},
            {MonsterState.Attack, Attack },
            {MonsterState.Patrol, Patrol },
            {MonsterState.Chase, Chase },
            {MonsterState.BackHome, BackHome },
            {MonsterState.Die, Die },
            {MonsterState.Hit, Hit },
        };
    }
    private void Start()
    {
        //transform.position = homepos; ���⼭ homePos�� SpawnManager�� �ִ� ��ġ �ֺ��� ����
    }
    private void OnEnable()
    {
        stateMon = MonsterState.Idle;
        transform.position = HomePos;
    }
    IEnumerator UpdateStateMachine() // ���� ���¸ӽ� ����
    {
        while (stateMon != MonsterState.Die)
        {
            if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hitPlayer, playerLayer))
                stateMon = MonsterState.Attack;

            else if (Physics.SphereCast(transform.position, 5f, transform.forward, out hitPlayer, playerLayer))
                stateMon = MonsterState.Chase;
            if(stateMon == MonsterState.Chase || stateMon == MonsterState.Attack)
            {
                if (Physics.SphereCast(transform.position, 20f, transform.forward, out hitPlayer, playerLayer))
                    stateMon = MonsterState.BackHome;
            }

            if (stateMon == MonsterState.BackHome)
            {
                yield return new WaitForSeconds(4f);
            }
            StateMachineDic[stateMon]?.Invoke();
        }
        if (stateMon == MonsterState.Die)
        {
            //TODO : ������Ʈ Ǯ������ ����
        }
        StateMachineDic[stateMon]?.Invoke();
        yield return null;
    }
    RaycastHit hitPlayer;
    private void Idle()
    {
        
    }
    private void Attack()
    {
        var player = hitPlayer.transform.GetComponent<PlayerInteractInput>();// �÷��̾� ����
        GiveDamage();
    }
    private void BackHome()
    {
        Speed = comeBackSpeed;
    }
    private void Chase()
    {
        Speed = moveSpeed;
    }
    private void Patrol()
    {
        Speed = moveSpeed;
    }
    private void Hit()
    {
        Speed = 0;
        stateMon = MonsterState.Chase;
    }

    public void Die()
    {
        // �Ŵ����� ���� ������ ����Ʈ�� ������ dropItemPrefab�� ������ Int�� ��ġ�ϴ� ������ ID�� �������� �����
        // ���Ͱ� ����Ҷ��� �ʿ� ������, ĳ���Ͱ� ��� �������� ȹ���Ҷ��� �ʿ�
        // ����ġ�� ���� �ٷ� �κ��丮�� ���Եȴ�.
        var item = Instantiate(dropItem.itemOnField); // �θ�� ���ӸŴ��� �ȿ� �ִ� �ʵ� ������ �θ� ��ü�� ��
        item.GetComponent<DropItem>().itemID = dropItem.itemID;
        item.transform.position = transform.position;
        item.gameObject.SetActive(true);
    }

    public void GetDamage()
    {
        stateMon = MonsterState.Hit;
    }

    public void GiveDamage()
    {
    }
}
