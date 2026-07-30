using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour




{
    public static UIManager Instance;

    [SerializeField] private GameObject m_interactableUi;
    [SerializeField] private GameObject m_inventoryPanel;
    [SerializeField] private GameObject m_gameOverPanel;

    [SerializeField] private TMP_Text m_gameOverText;
    [SerializeField] private RectTransform m_hungerProgressBar;
    [SerializeField] private RectTransform m_staminaProgressBar;
    [SerializeField] private RectTransform m_healthProgressBar;


   
    [SerializeField] private PlayerStats m_player;

    [SerializeField] private Animator m_staminaAnimator;

    private float m_hungerStep;
    private float m_hungerProgressFull;

    private float m_staminaStep;
    private float m_staminaProgressFull;

    private float m_healthProgressFull;
    private float m_healthStep;



    private bool m_isInDialogue;
    private void Awake()
    {
        Instance = this;
        m_hungerProgressFull = m_hungerProgressBar.rect.width;
        m_staminaProgressFull = m_staminaProgressBar.rect.width;
        m_healthProgressFull = m_healthProgressBar.rect.width;



    }
    private void Start()
    {
        UpdateStatsOnLoad();
        m_player.OnHungerChange += NotifyHungerChange;
        m_hungerStep = m_hungerProgressFull / m_player.GetMaxHunger();
        m_healthStep = m_healthProgressFull / m_player.GetMaxHealth();
        m_staminaStep = m_staminaProgressFull / m_player.GetMaxStamina();

        
        m_inventoryPanel.SetActive(false);
        m_gameOverPanel.SetActive(false);
        PlayerInteract.Instance.OnInteractableChanged += TogglePrompt;
        PlayerInteract.Instance.OnDialogueStateChanged += SetDialogueState;


        EventsManager.GetInstance().SubscribeTo(EEvents.ON_CONSUME_STAMINA, NotifyStaminaChange);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ADD_STAMINA, NotifyStaminaAdd);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_NOT_ENOUGHT_STAMINA, TriggerBarSquish);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_HEALTH_CHANGE, NotifyHealthBar);
       
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_INVENTORY_TOGGLE, ToggleInventoryPanel);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ITEM_CONSUME, NotifyConsumeItem);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_PLAYER_DEAD, ActiveGameOverPanel);




    }

    private void OnDestroy()
    {
        m_player.OnHungerChange -= NotifyHungerChange;
        PlayerInteract.Instance.OnInteractableChanged -= TogglePrompt;
        PlayerInteract.Instance.OnDialogueStateChanged -= SetDialogueState;
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_CONSUME_STAMINA, NotifyStaminaChange);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_ADD_STAMINA, NotifyStaminaAdd);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_NOT_ENOUGHT_STAMINA, TriggerBarSquish);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_HEALTH_CHANGE, NotifyHealthBar);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_INVENTORY_TOGGLE, ToggleInventoryPanel);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_ITEM_CONSUME, NotifyConsumeItem);
    }





    private void TogglePrompt(bool visible)
    {
        if (m_isInDialogue)
        {
            m_interactableUi.SetActive(false);
            return;
        }
        m_interactableUi.SetActive(visible);
    }

    private void SetDialogueState(bool inDialogue)
    {
        m_isInDialogue = inDialogue;

        if (inDialogue)
        {
            m_interactableUi.SetActive(false);
        }
    }
    private void NotifyHungerChange()
    {
        float targetWidth = m_hungerProgressBar.rect.width - m_hungerStep;
        float clampWidth = Mathf.Clamp(targetWidth, 0, m_hungerProgressFull);


        m_hungerProgressBar.sizeDelta = new Vector2(clampWidth, m_hungerProgressBar.sizeDelta.y);


    }

    private void NotifyStaminaChange(Dictionary<string, object> parameters)
    {
        m_staminaStep = m_staminaProgressFull / m_player.GetMaxStamina() * (float)parameters["AttackStamina"];


        float targetWidth = m_staminaProgressBar.rect.width - m_staminaStep;
        float clampWidth = Mathf.Clamp(targetWidth, 0, m_staminaProgressFull);


        m_staminaProgressBar.sizeDelta = new Vector2(clampWidth, m_staminaProgressBar.sizeDelta.y);
    }

    private void NotifyStaminaAdd(Dictionary<string, object> parameters)
    {
        m_staminaStep = m_staminaProgressFull / m_player.GetMaxStamina() * (float)parameters["AddStamina"];


        float targetWidth = m_staminaProgressBar.rect.width + m_staminaStep;
        float clampWidth = Mathf.Clamp(targetWidth, 0, m_staminaProgressFull);


        m_staminaProgressBar.sizeDelta = new Vector2(clampWidth, m_staminaProgressBar.sizeDelta.y);

    }



    private void NotifyHealthBar(Dictionary<string, object> parameters)
    {
        float m_healthStep = m_healthProgressFull / (m_player.GetMaxHealth() * (float)parameters["HealthChange"] );


        float targetWidth = m_healthProgressBar.rect.width - m_healthStep;
        float clampWidth = Mathf.Clamp(targetWidth, 0, m_healthProgressFull);


        m_healthProgressBar.sizeDelta = new Vector2(clampWidth, m_healthProgressBar.sizeDelta.y);
    }


    private void UpdateStatsOnLoad()
    {
        float hungerRatio = m_player.m_currentHunger / m_player.GetMaxHunger();
        float staminaRatio = m_player.m_currentStamina / m_player.GetMaxStamina();
        float healthRatio = m_player.m_currentHealth / m_player.GetMaxHealth();

        hungerRatio = Mathf.Clamp01(hungerRatio);
        staminaRatio= Mathf.Clamp01(staminaRatio);
        healthRatio = Mathf.Clamp01(healthRatio);

        m_hungerProgressBar.sizeDelta = new Vector2(m_hungerProgressFull * hungerRatio, m_hungerProgressBar.sizeDelta.y);
        m_staminaProgressBar.sizeDelta = new Vector2(m_staminaProgressFull * staminaRatio, m_staminaProgressBar.sizeDelta.y);
        m_healthProgressBar.sizeDelta = new Vector2(m_healthProgressFull * healthRatio, m_healthProgressBar.sizeDelta.y);

    }

    private void TriggerBarSquish(Dictionary<string, object> parameters)
    {
        m_staminaAnimator.SetTrigger("squish");
        SfxManager.PlaySfx("Error");
    }

    private void ToggleInventoryPanel(Dictionary<string, object> parameters)
    {
        bool toggle = (bool)parameters["toggle"];
        if (toggle)
        {
            m_inventoryPanel.SetActive(!m_inventoryPanel.activeSelf);
            if (m_inventoryPanel.activeSelf == true)
            {
                PauseController.SetPause(true);
            }
            else
            {
                PauseController.SetPause(false);
            }
        }
    }

    private void NotifyConsumeItem(Dictionary<string, object> parameters)
    {

        float targetWidth = 0;
        float clampWidth = 0;
        switch ((EItemType)parameters["Item"])
        {
            case EItemType.FOOD:
                targetWidth = m_hungerProgressBar.rect.width + m_hungerStep * (int)parameters["Amount"];
                clampWidth = Mathf.Clamp(targetWidth, 0, m_hungerProgressFull);


                m_hungerProgressBar.sizeDelta = new Vector2(clampWidth, m_hungerProgressBar.sizeDelta.y);


                break;

            case EItemType.STAMINA:

                targetWidth = m_staminaProgressBar.rect.width + m_staminaStep * (int)parameters["Amount"];
                clampWidth = Mathf.Clamp(targetWidth, 0, m_staminaProgressFull);


                m_staminaProgressBar.sizeDelta = new Vector2(clampWidth, m_staminaProgressBar.sizeDelta.y);

                break;

            case EItemType.HEALTH:
                targetWidth = m_healthProgressBar.rect.width + m_healthStep * (int)parameters["Amount"];
                clampWidth = Mathf.Clamp(targetWidth, 0, m_healthProgressFull);


                m_healthProgressBar.sizeDelta = new Vector2(clampWidth, m_healthProgressBar.sizeDelta.y);

                break;




        }
    }


    private void ActiveGameOverPanel(Dictionary<string, object> param)
    {
        
        //PauseController.SetPause(true);
        m_gameOverPanel.SetActive(true);
        m_gameOverPanel.GetComponent<CanvasGroup>().alpha = 0;
        StartCoroutine(GameOverPanelRoutine(2));
      
        m_gameOverText.color = (Color)param["textColor"];
    }


    private IEnumerator GameOverPanelRoutine(float delay = 2f, float fadeDuration = 2f)
    {
        yield return new WaitForSeconds(delay);

        CanvasGroup canvasGroup = m_gameOverPanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        float elapse = 0;

        while(elapse < fadeDuration)
        {
            elapse += Time.deltaTime;
            canvasGroup.alpha = elapse / fadeDuration;
            yield return null;
        }
        
        canvasGroup.alpha = 1;
    }
}
