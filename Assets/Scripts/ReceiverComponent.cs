
using System.Collections.Generic;
using UnityEngine;

public class ReceiverComponent : MonoBehaviour
{
    public static readonly List<GameObject> ActiveReceivers = new List<GameObject>();

    private void OnEnable()
    {
        ActiveReceivers.Add(gameObject);
    }

    private void OnDisable()
    {
        ActiveReceivers.Remove(gameObject);
    }
}
