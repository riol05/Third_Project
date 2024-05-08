using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Vector3[] SpawnPos;

    public Monster CommonMonsterPrefab;
    public Monster eliteMonsterPrefab;
    public int TotalMonster;
    public float SpawnCD;
    public float TotalCD;
    private void Start()
    {
        SpawnMonster();
        SpawnCD = TotalCD;
    }
    private void FixedUpdate()
    {
        if (TotalMonster == 0)
            SpawnCD -= Time.deltaTime;

        if (SpawnCD <= 0)
        {
            SpawnMonster();
            SpawnCD = TotalCD;
        }
    }
    public void SpawnMonster()
    {
        foreach(Vector3 pos in SpawnPos)
        {
            //Vector3 pos = new Vector3(UnityEngine.Random.Range(-3, 10),0, UnityEngine.Random.Range(-3, 10));

            int i = UnityEngine.Random.Range(0, 3);
            if (i == 3)
            {
                var monster = ObjectPoolingManager.Instance.SpawnMonster(eliteMonsterPrefab, pos);
                monster.GetComponent<Monster>().SetHome(this);
            }
            else
            {
                var monster = ObjectPoolingManager.Instance.SpawnMonster(CommonMonsterPrefab, pos);
                monster.GetComponent<Monster>().SetHome(this);
            }
            ++TotalMonster;
        }
    }
}
