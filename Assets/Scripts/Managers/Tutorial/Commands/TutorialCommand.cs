using System;
using UnityEngine;

[Serializable]

public abstract class TutorialCommand : ScriptableObject
{
    public abstract void Execute(TutorialContext tutorialContext, Action onActionComplete);
}
