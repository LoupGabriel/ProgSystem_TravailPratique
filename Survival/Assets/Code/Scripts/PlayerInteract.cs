using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
   
    public Action<bool> OnInteractableChanged;
    public Action<bool> OnDialogueStateChanged;
    public static PlayerInteract Instance { get; private set; }
    public bool m_isInDialogue { get; private set; }
    [SerializeField] private float m_interactionRadius = 2f;
    public bool m_canInteract= false;


    private Npc m_currentNpc;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (m_isInDialogue)
        {
            return;
        }

        Npc npc = GetInteractableObject();


        if (npc != m_currentNpc)
        { 
             m_currentNpc = npc;
            OnInteractableChanged?.Invoke(m_currentNpc != null);
        }

        if(m_currentNpc != null && Keyboard.current.eKey.wasPressedThisFrame )
        {
            m_currentNpc.Interact();
        }
       

        
    }


    /// <summary>
    /// Get npc in a sphere radius
    /// </summary>
    /// <returns></returns>
    public Npc GetInteractableObject()
    {

        Collider[] npcsCol = Physics.OverlapSphere(transform.position, m_interactionRadius);

        foreach (Collider col in npcsCol)
        {
            if (col.gameObject.CompareTag("Npc"))
            {

               return col.GetComponent<Npc>();

            }
        }

        return null;



    }


    public void SetDialogueState(bool state)
    {
        m_isInDialogue = state;
        OnDialogueStateChanged?.Invoke(state);
    }


}
