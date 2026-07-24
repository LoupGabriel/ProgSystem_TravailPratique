using UnityEngine;

public class UiButtonOnDeath : MonoBehaviour
{
    [SerializeField] private string m_sceneToLoad;
    public void LoadScene()
    {
        GameManager.GetInstance().LoadSave(m_sceneToLoad);
    }

    public void NewGame()
    {
        GameManager.GetInstance().NewGame(m_sceneToLoad);
    }


    public void ExitApp()
    {
        Application.Quit();
    }
}
