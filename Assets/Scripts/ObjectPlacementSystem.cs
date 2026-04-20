using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPlacementSystem : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float slopeAngle = 0f;
    [SerializeField] private bool placementModeEnabled = false;

    [SerializeField] private LevelData currentLevelData;
    [SerializeField] private DeviceCatalog prefabCatalog;
    
    [SerializeField] private Vector3 previewPrefabsIdlePos;
    [SerializeField] private int selectedPrefabIndex = 0; //For dev purposes
    [SerializeField] private ConnectionsManager connectionsManager;
    
    private List<Transform> _previewObjects = new List<Transform>();
    private Camera _camera;

    private bool _validPos;
    private Vector3 _currentPreviewPos;
    private Vector3 _currentSurfaceNormal;
    private Dictionary<int, int> _amountOfDevices =  new Dictionary<int,int>();
    
    public static event Action<Dictionary<int, int>> OnInitialization;
    public static event Action OnObjectPlaced;

    private void OnEnable()
    {
        PlayerControl.OnObjectPlaced += PlaceObject;
        PlayerControl.OnSlotSelected += SwitchIndex;
        PauseMenuManager.OnPause += StopPlacement;
        PauseMenuManager.OnResume += ResumePlacement;
    }

    private void OnDisable()
    {
        PlayerControl.OnObjectPlaced -= PlaceObject;
        PlayerControl.OnSlotSelected -= SwitchIndex;
        PauseMenuManager.OnPause -= StopPlacement;
        PauseMenuManager.OnResume -= ResumePlacement;
    }

    private void Start()
    {
        _camera = Camera.main;
        InstantiatePreviewObjects();
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

    private void Update()
    {
        if (placementModeEnabled)
        {
            ShowObjectPreview();
        }
    }

    private void ShowObjectPreview()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, math.INFINITY))
        {

            if (((1 << hit.collider.gameObject.layer) & placementLayer) != 0)
            {
                float surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);
                _currentSurfaceNormal = hit.normal;
                Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, _currentSurfaceNormal);
                _previewObjects[selectedPrefabIndex].rotation = surfaceRotation;
                
                if (!Mathf.Approximately(surfaceAngle, slopeAngle))
                {
                    _validPos = false;
                    return;
                }
            
                _previewObjects[selectedPrefabIndex].position = hit.point;
                _validPos = true;
                _currentPreviewPos = hit.point;
            }
            else
            {
                _validPos = false;
            }
        }
    }

    private void InstantiatePreviewObjects()
    {
        if (prefabCatalog.allAvailableDevices.Count > 0)
        {
            foreach (var prefab in prefabCatalog.allAvailableDevices)
            {
                GameObject previewPrefab =  Instantiate(prefab.previewPrefab);
                previewPrefab.transform.position = previewPrefabsIdlePos;
                _previewObjects.Add(previewPrefab.transform);
            }
        }
    }

    public void PlaceObject()
    {
        if (placementModeEnabled && _validPos)
        {
            if (_amountOfDevices[selectedPrefabIndex] == 0)
                return;
            
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, _currentSurfaceNormal);
            GameObject placedObject = Instantiate(prefabCatalog.allAvailableDevices[selectedPrefabIndex].devicePrefab, _currentPreviewPos, surfaceRotation);
            connectionsManager.LinkNewDevice(placedObject, prefabCatalog.allAvailableDevices[selectedPrefabIndex].deviceType);
            _amountOfDevices[selectedPrefabIndex]--;
            OnObjectPlaced?.Invoke();
        }
    }

    private void SwitchIndex(int newIndex)
    {
        _previewObjects[selectedPrefabIndex].position = previewPrefabsIdlePos;
        selectedPrefabIndex = newIndex;

        if (prefabCatalog.allAvailableDevices[selectedPrefabIndex].onlyOnWalls)
            slopeAngle = 90;
        else
            slopeAngle = 0;

    }

    private void DisablePlacement()
    {
        foreach (var previewObject in _previewObjects)
        {
           previewObject.position = previewPrefabsIdlePos;
        }
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
