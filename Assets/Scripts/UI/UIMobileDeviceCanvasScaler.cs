using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMobileDeviceCanvasScaler : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private Vector2 referenceResolution = new Vector2(2560f, 1440f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            AdjustScale();
    }

    private void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        AdjustScale();
    }
    
    private void AdjustScale()
    {
        float targetAspect = referenceResolution.x / referenceResolution.y;
        float currentAspect = (float)Screen.width / (float)Screen.height;

        if (currentAspect >= targetAspect)
            canvasScaler.matchWidthOrHeight = 1f;
        else
            canvasScaler.matchWidthOrHeight = 0f;
    }
}
