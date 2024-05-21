using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR.Haptics;

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
    private MonsterSpawner Home;
    public DropItem[] dropTable;
    public DropItem dropItem;
    private Dictionary<MonsterState, Action> StateMachineDic;

    private Vector3 dirPos;
    private bool patrolStart = false;

    [SerializeField]
    private AttackCollider AC;
    [SerializeField]
    private Animator anim;
    
    [SerializeField]
    private PathFollower pathFollow;

    public int curHp;
    private int fullHp;
    private float Speed;
    public float moveSpeed;
    private float comeBackSpeed;
    public float maxPatrolRadius;
    public float maxChaseRadius;

    private MonsterState prevState;
    public int damage;
    public LayerMask playerLayer;

    [SerializeField]
    private MonsterType typeMONSTER;
    [HideInInspector]
    public MonsterState stateMon;

    public Projectile bulletPrefab;
    public Transform gunPivot;

    private void Awake()
    {
        StateMachineDic = new Dictionary<MonsterState, Action>()
        {
            {MonsterState.Idle, Idle},
            {MonsterState.Attack, Attack },
            {MonsterState.Patrol, Patrol },
            {MonsterState.BackHome,BackHome },
            {MonsterState.Chase, Chase },            
            {MonsterState.Die, Die },
            {MonsterState.Hit, Hit },
        };
    }
    public void SetHome(MonsterSpawner home)
    {
        Home = home;
    }
    private void OnEnable()
    {
        stateMon = MonsterState.Idle;
        Home = transform.parent.GetComponent<MonsterSpawner>();
        pathFollow.target = Home.transform;
        StartCoroutine(UpdateStateMachine()); // 상태 머신
        //pathFollow.Follow(Home.transform.position, moveSpeed);
    }
    
    float IdleTime = 3f;
    float idleCd;
    IEnumerator UpdateStateMachine() // 몬스터 상태머신 구현
    {
        while (stateMon != MonsterState.Die)
        {
            if (stateMon != MonsterState.Patrol && patrolStart) patrolStart = false;
            if (patrolStart)
                stateMon = MonsterState.Patrol;

            if (stateMon == MonsterState.Idle)
            {
                Idle();
                if (idleCd >0) idleCd -= Time.deltaTime;
                if (idleCd < 0)
                {
                    idleCd = IdleTime;
                    stateMon = MonsterState.Patrol;
                    StateMachineDic[stateMon]?.Invoke();
                    yield return null;
                    continue;
                }
            }
            else idleCd = IdleTime;

            if (Vector3.Distance(Home.transform.position,transform.position) > 50) stateMon = MonsterState.BackHome;
            print(stateMon);
            
            if (stateMon == MonsterState.BackHome) 
            {
                BackHome();
                yield return null;
                continue;
            }
            else
            {
                if (typeMONSTER == MonsterType.Common)
                {
                    if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hitPlayer, playerLayer))
                        stateMon = MonsterState.Attack;
                    else if (Physics.SphereCast(transform.position, 5f, transform.forward, out hitPlayer, playerLayer))
                        stateMon = MonsterState.Chase;
                    else
                    {
                        stateMon = MonsterState.Idle;
                    }
                }
                else if (typeMONSTER == MonsterType.Elite)
                {
                    if (Physics.SphereCast(transform.position, 7f, transform.forward, out hitPlayer, playerLayer)) // 사거리임
                        stateMon = MonsterState.Attack;
                    else if (Physics.SphereCast(transform.position, 15f, transform.forward, out hitPlayer, playerLayer))
                        stateMon = MonsterState.Chase;
                    else stateMon = MonsterState.Idle;
                }
            }
            StateMachineDic[stateMon]?.Invoke();
            //anim.SetInteger("MonsterState",(int)stateMon); // TODO : 몬스터 애니메이션 만들자

            yield return null;
        }
        if (stateMon == MonsterState.Die)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            yield return new WaitForSeconds(3f);
            ObjectPoolingManager.Instance.DespawnMonster(this.transform.GetComponent<Monster>());
        }
        //anim.SetInteger("MonsterState", (int)stateMon);
        StateMachineDic[stateMon]?.Invoke();
        yield return null;
    }
    RaycastHit hitPlayer;
    private void Idle()
    {
        dirPos = transform.position;
        if (stateMon != prevState)
            pathFollow.Follow(dirPos, Speed);
        prevState = MonsterState.Idle;
    }

    private void BackHome()
    {
        curHp = fullHp;
        if (stateMon != prevState)
        {
            dirPos = Home.transform.position;
            Move(comeBackSpeed, dirPos);
            prevState = MonsterState.BackHome;
        }

        if (pathFollow.isCloseDir())
            stateMon = MonsterState.Idle;
    }
    static float chaseTCD = 3f;
    float chaseCD = 0;
    private void Chase()
    {
        chaseCD -= Time.deltaTime;
        if (stateMon != prevState || chaseCD <= 0)
        {
            Speed = moveSpeed;
            dirPos = hitPlayer.transform.GetComponent<PlayerInteractInput>().transform.position;
            chaseCD = chaseTCD;
            Debug.Log("Chasenow");
            pathFollow.Follow(dirPos, Speed);
            prevState = MonsterState.Chase;
        }
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
        if (stateMon != prevState)
            pathFollow.Follow(dirPos, Speed);
        if(pathFollow.isCloseDir())
        {
            stateMon = MonsterState.Idle;

        }
        prevState = MonsterState.Patrol;
    }
    private void Hit()
    {
        Speed = 0;
        stateMon = MonsterState.Idle;
    }
    private void Attack()
    {
        Speed = 0;
        if (hitPlayer.transform != null)
        {
            var player = hitPlayer.transform.GetComponent<PlayerInteractInput>();// 플레이어 공격
        }
        StartCoroutine(AttackCollider(0.5f));
    }

    private void ShotProjectile(int i) // 
    {
        var player = hitPlayer.transform.GetComponent<PlayerInteractInput>();// 플레이어 공격
        transform.LookAt(player.transform);
        // 라인 렌더러 로 총알 발사.
        // 총알에 라인 렌더러를 달아 발사한다.
        var bullet = ObjectPoolingManager.Instance.SpawnBullet(bulletPrefab,gunPivot.position);
        bullet.GetComponent<Projectile>().damage = i;
        bullet.gameObject.SetActive(true);
        stateMon = MonsterState.Chase;
    }
    public AttackCollider ACollider;
    IEnumerator AttackCollider(float f)
    {
        prevState = MonsterState.Attack;
        RaycastHit hit;
        yield return new WaitForSeconds(0.7f);

        if(typeMONSTER == MonsterType.Common)
        {
            if (Physics.SphereCast(transform.position, 2f, transform.forward, out hit))
            {
                //RaycastHit[] hits = Physics.SphereCastAll(transform.position, 2f, transform.forward, 0f,monsterMask);
                ACollider.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);

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
            }
            else
            {
                ShotProjectile(damage);
            }

            stateMon = MonsterState.Chase;
                yield return null;
        }
    }
    public void Die()
    {
        // 몬스터가 드랍할때는 필요 없지만, 캐릭터가 드랍 아이템을 획득할때는 필요
        // 골드는 바로 인벤토리로 들어가게된다. 경험치 구현 X
        
        int i = UnityEngine.Random.Range(0,dropTable.Length - 1);
        dropItem = dropTable[i];
        var item = ObjectPoolingManager.Instance.SpawnItemDrop(dropItem,transform.position); // 부모는 게임매니저 안에 있는 필드 아이템 부모 객체를 둠
        //item.GetComponent<DropItem>().itemID = dropItem.itemID;
        item.gameObject.SetActive(true);
        --Home.TotalMonster;
        
    }
    
    public void Move(float speed, Vector3 pos)
    {
        Speed = comeBackSpeed;
        dirPos = pos;
        pathFollow.Follow(dirPos, Speed);
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