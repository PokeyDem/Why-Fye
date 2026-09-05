using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataCatalog levelsData;
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
        _sceneLoader.SwitchLevelEnv(GameManager.Instance.GetTargetLevelStage(), GameManager.Instance.GetTargetLevel(), CleanUpLevel, InitializeNewLevel, true);
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
        GameManager.Instance.MarkAsCompleted(GameManager.Instance.GetTargetLevelStage(), GameManager.Instance.GetTargetLevel() - 1);
        GameManager.Instance.IncreaseTargetLevel();
        _sceneLoader.SwitchLevelEnv(GameManager.Instance.GetTargetLevelStage(), GameManager.Instance.GetTargetLevel(), CleanUpLevel, InitializeNewLevel, false);
    }

    private void InitializeNewLevel()
    {
        _connectionsManager.FindNewReceivers();
        
        if (GameManager.Instance.IsTutorialActive())
            return;
        
        _objectPlacementSystem.Initialize(levelsData.levelsData[GameManager.Instance.GetRawLevelIndex()]);
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
        GameManager.Instance.SetLoadedFromLevel(true);
        _sceneLoader.LoadMainMenuLevel();
    }
    
}
