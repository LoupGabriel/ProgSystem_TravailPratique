using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour



   
{
    public static UIManager Instance;

    [SerializeField] private GameObject m_interactableUi;

    [SerializeField] private RectTransform m_hungerProgressBar;
    [SerializeField] private RectTransform m_StaminaProgressBar;

    [SerializeField] private PlayerStats m_player;

    [SerializeField] private Animator m_staminaAnimator;
    
    private float m_hungerStep;
    private float m_hungerProgressFull;

    private float m_StaminaStep;
    private float m_StaminaProgressFull;
  
    private bool m_isInDialogue;
    private void Awake()
    {
        Instance = this;
        m_hungerProgressFull = m_hungerProgressBar.rect.width;
        m_StaminaProgressFull = m_StaminaProgressBar.rect.width;
      



    }
    private void Start()
    {
        m_player.OnHungerChange += NotifyHungerChange;
        m_hungerStep = m_hungerProgressFull / m_player.GetMaxHunger();
        
        PlayerInteract.Instance.OnInteractableChanged += TogglePrompt;
        PlayerInteract.Instance.OnDialogueStateChanged += SetDialogueState;
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_CONSUME_STAMINA, NotifyStaminaChange);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ADD_STAMINA, NotifyStaminaAdd);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_NOT_ENOUGHT_STAMINA, TriggerBarSquish);


    }

    private void OnDestroy()
    {
        m_player.OnHungerChange -= NotifyHungerChange;
        PlayerInteract.Instance.OnInteractableChanged -= TogglePrompt;
        PlayerInteract.Instance.OnDialogueStateChanged -= SetDialogueState;
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_CONSUME_STAMINA, NotifyStaminaChange);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_ADD_STAMINA, NotifyStaminaAdd);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_NOT_ENOUGHT_STAMINA, TriggerBarSquish);
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
        m_StaminaStep = m_StaminaProgressFull / m_player.GetMaxStamina() * (float)parameters["AttackStamina"];

      
        float targetWidth = m_StaminaProgressBar.rect.width - m_StaminaStep;
        float clampWidth = Mathf.Clamp(targetWidth, 0, m_StaminaProgressFull);


        m_StaminaProgressBar.sizeDelta = new Vector2(clampWidth, m_StaminaProgressBar.sizeDelta.y);
    }

    private void NotifyStaminaAdd(Dictionary<string, object> parameters)
    {
        m_StaminaStep = m_StaminaProgressFull / m_player.GetMaxStamina() * (float)parameters["AddStamina"];


        float targetWidth = m_StaminaProgressBar.rect.width + m_StaminaStep;
        float clampWidth = Mathf.Clamp(targetWidth, 0, m_StaminaProgressFull);


        m_StaminaProgressBar.sizeDelta = new Vector2(clampWidth, m_StaminaProgressBar.sizeDelta.y);

    }

    private void TriggerBarSquish(Dictionary<string, object> parameters)
    {
        m_staminaAnimator.SetTrigger("squish");
        SfxManager.PlaySfx("Error");
    }



}
