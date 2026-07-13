
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item m_item;
    private UIInventory m_inventory;

    /// <summary>
    /// initialize the item slot 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="inventory"></param>
    public void Init(Item item, UIInventory inventory)
    {
        m_item = item;
        m_inventory = inventory;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_inventory.ShowTooltip(m_item);
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        m_inventory.HideTooltip();
    }
}
