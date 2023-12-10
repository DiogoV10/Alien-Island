using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] private GameObject _text;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _text.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _text.SetActive(false);
    }
}
