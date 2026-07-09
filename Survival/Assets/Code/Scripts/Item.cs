using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSo m_itemData;

    Dictionary<string, object> eventParam;

    public void Consume()
    {
        eventParam.Add("Item", m_itemData.type);
        eventParam.Add("Amount", m_itemData.amount);

        EventsManager.GetInstance().TriggerEvents(EEvents.ON_ITEM_CONSUME, eventParam);
    }
}
