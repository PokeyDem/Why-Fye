using System;
using UnityEngine;

[Serializable]

public abstract class TutorialCommand : ScriptableObject
{
    [SerializeField] private bool waitForCompletion;
    public abstract void Execute(TutorialContext tutorialContext, Action onActionComplete);

    public bool WaitForCompletion()
    {
        return waitForCompletion;
    }
}
