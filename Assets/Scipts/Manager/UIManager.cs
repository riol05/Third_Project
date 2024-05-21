using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    
    public AlertInformation alert;
    public GetItemInformation Item;
    private void Awake()
    {
        instance = this;
    }
    public void alertShow(string s)
    {
        alert.gameObject.SetActive(true);
        alert.Alert(s);
    }

    public void GameOver()
    {

    }
}