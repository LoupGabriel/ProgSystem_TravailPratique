using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public Action OnPlayerInteract;

    public static PlayerInteract Instance { get; private set; } 

    [SerializeField] private float m_interactionRadius = 2f;
    public bool canInteract= false;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

        

        if (GetInteractableObject() != null)
        {

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                GetInteractableObject().Interact();

            }

        }

        
    }



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

   
}
