using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INextStepDetector
{
    public event Action OnActionDetectedEvent;
    public NextTutorialStepDetectionMethod GetNextStepDetectionMethod();
}
