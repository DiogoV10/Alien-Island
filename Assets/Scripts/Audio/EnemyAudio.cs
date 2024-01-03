using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deathClips;
    [SerializeField] private AudioClip[] _hurtClips;
    [SerializeField] private AudioClip[] _idleClips;

    [Header("Audio Settings")]
    [SerializeField] private float _volume = 1f;
    [SerializeField] private float _pitchVariation = 0.1f;
    [SerializeField] private float _delayBetweenIdleSounds = 1f;

    private AudioSource _audioSource;
    private float _idleSoundTimer;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out _audioSource)) AddAudioSource();

        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f;
        _audioSource.volume = _volume;
    }

    private void AddAudioSource()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        _idleSoundTimer -= Time.deltaTime;
        if (_idleSoundTimer <= 0f)
        {
            PlayIdleSound();
            _idleSoundTimer = _delayBetweenIdleSounds * UnityEngine.Random.Range(.8f, 1.2f);
        }
        
    }

    private void PlaySound(AudioClip[] audioClip)
    {
        if (audioClip.Length == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, audioClip.Length);

        _audioSource.pitch = UnityEngine.Random.Range(1f - _pitchVariation, 1f + _pitchVariation);
        _audioSource.PlayOneShot(audioClip[randomIndex]);
    }

    public void PlayAttackSound()
    {
        PlaySound(_attackClips);
    }
    public void PlayDeathSound()
    {
        PlaySound(_deathClips);
    }
    public void PlayHurtSound()
    {
        PlaySound(_hurtClips);
    }
    public void PlayIdleSound()
    {
        PlaySound(_idleClips);
    }

}
