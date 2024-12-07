using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryBackground;
    public GameObject slotPrefab;
    public const int numSlots = 5;
    public Image[] itemImages = new Image[numSlots];
    Item[] items = new Item[numSlots];
    GameObject[] slots = new GameObject[numSlots];

    // Start is called before the first frame update
    public void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateSlots()
    {
        if (slotPrefab != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(inventoryBackground.transform);
                slots[i] = newSlot;
                itemImages[i] = newSlot.transform.GetComponent<Slot>().itemImage;
            }
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable == true)
            {
                items[i].quantity = items[i].quantity + 1;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                TextMeshProUGUI quantityText = slotScript.qtyText;
                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();
                Debug.Log("setting shouldDisappear to true");

                if (items[i].quantity >= 10)
                {
                    Debug.Log("You win!");
                    AudioManager.instance.PlaySFX(3);
                }

                return true;
            }


            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return true;
            }
        }
        return false;
    }
}