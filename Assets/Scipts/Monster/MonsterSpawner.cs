using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSpawner : MonoBehaviour
{
    public Vector3[] SpawnPos;

    public Monster CommonMonsterPrefab;
    public Monster eliteMonsterPrefab;
    public int TotalMonster;
    public int spawnMonster;
    public float SpawnCD ;
    public float TotalCD = 20f;
    private void Start()
    {
        SpawnMonster();
        SpawnCD = TotalCD;
    }
    private void FixedUpdate()
    {
        if (TotalMonster == 0)
            SpawnCD -= Time.deltaTime;

        if (SpawnCD <= 0 && TotalMonster <= 0)
        {
            SpawnMonster();
            SpawnCD = TotalCD;
        }
    }
    public void SpawnMonster()
    {
        for(int i = 0; i <=spawnMonster; ++i)
        {
            Vector3 monsterStartPos = new Vector3(UnityEngine.Random.Range(-10, 10),1f, UnityEngine.Random.Range(-10, 10));
            monsterStartPos = transform.position + monsterStartPos;
            int r = UnityEngine.Random.Range(0, 3);
            if (r == 3)
            {
                var monster = ObjectPoolingManager.Instance.SpawnMonster(eliteMonsterPrefab, monsterStartPos,transform);
            }
            else
            {
                var monster = ObjectPoolingManager.Instance.SpawnMonster(CommonMonsterPrefab, monsterStartPos,transform);
            }
        }
        TotalMonster = spawnMonster;
    }
}
