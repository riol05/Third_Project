using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
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
            int k = Random.Range(3, 17);
            int l = Random.Range(3, 20) * 18;
            Node node = PathFinder.instance.graphData.GetNode(k + l);
            if (!node.isOpen)
            {
                while (!node.isOpen)
                {
                    node = PathFinder.instance.graphData.GetNode(node.NumberForNode - 1);
                }
            }
            Vector3 monsterStartPos = node.Pos +new Vector3(0,0.5f,0); // node에 풀어놓는다.
            
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
