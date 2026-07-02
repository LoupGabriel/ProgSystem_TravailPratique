using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    [SerializeField]
    private float m_maxHealth = 10f;
    private float m_currentHealth;
    [SerializeField]
    private float m_maxStamina = 10f;
    [SerializeField]
    private float m_staminaRegen = 2f;
    public float m_currentStamina;
    [SerializeField]
    private float m_maxHunger = 10f;
    public float m_currentHunger;


    public Action OnHungerChange;

    private Coroutine m_hungerCoroutine;
    private Coroutine m_staminaCoroutine;

    private void Start()
    {
        m_currentHunger = m_maxHunger;
        m_currentStamina = m_maxStamina;
        m_currentHealth = m_maxHealth;
        m_hungerCoroutine = StartCoroutine(HungerRoutine(2f, 1f));
        m_staminaCoroutine = StartCoroutine(StaminaRoutine(2f, m_staminaRegen));
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_PLAYER_ATTACK, ConsumeStamina);
    }
    private void OnDestroy()
    {
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_PLAYER_ATTACK, ConsumeStamina);
    }
    private IEnumerator HungerRoutine(float hungerTick,float hungerStep)
    {
        bool hasPlaySfx=false;
        while (true)
        {
            if(m_currentHunger == 0 && !hasPlaySfx)
            {
                SfxManager.PlaySfx("Hungry");
                SoundtrackManager.Instance.PlayMusic("Hungry");
                hasPlaySfx = true;
            }
            m_currentHunger -= hungerStep;
            OnHungerChange?.Invoke();
            m_currentHunger = Mathf.Clamp(m_currentHunger, 0, m_maxHunger);
            yield return new WaitForSeconds(hungerTick);


        }

    }
    private IEnumerator StaminaRoutine(float StaminaTick, float staminaStep)
    {
        Dictionary<string, object> eventParam = new Dictionary<string, object>();
        eventParam.Add("AddStamina", staminaStep);
        while (true)
        {
           
            m_currentStamina += staminaStep;

            m_currentStamina = Mathf.Clamp(m_currentStamina, 0, m_maxStamina);
            yield return new WaitForSeconds(StaminaTick);

            
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_ADD_STAMINA, eventParam);
        }

    }
    public float GetMaxHunger()
    {
        return m_maxHunger;
    }
    public float GetMaxStamina()
    {
        return m_maxStamina;
    }

    private void ConsumeStamina(Dictionary<string, object> parameters)
    {
        Dictionary<string, object> eventParam = new Dictionary<string, object>();
        
        eventParam.Add("NotEnoughtStamina", true);

        float staminaStep = (int)parameters["AttackStamina"];
        if (m_currentStamina < staminaStep)
        {
           
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_NOT_ENOUGHT_STAMINA, eventParam);
            return;
        }
           

        m_currentStamina -= staminaStep;
        eventParam.Add("AttackStamina", staminaStep);

        EventsManager.GetInstance().TriggerEvents(EEvents.ON_CONSUME_STAMINA, eventParam);

    }

    
   

}
