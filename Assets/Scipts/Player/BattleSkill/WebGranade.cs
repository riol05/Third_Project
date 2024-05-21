using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGranade : MonoBehaviour, ISkillAble
{
    float Cd;
    float Tcd;
    bool isOn;
    int damage = 10;
    RaycastHit[] hits;
    LayerMask MonsterMask;
    public void Damage()
    {
        hits = Physics.SphereCastAll(transform.position, 8f, Vector3.up, 0, MonsterMask);
        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.GetComponent<Monster>()) continue;

            hit.collider.GetComponent<Monster>().GetDamage(damage);
            //hit.collider.transform.position = ; // hit posiiton �� transform.pos�� �Űܿ�
        }
    }

    public void isCoolDownNow()
    {
        throw new System.NotImplementedException();
    }

    public void SkillActivate()
    {
        throw new System.NotImplementedException();
    }

    public void SkillReady()
    {
        throw new System.NotImplementedException();
    }
}
