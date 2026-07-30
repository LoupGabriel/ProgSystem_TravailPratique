using UnityEngine;
using UnityEngine.EventSystems;

public class UiButtonOnDeath : MonoBehaviour,IPointerEnterHandler
{
    [SerializeField] private string m_sceneToLoad;
    public void LoadScene()
    {
        SfxManager.PlaySfx("ItemUsed");
        GameManager.GetInstance().LoadSave(m_sceneToLoad);
    }

    public void NewGame()
    {
        SfxManager.PlaySfx("ItemUsed");
        GameManager.GetInstance().NewGame(m_sceneToLoad);
    }


    public void ExitApp()
    {
        Application.Quit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SfxManager.PlaySfx("Click");
    }
}
