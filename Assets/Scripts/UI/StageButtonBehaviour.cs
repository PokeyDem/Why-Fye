using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    
    public void LockButton()
    {
        button.interactable = false;
        lockImage.SetActive(true);
        
        image.color = lockedColor;
    }

    public void UnlockButton()
    {
        button.interactable = true;
        lockImage.SetActive(false);
        
        image.color = unlockedColor;
    }
}
