using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    TrailRenderer trajectory; // TODO :  Ʈ���� ������ ���� �س��� ���


    public int damage;
    public float speed = 2f;
    [SerializeField]
    LayerMask target; // Ÿ�ٸ� �������� �ְԵ�

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
        transform.position = transform.forward * speed * Time.deltaTime;
        
        if(Physics.Raycast(transform.position,Vector3.forward,out hit, 0.1f,target))
        {
            hit.transform.GetComponent<IDamageable>().GiveDamage(damage);
            DestroyBullet();
        }
        else if (Physics.Raycast(transform.position, Vector3.forward, 0.1f))
            DestroyBullet();
    }

    private void DestroyBullet() // TODO : ������Ʈ Ǯ�� ���
    {
        gameObject.SetActive(false);
    }
}