using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointerOverUIDetector : MonoBehaviour
{
    public static PointerOverUIDetector Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        
        if (Instance == null)
            Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    
    public bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.isInProgress && EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue()))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
