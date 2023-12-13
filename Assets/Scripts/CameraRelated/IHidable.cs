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
    private float _timeToFade = 1f;
    private float _fadeTimer = 0f;
    [Range(0, 1)] private float _fadeProgression = 0;
    private FadeState _fadeState = FadeState.Idle;

    //Bug fixing
    private float _graceTime = 0.1f;
    private float _graceTimer = 0f;
    private bool _hideWasCalled = false;

    [SerializeField] private Shader _litShader;
    [SerializeField] private Shader _fadeShader;



    private void Awake()
    {
        _litShader = Shader.Find("Universal Render Pipeline/Lit");
        _fadeShader = Shader.Find("Shader Graphs/TransparentWithShadows");
        GetAllRenderers();
        ToggleFadeShader(false);
    }

    //
    //ADD METHOD TO FORCE OBJECTS WITH THIS COMPONENT TO HAVE THE PROPER SHADER
    //

    private void ToggleFadeShader(bool on)
    {
        var targetShader = on ? _fadeShader : _litShader;
        foreach (var renderer in _renderers)
        {
            foreach (var material in renderer.materials)
            {
                material.shader = targetShader;
                if (material.HasProperty("_Color")) material.SetColor("_Color", Color.black);
            }
        }

    }



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

    public void FadeCall(float opacityValue)
    {
        ToggleFadeShader(true);
        _isHidden = true;
        StopCoroutine(nameof(MakeOpaque));
        StartCoroutine(Fade(opacityValue));

        //foreach (Material material in _materials)
        //{
        //    if (material.HasProperty("_Opacity")) material.SetFloat("_Opacity", opacityValue);
        //}
    }

    public void MakeOpaqueCall()
    {
        _isHidden = false;
        StopCoroutine(nameof(Fade));
        StartCoroutine(MakeOpaque(1));
        //foreach (Material material in _materials)
        //{
        //    if (material.HasProperty("_Opacity")) material.SetFloat("_Opacity", 1);
        //}
    }

    private IEnumerator Fade(float targetValue, float graceTime = 0)
    {
        Debug.Log("Fading");
        yield return new WaitForSeconds(graceTime);
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

        if(targetValue == 1)
        {
            ToggleFadeShader(false);
        }
        
    }

    private IEnumerator MakeOpaque(float targetValue, float graceTime = 0)
    {
        Debug.Log("Opaquing");
        yield return new WaitForSeconds(graceTime);
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

        if (targetValue == 1)
        {
            ToggleFadeShader(false);
        }

    }

}

public enum FadeState
{
    Fading,
    Unfading,
    Idle
}
