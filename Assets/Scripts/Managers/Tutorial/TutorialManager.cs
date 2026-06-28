using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private ObjectPlacementSystem objectPlacementSystem;
    [SerializeField] private CameraPivotControl cameraPivotControl;
    [SerializeField] private NextStepDetectorManager nextStepDetectorManager;
    [SerializeField] private List<TutorialStep> tutorialSteps;
    [SerializeField] private GameObject _tutorialWindow;
    private TextMeshProUGUI _tutorialTextField;
    private int _currentStep = 0;
    
    public static event Action<List<int>> OnDeviceAmountUpdate;

    private void OnEnable()
    {
        NextStepDetectorManager.OnNextStepDetected += SetNextStep;
    }

    private void Awake()
    {
        _tutorialTextField = _tutorialWindow.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (GameManager.Instance.GetTargetLevel() == 1)
        {
            EnableTutorial();
        }
    }

    private void EnableTutorial()
    {
        _tutorialWindow.SetActive(true);
        SetStep(0);
    }

    private void DisableTutorial()
    {
        _tutorialWindow.SetActive(false);
        _currentStep = 0;
        nextStepDetectorManager.DisableAllDetectors();
    }

    private void SetNextStep()
    {
        _currentStep++;
        SetStep(_currentStep);
    }

    private void SetStep(int step)
    {
        if (step > 0 && tutorialSteps[step - 1].highlightObject)
            tutorialSteps[step - 1].objectToHighlight.SetActive(false);
        
        if (step == tutorialSteps.Count)
        {
            DisableTutorial();
            return;
        }
        
        nextStepDetectorManager.SetDetectionMethod(tutorialSteps[step].nextStepDetectionMethod);
        _tutorialTextField.text = tutorialSteps[step].tutorialText;
        
        if (tutorialSteps[step].changeCameraPivot)
            cameraPivotControl.ChangePivotPositionTransform(tutorialSteps[step].newCameraPivot);
        
        if (tutorialSteps[step].changeAmountOfDevices)
            OnDeviceAmountUpdate?.Invoke(tutorialSteps[step].newAmountOfDevices);

        if (tutorialSteps[step].highlightObject)
            tutorialSteps[step].objectToHighlight.SetActive(true);

    }

    [Serializable]
    struct TutorialStep
    {
        [TextArea(3, 10)]
        public string tutorialText;
        
        [Tooltip("Camera pivot preferences for this step")]
        public bool changeCameraPivot;
        public Transform newCameraPivot;
        
        [Tooltip("Highlight an object for this step")]
        public bool highlightObject;
        public GameObject objectToHighlight;

        public bool changeAmountOfDevices;
        
        [Range(0,3)]
        public List<int> newAmountOfDevices;
        
        [Tooltip("Method to trigger next step switch")]
        public NextTutorialStepDetectionMethod nextStepDetectionMethod;
    }
}

public enum NextTutorialStepDetectionMethod
{
    OnScreenClick,
    OnObjectPlacement
}