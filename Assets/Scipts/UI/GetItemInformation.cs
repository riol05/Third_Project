using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetItemInformation : MonoBehaviour
{
    public ItemInformationView TextPrefab;
    private Queue<string> InfoString = new Queue<string>();
    private List<Transform> PrefabParent;
    public void ShowInfo(string s)
    {
        s = $"{s}을/를 얻었습니다.";
        InfoString.Enqueue(s);
        StartCoroutine(InfoRoutine());
    }
    IEnumerator InfoRoutine()
    {
        string s;
        while (InfoString.TryDequeue(out s))
        {
            var t = ObjectPoolingManager.Instance.SpawnText(TextPrefab,transform);
            t.GetComponent<TextMeshProUGUI>().text = s;
            PrefabParent.Add(t.transform);
            yield return new WaitForSeconds(1f);
        }
        foreach (var t in PrefabParent)
        {
            ObjectPoolingManager.Instance.DespawnText(t.GetComponent<ItemInformationView>());
        }
    }
}
/*
 * tt= texts0.text;
 * texts0.text = ss;
 * texts1.text = tt;
 * tt = texts1.text;
 * texts1.text = ss;
 * texts2.text = tt;
 * 
 */
