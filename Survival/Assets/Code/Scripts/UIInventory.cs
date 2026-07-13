using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class UIInventory : MonoBehaviour
{
    [SerializeField] private Transform m_inventoryItemParent;

    [SerializeField] private GameObject m_itemContainer;

    [SerializeField] private GameObject m_tooltips;

    private void Start()
    {
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_INVENTORY_TOGGLE, RefreshInventory);
        m_tooltips.SetActive(false);
    }

    private void OnDestroy()
    {
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_INVENTORY_TOGGLE, RefreshInventory);
    }

   

    /// <summary>
    /// Show the tool tip and update text 
    /// </summary>
    /// <param name="item">Item to load</param>
    public void ShowTooltip(Item item)
    {
        m_tooltips.SetActive(true);

        TMP_Text name = m_tooltips.transform.Find("Name").GetComponent<TMP_Text>();
        TMP_Text type = m_tooltips.transform.Find("Type").GetComponent<TMP_Text>(); ;
        TMP_Text amount = m_tooltips.transform.Find("Amount").GetComponent<TMP_Text>(); ;


        name.text = item.ItemData().name;
        type.text = item.ItemData().type.ToString();
        amount.text=item.ItemData().amount.ToString();
        m_tooltips.transform.position = Mouse.current.position.ReadValue();

    }


    public void HideTooltip()
    {
        m_tooltips.SetActive(false);
    }


    /// <summary>
    /// Update the item slot number of item
    /// </summary>
    /// <param name="param"></param>
    private void RefreshInventory(Dictionary<string, object> param)
    {

        //refresh the inventory 
        foreach (Transform child in m_inventoryItemParent)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, int> itemCount = new Dictionary<string, int>();
        Dictionary<string, Item> itemsByID = new Dictionary<string, Item>();

        foreach (Item item in InventorySystem.GetInstance().m_currentItems)
        {
            string id = item.ItemData().Id;

            if (!itemCount.ContainsKey(id))
            {
                itemCount[id] = 0;
                itemsByID[id] = item;

            }
            itemCount[id]++;
        }

        foreach(string id  in itemCount.Keys)
        {
            Item item = itemsByID[id];

            GameObject slot = Instantiate(m_itemContainer, m_inventoryItemParent);

            Image icon = slot.transform.Find("ItemIcon").GetComponent<Image>();

            TMP_Text count = slot.transform.Find("ItemCount").GetComponent<TMP_Text>();
            Button button = slot.transform.GetComponent<Button>();

            icon.sprite = item.ItemData().icon;
            count.text = itemCount[id].ToString();

            button.onClick.AddListener(() =>
            {
                InventorySystem.GetInstance().ConsumeItem(item);
                RefreshInventory(null);
            });

            UIInventorySlot inventorySlot = slot.GetComponent<UIInventorySlot>();
            inventorySlot.Init(item, this);
        }
       

    }


}

