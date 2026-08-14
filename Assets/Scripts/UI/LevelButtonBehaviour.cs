using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Image lockImage;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image selectionFrame;
    private Color _unlockedTextColor;
    private Color _lockedTextColor;
    
    private Color _unlockedButtonColor;
    private Color _lockedButtonColor;
    private Color _completedButtonColor;

    public void Initialize(Color unlockedTextColor, Color lockedTextColor, Color unlockedButtonColor, Color lockedButtonColor, Color completedButtonColor)
    {
        _unlockedTextColor = unlockedTextColor;
        _lockedTextColor = lockedTextColor;
        _unlockedButtonColor = unlockedButtonColor;
        _lockedButtonColor = lockedButtonColor;
        _completedButtonColor = completedButtonColor;
    }

    public void LockButton()
    {
        button.interactable = false;
        
        lockImage.gameObject.SetActive(true);
        
        text.color = _lockedTextColor;
        
        image.color = _lockedButtonColor;
    }

    public void UnlockButton()
    {
        button.interactable = true;
        
        lockImage.gameObject.SetActive(false);
        
        text.color = _unlockedTextColor;
        
        image.color = _unlockedButtonColor;
        selectionFrame.gameObject.SetActive(true);
    }

    public void SetCompleted()
    {
        UnlockButton();
       
        image.color = _completedButtonColor;
        selectionFrame.gameObject.SetActive(false);
    }
}
