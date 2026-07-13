
using System.Collections.Generic;

using UnityEngine;

public class InventorySystem 
{
    private static InventorySystem m_instance;

   
    public List<Item> m_currentItems;

    private InventorySystem()
    {
        m_currentItems = new List<Item>();
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ENEMY_DEATH, AddItemToInventory);
    }

    public static InventorySystem GetInstance()
    {

        if(m_instance == null)
        {
            m_instance = new InventorySystem();

        }
        return m_instance;
    }

    

    private void AddItemToInventory(Dictionary<string,object> param)
    {
        m_currentItems.Add((Item)param["DropItem"]);
        SfxManager.PlaySfx("Item");
    }
    public void ConsumeItem(Item item)
    {
        if (item == null)
            return;

        if (!m_currentItems.Contains(item))
            return;

        item.Consume();

        m_currentItems.Remove(item);
    }
}
