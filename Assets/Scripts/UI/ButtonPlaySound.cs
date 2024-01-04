using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPlaySound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip _onHoverSound;
    [SerializeField] private AudioClip _onClickSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    public void OnHover()
    {
        audioSource.PlayOneShot(_onHoverSound, AudioManager.Instance.soundMultiplier);
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(_onClickSound, AudioManager.Instance.soundMultiplier);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        OnClick();
    }
}
