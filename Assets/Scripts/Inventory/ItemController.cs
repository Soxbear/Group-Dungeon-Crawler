using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public string itemName;
    [TextArea(2, 10)]
    public string itemDescription;
    public Sprite itemSprite;
    //public bool interactable;

    private void Awake()
    {
        itemSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnMouseDown()
    {
        InventoryController.UpdateInventory(gameObject, itemName, itemDescription, itemSprite, true);
    }
}
