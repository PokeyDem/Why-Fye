using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] Image tutorialImage;
    [SerializeField] TextMeshProUGUI tutorialButtonText;
    [SerializeField] GameObject tutorialGameObject;
    [SerializeField] ObjectPlacementSystem objectPlacementSystem;
    
    private int _currentStep = 0;

    private void Start()
    {
        objectPlacementSystem.StopPlacement();
        tutorialText.text = tutorialSteps[_currentStep].tutorialText;
        tutorialImage.sprite = tutorialSteps[_currentStep].tutorialSprite;
        tutorialButtonText.text = tutorialSteps[_currentStep].tutorialButtonText;
    }

    public void OnNextClick()
    {
        _currentStep++;
        if (_currentStep <= tutorialSteps.Count - 1)
        {
            tutorialText.text = tutorialSteps[_currentStep].tutorialText;
            tutorialImage.sprite = tutorialSteps[_currentStep].tutorialSprite;
            tutorialButtonText.text = tutorialSteps[_currentStep].tutorialButtonText;
        }
        else
        {
            tutorialGameObject.SetActive(false);
            objectPlacementSystem.ResumePlacement();
        }
    }
    
    [Serializable]
    struct TutorialStep
    {
        [TextArea(3, 10)]
        public string tutorialText;
        public string tutorialButtonText;
        public Sprite tutorialSprite;
    }

}