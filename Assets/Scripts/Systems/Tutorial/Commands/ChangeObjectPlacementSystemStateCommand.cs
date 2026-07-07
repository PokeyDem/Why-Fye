using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Change Object Placement System State Command", menuName = "Tutorial Steps/Change Object Placement System State Command")]
public class ChangeObjectPlacementSystemStateCommand : TutorialCommand
{
    [SerializeField] private bool enablePlacement;
    private Action _onComplete;
    private ObjectPlacementSystem _objectPlacementSystemRef;


    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete = onActionComplete;
        _objectPlacementSystemRef = tutorialContext.objectPlacementSystem;

        _objectPlacementSystemRef.OnSystemStateUpdated += OnSystemStateChanged;
        
        if (enablePlacement)
            _objectPlacementSystemRef.EnablePlacement();
        else
            _objectPlacementSystemRef.DisablePlacement();
    }

    private void OnSystemStateChanged()
    {
        _objectPlacementSystemRef.OnSystemStateUpdated -= OnSystemStateChanged;
        _onComplete?.Invoke();
    }
}
