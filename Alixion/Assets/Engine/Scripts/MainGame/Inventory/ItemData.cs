using System;
using UnityEngine;

[Serializable]
public class ItemData : MonoBehaviour
{
    public string objectName;
    public PROPERTYTYPE propertyType;
    public int point;
    public int count;

    public string itemName;
    public string itemInfo;
    public Sprite itemSprite;
    public Sprite shadowSprite;
}
