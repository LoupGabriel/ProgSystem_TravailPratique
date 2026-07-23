
using System.Collections.Generic;

using UnityEngine;

public class InventorySystem 
{
    private static InventorySystem m_instance;

   
    public List<Item> m_currentItems;
    private bool m_isNewGame = false;
    private InventorySystem()
    {
        m_currentItems = new List<Item>();
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ENEMY_DEATH, AddItemToInventory);


        if (GameManager.GetInstance().m_shouldLoadSave)
        {
            SaveData data = SaveSystem.LoadGame();
          
            if (!m_isNewGame)
            {
                foreach (Item item in data.currentItems)
                {
                    m_currentItems.Add(item);
                }
            }
        }
        else
        {
            m_currentItems.Clear();
        }

        
       
    }

    public static InventorySystem GetInstance()
    {

        if(m_instance == null)
        {
            m_instance = new InventorySystem();

        }
        return m_instance;
    }

    
    /// <summary>
    /// Add an item to the inventory list
    /// </summary>
    /// <param name="param"></param>
    private void AddItemToInventory(Dictionary<string,object> param)
    {
        m_currentItems.Add((Item)param["DropItem"]);
        SfxManager.PlaySfx("Item");
        Dictionary<string, object> eventParam = new Dictionary<string, object>();
        eventParam.Add("items", m_currentItems);
        EventsManager.GetInstance().TriggerEvents(EEvents.ON_SAVEGAME, eventParam);
    }


    /// <summary>
    /// On click consume the item and make the effect
    /// </summary>
    /// <param name="item">item to consume</param>
    public void ConsumeItem(Item item)
    {
        if (item == null)
            return;

        if (!m_currentItems.Contains(item))
            return;

        item.Consume();

        m_currentItems.Remove(item);
    }

    public void SetIsNewGame(bool isNewGame)
    {
        m_isNewGame = isNewGame;
    }

  
}
