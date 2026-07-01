using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToHighlight;
    private int highlightedObjectIndex;
    
    public event Action OnHighlightComplete;

    public void HighlightObject(int index)
    {
        if (highlightedObjectIndex != -1)
            objectsToHighlight[highlightedObjectIndex].SetActive(false);
        
        objectsToHighlight[index].SetActive(true);
        highlightedObjectIndex = index;
        
        OnHighlightComplete?.Invoke();
    }
}
