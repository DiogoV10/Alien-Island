using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHidable : MonoBehaviour
{
    //Private Properties

    private List<Renderer> _renderers = new List<Renderer>();
    private List<Material> _materials = new List<Material>();

    private bool _isHidden = false;

    //Fade Properties
    private float _timeToFade = 2f;
    private float _fadeTimer = 0f;
    [Range(0, 1)] private float _fadeProgression = 0;
    private FadeState _fadeState = FadeState.Idle;

    private void Awake()
    {
        GetAllRenderers();
    }

    //
    //ADD METHOD TO FORCE OBJECTS WITH THIS COMPONENT TO HAVE THE PROPER SHADER
    //
    //ALSO, MAKE IT POSSIBLE TO LERP THE FADE
    //

    private void GetAllRenderers()
    {
        Renderer renderer = GetComponent<Renderer>();
        
        if(renderer != null)
        {
            _renderers.Add(renderer);
            _materials.Add(renderer.material);
        }

        Renderer[] childRenderes = GetComponentsInChildren<Renderer>();

        foreach (var rend in childRenderes)
        {
            _renderers.Add(rend);

            foreach (var mat in rend.materials)
            {
                _materials.Add(mat);
            }
        }
    }

    public void Fade(float opacityValue)
    {
        _isHidden = true;
        StopCoroutine(nameof(FadeOrUnfade));
        StartCoroutine(FadeOrUnfade(opacityValue));

        //foreach (Material material in _materials)
        //{
        //    if (material.HasProperty("_Opacity")) material.SetFloat("_Opacity", opacityValue);
        //}
    }

    public void MakeOpaque()
    {
        _isHidden = false;
        StopCoroutine(nameof(FadeOrUnfade));
        StartCoroutine(FadeOrUnfade(1));
        //foreach (Material material in _materials)
        //{
        //    if (material.HasProperty("_Opacity")) material.SetFloat("_Opacity", 1);
        //}
    }

    private IEnumerator FadeOrUnfade(float targetValue)
    {
        float elapsedTime = 0;

        while (elapsedTime < _timeToFade)
        {
            // Calculate the interpolation value between 0 and 1 based on elapsed time and _timeToFade
            float t = elapsedTime / _timeToFade;

            foreach (Material material in _materials)
            {
                if (material.HasProperty("_Opacity"))
                {
                    // Lerp the opacity value over time
                    material.SetFloat("_Opacity", Mathf.Lerp(material.GetFloat("_Opacity"), targetValue, t));
                }
            }

            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure that the final opacity value is set to the target value
        foreach (Material material in _materials)
        {
            if (material.HasProperty("_Opacity")) material.SetFloat("_Opacity", targetValue);
        }
    }

}

public enum FadeState
{
    Fading,
    Unfading,
    Idle
}
