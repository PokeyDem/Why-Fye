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
   [SerializeField] private float indicatorShiftX;
   [SerializeField] private float indicatorShiftY; 

   private float _fillAmount;
   private bool _isProgressing;

   public void StartFilling(Action onFinish, Vector2 position)
   {
      indicator.transform.position = new Vector3(position.x + indicatorShiftX, position.y + indicatorShiftY);
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