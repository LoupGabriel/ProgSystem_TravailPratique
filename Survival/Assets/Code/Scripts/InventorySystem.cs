
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private static InventorySystem m_instance;

    private List<Item> m_currentItems;

    private InventorySystem()
    {

    }

    public static InventorySystem GetInstance()
    {

        if(m_instance == null)
        {
            m_instance = new InventorySystem();

        }
        return m_instance;
    }



}
