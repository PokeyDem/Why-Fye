using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSpriteSwitcher : MonoBehaviour
{
    [SerializeField] private List<SpriteWithType> images;
    [SerializeField] private Image image;

    public event Action OnImageSwitched;
    
    public void SwitchImage(SpriteType type)
    {
        image.sprite = images.Find(i => i.type == type).sprite;
        OnImageSwitched?.Invoke();
    }
    

    public enum SpriteType
    {
        Greetings,
        Neutral,
        Warning
    }

    [Serializable]
    public struct SpriteWithType
    {
        [SerializeField] public Sprite sprite;
        [SerializeField] public SpriteType type;
    }
}
