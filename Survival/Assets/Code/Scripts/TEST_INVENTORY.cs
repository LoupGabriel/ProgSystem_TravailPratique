
using System.Collections.Generic;
using UnityEngine;

public class TEST_INVENTORY : MonoBehaviour
{
    [SerializeField] private List<Item> m_currentItem;


    private void Update()
    {
        m_currentItem = InventorySystem.GetInstance().m_currentItems;
    }
}
