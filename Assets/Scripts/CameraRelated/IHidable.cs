using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHidable : MonoBehaviour
{
    //Private Properties

    private List<Renderer> _renderers = new List<Renderer>();
    private List<Material> _materials = new List<Material>();

    private bool _isHidden = false;

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
        foreach (Material material in _materials)
        {
            if (material.HasProperty("_Opacity")) material.SetFloat("_Opacity", opacityValue);
        }
    }

    public void MakeOpaque()
    {
        _isHidden = false;
        foreach (Material material in _materials)
        {
            if (material.HasProperty("_Opacity")) material.SetFloat("_Opacity", 1);
        }
    }

}
