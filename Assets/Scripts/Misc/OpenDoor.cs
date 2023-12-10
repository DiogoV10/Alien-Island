using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Transform _minder;
    [SerializeField] private Transform _blocker;
    [SerializeField] private Transform _blocker2;

    private bool _readyToOpen = false;
    private Animator _openDoor;

    private void Awake()
    {
        _openDoor = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _minder.transform.GetComponent<Minder>().OnDeath += SetReadyToOpen;
    }

    private void SetReadyToOpen()
    {
        _readyToOpen = true;
        _blocker.gameObject.SetActive(false);
        _blocker2.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_readyToOpen && other.tag == "Player")
        {
            _openDoor.enabled = true;

            //this.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _openDoor.enabled = true;
        }
    }
}
