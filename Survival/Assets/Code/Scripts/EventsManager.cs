using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public enum EEvents
{
    ON_PLAYER_ATTACK,
    ON_CONSUME_STAMINA,
    ON_ADD_STAMINA,
    ON_NOT_ENOUGHT_STAMINA,
    ON_ENEMY_ATTACK,
    ON_HEALTH_CHANGE,
    ON_PLAYER_DEAD,
    ON_ITEM_CONSUME,
    ON_INVENTORY_TOGGLE
}

public class EventsManager : MonoBehaviour
{
    private static EventsManager m_instance;

    private Dictionary<EEvents, Action<Dictionary<string, object>>> m_events;


   

    private void Awake()
    {
        
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_events = new Dictionary<EEvents, Action<Dictionary<string, object>>>();
    }
    public static EventsManager GetInstance()
    {
        
        return m_instance;
    }


    public void SubscribeTo(EEvents eventId,Action<Dictionary<string,object>> func)
    {


        if (m_events.ContainsKey(eventId))
        {
            m_events[eventId] += func;

            
        }
        else
        {
            m_events.Add(eventId, func);
        }
           

       

    }

    public void UnsubscribeFrom(EEvents eventId, Action<Dictionary<string, object>> func)
    {
        if (m_events[eventId] != null)
        {
            m_events[eventId] -= func;
        }

        if (m_events[eventId] == null)
            m_events.Remove(eventId);
    }

    public void TriggerEvents(EEvents eventId, Dictionary<string, object> parameters)
    {
        if (m_events[eventId] != null)
        {
            m_events[eventId]?.Invoke(parameters);
        }
    }
}



