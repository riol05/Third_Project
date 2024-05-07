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
        //transform.position = homepos; 여기서 homePos는 SpawnManager에 있는 위치 주변을 배정
    }
    private void OnEnable()
    {
        stateMon = MonsterState.Idle;
        transform.position = HomePos;
    }
    IEnumerator UpdateStateMachine() // 몬스터 상태머신 구현
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
            //TODO : 오브젝트 풀링으로 죽음
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
        var player = hitPlayer.transform.GetComponent<PlayerInteractInput>();// 플레이어 공격
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
        // 매니저를 통해 아이템 리스트를 가져와 dropItemPrefab의 아이템 Int와 일치하는 아이템 ID를 가져오게 만든다
        // 몬스터가 드랍할때는 필요 없지만, 캐릭터가 드랍 아이템을 획득할때는 필요
        // 경험치와 골드는 바로 인벤토리로 들어가게된다.
        var item = Instantiate(dropItem.itemOnField); // 부모는 게임매니저 안에 있는 필드 아이템 부모 객체를 둠
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
