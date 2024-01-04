using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    public Material firstSkybox; 
    public Material secondSkybox; 
    private Material originalSkybox; 

    private void Start()
    {
        originalSkybox = RenderSettings.skybox; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleSkybox(); 
        }
    }

    private void ToggleSkybox()
    {
        if (RenderSettings.skybox == firstSkybox) 
        {
            RenderSettings.skybox = secondSkybox; 
        }
        else
        {
            RenderSettings.skybox = firstSkybox; 
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.skybox = originalSkybox; 
        }
    }
}
