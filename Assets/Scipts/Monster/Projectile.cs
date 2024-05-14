using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    TrailRenderer trajectory; // TODO :  트레일 렌더러 설정 해놓고 사용


    public int damage;
    public float speed = 2f;
    [SerializeField]
    LayerMask target; // 타겟만 데미지를 주게됨 // 스킬, 시전자에 따라 다른 prefab 사용

    RaycastHit hit;
    private void Start()
    {
        trajectory = GetComponent<TrailRenderer>();
    }
    private void OnDisable()
    {
        trajectory.Clear();
    }
    private void Update()
    {
        transform.position = Vector3.forward * speed * Time.deltaTime;
        
        if(Physics.Raycast(transform.position,Vector3.forward,out hit, 0.1f,target))
        {
            hit.transform.GetComponent<IDamageable>().GiveDamage(damage); // TODO : 투사체 대미지
            DestroyBullet();
        }
        else if (Physics.Raycast(transform.position, Vector3.forward, 0.1f))
            DestroyBullet();
    }

    private void DestroyBullet() // TODO : 오브젝트 풀링 사용
    {
        gameObject.SetActive(false);
    }
}