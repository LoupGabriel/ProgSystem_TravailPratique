using UnityEngine;



[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
}
public class SoundTrackLibrary : MonoBehaviour
{
    public MusicTrack[] m_tracks;

    public AudioClip GetClipFromName(string name)
    {
        foreach (MusicTrack track in m_tracks)
        {
            if(track.trackName == name)
            {
                return track.clip;
            }
        }
        return null;
    }
}
