using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    private static GameManager Instance;
    public  bool m_shouldLoadSave;
    public static GameManager GetInstance()
    {
        return Instance;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {   
            Destroy(gameObject);
        }

        

    }
  



    public void LoadSave(string sceneName)
    {
        PauseController.SetPause(false);
        m_shouldLoadSave = true;
        InventorySystem.GetInstance().Initialize();
        SceneManager.LoadScene(sceneName);
    }
    public void NewGame(string sceneName)
    {
       
        m_shouldLoadSave = false;
        SceneManager.LoadScene(sceneName);
    }
}
