using UnityEngine;
using UnityEngine.UI;

public class SfxManager : MonoBehaviour
{
    private static SfxManager Instance;

    public static SfxManager GetInstance() {  return Instance; }


    [SerializeField] private AudioPool m_audioPool;
    private static AudioSource m_audioSource;
    private static SfxLibrary m_sfxLibrary;

    [SerializeField] private  Slider m_sfxSlider;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    
        m_sfxLibrary = GetComponent<SfxLibrary>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
       if(m_sfxSlider  != null)
        {
            m_sfxSlider.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(m_sfxSlider.value);
        }
    }
    public static void PlaySfx(string soundName)
    {
        Instance.PlaySfxIntern(soundName);
    }

    private void PlaySfxIntern(string soundName)
    {
        AudioClip audioClip = m_sfxLibrary.GetRandomClip(soundName);

        if (audioClip != null)
        {
            AudioSource availableSfx = m_audioPool.GetAvailableSource();
            availableSfx.PlayOneShot(audioClip);
        }
    }

    public static void SetVolume(float volume)
    {
        Instance.SetPoolVolume(volume);
       
    }

    private void SetPoolVolume(float volume)
    {
        m_audioPool.SetVolume(volume);
    }
    public  void OnValueChanged(float value)
    {
        SetVolume(value);
    }
}
