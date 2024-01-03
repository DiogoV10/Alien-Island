using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private float pitchVariation = 0.1f;

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
    }

    public void PlaySoundAt(AudioClip audioClip, Vector3 position, float volume = 1f , bool hasPitchVariation = true)
    {
        if(audioClip == null) return;

        GameObject audioObject = new("AudioObject");
        audioObject.transform.position = position;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.volume = volume;

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
}
