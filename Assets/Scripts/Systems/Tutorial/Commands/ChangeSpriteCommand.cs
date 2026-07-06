using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Switch Sprite Command", menuName = "Tutorial Steps/Switch Sprite Command")]
public class ChangeSpriteCommand : TutorialCommand
{
    [SerializeField] private TutorialSpriteSwitcher.SpriteType spriteType;
    private Action _onComplete;
    private TutorialSpriteSwitcher _tutorialSpriteSwitcherRef;


    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete += onActionComplete;
        _tutorialSpriteSwitcherRef = tutorialContext.TutorialSpriteSwitcher;

        _tutorialSpriteSwitcherRef.OnImageSwitched += OnSpriteChanged;
        _tutorialSpriteSwitcherRef.SwitchImage(spriteType);

    }
    
    private void OnSpriteChanged()
    {
       _tutorialSpriteSwitcherRef.OnImageSwitched -= OnSpriteChanged;
        _onComplete?.Invoke();
    }
}
