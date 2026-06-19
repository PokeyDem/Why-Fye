using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataCatalog levelsData;
    [SerializeField] private int level = -1;
    [SerializeField] private HUDManager hudManager;
    private ObjectPlacementSystem _objectPlacementSystem;
    private ConnectionsManager _connectionsManager;
    private HUDManager _hudManager;
    private SceneLoader _sceneLoader;
   
    private void OnEnable()
    {
        ConnectionsManager.OnCompletion += ReceiversConnected;
        ConnectionsManager.OnCompletionRevoke += ReceiversDisconnected;
    }

    private void OnDisable()
    {
        ConnectionsManager.OnCompletion -= ReceiversConnected;
        ConnectionsManager.OnCompletionRevoke -= ReceiversDisconnected;
    }

    private void Start()
    {
        _objectPlacementSystem = FindObjectOfType<ObjectPlacementSystem>();
        _connectionsManager = FindObjectOfType<ConnectionsManager>();
        _hudManager = FindObjectOfType<HUDManager>();
        _sceneLoader = FindObjectOfType<SceneLoader>();
        level = GameManager.Instance.GetTargetLevel();
        _sceneLoader.SwitchLevelEnv(GameManager.Instance.GetTargetLevel(), CleanUpLevel, InitializeNewLevel, true);
    }

    private void ReceiversConnected()
    {
        _hudManager.ShowCompleteButton();
    }

    private void ReceiversDisconnected()
    {
        _hudManager.HideCompleteButton();
    }

    public void OnCompleteLevelClick()
    {
        _hudManager.HideCompleteButton();
        GameManager.Instance.MarkAsCompleted(level-1);
        level++;
        _sceneLoader.SwitchLevelEnv(level, CleanUpLevel, InitializeNewLevel, false);
    }

    private void InitializeNewLevel()
    {
        if (level == -1)
        {
            level = GameManager.Instance.GetTargetLevel();
        }
        
        _connectionsManager.FindNewReceivers();
        _objectPlacementSystem.Initialize(levelsData.levelsData[level-1]);
    }

    private void CleanUpLevel()
    {
        _connectionsManager.ResetDevices();
        _hudManager.HideCompleteButton();
    }

    public void ResetLevel()
    {
        _connectionsManager.ResetDevices();
        _objectPlacementSystem.ResetDevicesAmount();
        _hudManager.HideCompleteButton();
    }

    public void OnExitToMainMenuClick()
    {
        level = -1;
        GameManager.Instance.SetLoadedFromLevel(true);
        _sceneLoader.LoadMainMenuLevel();
    }
    
}
