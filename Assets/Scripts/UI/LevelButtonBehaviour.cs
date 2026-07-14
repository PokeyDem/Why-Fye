using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Image lockImage;
    [SerializeField] private Image checkmarkImage;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    private Color _unlockedTextColor;
    private Color _lockedTextColor;
    private int _unlockedButtonAlpha;
    private int _lockedButtonAlpha;

    public void Initialize(Color unlockedTextColor, Color lockedTextColor, int unlockedButtonAlpha, int lockedButtonAlpha)
    {
        _unlockedTextColor = unlockedTextColor;
        _lockedTextColor = lockedTextColor;
        _unlockedButtonAlpha = unlockedButtonAlpha;
        _lockedButtonAlpha = lockedButtonAlpha;
    }

    public void LockButton()
    {
        button.interactable = false;
        
        lockImage.gameObject.SetActive(true);
        checkmarkImage.gameObject.SetActive(false);
        
        text.color = _lockedTextColor;
        
        var color = image.color;
        color.a = _lockedButtonAlpha;
        image.color = color;
    }

    public void UnlockButton()
    {
        button.interactable = true;
        
        lockImage.gameObject.SetActive(false);
        checkmarkImage.gameObject.SetActive(false);
        
        text.color = _unlockedTextColor;
        
        var color = image.color;
        color.a = _unlockedButtonAlpha;
        image.color = color;
    }

    public void SetCompleted()
    {
        lockImage.gameObject.SetActive(false);
        checkmarkImage.gameObject.SetActive(true);
       
        var color = image.color;
        color.a = _unlockedButtonAlpha;
        image.color = color;
    }
}
