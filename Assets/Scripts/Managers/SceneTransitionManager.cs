using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private Image mask;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private Vector3 minScale = new Vector3(0.02f, 0.02f, 0.02f);
    [SerializeField] private Vector3 maxScale = new Vector3(55f, 55f, 55f);

    private bool _isTransitioning;

    private void Start()
    {
        StartCoroutine(PlayFadeOutAndSwitch());
    }

    public void MakeTransition(Action switchScene)
    {
        StartCoroutine(PlayFadeInAndSwitch(switchScene));
    }
    public IEnumerator PlayFadeInAndSwitch(Action switchScene)
    {
        _isTransitioning = true;
        
        yield return StartCoroutine(ScaleRoutine(maxScale, minScale));
        switchScene.Invoke();
        
        _isTransitioning = false;
    }
    
    public IEnumerator PlayFadeOutAndSwitch()
    {
        _isTransitioning = true;
        
        yield return StartCoroutine(ScaleRoutine(minScale, maxScale));

        _isTransitioning = false;
    }
    

    private IEnumerator ScaleRoutine(Vector3 start, Vector3 end)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            float easedT = Mathf.SmoothStep(0, 1, t);
            
            mask.rectTransform.localScale = Vector3.Lerp(start, end, easedT);
            yield return null;
        }
        mask.rectTransform.localScale = end;
    }
}
