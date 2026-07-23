using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerStats : MonoBehaviour
{

    [SerializeField]
    private float m_maxHealth = 10f;
    public float m_currentHealth;
    [SerializeField]
    private float m_maxStamina = 10f;
    [SerializeField]
    private float m_staminaRegen = 2f;
    public float m_currentStamina;
    [SerializeField]
    private float m_maxHunger = 10f;
    public float m_currentHunger;
    [SerializeField] private float m_distanceToHit = 2f;

    public Action OnHungerChange;

    private Coroutine m_hungerCoroutine;
    private Coroutine m_staminaCoroutine;
    private bool m_isDead;
    private bool m_dyingSfx = false;
    private PlayerController m_controller;


    Dictionary<string, object> eventParam;

    private void Awake()
    {
        m_controller = GetComponent<PlayerController>();

        InitializeDefaultStats();

        if (GameManager.GetInstance().m_shouldLoadSave)
        {
            SaveData data = SaveSystem.LoadGame();

            if (data != null)
            {
                LoadPlayerData(data);
                gameObject.transform.position = data.playerPos;
            }
        }
    }
    private void Start()
    {
        
 
        m_hungerCoroutine = StartCoroutine(HungerRoutine(2f, 1f));
        m_staminaCoroutine = StartCoroutine(StaminaRoutine(2f, m_staminaRegen));



        EventsManager.GetInstance().SubscribeTo(EEvents.ON_PLAYER_ATTACK, ConsumeStamina);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ENEMY_ATTACK, GetHit);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ITEM_CONSUME, AddRessource);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_SAVEGAME, Save);

    }

    private void InitializeDefaultStats()
    {
        m_currentHealth = m_maxHealth;
        m_currentStamina = m_maxStamina;
        m_currentHunger = m_maxHunger;

        m_isDead = false;
        m_dyingSfx = false;
    }

    private void LoadPlayerData(SaveData data)
    {
        m_currentHealth = data.currentHealth;
        m_currentHunger = data.currentHunger;
        m_currentStamina = data.currentStamina;

        transform.position = data.playerPos;
       
    }
    private void OnDestroy()
    {
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_PLAYER_ATTACK, ConsumeStamina);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_ENEMY_ATTACK, GetHit);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_ITEM_CONSUME, AddRessource);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_SAVEGAME, Save);
    }


    /// <summary>
    /// Update of hunger over time
    /// </summary>
    /// <param name="hungerTick">frequency of hunger </param>
    /// <param name="hungerStep">number of hunger by frequency</param>
    /// <returns></returns>
    private IEnumerator HungerRoutine(float hungerTick, float hungerStep)
    {
        bool hasPlaySfx = false;
        while (true)
        {
            if (m_currentHunger == 0 && !hasPlaySfx)
            {
                SfxManager.PlaySfx("Hungry");
                SoundtrackManager.Instance.PlayMusic("Hungry");
                hasPlaySfx = true;
                Dictionary<string, object> eventParam = new Dictionary<string, object>();
                eventParam.Add("textColor", Color.yellow);
                eventParam.Add("hungerDeath", true);

                //invoke dead
                m_isDead = true;
                EventsManager.GetInstance().TriggerEvents(EEvents.ON_PLAYER_DEAD, eventParam);

                if (!m_dyingSfx)
                {
                    SfxManager.PlaySfx("PlayerDying");
                    m_dyingSfx = true;
                }


            }
            m_currentHunger -= hungerStep;
            OnHungerChange?.Invoke();
            m_currentHunger = Mathf.Clamp(m_currentHunger, 0, m_maxHunger);
            yield return new WaitForSeconds(hungerTick);


        }

    }
    /// <summary>
    /// Stamina update over time
    /// </summary>
    /// <param name="StaminaTick">frequency of adding stamina</param>
    /// <param name="staminaStep"> number of stamina by frequency</param>
    /// <returns></returns>
    private IEnumerator StaminaRoutine(float StaminaTick, float staminaStep)
    {
        Dictionary<string, object> eventParam = new Dictionary<string, object>();
        eventParam.Add("AddStamina", staminaStep);
        eventParam.Add("EnoughtStamina", true);
        while (true)
        {

            m_currentStamina += staminaStep;

            m_currentStamina = Mathf.Clamp(m_currentStamina, 0, m_maxStamina);
            yield return new WaitForSeconds(StaminaTick);

            if (m_currentStamina >= staminaStep)
            {
                EventsManager.GetInstance().TriggerEvents(EEvents.ON_ENOUGHT_STAMINA, eventParam);
            }
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
    public float GetMaxHealth()
    {
        return m_maxHealth;
    }


    public float GetDistanceToHit()
    {
        return m_distanceToHit;
    }


    /// <summary>
    /// Handle stamina when attacking
    /// </summary>
    /// <param name="parameters"></param>
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

    /// <summary>
    /// Handle hit and health
    /// </summary>
    /// <param name="parameters"></param>
    private void GetHit(Dictionary<string, object> parameters)
    {
        if (m_isDead) return;
        m_currentHealth -= (float)parameters["AttackDamage"];
        Dictionary<string, object> eventParam = new Dictionary<string, object>();

        eventParam.Add("HealthChange", (float)parameters["AttackDamage"]);
        eventParam.Add("isDead", m_isDead);
        eventParam.Add("textColor", Color.red);
        eventParam.Add("hungerDeath", false);
        EventsManager.GetInstance().TriggerEvents(EEvents.ON_HEALTH_CHANGE, eventParam);

        if (m_currentHealth <= 0)
        {
            //invoke dead
            m_isDead = true;
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_PLAYER_DEAD, eventParam);

            if (!m_dyingSfx)
            {
                SfxManager.PlaySfx("PlayerDying");
                m_dyingSfx = true;
            }

        }
    }


    /// <summary>
    /// update player stat when an item is used
    /// </summary>
    /// <param name="parameter"></param>
    private void AddRessource(Dictionary<string, object> parameter)
    {
        EItemType type = (EItemType)parameter["Item"];
        int amount = (int)parameter["Amount"];
        switch (type)
        {
            case EItemType.HEALTH:
                {
                    m_currentHealth += amount;
                    break;
                }
            case EItemType.FOOD:
                {
                    m_currentHunger += amount;
                    break;
                }
            case EItemType.STAMINA:
                {
                    m_currentStamina += amount;
                    break;
                }
        }
    }


    public void Save(Dictionary<string, object> param)
    {


        SaveSystem.SaveGame(this, (List<Item>)param["items"]);


    }

}
