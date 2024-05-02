using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
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

            StateMachineDic[stateMon]?.Invoke();
        }
        if (stateMon == MonsterState.Die)
        {
            //TODO : ������Ʈ Ǯ������ ����
        }
        //switch (stateMon)
        //{
        //    case MonsterState.Idle:
        //        break;
        //    case MonsterState.Hit:
        //        break;
        //    case MonsterState.Attack:
        //        break;
        //    case MonsterState.Patrol:
        //        break;
        //    case MonsterState.Chase:
        //        break;
        //    case MonsterState.BackHome:
        //        break;
        //    case MonsterState.Die:
        //        break;
        //}
        StateMachineDic[stateMon]?.Invoke();
        yield return null;
    }
    public void Idle()
    {

    }
    public void Attack()
    {

    }
    public void BackHome()
    {

    }
    public void Chase()
    {

    }
    public void Patrol()
    {

    }
    public void Hit()
    {

    }

    public void Die()
    {
        // �Ŵ����� ���� ������ ����Ʈ�� ������ dropItemPrefab�� ������ Int�� ��ġ�ϴ� ������ ID�� �������� �����
        // ���Ͱ� ����Ҷ��� �ʿ� ������, ĳ���Ͱ� ��� �������� ȹ���Ҷ��� �ʿ�
        var item = Instantiate(dropItem.itemOnField); // �θ�� ���ӸŴ��� �ȿ� �ִ� �ʵ� ������ �θ� ��ü�� ��
        item.transform.position = transform.position;

        item.gameObject.SetActive(true);
    }
}
