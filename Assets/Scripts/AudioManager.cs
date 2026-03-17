using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton
    private AudioSource source;

    void Awake()
    {
        if (Instance == null) Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        if (source != null) source.Play();
    }
}
