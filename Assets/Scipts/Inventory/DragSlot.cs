using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public Slot dragslot;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image backGround;


    private void Start()
    {
        instance = this;
    }
    public void DragSetImage(Image SetImage)
    {
        itemImage.sprite = SetImage.sprite;
        SetColor(1);
    }

    public void SetColor(float alpha)
    {
        Color color = itemImage.color;
        Color c1 = backGround.color;
        color.a = alpha;
        c1.a = alpha;
        itemImage.color = color;
        backGround.color = c1;
    }
}