using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{

    [SerializeField] private Transform m_sfxContainer;
    private List<AudioSource> m_sources = new List<AudioSource>();

   

    public AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in m_sources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        GameObject newAudioObject = new GameObject("AudioSource");
        newAudioObject.transform.parent = m_sfxContainer;

        AudioSource audioComponent = newAudioObject.AddComponent<AudioSource>();

        m_sources.Add(audioComponent);

        return audioComponent;
    }

    public void SetVolume(float volume)
    {
        foreach (AudioSource source in m_sources)
        {
            source.volume = volume;
        }
    }
}
