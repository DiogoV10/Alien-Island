using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionaryDecoy : MonoBehaviour
{
    private SkillSO skill;

    private float startTime;
    private float duration;

    private void Start()
    {
        startTime = Time.time;
        duration = skill.duration;
    }

    private void Update()
    {
        if (Time.time - startTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(SkillSO skillSO)
    {
        skill = skillSO;
    }
}
