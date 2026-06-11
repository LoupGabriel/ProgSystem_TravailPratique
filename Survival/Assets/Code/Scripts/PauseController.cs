using UnityEngine;

public class PauseController : MonoBehaviour
{


    public static bool m_isGamePaused { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        m_isGamePaused = pause;
    }
}
