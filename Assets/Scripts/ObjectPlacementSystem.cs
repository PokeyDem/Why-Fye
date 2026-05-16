using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectPlacementSystem : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float slopeAngle = 0f;
    [SerializeField] private bool placementModeEnabled = false;

    [SerializeField] private LevelData currentLevelData;
    [SerializeField] private DeviceCatalog prefabCatalog;
    
    [SerializeField] private Vector3 previewPrefabsIdlePos;
    [SerializeField] private int selectedPrefabIndex = 0; 
    [SerializeField] private ConnectionsManager connectionsManager;
    [SerializeField] private PlayerControls playerControls;
    
    [SerializeField] private PlacementIndicatorBehaviour placementIndicator;
    
    private Camera _camera;

    private bool _validPos;
    private Vector3 _currentPreviewPos;
    private Vector3 _currentSurfaceNormal;
    private Dictionary<int, int> _amountOfDevices = new Dictionary<int,int>();
    
    public static event Action<Dictionary<int, int>> OnInitialization;
    public static event Action OnObjectPlaced;

    private void OnEnable()
    {
        playerControls.OnStartPlacement += StartPlacingObject;
        playerControls.OnStopPlacement += StopPlacingObject;
        playerControls.OnSlotSelected += SwitchIndex;
        PauseMenuManager.OnPause += StopPlacement;
        PauseMenuManager.OnResume += ResumePlacement;
    }

    private void OnDisable()
    {
        playerControls.OnStartPlacement -= StartPlacingObject;
        playerControls.OnStopPlacement -= StopPlacingObject;
        playerControls.OnSlotSelected -= SwitchIndex;
        PauseMenuManager.OnPause -= StopPlacement;
        PauseMenuManager.OnResume -= ResumePlacement;
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    public void Initialize(LevelData levelData)
    {
        currentLevelData = levelData;
        ResetDevicesAmount();
        placementModeEnabled = true;
    }

    public void ResetDevicesAmount()
    {
        _amountOfDevices.Clear();
        foreach (var deviceOnLevel in currentLevelData.devicesData)
        {
            _amountOfDevices.Add((int)deviceOnLevel.deviceType, deviceOnLevel.deviceAmount);
        }
        OnInitialization?.Invoke(_amountOfDevices);
    }

    private void StartPlacingObject()
    {
        if (PointerOverUIDetector.Instance.IsPointerOverUI())
            return;
        
        CheckThePosition();
        
        if (_amountOfDevices[selectedPrefabIndex] == 0 || !_validPos)
            return;
        
        placementIndicator.StartFilling(PlaceObject, playerControls.OnScreenPosition);
    }

    private void StopPlacingObject()
    {
        placementIndicator.StopFilling();
    }

    private void PlaceObject()
    {
        
        if (placementModeEnabled && _validPos)
        {
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, _currentSurfaceNormal);
            GameObject placedObject = Instantiate(prefabCatalog.allAvailableDevices[selectedPrefabIndex].devicePrefab, _currentPreviewPos, surfaceRotation);
            connectionsManager.LinkNewDevice(placedObject, prefabCatalog.allAvailableDevices[selectedPrefabIndex].deviceType);
            _amountOfDevices[selectedPrefabIndex]--;
            OnObjectPlaced?.Invoke();
        }
    }

    private void CheckThePosition()
    {
        Ray ray = _camera.ScreenPointToRay(playerControls.OnScreenPosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 10000);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, math.INFINITY))
        {
            _validPos = false;
            return;
        }

        if (((1 << hit.collider.gameObject.layer) & placementLayer) == 0)
        {
            _validPos = false;
            return;
        }
        
        
        float surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);
        _currentSurfaceNormal = hit.normal;
                
        if (!Mathf.Approximately(surfaceAngle, slopeAngle))
        {
            _validPos = false;
            return;
        }
                
        _validPos = true;
        _currentPreviewPos = hit.point;
        
        
        // if (Physics.Raycast(ray, out hit, math.INFINITY))
        // {
        //     if (((1 << hit.collider.gameObject.layer) & placementLayer) != 0)
        //     {
        //         float surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);
        //         _currentSurfaceNormal = hit.normal;
        //         
        //         if (!Mathf.Approximately(surfaceAngle, slopeAngle))
        //         {
        //             _validPos = false;
        //             return;
        //         }
        //         
        //         _validPos = true;
        //         _currentPreviewPos = hit.point;
        //     }
        //     else
        //     {
        //         _validPos = false;
        //     }
        // }
        // else
        // {
        //     _validPos = false;
        // }
    }

    private void SwitchIndex(int newIndex)
    {
        selectedPrefabIndex = newIndex;

        if (prefabCatalog.allAvailableDevices[selectedPrefabIndex].onlyOnWalls)
            slopeAngle = 90;
        else
            slopeAngle = 0;
    }

    public void StopPlacement()
    {
        placementModeEnabled = false;
    }

    public void ResumePlacement()
    {
        placementModeEnabled = true;
    }
}
