using UnityEngine;

public class UIManager : MonoBehaviour



   
{
    public static UIManager Instance;

    [SerializeField] private GameObject m_interactableUi;

    [SerializeField] private GameObject m_dialogRect;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(PlayerInteract.Instance.GetInteractableObject() != null)
        {
            m_interactableUi.SetActive(true);

        }
        else
        {
            m_interactableUi.SetActive(false);
        }
        
       
    }

 

    private void ShowPrompt()
    {

        m_interactableUi.SetActive(true);
    }

    public void HidePrompt()
    {
        m_interactableUi.SetActive(false);


    }



}
