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
        if(instance != null)
        instance = this;
    }
}