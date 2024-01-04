using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetAndDetach : MonoBehaviour
{
    private Transform _target;
    private Vector3 _startOffset;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        _target = transform.parent;
        _startOffset = transform.localPosition;
        transform.parent = null;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position + _startOffset;
        }
    }
}
