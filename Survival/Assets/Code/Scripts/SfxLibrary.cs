using System;
using System.Collections.Generic;
using UnityEngine;

public class SfxLibrary : MonoBehaviour
{
    private Dictionary<string, List<AudioClip>> m_soundDictionary;
    [SerializeField] sfxGroup[] m_sfxGroups;





    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        m_soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach(sfxGroup sfx in m_sfxGroups)
        {

            m_soundDictionary[sfx.sfxName] = sfx.audioClips;

        }


    }

    public AudioClip GetRandomClip(string name)
    {
        if (m_soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = m_soundDictionary[name];
            if(audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }

        return null;


    }
}
[System.Serializable]
public struct sfxGroup
{

    public string sfxName;
    public List<AudioClip> audioClips;

}
