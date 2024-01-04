using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Range(0,1)] public float soundMultiplier = 1f;    
    [SerializeField] private float pitchVariation = 0.1f;
    [SerializeField] private AudioMixer volumeMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("AudioManager already exists");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundAt(AudioClip audioClip, Vector3 position, float volume = 1f , bool hasPitchVariation = true)
    {
        if(audioClip == null) return;

        GameObject audioObject = new("AudioObject");
        audioObject.transform.position = position;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.volume = volume * soundMultiplier;

        if (hasPitchVariation)
        {
            audioSource.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        }
        audioSource.PlayOneShot(audioClip);
        Destroy(audioObject, audioClip.length);
    }
    
    public void PlaySoundAt(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        if (audioClips.Length == 0) return;
        int randomIndex = Random.Range(0, audioClips.Length);

        PlaySoundAt(audioClips[randomIndex], position, volume);
    }

    public void ChangeSoundMultiplier(float newValue)
    {
        soundMultiplier = newValue;
        volumeMixer.SetFloat("Volume", Mathf.Lerp(-30f,0f,newValue));
    }
}
