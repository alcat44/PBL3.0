using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public List<GameObject> instantiatedItems = new List<GameObject>();

    public Transform ItemContent;
    public GameObject InventoryItem, Inventory, Info, pickupText, dropText;
    public GameObject Serundeng, Telor, BerasKetan, Lantern, Kertas, KerakTelor;
    public GameObject itemInstance;
    public Transform player;
    public Vector3 itemPlacementPosition;
    public bool isInventoryOpen = false;
    public bool equip = false;
    public bool Reward = false;
    public int Index, Id;

    private void Awake()
    {
        Instance = this;
        Inventory.SetActive(false);
    }

    public void Add(Item item)
    {
        Items.Add(item);
        ListItems();
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        ListItems();
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemNo = obj.transform.Find("ItemNo").GetComponent<TextMeshProUGUI>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemNo.text = Items.IndexOf(item).ToString();
        }
    }

    public void UpdateItemInformation(Item item)
    {
        if (Info != null)
        {
            var itemInformation = Info.transform.Find("ItemInformation").GetComponent<TextMeshProUGUI>();
            var information = Info.transform.Find("Information").GetComponent<TextMeshProUGUI>();

            if (itemInformation != null)
            {
                itemInformation.text = item.itemName;
            }
            else
            {
                Debug.LogError("ItemInformation TextMeshProUGUI component not found in Info.");
            }

            if (information != null)
            {
                information.text = item.itemInformation;
            }
            else
            {
                Debug.LogError("Information TextMeshProUGUI component not found in Info.");
            }
        }
        else
        {
            Debug.LogError("Info GameObject is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        //Id = InventoryManager.Instance.Items[Index].id;
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseItem(i);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Unequip();
            }
        }

        
        
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        Inventory.SetActive(isInventoryOpen);
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Items.Count)
        {
            Debug.LogError("Invalid inventory slot index.");
            return;
        }

        var item = Items[slotIndex];
        Index = slotIndex;
        Id = InventoryManager.Instance.Items[Index].id;
        if (item != null)
        {
            Debug.Log($"Using item: {item.itemName}");

            foreach (var instantiatedItem in instantiatedItems)
            {
                if (instantiatedItem != null)
                {
                    Destroy(instantiatedItem);
                }
            }
            instantiatedItems.Clear();

            if (item.prefab != null && player != null)
            {
                itemInstance = Instantiate(item.prefab, player.position, player.rotation);
                itemInstance.transform.SetParent(player);
                

                instantiatedItems.Add(itemInstance);

                ItemPickUp itemPickUp = itemInstance.GetComponent<ItemPickUp>();
                //ItemDrop itemDrop = itemInstance.GetComponent<ItemDrop>();

                itemPickUp.interactable = false;
                //itemDrop.interactable = false;
                if (itemPickUp != null)
                {
                    //itemPickUp.enabled = false;
                    //itemDrop.enabled = true;
                    //itemPickUp.inventoryManager = this;
                    itemPickUp.pickupText = pickupText;
                    //itemDrop.dropText = dropText;
                    //itemDrop.dropText.SetActive(false);
                    itemPickUp.pickupText.SetActive(false);
                }
                else
                {
                    Debug.LogError("ItemPickUp script not found on item instance.");
                }

                //if (itemDrop != null)
                //{
                    //itemDrop.inventoryManager = this;
                    //itemDrop.dropText = dropText;
                    
                    //itemDrop.dropText.SetActive(false);
                //}
               // else
                //{
                    //Debug.LogError("ItemDrop script not found on item instance.");
                //}

                equip = true;
                Info.SetActive(true);
            }
            else
            {
                Debug.LogError("Item prefab or spawn point not set.");
            }

            UpdateItemInformation(item);
        }
    }

    private void Unequip()
    {
        foreach (var instantiatedItem in instantiatedItems)
        {
            if (instantiatedItem != null)
            {
                Destroy(instantiatedItem);
            }
        }
        Id = 0;
        Index = 0;
        Info.SetActive(false);
        instantiatedItems.Clear();
        equip = false;
    }

    public void PutBaju(Vector3 position)
    {
        if (instantiatedItems.Count > 0)
        {
            GameObject itemToPlace = instantiatedItems[0];
            itemToPlace.transform.SetParent(null);
            itemToPlace.transform.position = position;

            instantiatedItems.RemoveAt(0);
            instantiatedItems.Add(itemToPlace); // Add the placed item back to the list
        }
        else
        {
            Debug.LogError("No item to place.");
        }
    }

    
}