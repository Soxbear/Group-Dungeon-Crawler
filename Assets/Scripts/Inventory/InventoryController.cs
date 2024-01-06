using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private static InventoryController instance;
    public static InventoryController Instance
    {
        get
        {
            if (!instance)
            {
                GameObject inventoryController = new GameObject("InventoryController");
                inventoryController.AddComponent<InventoryController>();
            }

            return instance;
        }
    }

    public RectTransform hideableInvSlotsPos;
    bool invSlotsHidden = true;

    public static List<Item> items = new List<Item>();
    
    public static List<Image> slotImages = new List<Image>();
    public static List<Button> slotButtons = new List<Button>();
    public List<GameObject> pSlotObjects;

    public static Sprite emptySlotSprite;
    public Sprite pEmptySlotSprite;

    void Awake()
    {
        instance = this;
        emptySlotSprite = pEmptySlotSprite;

        for (int i = 0; i < 15; i++)
        {
            items.Add(new Item());
            //slotObjects.Add(pSlotObjects[i]);
            slotImages.Add(pSlotObjects[i].GetComponent<Image>());
            slotButtons.Add(pSlotObjects[i].GetComponent<Button>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && invSlotsHidden)
        {
            hideableInvSlotsPos.position = new Vector2(960, 540);
            invSlotsHidden = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !invSlotsHidden)
        {
            hideableInvSlotsPos.position = new Vector2(0, 0);
            invSlotsHidden = true;
        }
    }

    public static void UpdateInventory(GameObject item, string itemName, string itemDescription, Sprite itemSprite, bool interactable)
    {
        bool inventoryFull = true;

        for (int i = 0; i < 15; i++)
        {
            if (!items[i].interactable)
            {
                inventoryFull = false;
            }
        }

        if (!inventoryFull)
        {
            Destroy(item);
        }

        for (int i = 0; i < 15; i++)
        {
            if (!items[i].interactable)
            {
                items[i].interactable = interactable;
                items[i].itemDescription = itemDescription;
                items[i].itemName = itemName;
                slotImages[i].sprite = itemSprite;
                slotButtons[i].interactable = interactable;
                break;
            }
        }
    }

    public static void UseItem(int itemNumber)
    {
        //Switch statement should go here in the future

        items[itemNumber] = new Item();
        slotImages[itemNumber].sprite = emptySlotSprite;
        slotButtons[itemNumber].interactable = false;
    }
}
