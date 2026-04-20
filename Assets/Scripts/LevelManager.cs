using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataCatalog levelsData;
    [SerializeField] private int level;
    [SerializeField] private HUDManager hudManager;
    private ObjectPlacementSystem _objectPlacementSystem;
    private ConnectionsManager _connectionsManager;
    private HUDManager _hudManager;
    private SceneLoader _sceneLoader;
    private GameManager _gameManager;
    
    private void OnEnable()
    {
        ConnectionsManager.OnCompletion += ReceiversConnected;
    }

    private void OnDisable()
    {
        ConnectionsManager.OnCompletion -= ReceiversConnected;
    }

    private void Start()
    {
        _objectPlacementSystem = FindObjectOfType<ObjectPlacementSystem>();
        _connectionsManager = FindObjectOfType<ConnectionsManager>();
        _hudManager = FindObjectOfType<HUDManager>();
        _sceneLoader = FindObjectOfType<SceneLoader>();
        _objectPlacementSystem.Initialize(levelsData.levelsData[level]);
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void ReceiversConnected()
    {
        _hudManager.ShowCompleteButton();
    }

    public void OnCompleteLevel()
    {
        _gameManager.MarkAsCompleted(level);
        _gameManager.SetLoadedFromLevel(true);
        GoToMainMenu();
    }

    public void ResetLevel()
    {
        _connectionsManager.ResetDevices();
        _objectPlacementSystem.ResetDevicesAmount();
        _hudManager.HideCompleteButton();
    }

    public void GoToMainMenu()
    {
        _sceneLoader.LoadMainMenu();
    }
}
