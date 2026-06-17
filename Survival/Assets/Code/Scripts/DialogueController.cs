using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    [SerializeField] private GameObject m_dialoguePanel;

    [SerializeField] public TMP_Text m_dialogueText, m_nameText;

    [SerializeField] public Transform m_choicePanel;

    [SerializeField] public GameObject m_choiceButtonPrefab;
    [SerializeField] public Animator m_dialogueBoxAnimator;

  

    private void Awake()
    {
        Instance = this;
    }


    public void ShowDialogue(bool show)
    {
        m_dialoguePanel.SetActive(show);
       
        PauseController.SetPause(show);
    }


    public void SetNpcInfo(string npcName)
    {
        m_nameText.text = npcName;

    }

    public void SetDialogueText(string text, Color color)
    {

        m_dialogueText.text = text;
        m_dialogueText.color = color;
    }


    public void ClearChoices()
    {


        foreach (Transform child in m_choicePanel) Destroy(child.gameObject);

    }

    public void CreateChoiceButton(string choiceText,UnityEngine.Events.UnityAction onClick)
    {

        GameObject choiceButtonInstance = Instantiate(m_choiceButtonPrefab, m_choicePanel);
        choiceButtonInstance.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButtonInstance.GetComponent<Button>().onClick.AddListener(onClick);
        
    }
    


}
