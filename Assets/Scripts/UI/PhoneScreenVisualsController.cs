using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneScreenVisualsController : MonoBehaviour
{
    [SerializeField] private Color connectedColor;
    [SerializeField] private Color disconnectedColor;
    [SerializeField] private int screenMaterialIndex;
    [SerializeField] private GameObject noConnectionImage;
    [SerializeField] private float nextFlashDelay;
    
    private Renderer _renderer;

    private int _customFloatID;
    private int _customColorID;
    private bool _isConnected;

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

        _isConnected = true;
    }

    public void DisconnectDevice()
    {
        Material[] allMaterials = _renderer.materials;
        
        allMaterials[screenMaterialIndex].SetColor(_customColorID, disconnectedColor);

        StartCoroutine(FlashNoConnectionImage());
        _isConnected = false;
    }

    private IEnumerator FlashNoConnectionImage()
    {
        while (!_isConnected)
        {
            yield return new WaitForSeconds(nextFlashDelay);
            noConnectionImage.SetActive(!noConnectionImage.activeSelf);
        }
        noConnectionImage.SetActive(false);
    }
}
