using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string isConnectedBoolName;
    [SerializeField] private GameObject canvas;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ConnectDevice()
    {
        _animator.SetBool(isConnectedBoolName, true);
        canvas.SetActive(false);
    }

    public void DisconnectDevice()
    {
        _animator.SetBool(isConnectedBoolName, false);
        canvas.SetActive(true);
    }
}
