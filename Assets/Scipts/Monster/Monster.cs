using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public enum MonsterState
{
    Idle,
    Hit,
    Attack,
    Patrol,
    Chase,
    BackHome,
    Sturn,
    Die
}
public enum ElevatorState
{
    elevator,
    None
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

    private Vector3 dirPos;
    private bool patrolStart = false;

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
    public LayerMask playerLayer;
    
    public MonsterType typeMONSTER;
    public ElevatorState elevatorState;
    public Projectile bulletPrefab;
    public Transform gunPivot;

    private void Awake()
    {
        StateMachineDic = new Dictionary<MonsterState, Action>()
        {
            {MonsterState.Idle, Idle},
            {MonsterState.Attack, Attack },
            {MonsterState.Patrol, Patrol },
            {MonsterState.Chase, Chase },
            {MonsterState.BackHome, Idle },
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
            if (elevatorState == ElevatorState.elevator)
                
                
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

            if(stateMon != MonsterState.Patrol)
            {
                patrolStart = false;
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
        dirPos = transform.position;
    }

    private void BackHome()
    {
        Speed = comeBackSpeed;
        curHp = fullHp;
        dirPos = HomePos.transform.position;
    }
    private void Chase()
    {
        Speed = moveSpeed;
        dirPos = hitPlayer.transform.GetComponent<PlayerInteractInput>().transform.position;
    }

    private void Patrol()
    {
        Speed = moveSpeed;
        if (!patrolStart)
        {
            patrolStart = true;
            Vector3 vec = new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
            dirPos = transform.position + vec;
        }
        if (Vector3.Distance(dirPos, transform.position) >= 2f)
        {
            patrolStart = false;
            stateMon = MonsterState.Idle;
        }
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
        }
        else if (typeMONSTER == MonsterType.Elite)
        {
            ShotProjectile(damage);
        }
    }

    private void ShotProjectile(int i) // 
    {
        var player = hitPlayer.transform.GetComponent<PlayerInteractInput>();// �÷��̾� ����
        transform.LookAt(player.transform);
        // ���� ������ �� �Ѿ� �߻�.
        // �Ѿ˿� ���� �������� �޾� �߻��Ѵ�.
        var bullet = ObjectPoolingManager.Instance.SpawnBullet(bulletPrefab,gunPivot.position);
        bullet.GetComponent<Projectile>().damage = i;
        bullet.gameObject.SetActive(true);
    }

    public AttackCollider ACollider;
    IEnumerator AttackCollider(float f)
    {
        RaycastHit hit;
        yield return new WaitForSeconds(0.1f);
        if (Physics.SphereCast(transform.position, 2f, transform.forward, out hit))
        {
            //RaycastHit[] hits = Physics.SphereCastAll(transform.position, 2f, transform.forward, 0f,monsterMask);
            ACollider.gameObject.SetActive(true);
            yield return new WaitForSeconds(f);

            if (ACollider.targets.Count > 0)
            {
                Transform t;
                while (ACollider.targets.TryDequeue(out t))
                {
                    PlayerInteractInput player = t.GetComponent<PlayerInteractInput>();
                    //if (player != null) GiveDamage(damage, player);

                    //else throw new NullReferenceException("monster component is not there");
                }
            }
            ACollider.gameObject.SetActive(false);
            yield return null;
        }
    }
    public void Die()
    {
        // ���Ͱ� ����Ҷ��� �ʿ� ������, ĳ���Ͱ� ��� �������� ȹ���Ҷ��� �ʿ�
        // ���� �ٷ� �κ��丮�� ���Եȴ�. ����ġ ���� X
        
        int i = UnityEngine.Random.Range(0,dropTable.Length - 1);
        dropItem = dropTable[i];
        var item = ObjectPoolingManager.Instance.SpawnItemDrop(dropItem,transform.position); // �θ�� ���ӸŴ��� �ȿ� �ִ� �ʵ� ������ �θ� ��ü�� ��
        //item.GetComponent<DropItem>().itemID = dropItem.itemID;
        item.gameObject.SetActive(true);
        --HomePos.TotalMonster;
        
    }


    public void GetDamage(int i)
    {
        curHp -= i;
        if (curHp <= 0)  stateMon = MonsterState.Die;
        else  stateMon = MonsterState.Hit;
    }

    public void GiveDamage(int i,PlayerInteractInput player)
    {
        //player.Getdamage(i);
    }

    public void GiveDamage(int i)
    {
        throw new NotImplementedException();
    }
}
