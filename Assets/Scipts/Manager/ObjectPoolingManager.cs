using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public DropItem SpawnItemDrop(DropItem item, Vector3 dir) => LeanPool.Spawn(item, dir, Quaternion.identity) ;
    public void DespawnItem(DropItem go) => LeanPool.Despawn(go);
    public Monster SpawnMonster(Monster monster, Vector3 dir,Transform parent) => LeanPool.Spawn(monster,dir,Quaternion.identity,parent);
    public void DespawnMonster(Monster monster) => LeanPool.Despawn(monster);
    public Projectile SpawnBullet(Projectile bullet, Vector3 dir) => LeanPool.Spawn(bullet,dir, Quaternion.identity);
    public void DespawnBullet(Projectile bullet) => LeanPool.Despawn(bullet);
    public ItemInformationView SpawnText(ItemInformationView info, Transform parent) => LeanPool.Spawn(info,parent);
    public void DespawnText(ItemInformationView info) => LeanPool.Despawn(info);
    // TODO : 스킬도 오브젝트 풀링할것
}
