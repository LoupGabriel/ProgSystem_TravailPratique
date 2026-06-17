using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Npc : MonoBehaviour, Iinteractable, ITalkable
{

    [SerializeField] private DialogueTextData m_dialogueData;
    [SerializeField] private Image m_textBubble;
    private DialogueController m_dialogueController;


    private int m_dialogueIndex;
    private bool m_isTyping, m_isDialogueActive;
    private Coroutine DialogueBoxRoutineEndInstance;
    private Coroutine ChangeBubbleColorInstance;
    private Coroutine TypeLineRoutineInstance;


    private bool m_isChoosing;

    public bool canInteract()
    {
        return !m_isDialogueActive;
    }

    private void Start()
    {
        m_dialogueController = DialogueController.Instance;
    }


    private void Update()
    {
        if (!m_isDialogueActive || m_isChoosing)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Interact();
        }
    }
    public void Interact()
    {
        if (m_dialogueData == null)
            return;

        if (m_isDialogueActive)
        {
            //NextLine
            NextLine();
        }
        else
        {
            //startDialogue
            StartDialogue();

        }


    }

    private void StartDialogue()
    {
        PlayerInteract.Instance.SetDialogueState(true);

        m_isDialogueActive = true;
        m_dialogueIndex = 0;


        m_dialogueController.SetNpcInfo(m_dialogueData.npcName);
        m_dialogueController.ShowDialogue(true);

        //set pause controller to pause
        DisplayCurrentLine();

    }

    private void NextLine()
    {
        if (m_isTyping)
        {
            //skip animation and show completed text
            if (TypeLineRoutineInstance != null)
            {
                StopCoroutine(TypeLineRoutineInstance);
            }
            m_dialogueController.SetDialogueText(m_dialogueData.dialogueLines[m_dialogueIndex], m_dialogueData.lineColor[m_dialogueIndex]);
            m_isTyping = false;
        }


        m_dialogueController.ClearChoices();

        //check if line is ending text
        if (m_dialogueData.endConversationLine.Length > m_dialogueIndex && m_dialogueData.endConversationLine[m_dialogueIndex])
        {
            EndDialogue();
            return;


        }


        foreach (DialogueChoice dialogueChoice in m_dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == m_dialogueIndex)
            {

                //display choices panel
                DisplayChoice(dialogueChoice);
                return;
            }
        }

        if (++m_dialogueIndex < m_dialogueData.dialogueLines.Length)
        {
            //start typing the next line if it existe
            DisplayCurrentLine();
        }
        else
        {
            //end dialog
            EndDialogue();
        }
    }






    private void DisplayChoice(DialogueChoice choice)
    {

        m_isChoosing = true;
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            m_dialogueController.CreateChoiceButton(choice.choices[i], () => ChooseChoice(nextIndex));
        }

    }


    public void ChooseChoice(int nextIndex)
    {
        SfxManager.PlaySfx("Click");
        m_isChoosing = false;
        m_dialogueIndex = nextIndex;
        m_dialogueController.ClearChoices();
        DisplayCurrentLine();
    }


    private void DisplayCurrentLine()
    {
        if (TypeLineRoutineInstance != null)
        {
            StopCoroutine(TypeLineRoutineInstance);
        }


        TypeLineRoutineInstance = StartCoroutine(TypeLineRoutine());
        ChangeBubbleColorInstance = StartCoroutine(ChangeBubbleColor(m_textBubble, m_dialogueData.bubbleColor[m_dialogueIndex]));

    }
    public void EndDialogue()
    {
        if (TypeLineRoutineInstance != null)
        {
            StopCoroutine(TypeLineRoutineInstance);
        }
        if(ChangeBubbleColorInstance != null)
        {

            StopCoroutine(ChangeBubbleColorInstance);
        }
            
        
        m_isDialogueActive = false;
        m_dialogueController.SetDialogueText("", Color.white);

        DialogueBoxRoutineEndInstance = StartCoroutine(DialogueBoxRoutineEnd());

        PlayerInteract.Instance.SetDialogueState(false);
    }

    private IEnumerator DialogueBoxRoutineEnd()
    {

        m_dialogueController.m_dialogueBoxAnimator.SetTrigger("endDialogue");
        yield return new WaitForSeconds(0.35f);
        m_dialogueController.ShowDialogue(false);
    }

    private IEnumerator ChangeBubbleColor(Image bubblePanel, Color nextColor, float fadeDuration = 0.5f)
    {
        Color currentColor = bubblePanel.color;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            bubblePanel.color = Color.Lerp(currentColor, nextColor, percent);
            yield return null;

        }
        bubblePanel.color = nextColor;



    }
    private IEnumerator TypeLineRoutine()
    {
        m_isTyping = true;
        m_dialogueController.SetDialogueText("", Color.white);

        foreach (char letter in m_dialogueData.dialogueLines[m_dialogueIndex])
        {

            m_dialogueController.SetDialogueText(m_dialogueController.m_dialogueText.text += letter,
                m_dialogueData.lineColor[m_dialogueIndex]);
            SfxManager.PlaySfx("Talk");
            yield return new WaitForSeconds(m_dialogueData.typingSpeed);
        }
        m_isTyping = false;


    }
}