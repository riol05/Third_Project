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
        // TODO : 아이템 리스트 받아오기
    }
}
