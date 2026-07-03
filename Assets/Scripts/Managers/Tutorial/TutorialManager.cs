using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.tvOS;

public class TutorialManager : MonoBehaviour
{
    [Header("Systems used in the tutorial")]
    [SerializeField] private ObjectPlacementSystem objectPlacementSystem;
    [SerializeField] private CameraPivotControl cameraPivotControl;
    [SerializeField] private CameraOrbit cameraOrbit;
    [SerializeField] private NextStepDetectorsManager nextStepDetectorsManager;
    [SerializeField] private TextPrinter textPrinter;
    [SerializeField] private ObjectHighlighter objectHighlighter;
    [SerializeField] private DeviceAmountActualizer deviceAmountActualizer;
    
    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialWindow;
    
    [Header("Commands for each step")]
    [SerializeField] private List<TutorialStepCommands> tutorialStepCommands;
    
    private TextMeshProUGUI _tutorialTextField;
    private TutorialContext _tutorialContext;
    
    private int _currentStepIndex;
    private int _currentCommandIndex;

    private void Awake()
    {
        _tutorialTextField = tutorialWindow.GetComponentInChildren<TextMeshProUGUI>();
        textPrinter.Initialize(_tutorialTextField);
    }

    private void Start()
    {
        InitializeTutorialContext();
        
        if (GameManager.Instance.GetTargetLevel() == 1)
        {
            EnableTutorial();
        }
    }

    private void InitializeTutorialContext()
    {
        _tutorialContext = new TutorialContext();
        _tutorialContext.textPrinter = textPrinter;
        _tutorialContext.cameraPivotControl = cameraPivotControl;
        _tutorialContext.objectHighlighter = objectHighlighter;
        _tutorialContext.deviceAmountActualizer = deviceAmountActualizer;
        _tutorialContext.cameraOrbit = cameraOrbit;
        _tutorialContext.nextStepDetectorsManager = nextStepDetectorsManager;
    }
    
    private void SetStep(int step, int command)
    {
        _currentStepIndex = step;
        _currentCommandIndex = command;
        
        bool isExecutingSequenceWithoutWaiting = true;
        TutorialCommand currentCommand;
        
        while (isExecutingSequenceWithoutWaiting)
        {
            currentCommand = tutorialStepCommands[_currentStepIndex].commands[_currentCommandIndex];
            if (currentCommand.WaitForCompletion())
            {
                currentCommand.Execute(_tutorialContext, OnCommandFinished);
                isExecutingSequenceWithoutWaiting = false;
            }
            else
            {
                currentCommand.Execute(_tutorialContext,null);
                IncrementStepAndCommandIndices();
            }
        }
    }

    private void OnCommandFinished()
    {
        Debug.Log("Command finished");
        bool tutorialFinished = IncrementStepAndCommandIndices();
        if (!tutorialFinished)
        {
            SetStep(_currentStepIndex, _currentCommandIndex);
        }
    }

    private bool IncrementStepAndCommandIndices()
    {
        if (_currentCommandIndex + 1 < tutorialStepCommands[_currentStepIndex].commands.Count)
        {
            _currentCommandIndex++;
            
        }
        else if (_currentStepIndex + 1 < tutorialStepCommands.Count)
        {
            _currentStepIndex++;
            _currentCommandIndex = 0;
        }
        else
        {
            DisableTutorial();
            return true;
        }
        return false;
    }

    private void EnableTutorial()
    {
        tutorialWindow.SetActive(true);
        _currentCommandIndex = 0;
        _currentStepIndex = 0;
        
        SetStep(_currentStepIndex, _currentCommandIndex);
    }

    private void DisableTutorial()
    {
        tutorialWindow.SetActive(false);
        _currentStepIndex = 0;
        nextStepDetectorsManager.DisableAllDetectors();
    }

    [Serializable]
    public struct TutorialStepCommands
    {
        public List<TutorialCommand> commands;
    }
}

public enum NextTutorialStepDetectionMethod
{
    OnScreenClick,
    OnObjectPlacement
}