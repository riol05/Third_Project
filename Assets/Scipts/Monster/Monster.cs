using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

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
public enum MonsterType
{
    Common,
    Elite,
}
public class Monster : MonoBehaviour, IDamageable
{
    private MonsterSpawner HomePos;
    public DropItem[] dropTable;
    public DropItem dropItem;
    private MonsterState stateMon;
    private Dictionary<MonsterState, Action> StateMachineDic;

    [SerializeField]
    private AttackCollider AC;
    [SerializeField]
    private Animator anim;

    public int curHp;
    private int fullHp;

    private float Speed;
    public float moveSpeed;
    private float comeBackSpeed;
    public float maxPatrolRadius;
    public float maxChaseRadius;

    public int damage;
    public MonsterType typeMONSTER;
    public LayerMask playerLayer;
    public Projectile bulletPrefab;
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
    public void SetHome(MonsterSpawner home)
    {
        HomePos = home;
    }
    private void OnEnable()
    {
        stateMon = MonsterState.Idle;
    }
    IEnumerator UpdateStateMachine(int i) // ���� ���¸ӽ� ����
    {
        while (stateMon != MonsterState.Die)
        {
            if (typeMONSTER == MonsterType.Common)
            {
                if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hitPlayer, playerLayer))
                    stateMon = MonsterState.Attack;
                else if (Physics.SphereCast(transform.position, 5f, transform.forward, out hitPlayer, playerLayer))
                    stateMon = MonsterState.Chase;
                
                if (stateMon == MonsterState.Chase || stateMon == MonsterState.Attack)
                {
                    if (Physics.SphereCast(transform.position, 13f, transform.forward, out hitPlayer, playerLayer))
                        stateMon = MonsterState.BackHome;
                }
            }
            if(typeMONSTER == MonsterType.Elite)
            {
                if (Physics.SphereCast(transform.position, 7f, transform.forward, out hitPlayer, playerLayer)) // ��Ÿ���
                    stateMon = MonsterState.Attack;
                else if (Physics.SphereCast(transform.position, 15f, transform.forward, out hitPlayer, playerLayer))
                    stateMon = MonsterState.Chase;

                if (stateMon == MonsterState.Chase || stateMon == MonsterState.Attack)
                {
                    if (Physics.SphereCast(transform.position, 25f, transform.forward, out hitPlayer, playerLayer))
                        stateMon = MonsterState.BackHome;
                }
            }

            if (stateMon == MonsterState.BackHome)
            {
                yield return new WaitForSeconds(4f);
            }
            StateMachineDic[stateMon]?.Invoke();
            anim.SetInteger("MonsterState",(int)stateMon); // TODO : ���� �ִϸ��̼� ������
        }
        if (stateMon == MonsterState.Die)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            yield return new WaitForSeconds(3f);
            ObjectPoolingManager.Instance.DespawnMonster(this);
        }
        anim.SetInteger("MonsterState", (int)stateMon); // TODO : ���� �ִϸ��̼� ������
        StateMachineDic[stateMon]?.Invoke();
        yield return null;
    }
    RaycastHit hitPlayer;
    private void Idle()
    {
        
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
    private void Attack()
    {
        var player = hitPlayer.transform.GetComponent<PlayerInteractInput>();// �÷��̾� ����

        if (typeMONSTER == MonsterType.Common)
        {
            GiveDamage();
        }
        else if (typeMONSTER == MonsterType.Elite)
        {
            ShotProjectile(damage);
        }
    }

    public void ShotProjectile(int i) // 
    {
        var player = hitPlayer.transform.GetComponent<PlayerInteractInput>();// �÷��̾� ����
        transform.LookAt(player.transform);
        // ���� ������ �� �Ѿ� �߻�.
        // �Ѿ˿� ���� �������� �޾� �߻��Ѵ�.
        var bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<Projectile>().damage = i;
        bullet.gameObject.SetActive(true);
    }

    IEnumerator AttackCollider(int i)
    {

        yield return null;
    }
    public void Die()
    {
        // ���Ͱ� ����Ҷ��� �ʿ� ������, ĳ���Ͱ� ��� �������� ȹ���Ҷ��� �ʿ�
        // ���� �ٷ� �κ��丮�� ���Եȴ�. ����ġ ���� X
        // 
        int i = UnityEngine.Random.Range(0,dropTable.Length - 1);
        dropItem = dropTable[i];
        var item = ObjectPoolingManager.Instance.SpawnItemDrop(dropItem,transform.position); // �θ�� ���ӸŴ��� �ȿ� �ִ� �ʵ� ������ �θ� ��ü�� ��
        //item.GetComponent<DropItem>().itemID = dropItem.itemID;
        item.gameObject.SetActive(true);
        --HomePos.TotalMonster;
    }


    public void GetDamage(int i)
    {
        stateMon = MonsterState.Hit;
    }

    private void GiveDamage()
    {
    }

    public void GiveDamage(int i)
    {
        throw new NotImplementedException();
    }
}
