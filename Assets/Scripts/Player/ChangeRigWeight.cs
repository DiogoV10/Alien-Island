using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ChangeRigWeight : MonoBehaviour
{


    public static ChangeRigWeight Instance { get; private set; }


    private Rig rig;

    private float targetWeight;


    private void Awake()
    {
        Instance = this;

        rig = GetComponent<Rig>();
    }

    private void Update()
    {
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * 10f);
    }

    public void SetRigWeight(float weight)
    {
        targetWeight = weight;
    }


}
