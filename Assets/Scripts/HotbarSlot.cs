using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI counterTextField;
    [SerializeField] GameObject frame;
    private int counter;


    public void ChangeFrameState(bool newState)
    {
        frame.SetActive(newState);
    }

    public void SetCounter(int newValue)
    {
        counter = newValue;
        counterTextField.text = "x" + counter;
    }

    public void DecreaseCounter()
    {
        if (counter == 0)
            return;
        
        Debug.Log("DecreaseCounter (Hud) called");
        counter--;
        counterTextField.text = "x" + counter;
    }
}
