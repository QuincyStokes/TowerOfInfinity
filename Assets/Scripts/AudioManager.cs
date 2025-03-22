using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [Header("Audio Source Pool")]
    [SerializeField] private int poolSize = 10;
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup backgroundAMG;
    [Header("Background Music")]
    [SerializeField] private List<AudioClip> backgroundMusic;
    private List<AudioSource> audioSourcePool;
    private bool backgroundMusicPlaying;


    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize pool
            InitializeAudioSourcePool();
        }
        else
        {
            Destroy(gameObject);
        }
        backgroundMusicPlaying = false;
    }

    private void Update()
    {
        if(backgroundMusicPlaying == false)
        {
            backgroundMusicPlaying = true;
            AudioClip backgroundMusicToPlay = backgroundMusic[Random.Range(0, backgroundMusic.Count)];
            StartCoroutine(SongCooldown(backgroundMusicToPlay));
            Debug.Log($"Playing song {backgroundMusicToPlay.name} at 5% volume");
            PlayOneShot(backgroundMusicToPlay, .05f, backgroundAMG);
        }   
    }

    private void InitializeAudioSourcePool()
    {
        audioSourcePool = new List<AudioSource>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject("AudioSource_" + i);
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            audioSourcePool.Add(source);
        }
    }

    public void PlayOneShot(AudioClip clip, float volume, AudioMixerGroup mixerGroup, AudioSource audioSource = null)
    {
        AudioSource source;
        if(audioSource == null)
        {
            source = GetAvailableSource();
        }
        else{
            source = audioSource;
        }
        
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = volume;
        source.spatialBlend = 0f; // 2D by default; adjust as needed
        source.PlayOneShot(clip);
    }

    private AudioSource GetAvailableSource()
    {
        // Return first available or oldest if none free
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
                return source;
        }
        // If all are playing, return the first one
        return audioSourcePool[0];
    }

    public void PlayOneShotVariedPitch(AudioClip clip, float volume, AudioMixerGroup mixerGroup, float pitchOffset)
    {
        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = volume;
        source.spatialBlend = 0f; // 2D by default; adjust as needed
        source.pitch = Random.Range(1-pitchOffset, 1+pitchOffset);
        source.PlayOneShot(clip);
    }

    private IEnumerator SongCooldown(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        backgroundMusicPlaying = false;
    }
}
