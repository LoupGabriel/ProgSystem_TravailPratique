using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundtrackManager : MonoBehaviour
{
    public static SoundtrackManager Instance { get; private set; }

    [SerializeField] private SoundTrackLibrary m_soundLibrary;

    [SerializeField] private AudioSource m_musicSource;


    [SerializeField] private Slider m_musicSlider;

    private Coroutine m_fadeRoutine;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //m_musicSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }
    private void Start()
    {
        PlayMusic("Main");
    }

    /// <summary>
    /// Change music to the choosen one
    /// </summary>
    /// <param name="trackName">New music name</param>
    /// <param name="fadeDuration">Interpolation duration</param>
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        if (m_fadeRoutine != null)
        {
            StopCoroutine(m_fadeRoutine);
        }
        m_fadeRoutine = StartCoroutine(MusicCrossFade(m_soundLibrary.GetClipFromName(trackName), fadeDuration));
    }


    /// <summary>
    /// Cross fade music interpolation
    /// </summary>
    IEnumerator MusicCrossFade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            m_musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;

        }

        m_musicSource.clip = nextTrack;
        m_musicSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            m_musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;

        }
    }

    public void SetVolume(float volume)
    {
        m_musicSource.volume = volume;
    }
    public void OnValueChanged()
    {
        SetVolume(m_musicSlider.value);
    }
}
