using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBetweenTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject target;
    [SerializeField] LayerMask fadeableLayers;

    [Header("Fade Values")]
    [SerializeField] private float _fadeValue = .3f;
    [SerializeField] private float _revealCheckInterval = .5f;

    //Internal Fields
    private List<IHidable> _currentlyHidden = new List<IHidable>();
    private float _timeSinceLastCheck = 0f;
    private IHidable _lastHiddenObject;

    private void Update()
    {
        _timeSinceLastCheck += Time.deltaTime;
        if (_timeSinceLastCheck >= _revealCheckInterval) CheckForReveal();
    }

    private void FixedUpdate()
    {
        RaycastHit[] hit = Physics.RaycastAll(transform.position, GetDirection(), GetDistance());
        if (hit != null)
        {
            foreach (RaycastHit objectToHide in hit)
            {
                FadeObject(objectToHide);
            }
        }
    }

    private void FadeObject(RaycastHit objectToHide)
    {
        IHidable ihidable = objectToHide.transform.GetComponentInParent<IHidable>();
        if (ihidable == null) return;

        if (!_currentlyHidden.Contains(ihidable))
        {
            _currentlyHidden.Add(ihidable);
            ihidable.Fade(_fadeValue);
        }
        _lastHiddenObject = ihidable;
    }

    private float GetDistance()
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }

    private Vector3 GetDirection()
    {
        return (target.transform.position - transform.position).normalized;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, GetDirection() * GetDistance());
    }

    private void CheckForReveal()
    {
        if (_currentlyHidden.Count == 0) return;

        foreach (IHidable ihidable in _currentlyHidden)
        {
            if(ihidable != _lastHiddenObject && ihidable != null) ihidable.MakeOpaque();
        }

        _currentlyHidden.Clear();
        _currentlyHidden.Add(_lastHiddenObject);
        _timeSinceLastCheck = 0f;
        _lastHiddenObject = null;
    }
}
