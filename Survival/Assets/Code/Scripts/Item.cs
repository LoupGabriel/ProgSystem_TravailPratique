using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSo m_itemData;

   
    /// <summary>
    /// trigger consume event
    /// </summary>
    public void Consume()
    {
        EventsManager.GetInstance().TriggerEvents(EEvents.ON_ITEM_CONSUME,
       new Dictionary<string, object>
       {
            { "Item", m_itemData.type },
            { "Amount", m_itemData.amount }
       });

        SfxManager.PlaySfx("ItemUsed");
    }

    public ItemSo ItemData()
    {
        return m_itemData;
    }
}
