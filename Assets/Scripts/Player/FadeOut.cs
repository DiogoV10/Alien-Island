using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{


    [SerializeField] private float fadeTime = 2.0f;
    [SerializeField] private List<Renderer> rendererList;


    private List<Color> originalColors;
    private float alpha;


    private void Start()
    {
        originalColors = new List<Color>();

        foreach (Renderer renderer in rendererList)
        {
            if (renderer != null)
            {
                originalColors.Add(renderer.material.GetColor("_GlowColor"));
            }
        }


        alpha = 1.0f;
    }

    private void Update()
    {
        alpha -= Time.deltaTime / fadeTime;

        for (int i = 0; i < rendererList.Count; i++)
        {
            Renderer renderer = rendererList[i];

            if (renderer != null)
            {
                Color newEmissionColor = originalColors[i] * Mathf.Clamp01(alpha);
                renderer.material.SetColor("_GlowColor", newEmissionColor);
            }
        }

        if (alpha <= 0.0f)
        {
            Destroy(gameObject);
        }
    }


}

