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
    public Monster SpawnMonster(Monster monster, Vector3 dir) => LeanPool.Spawn(monster,dir,Quaternion.identity);
    public void DespawnMonster(Monster monster) => LeanPool.Despawn(monster);
    // TODO : ��ų�� ������Ʈ Ǯ���Ұ�
}
