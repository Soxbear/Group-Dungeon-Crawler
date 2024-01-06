using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public bool interactable = false;
}
