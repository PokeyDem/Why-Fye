using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenVisualsController : MonoBehaviour
{
    [SerializeField] private Color connectedColor;
    [SerializeField] private Color disconnectedColor;
    [SerializeField] private int screenMaterialIndex;
    
    private Renderer _renderer;

    private int _customFloatID;
    private int _customColorID;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _customColorID = Shader.PropertyToID("_ScreenColor");
        _customFloatID = Shader.PropertyToID("_Intensity");
        DisconnectDevice();
    }

    public void ConnectDevice()
    {
        Material[] allMaterials = _renderer.materials;
        
        allMaterials[screenMaterialIndex].SetColor(_customColorID, connectedColor);
    }

    public void DisconnectDevice()
    {
        Material[] allMaterials = _renderer.materials;
        
        allMaterials[screenMaterialIndex].SetColor(_customColorID, disconnectedColor);
    }
}
