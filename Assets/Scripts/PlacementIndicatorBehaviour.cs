using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementIndicatorBehaviour : MonoBehaviour
{
   [SerializeField] private float fillDuration;
   [SerializeField] private float fillStepAmount;
   [SerializeField] private Image indicator;

   private float _fillAmount;
   private bool _isProgressing;

   public void StartFilling(Action onFinish, Vector2 position)
   {
      indicator.transform.position = position;
      _isProgressing = true;
      StartCoroutine(Fill(onFinish));
   }

   public void StopFilling()
   {
      _isProgressing = false;
      indicator.fillAmount = 0;
   }
   
   private IEnumerator Fill(Action onFinish)
   {
      float elapsedTime = 0;
      while (elapsedTime < fillDuration && _isProgressing)
      {
         elapsedTime += Time.deltaTime;
         indicator.fillAmount = elapsedTime / fillDuration;
         yield return null;
      }
      if (indicator.fillAmount >= 1)
         onFinish?.Invoke();
      indicator.fillAmount = 0;
   }
}