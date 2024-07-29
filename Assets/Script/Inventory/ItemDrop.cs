using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public int Index = 0, id, dropIndex;
    public Item item;
    public List<GameObject> droppedItems = new List<GameObject>();

    [Header("Game Object")]
    public GameObject itemObject;
    public GameObject itemDrop;
    public GameObject KerakTelor;
    public GameObject KertasMusik;
    public GameObject lantai31;
    public GameObject lantai32;
    public GameObject lantai33;

    [Header("Position")]
    public Transform player;
    public Vector3 itemPlacementPosition;

    [Header("Text")]
    public GameObject dropText;

    [Header("Bool")]
    public bool interactable = false;
    public bool Misi1 = false;
    public bool Misi2 = false;

    void Drop(int Index)
    {
        dropIndex = droppedItems.Count;
        
        var item = InventoryManager.Instance.Items[Index];
        id = InventoryManager.Instance.Id;

        InventoryManager.Instance.Remove(item);
        itemDrop = Instantiate(itemObject, itemPlacementPosition, Quaternion.identity);
        droppedItems.Add(itemDrop);
        Destroy(itemObject);
        //Destroy(gameObject);
        interactable = false;
        
        foreach (var droppedItem in droppedItems)
        {
            ItemPickUp itemPickUp = droppedItem.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                itemPickUp.enabled = false;
            }
        }
        
    }

    void DropLantern(int Index)
    {
        var item = InventoryManager.Instance.Items[Index];
        InventoryManager.Instance.Remove(item);
        
        itemDrop = Instantiate(itemObject, new Vector3(player.position.x, player.position.y - 2, player.position.z -1), Quaternion.identity);
        droppedItems.Add(itemDrop);
        Destroy(itemObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("KerakTelorPlacement") && (InventoryManager.Instance.Id == 7|| InventoryManager.Instance.Id == 8 || InventoryManager.Instance.Id == 9))
        {
            if (InventoryManager.Instance.equip)
            {
                dropText.SetActive(true);
                interactable = true;
                itemPlacementPosition = other.transform.position;
            }
        }

        if (other.CompareTag("Misi2") && (InventoryManager.Instance.Id == 10))
        {
            if (InventoryManager.Instance.equip)
            {
                dropText.SetActive(true);
                interactable = true;
                itemPlacementPosition = other.transform.position;
            }
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("KerakTelorPlacement") || (other.CompareTag("Misi2")))
        {
            dropText.SetActive(false);
            interactable = false;
        }
        
    }

    private void CheckRewardConditions()
    {
        
        if (!Misi1 && droppedItems[0] != null && droppedItems[1] != null && droppedItems[2] != null)
        {
            
            if (Vector3.Distance(droppedItems[0].transform.position, itemPlacementPosition) < 0.5f &&
                Vector3.Distance(droppedItems[1].transform.position, itemPlacementPosition) < 0.5f &&
                Vector3.Distance(droppedItems[2].transform.position, itemPlacementPosition) < 0.5f)
            {
                Debug.Log("All objects are in position. Instantiating new object.");
                Instantiate(KerakTelor, new Vector3(-29.71f, 3, 4.65f), Quaternion.Euler(90, 0, 45));
                Misi1 = true;
                Destroy(droppedItems[0]);
                Destroy(droppedItems[1]);
                Destroy(droppedItems[2]);
            }
        }

        if (!Misi2 && droppedItems[3] != null)
        {
            if (Vector3.Distance(droppedItems[3].transform.position, itemPlacementPosition) < 0.5f )
            {
                Debug.Log("All objects are in position. Instantiating new object.");
                Instantiate(KertasMusik, new Vector3(5.152f, 20.5f, 15.184f), Quaternion.Euler(90, 0, 270));
                Misi2 = true;
                lantai31.SetActive(false);
                lantai32.SetActive(false);
                lantai33.SetActive(false);
            }
        }
        
    }

    void Update()
    {
        //id = InventoryManager.Instance.Items[Index].id;
        id = InventoryManager.Instance.Id;
        itemObject = InventoryManager.Instance.itemInstance;
        Index = InventoryManager.Instance.Index;
        if (interactable && Input.GetKeyDown(KeyCode.F) && InventoryManager.Instance.equip)
        {
            //dropText.SetActive(false);
            interactable = false;
            Drop(Index);
            InventoryManager.Instance.equip = false;
            
        }
        if (!interactable && Input.GetKeyDown(KeyCode.F) && InventoryManager.Instance.equip && id ==2)
        {
            interactable = false;
            DropLantern(Index);
            InventoryManager.Instance.equip = false;
        }

        if ((!Misi1 || !Misi2) && InventoryManager.Instance.instantiatedItems.Count > 0)
        {
            CheckRewardConditions();
        }

        
    }
}
