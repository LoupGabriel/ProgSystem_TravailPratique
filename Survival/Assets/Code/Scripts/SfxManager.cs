using UnityEngine;
using UnityEngine.UI;

public class SfxManager : MonoBehaviour
{
    private static SfxManager Instance;

    private static AudioSource m_audioSource;
    private static SfxLibrary m_sfxLibrary;

    [SerializeField] private  Slider m_sfxSlider;


    private void Awake()
    {
        Instance = this;
        m_audioSource = GetComponent<AudioSource>();
        m_sfxLibrary = GetComponent<SfxLibrary>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }
    public static void PlaySfx(string soundName)
    {
        AudioClip audioClip = m_sfxLibrary.GetRandomClip(soundName);
        if(audioClip != null)
        {
            m_audioSource.PlayOneShot(audioClip);
        }
    }

    public static void SetVolume(float volume)
    {
        m_audioSource.volume = volume;
    }
    public  void OnValueChanged()
    {
        SetVolume(m_sfxSlider.value);
    }
}
