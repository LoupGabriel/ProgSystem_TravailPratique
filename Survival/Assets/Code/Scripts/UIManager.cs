using UnityEngine;

public class UIManager : MonoBehaviour



   
{
    public static UIManager Instance;

    [SerializeField] private GameObject m_interactableUi;

    [SerializeField] private RectTransform m_hungerProgressBar;

    [SerializeField] private PlayerStats m_player;

    private float m_hungerStep;
    private float m_hungerProgressFull;
    private bool m_isInDialogue;
    private void Awake()
    {
        Instance = this;
        m_hungerProgressFull = m_hungerProgressBar.rect.width;

        
       

    }
    private void Start()
    {
        m_player.OnHungerChange += NotifyHungerChange;
        m_hungerStep = m_hungerProgressFull / m_player.GetMaxHunger();
 
        PlayerInteract.Instance.OnInteractableChanged += TogglePrompt;
        PlayerInteract.Instance.OnDialogueStateChanged += SetDialogueState;
    }

    private void OnDestroy()
    {
        m_player.OnHungerChange -= NotifyHungerChange;
        PlayerInteract.Instance.OnInteractableChanged -= TogglePrompt;
        PlayerInteract.Instance.OnDialogueStateChanged -= SetDialogueState;
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



}
