using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public List<Item> AllItemList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetItemList();
    }
    private void GetItemList()
    {
        // TODO : ������ ����Ʈ �޾ƿ���
    }
}
